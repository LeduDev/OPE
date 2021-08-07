using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipCinematic : MonoBehaviour
{
    [SerializeField] GameObject cinematicScreen;
    //Texto de índice 0 = PT e 1 = EN
    [SerializeField] string[] popUpMessages = new string[2];

    void Start()
    {

        Action action = () => {
            cinematicScreen.gameObject.SetActive(false);
        };

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            ConfirmationPopUp popUp = ScreenManager.Instance.CreatePopUp();
            //Iniciar PopUp
            popUp.InitPopup(ScreenManager.Instance.OverlayCanvas, popUpMessages[(int)Settings.language], action);
        });
    }
}
