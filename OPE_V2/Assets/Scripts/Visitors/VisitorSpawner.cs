using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorSpawner : MonoBehaviour
{
    [Header("Time Regulators")]
    public float currentTime;
    public float spawnTime = 0.0f;
    //Variáveis de tempo, min e máx em que os visitantes podem surgir
    private float minSpawnTime = 3.0f;
    private float maxSpawnTime = 6.0f;

    //contém todos os obj nos pontos de invocação 
    public GameObject spawnPosContainer;
    Transform[] spawnPoints;

    public GameObject visitorPrefab;

    public Visitor[] visitors;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        //Debug.Log(spawnTime);

        //NOTA: ele também pega o Transform do próprio gameObject, por isso vou desconsiderar o elemento de índice 0
        CheckAvailablePoints();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused && !GameManager.Instance.gameOver)
        {
            if (GameManager.Instance.allTowers.Count > 0 && GameManager.Instance.canSpawnVisitor)
            {
                if (spawnPoints.Length > 1 && GameManager.Instance.allVisitors.Count < GameManager.Instance.maxVisitors)
                {
                    if (currentTime < spawnTime)
                    {
                        currentTime += Time.deltaTime;
                    }
                    else if (currentTime >= spawnTime)
                    {
                        SpawnVisitor();
                        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
                        //Debug.Log(spawnTime);
                        currentTime = 0;
                    }
                }
            }
        }
    }

    public void SpawnVisitor()
    {
        //Transform[] spawnPoints = spawnPosContainer.GetComponentsInChildren<Transform>(false);

        //foreach (Transform point in spawnPoints)
        //{
        //    Debug.Log(point.gameObject.name);
        //}

        Transform spawnPos = spawnPoints[Random.Range(1, spawnPoints.Length)];
        //Debug.Log(spawnPos.gameObject.name);

        GameObject newVisitor = Instantiate(visitorPrefab, this.transform);

        //Adiciona o objeto do novo visitante a lista com todos os inimigos do Game Manager
        GameManager.Instance.allVisitors.Add(newVisitor);
        GameManager.Instance.visitorsSpawned++;
        //Debug.Log(GameManager.Instance.allVisitors.Count);
        //foreach (GameObject visit in GameManager.Instance.allVisitors)
        //{
        //    Debug.Log(visit.gameObject.name + " está na lista.");
        //}

        newVisitor.transform.position = spawnPos.position;
        VisitorControl visitorControl = newVisitor.GetComponent<VisitorControl>();
        visitorControl.visitorData = visitors[Random.Range(0, visitors.Length)];
        visitorControl.spawnedPoint = spawnPos;
        spawnPos.gameObject.SetActive(false);
        CheckAvailablePoints();
    }

    public void CheckAvailablePoints()
    {
        spawnPoints = spawnPosContainer.GetComponentsInChildren<Transform>(false);
    }
}
