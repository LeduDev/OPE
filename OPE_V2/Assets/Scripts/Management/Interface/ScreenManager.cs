using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ScreenManager : Singleton<ScreenManager>
{
    private static ScreenManager _instance;
    public static ScreenManager Instance { get { return _instance; } }

    public static readonly int mainMenuIndex = 0; //Build Index da cena Main Menu

    [Header("Canvas")]
    public Transform OverlayCanvas;

    [Header("Main Menu")]
    public GameObject preMenuScreen;
    public GameObject mainMenuButtons;
    public GameObject languagesScreen;
    public GameObject logoIemanja;
    [SerializeField] private GameObject[] hideInCredits; //Esse objetos serão escondidos na tela de créditos e devem voltar ao normal ao fechá-la
    [SerializeField] private GameObject[] showInCredits; //Esse objetos serão mostrados somente na tela de créditos e devem ser escondidos ao fechá-la


    [Header("Results Screen")]
    public GameObject resultsScreen;
    public Text winOrLoseText;
    public Text offeringPointsTxt;
    public Text enemiesPointsTxt;
    public Text garbagePointsTxt;
    public Text visitorsPointsTxt;
    public Text statuePointsTxt;
    public string[] ptResults;
    public string[] enResults;
    public Text[] resultsTaskTxt;
    public Text finalScorePtsTxt;
    public Text bestScorePtsTxt;

    [Header("SubMenu Screens")]
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private Text displayTitle;
    [SerializeField]
    private GameObject optionsDisplay;
    [SerializeField]
    private GameObject pauseScreenBG;
    private GameObject lastActiveDisplay; //A última subtela de display ativa dentro da tela de pausa (ex: catálogo, opções, etc.)
    [SerializeField] private GameObject CreditsDisplay;
    [SerializeField] private GameObject pauseScreenButtons;

    [Header("Catalog Data")]
    [SerializeField]
    private GameObject catalogDisplay;
    [SerializeField]
    private Text catalogPageTxt;
    [SerializeField] private Transform catalogPagePos;
    [SerializeField] private Transform catalogLastPagePos;
    [SerializeField]
    private GameObject nextCatalogPageBtn;
    [SerializeField]
    private GameObject prevCatalogPageBtn;
    [SerializeField]
    private GameObject[] catalogItemsInterfaces = new GameObject[3];
    [SerializeField]
    private CatalogItem[] catalogItems;
    public string[] catalogPagesTitles = new string[5]; //Guarda o título de cada página do catálogo e também a quantidade de páginas através do seu tamanho
    public string[] catalogTitlesPt = new string[5];
    public string[] catalogTitlesEn = new string[5];
    [SerializeField] private GameObject[] pauseBtns; //Todos os botões do pausa exceto o continuar/resume
    private int catalogPageIndex; //ìndice da página atual
    public int CatalogPageIndex 
    {
        get
        {
            return catalogPageIndex;
        }
        set
        {
            catalogPageIndex = value;
            UpdateCatalog();
        }
    }

    [Header("Tutorial Data")]
    [SerializeField]
    private GameObject tutorialDisplay;
    [SerializeField]
    private Image tutorialImage;
    [SerializeField]
    private Text tutorialPageTxt;
    [SerializeField]
    private Text tutorialTitleTxt;
    [SerializeField]
    private Text tutorialBodyTxt;
    [SerializeField]
    private GameObject nextTutoPageBtn;
    [SerializeField]
    private GameObject prevTutoPageBtn;
    [SerializeField]
    private TutorialPage[] tutorialPages;
    private int tutorialPageIndex; //ìndice da página atual
    public int TutorialPageIndex
    {
        get
        {
            return tutorialPageIndex;
        }
        set
        {
            tutorialPageIndex = value;
            UpdateTutorial();
        }
    }

    [Header("Visitors Satisfaction")]
    public GameObject visitorsMetersContainer;
    public Image foodMeterFill;
    public Image faithMeterFill;
    public Image recreationMeterFill;
    public Image cleanMeterFill; //cleanliness

    [Header("Overall Satisfaction")]
    public Image overallLv0Fill;
    public Image overallLv1Fill;
    public Image overallLv2Fill;
    public Image overallLv3Fill;
    public Text overallBarsCountTxt;
    public Image[] overallLvMetersArray;
    public Image overallBarsBorders;
    public Image fadePanel;
    public GameObject iemanjaSatisfaction;

    [Header("Iemanjá Data")]
    [SerializeField] private Image[] lifeSquares; //Preenchido manualmente através editor da Unity

    [Header("Others")]
    [SerializeField] private AudioSource myAudio;
    [SerializeField] private GameObject cinematicScreen;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }

    private void Start()
    {
        overallLvMetersArray = new Image[]
        {
            overallLv0Fill,
            overallLv1Fill,
            overallLv2Fill,
            overallLv3Fill
        };

        myAudio = GameObject.Find("SoundManager").GetComponent<AudioSource>();

        if (languagesScreen != null)
        {
            if (GameManager.showLanguagesScreen == true)
            {
                languagesScreen.gameObject.SetActive(true);
                if (myAudio != null)
                {
                    myAudio.Pause();
                }
            }
        }

        CheckAvailableItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > mainMenuIndex)
        {
            //Perde se a vida de Iemanjá zerar
            if (GameManager.Life <= 0 && !GameManager.Instance.isPaused)
            {
                GameManager.Instance.youLose = true;
                GameOver();
            }
        }
    }

    private void CheckAvailableItems()
    {
        for (int i = 0; i < catalogItems.Length; i++)
        {
            if (PlayerPrefs.HasKey(catalogItems[i].name))
            {
                catalogItems[i].unlocked = PlayerPrefs.GetInt(catalogItems[i].name) == 1 ? true : false;
            }
            else
            {
                catalogItems[i].unlocked = false;
            }
        }
    }

    public void UnlockItem(int itemIndex)
    {
        if (itemIndex < catalogItems.Length)
        {
            if (PlayerPrefs.HasKey(catalogItems[itemIndex].name) == false)
            {
                bool unlock = true;
                catalogItems[itemIndex].unlocked = unlock;
                PlayerPrefs.SetInt(catalogItems[itemIndex].name, unlock ? 1 : 0);
                GameManager.Instance.itemsUnlocked++;
            }
        }
    }

    public void UnlockAllItems()
    {
        for (int i = 0; i < catalogItems.Length; i++)
        {
            UnlockItem(i);
        }
    }

    public void DeleteAllItems()
    {
        for (int i = 0; i < catalogItems.Length; i++)
        {
            if (PlayerPrefs.HasKey(catalogItems[i].name) == true)
            {
                PlayerPrefs.DeleteKey(catalogItems[i].name);
            }
        }
    }


    public void DeleteUnlockedItem(int itemID)
    {
        if (itemID < catalogItems.Length)
        {
            if (PlayerPrefs.HasKey(catalogItems[itemID].name) == true)
            {
                PlayerPrefs.DeleteKey(catalogItems[itemID].name);
                Debug.Log("Apagou o save do item " + catalogItems[itemID].name);
            }
        }
    }

    public void FromPreToMainMenu()
    {
        preMenuScreen.gameObject.SetActive(false);
        logoIemanja.gameObject.SetActive(true);
        mainMenuButtons.gameObject.SetActive(true);
    }

    public void CloseLanguagesScreen()
    {
        GameManager.showLanguagesScreen = false;
        myAudio.Play();
        cinematicScreen.gameObject.SetActive(true);
        languagesScreen.gameObject.SetActive(false);
    }

    //Cria o pop-up de confirmação
    public ConfirmationPopUp CreatePopUp()
    {
        GameObject popUpGO = Instantiate(Resources.Load("UI/ConfirmationScreenBG") as GameObject);
        return popUpGO.GetComponent<ConfirmationPopUp>();
    }

    //Começa a partida
    public void StartLevel()
    {
        //lastActiveDisplay = null; //Evita um erro de stack overflow
        SceneManager.LoadScene("Level1");
    }


    //Termina a partida e informa se foi vitória ou derrota
    public void GameOver()
    {
        GameManager.Instance.isPaused = true;

        SoundManager.Instance.musicSource.Stop();

        iemanjaSatisfaction.gameObject.SetActive(false);

        if (!GameManager.Instance.gameOver)
        {
            GameManager.Instance.gameOver = true;
        }

        GameManager.Instance.CalculateFinalScore();
        SetResultsScreenTexts();

        resultsScreen.gameObject.SetActive(true);
        fadePanel.gameObject.SetActive(false);

        GameManager.Instance.CheckUnlockRequirements();

        //Chama o pop-up informando que há novos itens no catálogo quando isso for verdade
        if (GameManager.Instance.itemsUnlocked > 0)
        {
            SoundManager.Instance.PlaySFX("Unlock");
            string[] newItemsMessage = new string[2];
            newItemsMessage[0] = "Você desbloqueou " + GameManager.Instance.itemsUnlocked + " novo(s) item(s) no catálogo. Gostaria de dar uma olhada nele(s) agora mesmo?";
            newItemsMessage[1] = "You have unlocked " + GameManager.Instance.itemsUnlocked + " new item(s) in the catalog. Would you like to take a look at them right now?";

            Action action = () =>
            {
                for (int i = 0; i < pauseBtns.Length; i++)
                {
                    pauseBtns[i].gameObject.SetActive(false);
                }
                TogglePauseScreen();
            };
            ConfirmationPopUp popUp = CreatePopUp();
            //Iniciar PopUp
            popUp.InitPopup(OverlayCanvas, newItemsMessage[(int)Settings.language], action);

            GameManager.Instance.itemsUnlocked = 0;
        }
    }

    public void SetResultsScreenTexts()
    {
        if (GameManager.Life <= 0 || GameManager.Instance.youLose == true)
        {
            //Perdeu
            SoundManager.Instance.PlaySFX("Lose");
            if (Settings.language == Settings.Languages.English)
            {
                winOrLoseText.text = "DEFEAT";
            }
            else if (Settings.language == Settings.Languages.Portuguese)
            {
                winOrLoseText.text = "DERROTA";
            }
        }
        else if (GameManager.Life > 0)
        {
            //Venceu
            SoundManager.Instance.PlaySFX("Win");
            if (Settings.language == Settings.Languages.English)
            {
                winOrLoseText.text = "VICTORY";
            }
            else if (Settings.language == Settings.Languages.Portuguese)
            {
                winOrLoseText.text = "VITÓRIA";
            }
        }

        offeringPointsTxt.text = GameManager.Instance.offeringsMade.ToString();
        enemiesPointsTxt.text = GameManager.Instance.enemiesDefeated.ToString();
        garbagePointsTxt.text = GameManager.Instance.garbageCleaned.ToString();
        visitorsPointsTxt.text = GameManager.Instance.visitorsSatisfied.ToString();
        statuePointsTxt.text = GameManager.Instance.statueDamage.ToString() + "%";
        finalScorePtsTxt.text = GameManager.Instance.finalScore.ToString("0.0");
        bestScorePtsTxt.text = PlayerPrefs.GetFloat("bestScore").ToString("0.0");
    }

    //Quando perdi pela barra de satisfação de Iemanjá
    public IEnumerator FadeScreen()
    {
        GameManager.Instance.isPaused = true;
        SoundManager.Instance.musicSource.Stop();
        if (!GameManager.Instance.gameOver)
        {
            GameManager.Instance.gameOver = true;
        }

        fadePanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        GameOver();
    }

    //Recomeça a fase
    public void RestartLevel()
    {
        //Caso o jogo tenha sido pausado retorna ao normal
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void TogglePauseScreen()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (lastActiveDisplay == CreditsDisplay)
            {
                //Mostra o que foi escondido na tela de créditos
                for (int i = 0; i < hideInCredits.Length; i++)
                {
                    hideInCredits[i].gameObject.SetActive(true);
                }

                //Esconde o que só deve aparecer na tela de créditos
                for (int i = 0; i < showInCredits.Length; i++)
                {
                    showInCredits[i].gameObject.SetActive(false);
                }

                if (pauseScreenButtons != null)
                {
                    pauseScreenButtons.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleRight;
                }
            }
        }

        //ativa se estiver desativado e desativa se estiver ativado
        pauseScreenBG.SetActive(!pauseScreenBG.activeSelf);
        pauseScreen.SetActive(!pauseScreen.activeSelf);

        if (pauseScreen.activeInHierarchy)
        {
            //Tela principal que sempre aparece ao abrir o pause
            ShowCatalogDisplay();

            Time.timeScale = 0;
            GameManager.Instance.isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            GameManager.Instance.isPaused = false;
            lastActiveDisplay.SetActive(false);
        }
    }

    public void ShowCreditsDisplay()
    {
        if (!optionsDisplay.gameObject.activeInHierarchy)
        {
            //Liga a tela de pause caso seja chamado fora da função TogglePauseScreen
            if (!pauseScreen.activeInHierarchy)
            {
                TogglePauseScreen();
            }

            if (lastActiveDisplay != null)
            {
                lastActiveDisplay.gameObject.SetActive(false);
            }

            //Coloca o título do "submenu" sendo exibido baseado na língua selecionada
            if (Settings.language == Settings.Languages.Portuguese)
            {
                displayTitle.text = "SOBRE O JOGO";
            }
            else if (Settings.language == Settings.Languages.English)
            {
                displayTitle.text = "ABOUT THE GAME";
            }

            //Esconde o que não deve aparecer na tela de créditos
            for (int i = 0; i < hideInCredits.Length; i++)
            {
                hideInCredits[i].gameObject.SetActive(false);
            }

            //Mostra o que só deve aparecer na tela de créditos
            for (int i = 0; i < showInCredits.Length; i++)
            {
                showInCredits[i].gameObject.SetActive(true);
            }


            if (pauseScreenButtons != null)
            {
                pauseScreenButtons.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
            }

            CreditsDisplay.SetActive(true);
            lastActiveDisplay = CreditsDisplay.gameObject;
        }
    }


    public void ShowConfigDisplay()
    {
        if (!optionsDisplay.gameObject.activeInHierarchy)
        {
            //Liga a tela de pause caso seja chamado fora da função TogglePauseScreen
            if (!pauseScreen.activeInHierarchy)
            {
                TogglePauseScreen();
            }

            if (lastActiveDisplay != null)
            {
                lastActiveDisplay.gameObject.SetActive(false);
            }

            //Coloca o título do "submenu" sendo exibido baseado na língua selecionada
            if (Settings.language == Settings.Languages.Portuguese)
            {
                displayTitle.text = "CONFIGURAÇÕES";
            }
            else if (Settings.language == Settings.Languages.English)
            {
                displayTitle.text = "SETTINGS";
            }

            optionsDisplay.SetActive(true);
            lastActiveDisplay = optionsDisplay.gameObject;
        }
    }

    public void ShowCatalogDisplay()
    {
        if (!catalogDisplay.gameObject.activeInHierarchy)
        {
            //Liga a tela de pause caso seja chamado fora da função TogglePauseScreen
            if (!pauseScreen.activeInHierarchy)
            {
                TogglePauseScreen();
            }

            if (lastActiveDisplay != null)
            {
                lastActiveDisplay.gameObject.SetActive(false);
            }

            CatalogPageIndex = 0;
            LoadCatalogPagesTitles();
            UpdateCatalog();

            //Coloca o título do "submenu" sendo exibido baseado na língua selecionada
            if (Settings.language == Settings.Languages.Portuguese)
            {
                displayTitle.text = "CATÁLOGO";
            }
            else if (Settings.language == Settings.Languages.English)
            {
                displayTitle.text = "CATALOG";
            }

            catalogDisplay.SetActive(true);
            lastActiveDisplay = catalogDisplay.gameObject;
        }
    }

    public void UpdateCatalog()
    {
        if (CatalogPageIndex < catalogPagesTitles.Length - 1)
        {
            nextCatalogPageBtn.gameObject.SetActive(true);
        }
        else if (CatalogPageIndex >= catalogPagesTitles.Length - 1)
        {
            nextCatalogPageBtn.gameObject.SetActive(false);
        }

        if (CatalogPageIndex == 0)
        {
            prevCatalogPageBtn.gameObject.SetActive(false);
        }
        else if (CatalogPageIndex >= 0)
        {
            prevCatalogPageBtn.gameObject.SetActive(true);
        }

        catalogPageTxt.text = catalogPagesTitles[CatalogPageIndex] + " " + (CatalogPageIndex + 1).ToString() + "/" + catalogPagesTitles.Length.ToString();

        //Se está na última página, que possui apenas 2 itens, ajustar a posição do título da página
        if (CatalogPageIndex == catalogPagesTitles.Length - 1)
        {
            catalogPageTxt.gameObject.transform.parent.gameObject.transform.position = catalogLastPagePos.position;
            //catalogPageContainer.position = new Vector3 (1327f, catalogPageContainer.position.y, catalogPageContainer.position.z);
        }
        else
        {
            catalogPageTxt.gameObject.transform.parent.gameObject.transform.position = catalogPagePos.position;
        }

        for (int i = 0; i < catalogItemsInterfaces.Length; i++)
        {
            if (catalogItems.Length < i + (CatalogPageIndex * catalogItemsInterfaces.Length) + 1)
            {
                catalogItemsInterfaces[i].gameObject.SetActive(false);
            }
            else
            {
                catalogItemsInterfaces[i].gameObject.SetActive(true);
                Image itemImage = catalogItemsInterfaces[i].GetComponentInChildren<Image>();
                GameObject lockedImage = catalogItemsInterfaces[i].GetComponentInChildren<Image>().gameObject.transform.GetChild(0).gameObject;
                Text itemNameTxt = catalogItemsInterfaces[i].transform.GetChild(1).gameObject.GetComponent<Text>();
                Text itemDescTxt = catalogItemsInterfaces[i].transform.GetChild(2).gameObject.GetComponent<Text>();
                itemImage.sprite = catalogItems[i + (CatalogPageIndex * catalogItemsInterfaces.Length)].itemSprite;

                if (catalogItems[i + (CatalogPageIndex * catalogItemsInterfaces.Length)].unlocked == true)
                {
                    itemNameTxt.text = catalogItems[i + (CatalogPageIndex * catalogItemsInterfaces.Length)].itemName[(int) Settings.language];
                    itemDescTxt.text = catalogItems[i + (CatalogPageIndex * catalogItemsInterfaces.Length)].itemDescription[(int)Settings.language];
                    lockedImage.gameObject.SetActive(false);
                }
                else
                {
                    //itemImage.sprite = null;
                    lockedImage.gameObject.SetActive(true);
                    itemDescTxt.text = catalogItems[i + (CatalogPageIndex * catalogItemsInterfaces.Length)].howToUnlock[(int)Settings.language];
                    if (Settings.language == Settings.Languages.Portuguese)
                    {
                        itemNameTxt.text = "ITEM BLOQUEADO";
                        //itemDescTxt.text = "Você deve desbloquear este item atendendo a requisitos especiais.";
                    }
                    else if (Settings.language == Settings.Languages.English)
                    {
                        itemNameTxt.text = "ITEM LOCKED";
                        //itemDescTxt.text = "You have to unlock this item by meeting special requirements.";
                    }
                }
            }
        }
    }

    public void NextCatalogPage()
    {
        CatalogPageIndex++;
    }

    public void PrevCatalogPage()
    {
        CatalogPageIndex--;
    }

    public void ShowTutorialDisplay()
    {
        if (!tutorialDisplay.gameObject.activeInHierarchy)
        {
            //Liga a tela de pause caso seja chamado fora da função TogglePauseScreen
            if (!pauseScreen.activeInHierarchy)
            {
                TogglePauseScreen();
            }

            if (lastActiveDisplay != null)
            {
                lastActiveDisplay.gameObject.SetActive(false);
            }

            TutorialPageIndex = 0;
            //UpdateTutorial();

            //Coloca o título do "submenu" sendo exibido baseado na língua selecionada
            if (Settings.language == Settings.Languages.Portuguese)
            {
                displayTitle.text = "TUTORIAL";
            }
            else if (Settings.language == Settings.Languages.English)
            {
                displayTitle.text = "TUTORIAL";
            }

            tutorialDisplay.SetActive(true);
            lastActiveDisplay = tutorialDisplay.gameObject;
        }
    }


    public void UpdateTutorial()
    {
        if (TutorialPageIndex < tutorialPages.Length - 1)
        {
            nextTutoPageBtn.gameObject.SetActive(true);
        }
        else if (TutorialPageIndex >= tutorialPages.Length - 1)
        {
            nextTutoPageBtn.gameObject.SetActive(false);
        }

        if (TutorialPageIndex == 0)
        {
            prevTutoPageBtn.gameObject.SetActive(false);
        }
        else if (TutorialPageIndex >= 0)
        {
            prevTutoPageBtn.gameObject.SetActive(true);
        }

        tutorialPageTxt.text = (TutorialPageIndex + 1).ToString() + "/" + tutorialPages.Length.ToString();
        tutorialImage.sprite = tutorialPages[TutorialPageIndex].tutorialImage;
        tutorialTitleTxt.text = tutorialPages[TutorialPageIndex].tutorialTitle[(int)Settings.language];
        tutorialBodyTxt.text = tutorialPages[TutorialPageIndex].tutorialInstructions[(int)Settings.language];
    }

    public void NextTutorialPage()
    {
        TutorialPageIndex++;
    }

    public void PrevTutorialPage()
    {
        TutorialPageIndex--;
    }

    public void LoadCatalogPagesTitles()
    {
        if (Settings.language == Settings.Languages.Portuguese)
        {
            for (int i = 0; i < catalogPagesTitles.Length; i++)
            {
                catalogPagesTitles[i] = catalogTitlesPt[i];
            }
        }
        else if (Settings.language == Settings.Languages.English)
        {
            for (int i = 0; i < catalogPagesTitles.Length; i++)
            {
                catalogPagesTitles[i] = catalogTitlesEn[i];
            }
        }
    }

    //IEMANJÁ-DATA-MANAGEMENT-----------------------
    //Define corretamente a vida de Iemanjá
    public void SetLife()
    {
        //float lifeValue = GameManager.Life / GameManager.maxLife;
        //lifeMask.fillAmount = lifeValue;
        //lifeFill.color = lifeGradient.Evaluate(GameManager.Life / GameManager.maxLife);

        //if (GameManager.Life < 0)
        //{
        //    GameManager.Life = 0;
        //}
        //if (GameManager.Life > GameManager.maxLife)
        //{
        //    GameManager.Life = GameManager.maxLife;
        //}

        float lifeValue = GameManager.Life;

        for (int i = 0; i < this.lifeSquares.Length; i++)
        {
            int squareMinValue = i * 1;
            int squareMaxValue = (i + 1) * 1;

            if (lifeValue <= squareMinValue)
            {
                lifeSquares[i].fillAmount = 0f;
            }
            else
            {
                if (lifeValue >= squareMaxValue)
                {
                    lifeSquares[i].fillAmount = 1f;
                }
                else
                {
                    float lifeFillValue = (float)(lifeValue - squareMinValue) / 1;
                    lifeSquares[i].fillAmount = lifeFillValue;
                }
            }
        }

    }

    public void SetSatisfactionMeter(int currentValue, int maxValue, Specialty satisfactionType)
    {
        float fillValue = (float)currentValue / maxValue;

        switch (satisfactionType)
        {
            case Specialty.Food:
                foodMeterFill.fillAmount = fillValue;
                break;
            case Specialty.Faith:
                faithMeterFill.fillAmount = fillValue;
                break;
            case Specialty.Recreation:
                recreationMeterFill.fillAmount = fillValue;
                break;
            case Specialty.Cleanliness:
                cleanMeterFill.fillAmount = fillValue;
                break;
            default:
                break;
        }
    }

    public void UpdateOverallMeter()
    {
        int overallSatisfactionValue = GameManager.Instance.foodSatisfaction + GameManager.Instance.faithSatisfaction + GameManager.Instance.recreationSatisfaction + GameManager.Instance.cleanlinessSatisfaction;
        int lvMetersCount = GameManager.Instance.overallMeterLvAmount;
        int fullBarsAmount = 0;
        GameManager.Instance.overallTotalValue = overallSatisfactionValue;
        if (overallSatisfactionValue > 0)
        {
            overallBarsBorders.color = new Color32(255, 255, 255, 255);
            GameManager.Instance.criticalState = false;
        }
        else
        {
            overallBarsBorders.color = new Color32(255, 100, 100, 255);
            GameManager.Instance.criticalState = true;
        }

        for (int i = 0; i < lvMetersCount; i++)
        {
            //Valor mínimo por cada nível do medidor geral
            int overallLvMeterMin = i * GameManager.Instance.overallMeterMaxPerLv;
            //Valor máximo por cada nível do medidor geral
            int overallLvMeterMax = (i+1) * GameManager.Instance.overallMeterMaxPerLv;

            if (overallSatisfactionValue <= overallLvMeterMin)
            {
                //Quantidade de satisfação geral abaixo do valor mínimo da barra desse nível
                overallLvMetersArray[i].fillAmount = 0f;
            }
            else
            {
                if (overallSatisfactionValue >= overallLvMeterMax)
                {
                    //Quantidade de satisfação geral acima do valor máximo da barra desse nível
                    overallLvMetersArray[i].fillAmount = 1f;
                    fullBarsAmount++;
                }
                else
                {
                    //Quantidade de satisfação geral entre o mínimo e máximo da barra desse nível
                    float overallFillAmount = (float)(overallSatisfactionValue - overallLvMeterMin) / GameManager.Instance.overallMeterMaxPerLv;
                    //Debug.Log(overallFillAmount);
                    overallLvMetersArray[i].fillAmount = overallFillAmount;
                }
            }
        }
        GameManager.Instance.overallMeterFullBars = fullBarsAmount;
        overallBarsCountTxt.text = GameManager.Instance.overallMeterFullBars.ToString();
        //Debug.Log(fullBarsAmount + " barras cheias!");
    }
}
