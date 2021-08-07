using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManifestationController : MonoBehaviour
{
    [Header("Manifestation SO")]
    public Manifestation manifestationSO;

    [Header("Manifestation Data")]
    public int cost;
    public float cooldownCounter;

    [Header("UI Elements")]
    public Text costText;
    public Image coloredIcon;
    public Image invalidCover;
    public Image unableIcon;

    // Start is called before the first frame update
    void Start()
    {
        SetManifestationCost();
        cooldownCounter = manifestationSO.cooldown;
    }

    private void Update()
    {
        if (GameManager.Instance.overallMeterFullBars >= manifestationSO.unlockLv)
        {
            unableIcon.gameObject.SetActive(false);
            invalidCover.gameObject.SetActive(false);
            if (cooldownCounter >= manifestationSO.cooldown)
            {
                gameObject.GetComponent<Button>().enabled = true;
                SetManifestationCost();
                costText.enabled = true;
            }
            else if (cooldownCounter < manifestationSO.cooldown)
            {
                cooldownCounter += Time.deltaTime;
                coloredIcon.fillAmount = cooldownCounter / manifestationSO.cooldown;
            }
        }
        else if (GameManager.Instance.overallMeterFullBars < manifestationSO.unlockLv)
        {
            unableIcon.gameObject.SetActive(true);
            invalidCover.gameObject.SetActive(true);
            gameObject.GetComponent<Button>().enabled = false;
            costText.enabled = false;
        }
    }

    public void SetManifestationCost()
    {
        int newCost = manifestationSO.orbCost - GameManager.Instance.manifestationCostDown;
        if (newCost <= 1)
        {
            cost = 1;
        }
        else
        {
            cost = newCost;
        }
        costText.text = cost.ToString();
    }

    public void ActivateManifestation()
    {
        if (cooldownCounter >= manifestationSO.cooldown)
        {
            if (axeOrbs.Instance.BuyTower(cost))
            {
                SoundManager.Instance.PlaySFX("Manifestation");

                //Chama o poder dependendo do efeito dele
                switch (manifestationSO.manifestationEffect)
                {
                    //Se for o de Amor
                    case ManifestationEffects.MotherLove:
                        GameManager.Instance.manifestationUses[0]++;
                        foreach (GameObject visitorObj in GameManager.Instance.allVisitors)
                        {
                            VisitorControl visitorControl = visitorObj.GetComponent<VisitorControl>();
                            if (visitorControl != null)
                            {
                                visitorControl.LoveResquest();
                            }
                        }
                        break;
                    //Se for o de Cura
                    case ManifestationEffects.Heal:
                        GameManager.Instance.manifestationUses[1]++;
                        foreach (GameObject towerObj in GameManager.Instance.allTowers.ToArray())
                        {
                            TowerControl towerCtrl = towerObj.GetComponent<TowerControl>();
                            if (towerCtrl != null)
                            {
                                //Effect
                                towerCtrl.CurrentDurability += 999f;
                            }
                        }
                        break;
                    //Se for o de Obliterar (derrotar os inimigos instantâneamente)
                    case ManifestationEffects.Obliterate:
                        GameManager.Instance.manifestationUses[2]++;
                        foreach (GameObject enemyObj in GameManager.Instance.allEnemies.ToArray())
                        {
                            EnemyData enemyData = enemyObj.GetComponent<EnemyData>();
                            if (enemyData != null)
                            {
                                enemyData.TakeDamage(999f);
                            }
                        }
                        break;
                    default:
                        break;
                }
                GameManager.Instance.manifestationCostDown = 0;
                costText.enabled = false;
                gameObject.GetComponent<Button>().enabled = false;
                cooldownCounter = 0f;
                coloredIcon.fillAmount = cooldownCounter / manifestationSO.cooldown;
            }
            else
            {
                SoundManager.Instance.PlaySFX("Error");
                //Debug.Log("Não tem orbes suficientes.");
                return;
            }
        }
        else
        {
            return;
        }
    }
}
