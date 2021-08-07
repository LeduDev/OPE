using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ConfirmationPopUp : MonoBehaviour
{
    [SerializeField] Button confirmButton;
    [SerializeField] Text confirmBtnTxt;
    [SerializeField] Button denyButton;
    [SerializeField] Text denyBtnTxt;
    [SerializeField] Text popUpText;

    public void InitPopup(Transform canvas, string popupMessage, Action action)
    {
        popUpText.text = popupMessage;

        if (Settings.language == Settings.Languages.Portuguese)
        {
            confirmBtnTxt.text = "SIM";
            denyBtnTxt.text = "NÃO";
        }
        else if (Settings.language == Settings.Languages.English)
        {
            confirmBtnTxt.text = "YES";
            denyBtnTxt.text = "NO";
        }

        transform.SetParent(canvas);
        transform.localScale = Vector3.one;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        denyButton.onClick.AddListener(()=> {
            SoundManager.Instance.PlaySFX("Drop");
            GameObject.Destroy(this.gameObject);
        });

        confirmButton.onClick.AddListener(() => {
            action();
            SoundManager.Instance.PlaySFX("Click");
            GameObject.Destroy(this.gameObject);
        });
    }

}


