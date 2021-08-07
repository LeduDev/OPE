using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject optionsMenu, creditsScreen, tutorialScreen, aboutScreen, catalogCount, catalogInterface, tutorialCount, tutorialInterface, mainMenu;

    [SerializeField]
    private GameObject prevBtnCata, nextBtnCata, prevBtnTuto, nextBtnTuto;

    [SerializeField]
    private GameObject[] catalogScreens;
    //private Text catalogCountTxt;

    [SerializeField]
    private GameObject[] tutorialScreens;

    //Referente ao menu de catálogo
    private int catalogIndex;
    public int CatalogIndex
    {
        get
        {
            return catalogIndex;
        }
        set
        {
            catalogIndex = value;
            catalogCount.GetComponent<Text>().text = string.Format("CATÁLOGO ") + (CatalogIndex + 1).ToString("0") + string.Format("/") + catalogScreens.Length.ToString("0");
        }
    }

    //Refrente ao menu de como jogar
    private int tutorialIndex;
    public int TutorialIndex
    {
        get
        {
            return tutorialIndex;
        }
        set
        {
            tutorialIndex = value;
            tutorialCount.GetComponent<Text>().text = string.Format("COMO JOGAR ") + (TutorialIndex + 1).ToString("0") + string.Format("/") + tutorialScreens.Length.ToString("0");
        }
    }

    //FUNÇÕES DE CONTROLE DE MENUS/INTERFACE

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {

        SceneManager.LoadScene(1);
    }

    public void ShowOptionsMenu()
    {
        ShowMain(false);
        optionsMenu.SetActive(true);
    }
    public void HideOptionsMenu()
    {
        optionsMenu.SetActive(false);
        ShowMain(true);
    }

    public void ShowCredits()
    {
        ShowMain(false);        
        creditsScreen.SetActive(true);
    }

    public void HideCredits()
    {
        creditsScreen.SetActive(false);
        ShowMain(true);
    }

    public void ShowTutorial()
    {
        ShowMain(false);
        tutorialScreen.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorialScreen.SetActive(false);
        ShowMain(true);
    }

    public void ShowAbout()
    {
        ShowMain(false);
        aboutScreen.SetActive(true);
    }

    public void HideAbout()
    {
        aboutScreen.SetActive(false);
        ShowMain(true);
    }

    //Esconder ou mostrar opções do menu principal
    public void ShowMain(bool show)
    {
        mainMenu.SetActive(show);
    }

    public void ShowCatalog()
    {
        ShowMain(false);
        CatalogIndex = 0;
        catalogInterface.SetActive(true);
        prevBtnCata.SetActive(false);
        if (catalogScreens.Length > 1)
        {
            nextBtnCata.SetActive(true);
        }
        else
        {
            nextBtnCata.SetActive(false);
        }
        catalogScreens[0].SetActive(true);
        catalogCount.SetActive(true);
    }

    public void HideCatalog()
    {
        catalogInterface.SetActive(false);
        catalogScreens[CatalogIndex].SetActive(false);
        catalogCount.SetActive(false);
        CatalogIndex = 0;
        ShowMain(true);
    }

    public void NextCatalogScreen()
    {
        if (CatalogIndex + 1 < catalogScreens.Length)
        {        
            catalogScreens[CatalogIndex].SetActive(false);
            CatalogIndex++;
            catalogScreens[CatalogIndex].SetActive(true);
            if (CatalogIndex + 1 > 1)
            {
                prevBtnCata.SetActive(true);
            }
            if (CatalogIndex + 1 >= catalogScreens.Length)
            {
                nextBtnCata.SetActive(false);
            }
            //Tira a cor do "pressed"
            DisableSelectedButton();
        }
    }

    public void PreviousCatalogScreen()
    {
        if (CatalogIndex + 1 > 1)
        {            
            catalogScreens[CatalogIndex].SetActive(false);
            CatalogIndex--;
            catalogScreens[CatalogIndex].SetActive(true);
            if (CatalogIndex + 1 <= 1)
            {
                prevBtnCata.SetActive(false);
            }
            if (CatalogIndex + 1 < catalogScreens.Length)
            {
                nextBtnCata.SetActive(true);
            }
            //Tira a cor do "pressed"
            DisableSelectedButton();
        }
    }

    public void ShowHowToPlay()
    {
        ShowMain(false);
        TutorialIndex = 0;
        tutorialInterface.SetActive(true);
        prevBtnTuto.SetActive(false);
        if (tutorialScreens.Length > 1)
        {
            nextBtnTuto.SetActive(true);
        }
        else
        {
            nextBtnTuto.SetActive(false);
        }
        tutorialScreens[0].SetActive(true);
        tutorialCount.SetActive(true);
    }

    public void HideHowToPlay()
    {
        tutorialInterface.SetActive(false);
        tutorialScreens[TutorialIndex].SetActive(false);
        tutorialCount.SetActive(false);
        TutorialIndex = 0;
        ShowMain(true);
    }

    public void NextTutorialScreen()
    {
        if (TutorialIndex + 1 < tutorialScreens.Length)
        {
            tutorialScreens[TutorialIndex].SetActive(false);
            TutorialIndex++;
            tutorialScreens[TutorialIndex].SetActive(true);
            if (TutorialIndex + 1 > 1)
            {
                prevBtnTuto.SetActive(true);
            }
            if (TutorialIndex + 1 >= tutorialScreens.Length)
            {
                nextBtnTuto.SetActive(false);
            }
            //Tira a cor do "pressed"
            DisableSelectedButton();
        }
    }

    public void PreviousTutorialScreen()
    {
        if (TutorialIndex + 1 > 1)
        {
            tutorialScreens[TutorialIndex].SetActive(false);
            TutorialIndex--;
            tutorialScreens[TutorialIndex].SetActive(true);

            if (TutorialIndex + 1 <= 1)
            {
                prevBtnTuto.SetActive(false);
            }
            if (TutorialIndex + 1 < tutorialScreens.Length)
            {
                nextBtnTuto.SetActive(true);
            }
            //Tira a cor do "pressed"
            DisableSelectedButton();
        }
    }

    //Remove o último objeto selecionado do canvas. Usarei nos métodos dos botões para que não permaneçam na cor "pressed" depois de pressionado
    public void DisableSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
