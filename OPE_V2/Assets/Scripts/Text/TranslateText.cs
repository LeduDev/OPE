using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TranslateText : MonoBehaviour
{
    //public Text[] textToTranslate; //Guarda todos os objetos de texto
    //Os Scriptable Objects com os textos em cada língua devem seguir a mesma ordem dos elementos neste array, para isso será listado onde se aplica cada um:

    [Header("Language Data")]
    public LanguageSO portuguese;
    public LanguageSO english;

    [Header("Cinematic Screen")]
    public Text skipBtnTxt;
    public Text clickContinueText;

    [Header("Pre Menu Screen")]
    public Text clickMessage;
    public Text welcomeMessage;

    [Header("Main Menu Topics")]
    public Text playText;
    public Text howToPlayText;
    public Text creditsText;
    public Text aboutGameText;
    public Text catalogText;
    public Text configText;
    public Text quitText;

    [Header("About Game Screen")]
    public Text creditsBodyText;

    [Header("Options Screen")]
    public Text ConfTitleText;
    public Text musicConfText;
    public Text soundConfText;
    public Text chooseLangText;
    public Text configBackText;
    public Text resetGameBtnText;

    [Header("Ingame Pause Buttons Text")]
    public Text resumeBtnTxt;
    public Text catalogBtnTxt;
    public Text tutorialBtnTxt;
    public Text restartBtnTxt;
    public Text mainMenuBtnTxt;
    public Text settingsBtnTxt;

    [Header("Ingame Interfaces")]
    public Text wavesTxt;
    public Text intolerantsTxt;

    [Header("Results Screen")]
    public Text eventResultsTitleTxt;
    public Text offeringsMadeTxt;
    public Text consciousIntolerantsTxt;
    public Text discardedGarbageTxt;
    public Text attendedVisitorsTxt;
    public Text statueDamageTxt;
    public Text finalScoreTxt;
    public Text bestScoreTxt;
    public Text playAgainBtnTxt;
    public Text resultsMainMenuBtnTxt;
    public Text curiosityMessageTxt;

    // Start is called before the first frame update
    void Start()
    {
        if (Settings.language == Settings.Languages.Portuguese)
        {
            Translate(portuguese);
        }
        else if (Settings.language == Settings.Languages.English)
        {
            Translate(english);
        }
        else
        {
            Debug.Log("ERRO: Linguagem não encontrada!");
        }
    }

    //Traduz e adiciona os textos das mensagens de curiosidades da tela de resultados
    public void SetCuriosityText()
    {
        int curiosityIndex = (int)Random.Range(0, portuguese.curiosityMessageTxt.Length);
        if (Settings.language == Settings.Languages.Portuguese)
        {
            curiosityMessageTxt.text = portuguese.curiosityMessageTxt[curiosityIndex];
        }
        else if (Settings.language == Settings.Languages.English)
        {
            curiosityMessageTxt.text = english.curiosityMessageTxt[curiosityIndex];
        }            
    }

    public void Translate(LanguageSO translation)
    {
        if (translation.name == "Portuguese")
        {
            Settings.language = Settings.Languages.Portuguese;
        }
        else
        {
            Settings.language = Settings.Languages.English;
        }
        PlayerPrefs.SetInt("Language", (int)Settings.language);
        //Debug.Log("Linguagem mudou para " + Settings.language.ToString());

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            //Cinematic Screen
            skipBtnTxt.text = translation.skipBtnText;
            clickContinueText.text = translation.clickToContinueMessage;

            //Pre Menu
            clickMessage.text = translation.clickMessage;
            welcomeMessage.text = translation.welcomeMessage;

            //Main Menu Topics
            playText.text = translation.play;
            howToPlayText.text = translation.howToPlay;
            creditsText.text = translation.credits;
            aboutGameText.text = translation.aboutGame;
            catalogText.text = translation.catalog;
            configText.text = translation.configurations;
            quitText.text = translation.quit;

            //About the Game + Credits text
            if (creditsText != null)
            {
                creditsBodyText.text = translation.creditsText;
            }

            //Options Screen
            chooseLangText.text = translation.chooseLangText;
            ConfTitleText.text = translation.ConfTitleText;
            configBackText.text = translation.configBackText;
            resetGameBtnText.text = translation.resetGameBtnText;
        }

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            //pause buttons text
            resumeBtnTxt.text = translation.resumeBtnTxt;
            catalogBtnTxt.text = translation.catalogBtnTxt;
            tutorialBtnTxt.text = translation.tutorialBtnTxt;
            restartBtnTxt.text = translation.restartBtnTxt;
            mainMenuBtnTxt.text = translation.mainMenuBtnTxt;
            settingsBtnTxt.text = translation.settingBtnTxt;

            //Ingame Interfaces
            wavesTxt.text = translation.wavesTxt;
            intolerantsTxt.text = translation.intolerantsTxt;

            //results screen text
            eventResultsTitleTxt.text = translation.eventResultsTitleTxt;
            offeringsMadeTxt.text = translation.offeringsMadeTxt;
            consciousIntolerantsTxt.text = translation.consciousIntolerantsTxt;
            discardedGarbageTxt.text = translation.discardedGarbageTxt;
            attendedVisitorsTxt.text = translation.attendedVisitorsTxt;
            statueDamageTxt.text = translation.statueDamageTxt;
            finalScoreTxt.text = translation.finalScoreTxt;
            bestScoreTxt.text = translation.bestScoreTxt;
            playAgainBtnTxt.text = translation.playAgainTxt;
            resultsMainMenuBtnTxt.text = translation.mainMenuBtnTxt;
        }

        //Options Screen
        musicConfText.text = translation.musicConfText;
        soundConfText.text = translation.soundConfText;
    }
}
