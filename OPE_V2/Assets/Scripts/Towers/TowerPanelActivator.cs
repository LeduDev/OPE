using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPanelActivator : MonoBehaviour
{
    public GameObject towerPanel;

    // Start is called before the first frame update
    void Start()
    {
        towerPanel = gameObject.transform.GetChild(1).gameObject;
    }

    //Função para ligar o painel com os botões das instalações
    public void ShowTowerPanel()
    {
        if (towerPanel != null)
        {
            if (!towerPanel.gameObject.activeInHierarchy)
            {
                towerPanel.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(towerPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
            }
        }
    }
}
