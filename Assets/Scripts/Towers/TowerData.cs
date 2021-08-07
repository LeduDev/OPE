using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : Singleton<TowerData>
{
    [SerializeField]
    //custo da torre
    private int AxeCost;


    //SpriteRenderer do Range da filha da torre
    private SpriteRenderer myRange;

    //Alcance da torre
    [SerializeField]
    public float range;
    
    public int axeCost //preço da 'torre'
    {
        get 
        {
            return AxeCost;
        }
        set
        {
            AxeCost = value;
        }
    }

    public string TowerName;
    private bool overThisTower = false;
    //valor que influencia o dano causado;
    public float power;


    // Start is called before the first frame update
    void Start()
    {
        myRange = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameOver)
        {
            Destroy(GetComponent<TowerData>());
        }

        //Ajustes caso o jogo esteja pausado
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (myRange && overThisTower)
            {
                myRange.enabled = !myRange.enabled;
            }
            if (!myRange && overThisTower)
            {
                myRange.enabled = !myRange.enabled;
            }
        }
    }

    private void OnMouseEnter()
    {
        //Mostra o range da torre se colocar o mouse sobre ela
        GameManager.isOverTower = true;
        overThisTower = true;
        if (!GameManager.Instance.isPaused)
        {
            myRange.enabled = !myRange.enabled;
        }
        //GameManager.Instance.towerCanvas = gameObject.transform.GetChild(1).gameObject;
    }

    //Esconde o range da torre se tirar o mouse de cima dela
    private void OnMouseExit()
    {
        if (!GameManager.Instance.isPaused)
        {
            myRange.enabled = !myRange.enabled;
        }
        GameManager.isOverTower = false;
        overThisTower = false;
    }
}
