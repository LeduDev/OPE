using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerPanel : MonoBehaviour, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Specialty mySpecialty = Specialty.None;
    [SerializeField] private Sprite foodCharmBtn;
    [SerializeField] private Sprite foodRepairBtn;
    [SerializeField] private Sprite foodRemoveBtn;
    [SerializeField] private Sprite faithCharmBtn;
    [SerializeField] private Sprite faithRepairBtn;
    [SerializeField] private Sprite faithRemoveBtn;
    [SerializeField] private Sprite recreationCharmBtn;
    [SerializeField] private Sprite recreationRepairBtn;
    [SerializeField] private Sprite recreationRemoveBtn;

    private static bool mouseIsOver = false;
    private TowerControl towerControl;
    [SerializeField]
    Button thisButton;
    [SerializeField]
    private Image buttonImage;
    private bool canRepair;
    public bool CanRepair
    {
        get
        {
            return canRepair;
        }
        set
        {
            canRepair = value;
            adjustRepair();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //Close the Window on Deselect only if a click occurred outside this panel
        if (!mouseIsOver)
        {
            ClosePanel();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void ClosePanel()
    {
        if (towerControl != null)
        {
            towerControl.towerNameTextContainer.gameObject.SetActive(false);
            towerControl.panelContainer.SetActive(false);
            if (towerControl.isAttacking == false && towerControl.CharmActive == false)
            {
                towerControl.towerBarsContainer.SetActive(false);
            }
        }
        //this.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    public void SetTowerButtons(TowerControl twrCtrl)
    {
        this.towerControl = twrCtrl;
        if (CompareTag("removeTowerBtn"))
        {
            //Altera a imagem do botão conforme a especialidade da instalação
            switch (mySpecialty)
            {
                case Specialty.Food:
                    buttonImage.sprite = foodRemoveBtn;
                    break;
                case Specialty.Faith:
                    buttonImage.sprite = faithRemoveBtn;
                    break;
                case Specialty.Recreation:
                    buttonImage.sprite = recreationRemoveBtn;
                    break;
                default:
                    break;
            }

            if (thisButton != null)
            {
                thisButton.onClick.AddListener(towerControl.CallRemoveTowerCoroutine);
            }
        }
        else if (CompareTag("repairTowerBtn"))
        {
            //Altera a imagem do botão conforme a especialidade da instalação
            switch (mySpecialty)
            {
                case Specialty.Food:
                    buttonImage.sprite = foodRepairBtn;
                    break;
                case Specialty.Faith:
                    buttonImage.sprite = faithRepairBtn;
                    break;
                case Specialty.Recreation:
                    buttonImage.sprite = recreationRepairBtn;
                    break;
                default:
                    break;
            }

            if (thisButton != null)
            {
                thisButton.onClick.AddListener(towerControl.RepairTower);
            }
        }
        else if (CompareTag("charmBtn"))
        {
            //Altera a imagem do botão conforme a especialidade da instalação
            switch (mySpecialty)
            {
                case Specialty.Food:
                    buttonImage.sprite = foodCharmBtn;
                    break;
                case Specialty.Faith:
                    buttonImage.sprite = faithCharmBtn;
                    break;
                case Specialty.Recreation:
                    buttonImage.sprite = recreationCharmBtn;
                    break;
                default:
                    break;
            }

            if (thisButton != null)
            {
                thisButton.onClick.AddListener(towerControl.ToggleCharm);
            }
        }

    }

    //public void AbleRepair()
    //{
    //    if (buttonImage != null)
    //    {
    //        buttonImage.color = new Color32(255, 255, 255, 255);
    //    }
    //    thisButton.enabled = true;
    //}

    //public void DisableRepair()
    //{
    //    if (buttonImage != null)
    //    {
    //        buttonImage.color = new Color32(100, 100, 100, 255);
    //    }
    //    thisButton.enabled = false;
    //}

    public void adjustRepair()
    {
        if (buttonImage != null)
        {
            if (CanRepair)
            {
                buttonImage.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                buttonImage.color = new Color32(100, 100, 100, 255);
            }
        }
        thisButton.enabled = CanRepair;
    }
}
