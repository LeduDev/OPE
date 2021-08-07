using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Language", menuName = "New Laguage Object")]
public class LanguageSO : ScriptableObject
{
    //Cada scriptble object derivado desta classe deve escrever o texto de cada váriavel em sua respectiva língua
    [Header("Cinematic Text")]
    public string skipBtnText;
    public string clickToContinueMessage;

    [Header("Pre Menu Text")]
    public string clickMessage; //Informa ao jogador para apertar para continuar
    public string welcomeMessage; //Mensagem que aparece antes do menu principal

    [Header("Main Menu Topics")]
    public string play;
    public string howToPlay;
    public string credits;
    public string aboutGame;
    public string catalog;
    public string configurations;
    public string quit;

    [Header("About Game Screen")]
    [TextArea(10, 10)]
    public string creditsText;

    [Header("Options Screen")]
    public string ConfTitleText;
    public string musicConfText;
    public string soundConfText;
    public string chooseLangText;
    public string configBackText;
    public string resetGameBtnText;

    [Header("Ingame Pause Buttons")]
    public string resumeBtnTxt;
    public string catalogBtnTxt;
    public string tutorialBtnTxt;
    public string restartBtnTxt;
    public string mainMenuBtnTxt;
    public string settingBtnTxt;

    [Header("Ingame Interfaces")]
    public string wavesTxt;
    public string intolerantsTxt;

    [Header("Results Screen")]
    public string eventResultsTitleTxt;
    public string offeringsMadeTxt;
    public string consciousIntolerantsTxt;
    public string discardedGarbageTxt;
    public string attendedVisitorsTxt;
    public string statueDamageTxt;
    public string finalScoreTxt;
    public string bestScoreTxt;
    public string playAgainTxt;
    public string[] curiosityMessageTxt = new string[8];
}
