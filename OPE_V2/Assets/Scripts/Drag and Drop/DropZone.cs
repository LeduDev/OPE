using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    CardData cardData;
    public Tower tower; //Guarda os dados do scriptable object da torre atual
    public GameObject towerPrefab; //Prefab da torre que será instanciada
    private GameObject newTower; //Guarda os dados da nova torre criada a partir do prefab
    public GameObject buildMark; //Objeto com a imagem da marca de construção
    private TowerControl towerControl; //Scripts com os dados da torre criada
    GameObject dragObject; //Object that is being drag
    public bool isOccupied; //Há construções nessa área? 
    private float energyBonus = 1f; //Quantidade de energia incrementada ao jogar uma carta da mesma especialidade na instalação ativa
    private TowerPanel towerPanel;

    private void Start()
    {
        isOccupied = false;
        if (gameObject.CompareTag("towerArea"))
        {
            buildMark = gameObject.transform.GetChild(0).gameObject;
        }
        if (gameObject.CompareTag("towerArea"))
        {
            this.towerPanel = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TowerPanel>();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Se este script está em uma área de construção de torre
        if (gameObject.CompareTag("towerArea"))
        {
            if (eventData.pointerDrag.gameObject.tag == "buildingCard")
            {
                cardData = eventData.pointerDrag.gameObject.GetComponent<CardData>();
                Draggable draggable = eventData.pointerDrag.gameObject.GetComponent<Draggable>();

                if (!isOccupied)
                {
                    //O que acontece se a área não possui uma instalação
                    if (cardData != null)
                    {
                        tower = cardData.towerData;
                        if (axeOrbs.Instance.BuyTower(cardData.towerData.cost))
                        {
                            BuildTower(tower);                            
                            Destroy(cardData.gameObject);
                        }
                        else
                        {
                            if (draggable != null)
                            {
                                draggable.ReturnCard();
                                //Não pode comprar
                            }
                        }
                    }
                }
                else
                {
                    //O que acontece se a área já possui uma instalação
                    //Aqui vai encher o charme
                    if (cardData != null)
                    {
                        Specialty cardSpecialty = cardData.towerData.specialty;
                        if (axeOrbs.Instance.BuyTower(cardData.towerData.cost))
                        {
                            if (cardSpecialty == tower.specialty)
                            {
                                SoundManager.Instance.PlaySFX("Energy");
                                towerControl.SetTowerEnergy(energyBonus);
                            }
                            else
                            {
                                SoundManager.Instance.PlaySFX("Error");
                                towerControl.SetTowerEnergy(energyBonus*(-1));
                            }

                            Destroy(cardData.gameObject);
                        }
                        else
                        {
                            if (draggable != null)
                            {
                                draggable.ReturnCard();
                                //Não pode comprar
                            }
                        }
                    }
                }
            }
            else if (eventData.pointerDrag.gameObject.tag == "visitor")
            {
                if (isOccupied)
                {
                    VisitorControl visitorData = eventData.pointerDrag.gameObject.GetComponent<VisitorControl>();
                    if (visitorData != null)
                    {
                        if (visitorData.request != Specialty.None)
                        {
                            if (visitorData.request == tower.specialty || visitorData.request == Specialty.Love)
                            {
                                //Debug.Log("Pontos de " + tower.specialty + " subiu!");
                                GameManager.Instance.SetSatisfaction(this.tower.specialty, 1);
                                SoundManager.Instance.PlaySFX("Vanish");
                                GameManager.Instance.visitorsSatisfied++;
                            }
                            else
                            {
                                //Debug.Log("Pontos de " + visitorData.request + " caiu!");
                                GameManager.Instance.SetSatisfaction(visitorData.request, -1);
                                SoundManager.Instance.PlaySFX("Error");
                            }
                        }
                        else
                        {
                            //Quando não atribuir pontos
                            //SoundManager.Instance.PlaySFX("Pick");
                        }
                        visitorData.spawnedPoint.gameObject.SetActive(true);
                        VisitorSpawner visitorSpawner = visitorData.transform.parent.gameObject.GetComponent<VisitorSpawner>();
                        visitorSpawner.CheckAvailablePoints();

                        //Remove o objeto da lista com todos os visitantes do GameManager
                        GameManager.Instance.allVisitors.Remove(visitorData.gameObject);
                        //Debug.Log(GameManager.Instance.allVisitors.Count);
                        //foreach (GameObject visit in GameManager.Instance.allVisitors)
                        //{
                        //    Debug.Log(visit.gameObject.name + " está na lista.");
                        //}


                        Destroy(visitorData.gameObject);
                    }
                }
                else
                {
                    Draggable draggable = eventData.pointerDrag.gameObject.GetComponent<Draggable>();
                    if (draggable != null)
                    {
                        draggable.ReturnCard();
                        return;
                    }
                }
            }
            else
            {
                Draggable draggable = eventData.pointerDrag.gameObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    draggable.ReturnCard();
                    return;
                }
            }
        }
        else if (gameObject.CompareTag("garbageCan"))
        {            
            if (eventData.pointerDrag.gameObject.tag == "garbageCard")
            {
                SoundManager.Instance.PlaySFX("Vanish");
                GameManager.Instance.SetSatisfaction(Specialty.Cleanliness, +1);
                GameManager.Instance.garbageCleaned++;
                Destroy(eventData.pointerDrag.gameObject);
            }
            else
            {
                Draggable draggable = eventData.pointerDrag.gameObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    draggable.ReturnCard();
                    return;
                }
            }
        }
        else if (gameObject.CompareTag("IemanjaStatue"))
        {
            if (eventData.pointerDrag.gameObject.tag == "offeringCard")
            {
                SoundManager.Instance.PlaySFX("Vanish");
                GameManager.Instance.manifestationCostDown++;
                GameManager.Instance.offeringsMade++;
                Destroy(eventData.pointerDrag.gameObject);
            }
            else
            {
                Draggable draggable = eventData.pointerDrag.gameObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    draggable.ReturnCard();
                    return;
                }
            }
        }
        else
        {
            Draggable draggable = eventData.pointerDrag.gameObject.GetComponent<Draggable>();
            if (draggable != null)
            {
                draggable.ReturnCard();
                return;
            }
        }
    }

    //OnPointerEnter e OnPointerExit podem ser usados para chamar eventos quando o mouse entrar ou sair dá área do objeto com esse script
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            dragObject = eventData.pointerDrag.gameObject;
            if (dragObject != null)
            {
                Draggable draggable = dragObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    draggable.isInteracting = true;
                }
            }
        }

        if (isOccupied)
        {
            if (gameObject.CompareTag("towerArea"))
            {
                //Liga a imagem do range da torre
                newTower.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            dragObject = eventData.pointerDrag.gameObject;
            if (dragObject != null)
            {
                Draggable draggable = dragObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    draggable.isInteracting = false;
                }
            }
        }

        if (isOccupied)
        {
            if (gameObject.CompareTag("towerArea"))
            {
                //Desliga a imagem do range da torre
                newTower.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //if (eventData.pointerDrag == null)
        //{
        //    return;
        //}
        //draggable = eventData.pointerDrag.GetComponent<Draggable>();
        //if (draggable != null && draggable.placeholderParent == this.transform)
        //{
        //    draggable.placeholderParent = draggable.parentToReturn;
        //}
    }

    public void BuildTower(Tower tower)
    {
        SoundManager.Instance.PlaySFX("Install");
        SoundManager.Instance.PlaySFX("Build");

        //Apaga imagem do chão
        buildMark.SetActive(false);

        newTower = Instantiate(towerPrefab, this.gameObject.transform, true);
        newTower.GetComponent<SpriteRenderer>().sprite = tower.sprite;
        newTower.GetComponent<Animator>().runtimeAnimatorController = tower.animatorController;
        towerControl = newTower.GetComponentInChildren<TowerControl>();
        //towerControl.towerPanel = this.towerPanel;

        //Passa o towerControl para o TowerPanel de cada botão e chama a função que seta cada um
        //towerPanel.SetTowerButtons(this.towerControl);
        TowerPanel[] towerPanels = this.towerPanel.transform.parent.transform.parent.GetComponentsInChildren<TowerPanel>();
        foreach (TowerPanel twrPnl in towerPanels)
        {
            twrPnl.mySpecialty = tower.specialty;
            twrPnl.SetTowerButtons(this.towerControl);
            if (twrPnl.gameObject.CompareTag("repairTowerBtn"))
            {
                //Manda o towerPanel do botão de Reparar para o TowerControl
                towerControl.towerPanel = twrPnl;
            }
            else if (twrPnl.gameObject.CompareTag("charmBtn"))
            {
                towerControl.SetCharmReferences(twrPnl.gameObject);
            }
        }
        //Debug.Log("Tower Painels tem " + towerPanels.Length);


        newTower.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+1.3f, this.transform.position.z);
        isOccupied = true;

        //Ativa o componente Button no objeto do Drop Zone para ser possível ligar
        Button button = this.gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.enabled = true;
        }

        //Adiciona o objeto para lista com todos as torres do GameManager
        GameManager.Instance.allTowers.Add(newTower.gameObject);

        //Deixo a cor do sprite transparente para que seja possível ver a torre atrás mas ainda bloqueia raycast
        //GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }
}
