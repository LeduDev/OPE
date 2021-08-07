using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTower : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyLifeBar; //prefab da barra de vida dos inimigos

    //Declaração para futura referência a classe que contém os dados dos inimigos
    private EnemyData enemyTarget;
    //Cria fila de inimigos que será registrada pela torre
    //private Queue<EnemyData> enemiesQ = new Queue<EnemyData>();

    private List<EnemyData> enemyTargetList;

    DropZone dropZone;
    public Tower tower;
    public TowerControl towerControl;

    private void Awake()
    {
        enemyTargetList = new List<EnemyData>();
    }

    void Start()
    {
        //Busca scriptable Object da torre no script de Drop
        dropZone = GetComponentInParent<DropZone>();
        if (dropZone != null)
        {
            tower = dropZone.tower;
        }

        //Aumenta a imagem do círculo corretamente de acordo com o range da torre ativa
        transform.localScale = new Vector3(tower.range, tower.range, 0);

        towerControl = GetComponentInParent<TowerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (enemyTarget == null)
            {
                if (enemyTargetList.Count > 0)
                {
                    if (towerControl != null)
                    {
                        towerControl.isAttacking = true;
                    }
                    enemyTarget = enemyTargetList[0];
                }
                else
                {
                    if (towerControl != null)
                    {
                        if (towerControl.isAttacking)
                        {
                            towerControl.isAttacking = false;
                        }
                    }
                }

                ////Liga a barra de vida do inimigo alvo
                //enemyTarget.ShowLifebar();
            }

            if (enemyTarget != null)
            {
                Attack();
            }
        }
    }

    //Função que causa dano aos inimigos ao alcance
    private void Attack()
    {
        if (enemyTarget.TakeDamage(towerControl.currentPower * Time.deltaTime) <= 0f)
        {
            enemyTargetList.Remove(enemyTarget);
            enemyTarget = null;
        }
    }

    public void DeactivateTargetHUD()
    {
        if (this.enemyTarget != null)
        {
            enemyTarget.HideLifeBar();
        }
    }

    //Função para acessar veríavel privada enemyTarget (alvo atual da torre)
    public EnemyData GetTowerTarget()
    {
        if (enemyTarget != null)
        {
            return enemyTarget;
        }
        else
        {
            return null;
        }
    }

    public void SlowAllEnemies(bool applyEffect)
    {
        if (enemyTargetList.Count > 0)
        {
            //When applyEffect is true slow enemies, when false, give their speed back
            if (applyEffect)
            {
                //Aplicando efeito ao alvo atual (que está fora da fila)
                //if (enemyTarget != null)
                //{
                //    if (enemyTarget.currentMovSpeed == enemyTarget.enemy.movSpeed)
                //    {
                //        enemyTarget.currentMovSpeed /= 2;
                //    }
                //    if (enemyTarget.currentAtkSpeed == enemyTarget.enemy.atkSpeed)
                //    {
                //        enemyTarget.currentAtkSpeed /= 2;
                //    }
                //}

                //Aplicando efeito aos inimigos aguardando na fila
                if (enemyTargetList.Count > 0)
                {
                    foreach (EnemyData enemyDt in enemyTargetList)
                    {
                        if (enemyDt.currentMovSpeed == enemyDt.enemy.movSpeed)
                        {
                            enemyDt.currentMovSpeed /= 2;
                        }
                        if (enemyDt.currentAtkSpeed == enemyDt.enemy.atkSpeed)
                        {
                            enemyDt.currentAtkSpeed /= 2;
                        }
                    }
                }
            }
            else
            {
                //Retirando efeito do alvo atual (que está fora da fila)
                //if (enemyTarget != null)
                //{
                //    enemyTarget.currentMovSpeed = enemyTarget.enemy.movSpeed;
                //    enemyTarget.currentAtkSpeed = enemyTarget.enemy.atkSpeed;
                //}

                //Retirando efeito dos inimigos aguardando na fila
                if (enemyTargetList.Count > 0)
                {
                    foreach (EnemyData enemyDt in enemyTargetList)
                    {
                        enemyDt.currentMovSpeed = enemyDt.enemy.movSpeed;
                        enemyDt.currentAtkSpeed = enemyDt.enemy.atkSpeed;
                    }
                }
            }
        }
    }

    public void SlowSingleEnemy(EnemyData enemyToSlow, bool applyEffect)
    {
        //When applyEffect is true, slow enemy, when false, give their speed back
        if (applyEffect)
        {
            if (enemyToSlow != null)
            {
                if (enemyToSlow.currentMovSpeed == enemyToSlow.enemy.movSpeed)
                {
                    enemyToSlow.currentMovSpeed /= 2;
                }
                if (enemyToSlow.currentAtkSpeed == enemyToSlow.enemy.atkSpeed)
                {
                    enemyToSlow.currentAtkSpeed /= 2;
                }
            }
        }
        else
        {
            if (enemyToSlow != null)
            {
                enemyToSlow.currentMovSpeed = enemyToSlow.enemy.movSpeed;
                enemyToSlow.currentAtkSpeed = enemyToSlow.enemy.atkSpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D targetCollider)
    {
        if (targetCollider.CompareTag("enemy"))
        {
            EnemyData enemyData = targetCollider.GetComponent<EnemyData>();

            if (towerControl.CharmActive == true)
            {
                SlowSingleEnemy(enemyData, true);
            }

            //Adiciona a fila enemiesQ o componente Enemydata do inimigo colidido com o range
            //enemiesQ.Enqueue(enemyData);

            //Adiciona para a lista o componente enemyData do inimigo que colidiu com o range
            enemyTargetList.Add(enemyData);

            if (!towerControl.towerBarsContainer.activeInHierarchy)
            {
                towerControl.towerBarsContainer.SetActive(true);
            }

            //Se for um inimigo que ataca a torre, passar o script e posição da torre para ele
            if (enemyData.enemy.enemyBehavior == EnemyBehavior.Destructive)
            {
                EnemyMovement enemyMov = enemyData.gameObject.GetComponent<EnemyMovement>();
                if (enemyMov != null)
                {
                    if (enemyMov.towerTarget == null)
                    {
                        enemyMov.AttackCheck();
                        enemyMov.towerControl = this.towerControl;
                        enemyMov.towerTarget = this.gameObject.transform.parent.gameObject.GetComponent<Transform>();
                        enemyMov.isMoving = false;
                    }
                }
            }
        }
    }

    //Retira o inimigo como alvo se ele sai do alcance da torre
    private void OnTriggerExit2D(Collider2D targetCollider)
    {
        if (targetCollider.CompareTag("enemy"))
        {
            //EnemyData colliderEnemyData = targetCollider.gameObject.GetComponent<EnemyData>();
            //if (targetCollider.gameObject.GetInstanceID() == enemyTarget.gameObject.GetInstanceID())
            //{
            //Debug.Log("O ALVO SAIU DO ALCANCE!");

            //Desliga a barra de vida do inimigo alvo
            EnemyData leavingEnemy = targetCollider.gameObject.GetComponent<EnemyData>();
            if (towerControl.CharmActive == true)
            {
                SlowSingleEnemy(leavingEnemy, false);
            }
            leavingEnemy.HideLifeBar();
            enemyTargetList.Remove(leavingEnemy);
            enemyTarget = null;
                       
            //if (leavingEnemy == enemyTarget)
            //{
            //    enemyTarget = null;
            //}

            //if (targetCollider.gameObject.GetComponent<EnemyData>().hideLifebar)
            //{
            //    targetCollider.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //}
            //else
            //{
            //    targetCollider.gameObject.GetComponent<EnemyData>().hideLifebar = true;
            //}

            //enemyTarget = null;

            if (enemyTargetList.Count <= 0 && towerControl.towerBarsContainer.activeInHierarchy && !towerControl.CharmActive && !towerControl.panelContainer.activeInHierarchy)
            {
                towerControl.towerBarsContainer.SetActive(false);
            }
        }
    }
}
