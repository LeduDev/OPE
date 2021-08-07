using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DragObj : MonoBehaviour
{
    public CardData cardData;


    private Vector3 startPos; //Onde o obj se encontra antes de começar a ser arrastado
    Vector3 mousePos;
    private float deltaX;
    private float deltaY;
    public bool isBeingHeld = false;
    public bool isColliding = false;

    public float activeCostTime = 0.2f;

    [SerializeField]
    private Transform cardSlotPos; //Vai guardar a posição da carta na esteira

    private CardSlotMov cardSlotScript; //Script do card slot


    //Carrega as variáveis
    void Start()
    {
        cardSlotPos = transform.parent.GetComponent<Transform>();
        cardSlotScript = transform.parent.GetComponent<CardSlotMov>();
        //cardSlotScript.activeTime = activeCostTime;
        cardData = gameObject.GetComponent<CardData>();
    }

    void Update()
    {
        if (GameManager.Instance.gameOver)
        {
            Destroy(GetComponent<DragObj>());
        }
    }

    //Pega a carta ao clicar com o botão esquerdo do mouse se as condições forem cumpridas
    private void OnMouseDown()
    {
        if (GameManager.canPickCard == true && !GameManager.Instance.isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SoundManager.Instance.PlaySFX("Pick");

                cardData.costTxt.gameObject.SetActive(true);
                gameObject.transform.parent = null;
                setAlpha(0.5f, true);
                startPos = cardSlotPos.position;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                //calcula a diferença da posição inicial do objeto com a atual do mouse para "suavizar" o "drag" no momento de locomoção do objeto
                deltaX = mousePos.x - this.transform.localPosition.x;
                deltaY = mousePos.y - this.transform.localPosition.y;
                isBeingHeld = true;
                //spriteRend.maskInteraction = SpriteMaskInteraction.None;
            }
        }
    }

    //Movimenta a carta so segurar o botão esquerdo do mouse e arrastar 
    private void OnMouseDrag()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isBeingHeld = false;
            ReturnCard();
        }
        if (isBeingHeld == true)
        {
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY, 0);
        }
    }

    //Ao soltar o botão a carta volta para sua vaga
    private void OnMouseUp()
    {
            if (!isColliding && isBeingHeld)
            {
                SoundManager.Instance.PlaySFX("Drop");
                ReturnCard();
            }
            isBeingHeld = false;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("cardTrack") || collision.gameObject.CompareTag("trackEnd") && !isBeingHeld)
        {
            StartCoroutine(showCardCost(collision.gameObject));
        }
    }

    //Mostra o custo da carta ou o esconde segundos depois
    private IEnumerator showCardCost(GameObject colObj)
    {
        yield return new WaitForSeconds(activeCostTime);
        if (colObj.gameObject.CompareTag("cardTrack"))
        {
            cardData.costTxt.gameObject.SetActive(true);
        }
        else if (!isBeingHeld)
        {
            cardData.costTxt.gameObject.SetActive(false);
        }
        
    }

    //Deixa a carta e seus componentes semi-transparentes
    public void setAlpha(float alpha, bool visible)
    {
        this.GetComponentsInChildren<SpriteRenderer>();

        Color newColor;

        SpriteRenderer[] childrenSprite = this.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in childrenSprite)
        {
            newColor = sprite.color;
            newColor.a = alpha;
            sprite.color = newColor;
            if (visible)
            {
                sprite.maskInteraction = SpriteMaskInteraction.None;
            }
            else
            {
                sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
        }
    }

    //Retorna a carta para sua vaga na esteira ou a destrói caso a vaga já tenha sido destruída
    public void ReturnCard()
    {
        if (cardSlotPos != null)
        {
            //    if (cardSlotScript.costTxtActive)
            //    {
            //        cardData.costTxt.gameObject.SetActive(true);
            //    }
            //    else
            //    {
            //        cardData.costTxt.gameObject.SetActive(false);
            //    }
            gameObject.transform.parent = cardSlotPos.transform;
            gameObject.transform.position = transform.parent.position;
            setAlpha(1f, false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
