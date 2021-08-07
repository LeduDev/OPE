using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerControl : MonoBehaviour
{
    private bool isReady = false; //A isntalação já está pronta (true se acabou a animação dela caindo e ainda não foi destruída)
    public bool IsReady
    {
        get
        {
            return isReady;
        }
        set
        {
            isReady = value;
            SetIsReadySettings();
        }
    }

    [SerializeField]
    private Image[] lifeSquares; //Preenchido manualmente através editor da Unity
    [SerializeField]
    private Image[] energySquares; //Preenchido manualmente através editor da Unity
    Animator myAnimator;
    DropZone dropZone;
    RangeTower rangeTower;
    public TowerPanel towerPanel;
    Button towerButton; //Botão que liga o painel da torre
    private bool charmActive = false; //Charme está "ligado"?
    public bool CharmActive
    {
        get
        {
            return charmActive;
        }
        set
        {
            charmActive = value;
            CheckCharmBonuses();
        }
    }
    public bool isAttacking = false; //Está atacando algum inimigo?

    [Header("Tower Data")]
    public Tower towerSO;
    public float currentPower;
    private float resistance; 
    //Durabilidade é a "vida" da torre
    private float currentDurability;
    public float CurrentDurability
    {
        get
        {
            return currentDurability;
        }
        set
        {
            currentDurability = value;
            UpdateDurabilityBar();
        }
    }
    private float maxEnergy;
    //Energia é como a "mana" da torre, consumida para ativar o charme
    private float currentEnergy;
    public float CurrentEnergy
    {
        get
        {
            return currentEnergy;
        }
        set
        {
            currentEnergy = value;
            UpdateEnergyBar();
        }
    }
    private float repairAmount;
    private int repairCost;


    [Header("Tower HUD")]
    public Image durabilityFill;
    public Image energyFill;
    public GameObject towerBarsContainer;
    [SerializeField]
    private Button panelToggleButton; //Botão que ativa/desativa painel de opções da torre
    public GameObject panelContainer;
    public GameObject towerNameTextContainer;

    [Header("Charm Settings")]
    private GameObject charmButtonGO; //Objeto do botão de charme desta torre, declarado no DropZone
    private Button charmButton;
    private Image charmBtnImg;
    private float powerBonus = 4f; //Qtd de poder da torre multiplicado durante charme da instalação de ALIMENTAÇÃO
    private float resistanceBonus = 0.5f; //Qtd de resistência da torre aumentada durante charme da instalação de FÉ
    public float slowAmount; //Qtd de velocidade (movimentação e ataque) reduzido do inimigo durante charme da instalação de RECREAÇÃO

    private void Awake()
    {
        rangeTower = gameObject.GetComponentInChildren<RangeTower>(true);

        panelToggleButton = this.transform.GetComponentInParent<Button>();
        if (panelToggleButton != null)
        {
            panelContainer = panelToggleButton.gameObject.transform.GetChild(1).gameObject;
            if (panelContainer != null)
            {
                panelToggleButton.onClick.AddListener(this.ShowTowerPanel);
            }
        }
    }

    void Start()
    {
        StartCoroutine(MakeTowerReady());
        myAnimator = this.gameObject.GetComponent<Animator>();

        //Busca scriptable Object da torre no script de Drop
        dropZone = GetComponentInParent<DropZone>();
        if (dropZone != null)
        {
            towerSO = dropZone.tower;
            towerButton = dropZone.gameObject.GetComponent<Button>();
        }
        if (towerSO != null)
        {
            CurrentDurability = towerSO.life;
            repairCost = towerSO.cost;
            repairAmount = towerSO.life / 2;
            currentPower = towerSO.power;
            if (towerNameTextContainer != null)
            {
                Text nameTxt = towerNameTextContainer.GetComponent<Text>();
                if (nameTxt != null)
                {
                    nameTxt.text = towerSO.towerName[(int)Settings.language];
                }
            }
            if (towerSO.specialty != Specialty.Recreation)
            {
                towerBarsContainer.transform.position = new Vector3(towerBarsContainer.transform.position.x, towerBarsContainer.transform.position.y + 0.8f, towerBarsContainer.transform.position.z);
            }
        }
        resistance = 0;
        maxEnergy = 5f;
        CurrentEnergy = 0;
        //CurrentEnergy = maxEnergy;
    }

    private void Update()
    {
        if (isReady)
        {
            if (CharmActive == true)
            {
                if (CurrentEnergy > 0)
                {
                    CurrentEnergy -= 0.2f * Time.deltaTime;
                }
                else
                {
                    if (this.isAttacking == false && !this.panelContainer.activeInHierarchy)
                    {
                        this.towerBarsContainer.SetActive(false);
                    }
                    CharmActive = false;
                }
            }
        }
    }

    public IEnumerator MakeTowerReady()
    {
        yield return new WaitForSeconds(0.4f);
        CameraManager.Instance.ShakeCamera();
        yield return new WaitForSeconds(0.3f);
        this.IsReady = true;
    }

    public bool isTowerReady()
    {
        return this.IsReady;
    }

    public void SetIsReadySettings()
    {
        if (IsReady)
        {
            if (rangeTower != null)
            {
                rangeTower.gameObject.SetActive(true);
            }
        }
        else
        {
            rangeTower.gameObject.SetActive(false);
        }
    }

    public void UpdateDurabilityBar()
    {
        float durabilityValue = CurrentDurability;

        for (int i = 0; i < lifeSquares.Length; i++)
        {
            int squareMinValue = i * 1;
            int squareMaxValue = (i + 1) * 1;

            if (durabilityValue <= squareMinValue)
            {
                lifeSquares[i].fillAmount = 0f;
            }
            else
            {
                if (durabilityValue >= squareMaxValue)
                {
                    lifeSquares[i].fillAmount = 1f;
                }
                else
                {
                    float lifeFillValue = (float)(durabilityValue - squareMinValue) / 1;
                    lifeSquares[i].fillAmount = lifeFillValue;
                }
            }
        }
    }

    public void UpdateEnergyBar()
    {
        //float energyValue = CurrentEnergy / maxEnergy;
        //energyFill.fillAmount = energyValue;

        for (int i = 0; i < energySquares.Length; i++)
        {
            int squareMinValue = i * 1;
            int squareMaxValue = (i + 1) * 1;

            if (CurrentEnergy <= squareMinValue)
            {
                energySquares[i].fillAmount = 0f;
            }
            else
            {
                if (CurrentEnergy >= squareMaxValue)
                {
                    energySquares[i].fillAmount = 1f;
                }
                else
                {
                    float energyFillValue = (float)(CurrentEnergy - squareMinValue) / 1;
                    energySquares[i].fillAmount = energyFillValue;
                }
            }
        }


        //Define se pode ou não usar o charme
        //Button charmButton = charmButtonGO.GetComponent<Button>();
        //Image charmBtnImg = charmButtonGO.GetComponent<Image>();
        if (charmButton != null && charmBtnImg != null)
        {
            if (CurrentEnergy <= 0 && charmButton.enabled)
            {
                charmBtnImg.color = new Color32(100, 100, 100, 255);
                charmButton.enabled = false;
            }
            else if (CurrentEnergy > 0 && !charmButton.enabled)
            {
                charmBtnImg.color = new Color32(255, 255, 255, 255);
                charmButton.enabled = true;
            }
        }
    }

    public void SetCharmReferences(GameObject charmBtnObj)
    {
        charmButtonGO = charmBtnObj;
        if (charmButtonGO != null)
        {
            charmButton = charmButtonGO.GetComponent<Button>();
            charmBtnImg = charmButtonGO.GetComponent<Image>();
        }
    }

    public void CheckCharmBonuses()
    {
        if (charmActive) //Ativar efeito baseado especialidade da torre
        {
            //ativar animação lv2
            if (myAnimator != null)
            {
                myAnimator.SetBool("charmON", true);
            }

            switch (towerSO.specialty)
            {
                case Specialty.Food:
                    if (currentPower == towerSO.power)
                    {
                        currentPower = currentPower * powerBonus;
                    }
                    break;
                case Specialty.Faith:
                    if (resistance == 0)
                    {
                        resistance += resistanceBonus;
                    }
                    break;
                case Specialty.Recreation:
                    rangeTower.SlowAllEnemies(charmActive);
                    break;
                default:
                    break;
            }
        }
        else //Desativar efeito baseado especialidade da torre
        {
            //desativar animação lv2
            if (myAnimator != null)
            {
                myAnimator.SetBool("charmON", false);
            }

            switch (towerSO.specialty)
            {
                case Specialty.Food:
                    currentPower = towerSO.power;
                    break;
                case Specialty.Faith:
                    resistance = 0;
                    break;
                case Specialty.Recreation:
                    rangeTower.SlowAllEnemies(charmActive);
                    break;
                default:
                    break;
            }
        }
    }

    public float DamageTower(float damageAmount, float damageSpeed)
    {
        float realDamage = damageAmount - resistance;
        CurrentDurability -= realDamage * damageSpeed * Time.deltaTime;

        if (CurrentDurability > towerSO.life)
        {
            CurrentDurability = towerSO.life;
        }

        if (CurrentDurability <= 0)
        {
            //Som quando inimigos destroem a torre
            SoundManager.Instance.PlaySFX("Lose");
            StartCoroutine(RemoveTower());
            return CurrentDurability;
        }

        return CurrentDurability;
    }

    public void CallRemoveTowerCoroutine()
    {
        //Som da remoção da torre
        SoundManager.Instance.PlaySFX("Drop");
 
        StartCoroutine(this.RemoveTower());
    }

    public IEnumerator RemoveTower()
    {
        IsReady = false;
        towerBarsContainer.gameObject.SetActive(false);

        //Desliga o painel de botões
        if (towerPanel != null)
        {
            towerPanel.CanRepair = true;
            towerPanel.ClosePanel();
        }

        //Desabilita o botão de ligar o painel da torre
        if (towerButton != null)
        {
            towerButton.enabled = false;
        }

        //Desliga a HUD do alvo que estava sendo atacado
        rangeTower.DeactivateTargetHUD();

        //Se foi removido por ter sido destruída por inimigos
        if (CurrentDurability <= 0f)
        {
            myAnimator.SetTrigger("destroyTower");
            yield return new WaitForSeconds(1);
        }

        this.CharmActive = false;

        CurrentDurability = 0;

        dropZone.buildMark.SetActive(true);
        dropZone.isOccupied = false;
        //Remove o objeto do inimigo da lista com todos os inimigos do Game Manager
        GameManager.Instance.allTowers.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public float SetTowerEnergy(float energyAmount)
    {
        CurrentEnergy += energyAmount;

        if (CurrentEnergy < 0)
        {
            CurrentEnergy = 0;
        }
        if (CurrentEnergy > maxEnergy)
        {
            CurrentEnergy = maxEnergy;
        }

        return CurrentEnergy;
    }

    public void RepairTower()
    {

        if (axeOrbs.Instance.BuyTower(repairCost))
        {
            //Mudar para som de cura depois
            SoundManager.Instance.PlaySFX("Repair");
            CurrentDurability += repairAmount;
            towerPanel.CanRepair = false;
        }
        else
        {
            //Mudar para som ruim depois
            SoundManager.Instance.PlaySFX("Error");
        }
    }

    public void ShowTowerPanel()
    {
        if (this.IsReady == true)
        {
            if (panelContainer != null)
            {
                if (!panelContainer.gameObject.activeInHierarchy)
                {
                    this.towerBarsContainer.SetActive(true);
                    this.towerNameTextContainer.gameObject.SetActive(true);
                    panelContainer.gameObject.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(panelContainer.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
                }
            }
        }
    }


    public void ToggleCharm()
    {
        if (this.CurrentEnergy > 0)
        {
            if (CharmActive)
            {
                //Som de desativação do charme
                SoundManager.Instance.PlaySFX("Drop");
                if (!this.isAttacking && !this.towerBarsContainer.activeInHierarchy)
                {
                    this.towerBarsContainer.SetActive(false);
                }
                CharmActive = false;
            }
            else
            {
                //Som de ativação do charme
                SoundManager.Instance.PlaySFX("Charm");
                this.towerBarsContainer.SetActive(true);
                CharmActive = true;
            }
        }        
    }
}
