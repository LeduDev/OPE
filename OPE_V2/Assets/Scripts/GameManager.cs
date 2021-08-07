using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameManager : Singleton<GameManager>
{
    [Header("Visitors Satisfaction")]
    public int foodSatisfaction;
    public int faithSatisfaction;
    public int recreationSatisfaction;
    public int cleanlinessSatisfaction;
    public int initialSatisfaction;
    public int satisfactionMaxValue;

    [Header("Visitors Settings")]
    public int maxVisitors; //Qtd máx. de visitantes permitidas naquela wave
    public bool canSpawnVisitor = true; //Pode gerar visitantes?

    [Header("Overall Satisfaction")]
    //public int overallSatisfaction;
    public int overallMeterLvAmount; //Quantos níveis possui, istó é: quantas barras no total (1 por lv)
    public int overallMeterMaxPerLv; //Quanto cada barra "aguenta" de valor máximo
    public int overallMeterFullBars; //Quantas barras estão cheias no momento
    public int overallTotalValue;

    [Header("Iemanjá Data")]
    [SerializeField] private static float life;  //Vida de Iemanjá, se zerar acaba o jogo
    [SerializeField]
    public static float Life 
    {
        get
        {
            return life;
        }
        set
        {
            life = value;
            ScreenManager screenManager = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
            if (screenManager != null)
            {
                screenManager.SetLife();
            }            
        }
    }
    //Máximo que a vida da estátua pode chegar
    public static float maxLife = 10f;

    [Header("Game States")]    
    public bool gameOver = false; //Acabou o jogo?   
    public bool isPaused = false; //Está pausado?
    public bool criticalState = false; //Estado crítico é quando a próxima redução de alguma satisfação resulta em Game Over
    public bool youLose = false;
    public static bool showLanguagesScreen = true;

    [Header("Card Controller")]
    [SerializeField]
    //Informa se o jogador pode segurar a carta da esteira
    public static bool canPickCard = true;
    public static bool isOverTower = false;

    [Header("Manifestation Control")]
    public int manifestationCostDown; //Quantidade pela qual as manifestações serão reduzidas
    public int[] manifestationUses = new int[3]; //Quantas vezes foram usadas cada manifestação 0 = (Amor) | 1 = (Cura) | 2 = (Ori)

    [Header("Game Results")]
    public int offeringsSpawned = 0;
    public int offeringsMade = 0;
    public float offeringPoints = 0;
    public int totalEnemies = 0;
    public int enemiesDefeated = 0;
    public float enemiesPoints = 0;
    public int garbageSpawned = 0;
    public int garbageCleaned = 0;
    public float garbagePoints = 0;
    public int visitorsSpawned = 0;
    public int visitorsSatisfied = 0;
    public float visitorPoints = 0;
    public int statueDamage = 0; //Em porcentagem
    public float statuePoints = 0;
    public float finalScore = 0;
    public int itemsUnlocked = 0; //Qtd de itens desbloqueados nesta partida

    [Header("Lists")]
    public List<GameObject> allVisitors; //Guarda o GameObject de todos os visitantes na cena
    public List<GameObject> allEnemies; //Guarda o GameObject de todos os inimigos na cena
    public List<GameObject> allTowers; //Guarda o GameObject de todas as torres (instalações) na cena

    [SerializeField] private TranslateText translator;
    private ScreenManager screenManagerScript;

    // Start is called before the first frame update
    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex > ScreenManager.mainMenuIndex)
        {
            allVisitors = new List<GameObject>();
            allEnemies = new List<GameObject>();
            allTowers = new List<GameObject>();

            Life = maxLife;
            maxVisitors = 0;

            initialSatisfaction = 3;
            satisfactionMaxValue = 10;
            overallMeterLvAmount = 4;
            overallMeterMaxPerLv = 10;
            overallMeterFullBars = 0;

            screenManagerScript = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();

            //Seta valores inicias das necessidades dos visitantes
            foodSatisfaction = initialSatisfaction;
            faithSatisfaction = initialSatisfaction;
            recreationSatisfaction = initialSatisfaction;
            cleanlinessSatisfaction = initialSatisfaction;
            //overallSatisfaction = foodSatisfaction + faithSatisfaction + recreationSatisfaction + cleanlinessSatisfaction;
            overallTotalValue = foodSatisfaction + faithSatisfaction + recreationSatisfaction + cleanlinessSatisfaction;

            manifestationCostDown = 0;
        }
    }

    private void Start()
    {
        translator = GameObject.Find("Translator").GetComponent<TranslateText>();

        if (SceneManager.GetActiveScene().buildIndex > ScreenManager.mainMenuIndex)
        {
            screenManagerScript.SetSatisfactionMeter(foodSatisfaction, satisfactionMaxValue, Specialty.Food);
            screenManagerScript.SetSatisfactionMeter(faithSatisfaction, satisfactionMaxValue, Specialty.Faith);
            screenManagerScript.SetSatisfactionMeter(recreationSatisfaction, satisfactionMaxValue, Specialty.Recreation);
            screenManagerScript.SetSatisfactionMeter(cleanlinessSatisfaction, satisfactionMaxValue, Specialty.Cleanliness);
            screenManagerScript.UpdateOverallMeter();
        }

        //Desbloquear itens que virão desbloqueados por padrão
        ScreenManager.Instance.UnlockItem(0);
    }

    //Ao fim da partida vai checar todos os requerimentos e aqueles atendidos vão desbloquear os itens correspontes
    public void CheckUnlockRequirements()
    {
        //Inserir requerimentos

        //Se venceu a partida
        if (youLose == false)
        {
            ScreenManager.Instance.UnlockItem(2);

            //Se venceu a partida com 6 ou mais instalações montadas
            if (allTowers.Count >= 6)
            {
                ScreenManager.Instance.UnlockItem(3);
            }

            //Se Iemanjá não tomou dano e venceu
            if (Life == maxLife)
            {
                ScreenManager.Instance.UnlockItem(12);
            }
        }

        //Se o n° da wave for >= 6
        WaveManager waveManager = gameObject.GetComponent<WaveManager>();
        if (waveManager != null)
        {
            if (waveManager.WaveNumber >= 6)
            {
                ScreenManager.Instance.UnlockItem(1);
            }
        }

        //Se a satisfação em comida estiver cheia ao fim da partida
        if (foodSatisfaction >= satisfactionMaxValue)
        {
            ScreenManager.Instance.UnlockItem(4);
        }

        //Se a satisfação em fé estiver cheia ao fim da partida
        if (faithSatisfaction >= satisfactionMaxValue)
        {
            ScreenManager.Instance.UnlockItem(5);
        }

        //Se a satisfação em recreação estiver cheia ao fim da partida
        if (recreationSatisfaction >= satisfactionMaxValue)
        {
            ScreenManager.Instance.UnlockItem(6);
        }

        //Se a satisfação em limpeza estiver cheia ao fim da partida
        if (cleanlinessSatisfaction >= satisfactionMaxValue)
        {
            ScreenManager.Instance.UnlockItem(7);
        }

        //Se a barra de satisfação de Iemanjá está cheia
        if (overallMeterFullBars >= overallMeterLvAmount)
        {
            ScreenManager.Instance.UnlockItem(8);
        }

        //Se usou a manifestação Amor da Grande Mãe ao menos 1 vez
        if (manifestationUses[0] > 0)
        {
            ScreenManager.Instance.UnlockItem(9);
        }

        //Se usou a manifestação Bênção das Águas ao menos 1 vez
        if (manifestationUses[1] > 0)
        {
            ScreenManager.Instance.UnlockItem(10);
        }

        //Se usou a manifestação Purificação do Ori ao menos 1 vez
        if (manifestationUses[2] > 0)
        {
            ScreenManager.Instance.UnlockItem(11);
        }

        //Se a pontuação final foi perfeita (5.0)
        if (finalScore >= 5f)
        {
            ScreenManager.Instance.UnlockItem(13);
        }
    }

    public float CalculateFinalScore()
    {
        //Coloca a mensagem de curiosidade quando for calcular os resultados da tela de resultados
        if (translator != null)
        {
            translator.SetCuriosityText();
        }

        //Potuação das oferendas
        if (offeringsSpawned > 0)
        {
            offeringPoints = (float) offeringsMade / offeringsSpawned;
        }
        else
        {
            offeringPoints = 1f;
        }

        //Potuação dos inimigos derrotados
        enemiesPoints = enemiesDefeated / totalEnemies;

        //Potuação dos lixos
        if (garbageSpawned > 0)
        {
            garbagePoints = (float) garbageCleaned / garbageSpawned;
        }
        else
        {
            garbagePoints = 1f;
        }

        //Potuação dos visitantes
        if (visitorsSpawned > 0)
        {
            visitorPoints = (float) visitorsSatisfied / visitorsSpawned;
        }
        else
        {
            visitorPoints = 1f;
        }

        //Potuação dos visitantes
        statuePoints = Life / maxLife;
        float totalDamage = ((maxLife - Life) / maxLife) * 100;
        statueDamage = (int) totalDamage;

        //Debug.Log(offeringPoints + " Offering points");
        //Debug.Log(enemiesPoints + " Enemies points");
        //Debug.Log(garbagePoints + " Garbage points");
        //Debug.Log(visitorPoints + " Visitor points");
        //Debug.Log(statuePoints + " Statue points");

        finalScore = offeringPoints + enemiesPoints + garbagePoints + visitorPoints + statuePoints;
        if (youLose)
        {
            finalScore /= 2;
        }

        //Salva a MELHOR PONTUAÇÃO, isto é, se ela for maior que a que está lá ou se a que está lá for nula
        float bestScore = PlayerPrefs.GetFloat("bestScore");

        if (finalScore > bestScore)
        {
            PlayerPrefs.SetFloat("bestScore", finalScore);
        }
        //Debug.Log("Sua melhor pontuação é " + bestScore);

        return finalScore;
    }

    //Ajusta valores das satisfações corretamente. Colocar valor de point negativo para diminuir.
    public void SetSatisfaction(Specialty request, int point)
    {
        if (point < 0 && criticalState == true)
        {
            youLose = true;
            StartCoroutine(ScreenManager.Instance.FadeScreen());
            //ScreenManager.Instance.GameOver();
        }
        else
        {
            switch (request)
            {
                case Specialty.Food:
                    foodSatisfaction += point;
                    if (foodSatisfaction > satisfactionMaxValue)
                    {
                        foodSatisfaction = satisfactionMaxValue;
                    }
                    else if (foodSatisfaction < 0)
                    {
                        foodSatisfaction = 0;
                        //Pode ter alguma outra penalidade aqui
                    }
                    screenManagerScript.SetSatisfactionMeter(foodSatisfaction, satisfactionMaxValue,request);
                    break;
                case Specialty.Faith:
                    faithSatisfaction += point;
                    if (faithSatisfaction > satisfactionMaxValue)
                    {
                        faithSatisfaction = satisfactionMaxValue;
                    }
                    else if (faithSatisfaction < 0)
                    {
                        faithSatisfaction = 0;
                        //Pode ter alguma outra penalidade aqui
                    }
                    screenManagerScript.SetSatisfactionMeter(faithSatisfaction, satisfactionMaxValue, request);
                    break;
                case Specialty.Recreation:
                    recreationSatisfaction += point;
                    if (recreationSatisfaction > satisfactionMaxValue)
                    {
                        recreationSatisfaction = satisfactionMaxValue;
                    }
                    else if (recreationSatisfaction < 0)
                    {
                        recreationSatisfaction = 0;
                        //Pode ter alguma outra penalidade aqui
                    }
                    screenManagerScript.SetSatisfactionMeter(recreationSatisfaction, satisfactionMaxValue, request);
                    break;
                case Specialty.Cleanliness:
                    cleanlinessSatisfaction += point;
                    if (cleanlinessSatisfaction > satisfactionMaxValue)
                    {
                        cleanlinessSatisfaction = satisfactionMaxValue;
                    }
                    else if (cleanlinessSatisfaction < 0)
                    {
                        cleanlinessSatisfaction = 0;
                        //Pode ter alguma outra penalidade aqui
                    }
                    screenManagerScript.SetSatisfactionMeter(cleanlinessSatisfaction, satisfactionMaxValue, request);
                    break;
                default:
                    //return;
                    break;               
            }

            //Acerta a barra de medidor geral
            screenManagerScript.UpdateOverallMeter();
        }

    }
}
