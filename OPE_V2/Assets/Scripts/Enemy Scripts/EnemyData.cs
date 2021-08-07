using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyData : MonoBehaviour
{
    [Header("Enemy Data SO")]
    public Enemy enemy;

    [Header("Current Attributes")]
    [SerializeField]
    private float currentLife;
    public float CurrentLife
    {
        get
        {
            return currentLife;
        }
        set
        {
            currentLife = value;
            UpdateLifeBar();

        }
    }
    public float currentMovSpeed;
    public float currentResistance;
    public float currentAtkSpeed;
    public float currentPower;

    [Header("Lifebar References")]
    //Todas as referências foram feitas através do editor
    public GameObject lifeBarPrefab;
    public Image enemyLifeMask;
    public Image enemyLifeFill;
    public Gradient lifeGradient;
    public bool lifebarIsOn;
    public bool hideLifebar; //Hide lifebar when exiting tower range?

    private void Start()
    {
        lifebarIsOn = false;
        hideLifebar = true;
        lifeBarPrefab = this.gameObject.transform.GetChild(0).gameObject;
        if (lifeBarPrefab != null)
        {
            lifeBarPrefab.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        if (enemy != null)
        {
            GetComponent<SpriteRenderer>().sprite = enemy.sprite;
            GetComponent<Animator>().runtimeAnimatorController = enemy.movAnimController;
            CurrentLife = enemy.life;
            currentMovSpeed = enemy.movSpeed;
            currentResistance = enemy.resistance;
            currentAtkSpeed = enemy.atkSpeed;
            currentPower = enemy.power;
        }
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        enemyMovement.Animate(enemyMovement.waypointTarget);
    }

    //Acerta a barra de vida dos inimigos com seus respectivos valores e cores
    public void UpdateLifeBar()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            //Debug.Log(canvas.name);
            Image lifeMask = canvas.gameObject.transform.GetComponentInChildren<Mask>().gameObject.GetComponent<Image>();
            Image lifeFill = lifeMask.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
            float lifeValue = CurrentLife / enemy.life;
            lifeMask.fillAmount = lifeValue;
            lifeFill.color = lifeGradient.Evaluate(CurrentLife / enemy.life);
        }
        else
        {
            //Debug.Log("Não existe canvas aqui.");
        }

        //float lifeValue = CurrentLife / enemy.life;
        //this.enemyLifeMask.fillAmount = lifeValue;

        //enemyLifeFill.color = lifeGradient.Evaluate(CurrentLife / enemy.life);
    }

    public float TakeDamage(float damageAmount)
    {
        if (!this.lifeBarPrefab.activeInHierarchy)
        {
            this.lifeBarPrefab.SetActive(true);
        }

        CurrentLife -= damageAmount;

        if (CurrentLife < 0)
        {
            CurrentLife = 0;
        }
        if (CurrentLife > enemy.life)
        {
            CurrentLife = enemy.life;
        }

        if (CurrentLife <= 0)
        {
            WaveManager.Instance.enemiesDefeated += 1;
            SoundManager.Instance.PlaySFX("Conscious");
            GameManager.Instance.enemiesDefeated++;

            //Remove o objeto do inimigo da lista com todos os inimigos do Game Manager
            GameManager.Instance.allEnemies.Remove(this.gameObject);

            //Debug.Log(GameManager.Instance.allEnemies.Count);
            //foreach (GameObject enem in GameManager.Instance.allEnemies)
            //{
            //    Debug.Log(enem.gameObject.name + " está na lista.");
            //}

            Destroy(gameObject);
        }
        return this.CurrentLife;
    }

    public void ShowLifebar()
    {
        if (!lifebarIsOn)
        {
            lifeBarPrefab.SetActive(true);
            lifebarIsOn = true;

            Debug.Log("Ligou a barra");
        }
        else
        {
            hideLifebar = false;

            Debug.Log("Já está ligada");
        }
    }

    public void HideLifeBar()
    {
        //if (hideLifebar)
        //{
        //    lifeBarPrefab.SetActive(false);
        //    lifebarIsOn = false;    
        //}
        //else
        //{
        //    hideLifebar = true;
        //}

        if (this.lifeBarPrefab.activeInHierarchy)
        {
            this.lifeBarPrefab.SetActive(false);
        }
    }

}
