using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Transform cardSlotPos; //Vai guardar a posição da carta na esteira
    public bool isInteracting; //Is it interacting with something else?
    private Vector3 returnPos; //A posição para qual o visitante deve voltar
    public bool isDragging;

    private void Start()
    {
        isDragging = false;
    }

    void Update()
    {
        if (GameManager.Instance.gameOver)
        {
            if (isDragging)
            {
                Destroy(this.gameObject);
            }
            //this.GetComponent<Draggable>().enabled=false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject.CompareTag("buildingCard") || gameObject.CompareTag("garbageCard") || gameObject.CompareTag("offeringCard"))
        {
            gameObject.transform.position = eventData.position;
        }
        else if (gameObject.CompareTag("visitor"))
        {
            DragOnCameraCanvas();
        }
    }

    //Mexe na opacidade da carta, podendo deixá-la semi-transparente
    public void SetAlpha(float alpha)
    {
        GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void ReturnCard()
    {
        SoundManager.Instance.PlaySFX("Drop");

        if (gameObject.CompareTag("buildingCard") || gameObject.CompareTag("garbageCard") || gameObject.CompareTag("offeringCard"))
        {
            if (cardSlotPos != null)
            {
                gameObject.transform.SetParent(cardSlotPos);
                gameObject.transform.position = transform.parent.position;
                SetAlpha(1f);
            }
            else
            {
                if (gameObject.CompareTag("garbageCard"))
                {
                    GameManager.Instance.SetSatisfaction(Specialty.Cleanliness, -1);
                }
                Destroy(gameObject);
            }
        }
        else if (gameObject.CompareTag("visitor"))
        {
            gameObject.transform.position = returnPos;
            SetAlpha(1f);
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX("Pick");
        isDragging = true;
        
        if (gameObject.CompareTag("buildingCard") || gameObject.CompareTag("garbageCard") || gameObject.CompareTag("offeringCard")) //Se pegar uma carta
        {
            cardSlotPos = transform.parent.GetComponent<Transform>();
            gameObject.transform.SetParent(gameObject.transform.root);
            gameObject.transform.position = eventData.position;
        }
        else if (gameObject.CompareTag("visitor")) //Se pegar um visitante
        {
            returnPos = this.transform.position;
            DragOnCameraCanvas();
        }

        SetAlpha(0.5f);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isInteracting)
        {
            ReturnCard();
        }
    }

    //Permite mover objetos ou elementos de interface para a posição do mouse corretamente em canvas que não são "Overlay"
    public void DragOnCameraCanvas()
    {
        Canvas myCanvas = gameObject.transform.root.gameObject.GetComponent<Canvas>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out Vector2 pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
    }
}
