using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaveManager : Singleton<WaveManager>
{
    EnemyUIManager enemyUIManager;

    [SerializeField]
    private Enemy[] EnemiesSO; //Scriptable Objects com os dados de cada inimigo

    public GameObject enemyPrefab; //Prefab base que será usado para instanciar novos inimigos

    [SerializeField]
    private int[] enemiesAmount; //Armazena a qtd de inimigos que pode ser instanciada do prefab de mesmo index de EnemiesPrefabs

    //A soma dos amounts do array enemiesAmount
    private int EnemiesTotal;
    public int enemiesTotal
    {
        get
        {
            return EnemiesTotal;
        }
        set
        {
            EnemiesTotal = value;
            //enemyUIManager.UpdateEnemyCountTxt();
        }
    }

    public static int despatchedEnemies; //Qtd de inimigos que foram instanciados esse turno/wave. Alcançando o total, deve parar de instanciar.

    //Qtd de inimigos q já foram derrotados nessa wave, se for igual ao enemiesTotal quer dizer q a wave acabou
    [SerializeField]
    private int EnemiesDefeated;
    public int enemiesDefeated
    {
        get
        {
            return EnemiesDefeated;
        }
        set
        {
            EnemiesDefeated = value;
            //enemyUIManager.UpdateEnemyCountTxt();
        }
    }

    [SerializeField]
    private Transform enemiesParentTransform; //Objeto vazio que guarda todos os inimigos como filhos
    public Transform spawnPoint; //Posição onde será criado o inimigo

    public float waveCountdownTime = 3f; //Tempo inicial de contagem do timer
    [SerializeField]
    public float waveCountdown; //Guarda o tempo atual do timer | Deve iniciar com o tempo inicial
    [SerializeField] private float waitNextEnemy = 2f; //Tempo de intervalo entre a saída de um inimigo e outro

    private int waveNumber = 0; //número da wave atual
    public int WaveNumber
    {
        get
        {
            return waveNumber;
        }
        set
        {
            waveNumber = value;
            enemyUIManager.UpdateWaveCountText();

            //Aumenta a quantidade de visitantes que podem surgir na fase
            if (GameManager.Instance.maxVisitors < 6)
            {
                GameManager.Instance.maxVisitors++;
            }
        }
    }

    //Número máx de waves
    public int maxWaves; 

    void Start()
    {
        enemyUIManager = GetComponent<EnemyUIManager>();

        maxWaves = 10;

        //Carrega as variáveis e atualiza a interface
        waveCountdown = waveCountdownTime;
        WaveNumber = 0;
        enemiesAmount = new int[EnemiesSO.Length];

        //Calcula a quantidade total de inimigos somando todas as waves
        for (int i = 0; i < maxWaves; i++)
        {
            SetEnemiesAmount(i + 1);
            SumEnemiesAmount();
            GameManager.Instance.totalEnemies += enemiesTotal;
        }

        SetEnemiesAmount(0);
        SumEnemiesAmount();

        despatchedEnemies = 0;
        enemiesTotal = 0;
        enemiesDefeated = 0;
    }

    void Update()
    {
        if (WaveNumber < maxWaves)
        {
            if (waveCountdown <= 0f)
            {
                WaveNumber++;

                //A cada 2 hordas aumenta o tempo do temporizador em 1 seg
                if (WaveNumber % 2 == 0)
                {
                    waveCountdownTime += 1f; //Aumenta o tempo inicial do contador
                }

                //Acelera a velocidade em que os inimigos spawnam
                waitNextEnemy -= 0.05f;

                //Define a quantidade de inimigos de cada tipo que virá nessa horda
                SetEnemiesAmount(WaveNumber);
                //Conta o total geral de inimigos dessa horda
                SumEnemiesAmount();
                //Chama a corotina que instancia os inimigos
                StartCoroutine(SpawnWave());

                //if (waveCountdownTime > 1f)
                //{
                //    waveCountdownTime -= 1f; //diminui o tempo inicial do contador
                //}
                waveCountdown = waveCountdownTime;

                //enemiesCounter.GetComponent<Text>().text = (enemiesDefeated + "/" + enemiesTotal).ToString();

                //Esconde o temporizador e mostra a quantidade de inimigos enquanto enfrenta a horda
                enemyUIManager.WaveTimerIcon.SetActive(false);
                //enemyUIManager.enemiesCountTxt.gameObject.SetActive(true);
            }
            if (enemyUIManager.WaveTimerIcon.activeInHierarchy)
            {
                waveCountdown -= Time.deltaTime;
                enemyUIManager.SetTimerBar();
            }
            //Zera as variáveis e reinicia a contagem do temporizador quando a horda acaba
            if (enemiesDefeated == enemiesTotal)
            {
                despatchedEnemies = 0;
                enemiesTotal = 0;
                enemiesDefeated = 0;

                enemyUIManager.WaveTimerIcon.SetActive(true);
                enemyUIManager.enemiesCountTxt.gameObject.SetActive(false);
            }
        }
        //Se acaba todas as hordas acaba o jogo
        if (WaveNumber == maxWaves && GameManager.Instance.isPaused == false && enemiesDefeated == enemiesTotal)
        {
            ScreenManager.Instance.GameOver();
        }
    }

    //Gera os inimigos no mapa enquanto restar inimigos para serem instanciados no array
    private IEnumerator SpawnWave()
    {
        while (despatchedEnemies != enemiesTotal)
        {
            int enemyIndex = Random.Range(0, EnemiesSO.Length);

            while (enemiesAmount[enemyIndex] <= 0)
            {
                enemyIndex = Random.Range(0, EnemiesSO.Length);
            }
            if (enemiesAmount[enemyIndex] > 0)
            {
                GameObject newObject = Instantiate(enemyPrefab);
                newObject.name = EnemiesSO[enemyIndex].name;
                newObject.transform.SetParent(enemiesParentTransform);
                EnemyData enemyData = newObject.GetComponent<EnemyData>();
                enemyData.enemy = EnemiesSO[enemyIndex];
                EnemyMovement enemyMov = newObject.GetComponent<EnemyMovement>();
                enemyMov.SpawnPos(); //define a posição do inimigo

                //Adiciona o objeto do novo inimigo a lista com todos os inimigos do Game Manager
                GameManager.Instance.allEnemies.Add(newObject);

                //Debug.Log(GameManager.Instance.allEnemies.Count);
                //foreach (GameObject enem in GameManager.Instance.allEnemies)
                //{
                //    Debug.Log(enem.gameObject.name + " está na lista.");
                //}

                enemiesAmount[enemyIndex]--;
                despatchedEnemies++;
            }
            yield return new WaitForSeconds(waitNextEnemy);
        }

        //Se acabou de spawnar todos os inimigos da última horda, impedi de spawnar visitantes
        if (WaveNumber == maxWaves)
        {
            if (despatchedEnemies == enemiesTotal)
            {
                GameManager.Instance.canSpawnVisitor = false;
            }
        }
    }

    //Conta o total geral de inimigos dessa horda
    private void SumEnemiesAmount()
    {
        enemiesTotal = 0;
        for (int i = 0; i < enemiesAmount.Length; i++)
        {
            enemiesTotal += enemiesAmount[i];
        }
    }

    /*Define a quantidade de inimigos de cada tipo que surgirão em cada horda (wave) de acordo com seu valor
     [0] - Corredor
     [1] - Baderneira
     [2] - Grandalhão
    */
    public void SetEnemiesAmount(int wave)
    {
        switch (wave)
        {
            case 0:
                enemiesAmount[0] = 0;
                enemiesAmount[1] = 0;
                enemiesAmount[2] = 0;
                break;
            case 1:
                enemiesAmount[0] = 3;
                enemiesAmount[1] = 0;
                enemiesAmount[2] = 0;
                break;
            case 2:
                enemiesAmount[0] = 4;
                enemiesAmount[1] = 2;
                enemiesAmount[2] = 0;
                break;
            case 3:
                enemiesAmount[0] = 7;
                enemiesAmount[1] = 4;
                enemiesAmount[2] = 0;
                break;
            case 4:
                enemiesAmount[0] = 10;
                enemiesAmount[1] = 7;
                enemiesAmount[2] = 3;
                break;
            case 5:
                enemiesAmount[0] = 12;
                enemiesAmount[1] = 9;
                enemiesAmount[2] = 5;
                break;
            case 6:
                enemiesAmount[0] = 14;
                enemiesAmount[1] = 12;
                enemiesAmount[2] = 8;
                break;
            case 7:
                enemiesAmount[0] = 17;
                enemiesAmount[1] = 14;
                enemiesAmount[2] = 9;
                break;
            case 8:
                enemiesAmount[0] = 20;
                enemiesAmount[1] = 16;
                enemiesAmount[2] = 11;
                break;
            case 9:
                enemiesAmount[0] = 22;
                enemiesAmount[1] = 18;
                enemiesAmount[2] = 13;
                break;
            case 10:
                enemiesAmount[0] = 25;
                enemiesAmount[1] = 20;
                enemiesAmount[2] = 15;
                break;
            default:
                enemiesAmount[0] = 20;
                enemiesAmount[1] = 15;
                enemiesAmount[2] = 10;
                break;
        }
    }
}
