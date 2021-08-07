using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Gerencia os dados vindos dos inimigos que serão exibidos/guardados
public class EnemyUIManager : MonoBehaviour
{
    //Guarda o script com os dados dos inimigos spawnados
    WaveManager waveManager;
    [Header("Game Objects")]
    public GameObject WaveTimerIcon; //Ícone q aparece na UI mostrando o tempo até a próxima wave

    [Header("UI Text")]
    public Text waveCountTxt;
    public Text enemiesCountTxt;
    public Text waveCountdownTxt;

    [Header("Image")]
    //Referente a "barra" do temporizador
    public Image fillBar;


    void Start()
    {
        waveManager = GetComponent<WaveManager>();
        //Carrega as variáveis e atualiza a interface
        waveCountdownTxt.text = Mathf.Ceil(waveManager.waveCountdown).ToString();
        enemiesCountTxt.text = (waveManager.enemiesDefeated/ 2).ToString() + string.Format(" / ") + waveManager.enemiesTotal.ToString();
    }

    //Regula a "barra" do temporizador
    public void SetTimerBar()
    {
        float progress = waveManager.waveCountdown / waveManager.waveCountdownTime;
        fillBar.fillAmount = progress;
        //Arruma tbm o texto com o tempo restante
        waveCountdownTxt.text = Mathf.Ceil(waveManager.waveCountdown).ToString();
    }

    public void UpdateEnemyCountTxt()
    {
        enemiesCountTxt.text = (waveManager.enemiesDefeated).ToString() + string.Format(" / ") + waveManager.enemiesTotal.ToString();
    }

    public void UpdateWaveCountText()
    {
        waveCountTxt.text = waveManager.WaveNumber.ToString("00") + string.Format(" / ") + waveManager.maxWaves.ToString("00");
    }

    //Chamado por um button | Ao clicar chama a próx. wave instantaneamente
    public void CallWave()
    {
        SoundManager.Instance.PlaySFX("Click");

        axeOrbs.Instance.AxeBonus(waveManager.waveCountdown);

        waveManager.waveCountdown = 0;
    }
}
