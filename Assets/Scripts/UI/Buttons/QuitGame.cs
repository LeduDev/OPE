using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class QuitGame : MonoBehaviour
{
    //Texto de índice 0 = PT e 1 = EN
    [SerializeField] string[] popUpMessages = new string[2];

    // Start is called before the first frame update
    void Start()
    {
        Action action = () =>
        {
            Application.Quit();
        };

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            ConfirmationPopUp popUp = ScreenManager.Instance.CreatePopUp();
            //Iniciar PopUp
            popUp.InitPopup(ScreenManager.Instance.OverlayCanvas, popUpMessages[(int)Settings.language], action);
        });
    }
}
