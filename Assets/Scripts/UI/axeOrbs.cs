using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class axeOrbs : Singleton<axeOrbs>
{
    [Header("Values")]
    private int CurrentOrbs; //Orbes cheios atualmente
    public int currentOrbs
    {
        get
        {
            return CurrentOrbs;
        }
        set
        {
            CurrentOrbs = value;
            UpdateOrbsSprites();
            UpdateValueText();
        }
    }
    public int maxOrbs; //Máximo de orbes permitidos
    public float axeFill; //Valor atual da barra de axé
    public float axeLimit; //Valor limite da barra de axé
    public float fillingSpeed; //velocidade de preenchimento da barra de axé
    public float timeBonus; //Multiplicador de axe bônus pelo tempo restante antes de chamar a próxima horda


    [Header("Graphics")]
    public Image[] orbs;
    public Sprite emptyOrb;
    public Sprite fullOrb;
    public Image fillBar;

    [Header("Text")]
    public Text axeValueText;

    void Start()
    {
        //Valores iniciais
        maxOrbs = 10;
        currentOrbs = 2; //Og = 2
        axeLimit = 5f;
        axeFill = 0;
        fillingSpeed = 1f; //Og = 1f
        fillBar.fillAmount = 0;
        timeBonus = 1.5f;
    }

    void Update()
    {
        if (currentOrbs < maxOrbs && !GameManager.Instance.isPaused)
        {
            //enche a barra de axé
            FillAxeOrbs();
        }
    }

    public void UpdateValueText()
    {
        axeValueText.text = currentOrbs.ToString("0");
    }

    //função que enche a barra de axé
    public void FillAxeOrbs()
    {
        if (currentOrbs < maxOrbs)
        {
            axeFill += fillingSpeed * Time.deltaTime;
            float currentAxePercent = axeFill / axeLimit;
            fillBar.fillAmount = currentAxePercent;
            if (axeFill >= axeLimit && currentOrbs < maxOrbs)
            {
                float surplus = axeFill - axeLimit;
                SoundManager.Instance.PlaySFX("Score");
                currentOrbs++;
                if (currentOrbs < maxOrbs)
                {
                    axeFill = surplus;
                    currentAxePercent = axeFill / axeLimit;
                    fillBar.fillAmount = currentAxePercent;
                }
                else
                {
                    axeFill = 0;
                    fillBar.fillAmount = 0;
                }
            }
        }
    }

    public void UpdateOrbsSprites()
    {
        for (int i = 0; i < orbs.Length; i++)
        {
            if (i < currentOrbs)
            {
                orbs[i].sprite = fullOrb;
            }
            else
            {
                orbs[i].sprite = emptyOrb;
            }
        }        
    }

    //Acréscimo bônus de axé por chamar a próxima horda mais cedo
    public void AxeBonus(float remainingTime)
    {
        if (currentOrbs < maxOrbs)
        {
            axeFill += remainingTime * timeBonus;
        }
    }

    //Faz a subtração do custo da carta dos seus orbes
    public bool BuyTower(int axeCost)
    {
        bool canBuy;
        if (currentOrbs >= axeCost)
        {
            currentOrbs -= axeCost;
            canBuy = true;
        }
        else
        {
            canBuy = false;
        }
        return canBuy;
    }

}