using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisitorControl : MonoBehaviour
{
    [Header("Request")]
    public Specialty request = Specialty.None;

    [Header("Time Variables")]
    [SerializeField]
    private float requestTime = 0.0f;
    [SerializeField]
    private float currentTime;
    [SerializeField]
    private float requestDuration;
    //Variáveis de tempo, min e máx em que os visitantes podem surgir
    private float minRequestTime = 3.0f;
    private float maxRequestTime = 6.0f;


    [Header("Request Interface Object")]
    //Elemento visual de interface do pedido do visitante
    public GameObject requestBox;
    public Image requestIconImage;

    [Header("Request Icons Images")]
    public Sprite foodIcon;
    public Sprite faithIcon;
    public Sprite recreationIcon;
    public Sprite loveIcon;

    [Header("Spawned Point")]
    public Transform spawnedPoint;

    private Draggable draggable;

    [Header("Visitor Scriptable Object")]
    public Visitor visitorData;

    //Object do visitante que é child deste
    private GameObject visitorObject;

    private void Start()
    {
        draggable = gameObject.GetComponent<Draggable>();
        visitorObject = gameObject.transform.GetChild(0).gameObject;

        if (visitorData != null && visitorObject != null)
        {
            visitorObject.GetComponent<SpriteRenderer>().sprite = visitorData.sprite;
            visitorObject.GetComponent<Animator>().runtimeAnimatorController = visitorData.visitorAnimControl;
        }

        requestDuration = 7.0f;
        requestTime = Random.Range(minRequestTime, maxRequestTime);

        //foodIcon = Resources.Load<Sprite>("Graphics/Icons/pedidos/pedidos_0");
        //faithIcon = Resources.Load<Sprite>("Graphics/Icons/pedidos/pedidos_1");
        //recreationIcon = Resources.Load<Sprite>("Graphics/pedidos/Icons/pedidos_2");
        //loveIcon = Resources.Load<Sprite>("Graphics/Icons/Love");
    }

    private void Update()
    {
        if (!GameManager.Instance.isPaused && !GameManager.Instance.gameOver)
        {
            if (draggable.isDragging == false)
            {
                if (!requestBox.activeInHierarchy)
                {
                    if (GameManager.Instance.allTowers.Count > 0)
                    {
                        if (currentTime < requestTime)
                        {
                            currentTime += Time.deltaTime;
                        }
                        else if (currentTime >= requestTime)
                        {
                            //int specialtyIndex = Random.Range(1, 4);
                            //request = (Specialty)specialtyIndex;

                            //Faz pedido apenas das torres (instalações) que já estão em jogo
                            int towerId = Random.Range(0, GameManager.Instance.allTowers.Count);
                            RangeTower rangeTower = GameManager.Instance.allTowers[towerId].GetComponentInChildren<RangeTower>();
                            if (rangeTower != null)
                            {
                                request = rangeTower.tower.specialty;
                                PopRequestBox(request);
                                currentTime = 0;
                            }
                            else
                            {
                                return;
                            }

                        }
                    }
                }
                else
                {
                    if (currentTime < requestDuration)
                    {
                        currentTime += Time.deltaTime;
                    }
                    else if (currentTime >= requestDuration)
                    {
                        if (request != Specialty.Love)
                        {
                            GameManager.Instance.SetSatisfaction(request, -1);
                        }
                        else
                        {
                            requestDuration /= 2;
                        }
                        request = Specialty.None;
                        requestBox.SetActive(false);
                        requestTime = Random.Range(minRequestTime, maxRequestTime);
                        currentTime = 0;
                    }
                }
            }
        }
    }

    public void PopRequestBox(Specialty request)
    {
        switch (request)
        {
            case Specialty.Food:
                requestIconImage.sprite = foodIcon;
                break;
            case Specialty.Faith:
                requestIconImage.sprite = faithIcon;
                break;
            case Specialty.Recreation:
                requestIconImage.sprite = recreationIcon;
                break;
            case Specialty.Love:
                requestIconImage.sprite = loveIcon;
                break;
            default:
                return;
        }
        requestBox.SetActive(true);
    }

    public void LoveResquest()
    {
        currentTime = 0;
        request = Specialty.Love;
        PopRequestBox(request);
        requestDuration *= 2;
    }
}
