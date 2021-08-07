using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image mask;
    public Image fillBar;
    //public Color Color;
    public float currentProgress = 100f;
    public float maxProgress = 100f;

    void Update()
    {
        SetProgress();
    }

    //Controle da barra
    public void SetProgress()
    {
        float progress = currentProgress/maxProgress;
        mask.fillAmount = progress;
        SetCorrectProgress();
    }

    //Não permite que a barra ultrapasse os valores min/máx
    public void SetCorrectProgress()
    {
        if (currentProgress <= 0)
        {
            currentProgress = 0;
        }
        if (currentProgress >= maxProgress)
        {
            currentProgress = maxProgress;
        }
    }

}
