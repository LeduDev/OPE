using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Animator myAnimator;

    //Script que regula o estado atual do inimigo
    EnemyData enemyData;

    //Quando chega a 0 (zero) o inimigo desfere um ataque
    private float atkCooldown;
    //Tempo de duração até o próximo ataque do inimigo
    private float atkTime = 1f;

    private bool willAttack = false; //Se o inimigo vai atacar a torre próxima

    //Informa se o inimigo está se movendo
    public bool isMoving;
    //Chegou no ponto final? (Estátua de Iemanjá)
    private bool reachedStatue;

    private float waypointMinDist = 0.2f;
    private float towerMinDist = 2f;

    public Transform towerTarget;
    public TowerControl towerControl;
    bool chasingTower;

    public Transform waypointTarget;
    private int waypointIndex = 0;
    public static int pathIndex; //Informa em qual caminho o inimigo vai spawnar e seguir até a estátua
    Waypoints waypointsParent; //GameObject que guarda todos os waypoints de determinado caminho

    [SerializeField]
    private Transform[] waypointsArray;

    private void Awake()
    {
        //Cria e carrega o array com os caminhos
        waypointsArray = new Transform[4];
        waypointsArray[0] = GameObject.Find("LeftSpawnPoint").transform;
        waypointsArray[1] = GameObject.Find("TopSpawnPoint").transform;
        waypointsArray[2] = GameObject.Find("BottomSpawnPoint").transform;
        waypointsArray[3] = GameObject.Find("RightSpawnPoint").transform;

        //Escolhe o caminho randomicamente
        pathIndex = Random.Range(0, waypointsArray.Length);
    }

    void Start()
    {
        //canvasRectTrans = GameObject.Find("Canvas - Camera").GetComponent<RectTransform>();
        //Debug.Log(canvasRectTrans.sizeDelta.x);
        //Debug.Log(canvasRectTrans.sizeDelta.y);

        //if (enemy != null)
        //{
        //    GetComponent<SpriteRenderer>().sprite = enemy.sprite;
        //    myAnimator.runtimeAnimatorController = enemy.movAnimController;
        //}

        reachedStatue = false;
        isMoving = false;

        towerTarget = null;
        towerControl = null;
        chasingTower = false;

        enemyData = GetComponent<EnemyData>();
        myAnimator = gameObject.GetComponent<Animator>();
        waypointsParent = waypointsArray[pathIndex].GetChild(0).GetComponent<Waypoints>();

        waypointTarget = waypointsParent.waypoints[0];
        atkCooldown = atkTime;
        //Animate();

    }

    void Update()
    {
        if (!GameManager.Instance.isPaused && !GameManager.Instance.gameOver)
        {
            if (!reachedStatue)
            {
                if (towerTarget == null)
                {
                    if (isMoving)
                    {
                        MoveToTarget(waypointTarget);
                    }
                    else
                    {
                        chasingTower = false;
                        Animate(waypointTarget);
                        isMoving = true;
                    }
                }
                else if(Vector2.Distance(transform.position, waypointTarget.position) <= Vector3.Distance(transform.position, towerTarget.position))
                {
                    if (isMoving)
                    {
                        MoveToTarget(waypointTarget);
                    }
                    else
                    {
                        chasingTower = false;
                        Animate(waypointTarget);
                        isMoving = true;
                    }
                }
                else
                {
                    if (willAttack == true)
                    {
                        if (isMoving)
                        {
                            MoveToTarget(towerTarget);
                        }
                        else
                        {
                            chasingTower = true;
                            Animate(towerTarget);
                            isMoving = true;
                        }
                    }
                    else
                    {
                        isMoving = false;
                        towerTarget = null;
                    }
                }
            }
            else
            {
                AttackIemanja();
            }

        }
    }

    public void AttackCheck()
    {
        int attackChance = (int) Random.Range(1, 101);
        if (attackChance <= enemyData.enemy.attackChance)
        {
            willAttack = true;
        }
        else
        {
            willAttack = false;
        }
    }

    public void MoveToTarget(Transform target)
    {
        if (chasingTower)
        {
            if (Vector3.Distance(transform.position, target.position) <= towerMinDist)
            {
                if (towerControl != null)
                {
                    if (towerControl.CurrentDurability > 0)
                    {
                        //Ataca a instalação
                        towerControl.DamageTower(enemyData.currentPower, enemyData.currentAtkSpeed);
                    }
                    else
                    {
                        isMoving = false;
                        towerTarget = null;
                    }

                    //if (towerControl.DamageTower(enemyData.currentPower, enemyData.currentAtkSpeed) <= 0f)
                    //{
                    //    //Animate(waypointTarget);
                    //}
                }
                else
                {
                    isMoving = false;
                }
            }
            else
            {
                Vector3 direction = target.position - transform.position;
                transform.Translate(direction.normalized * enemyData.currentMovSpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.position) <= waypointMinDist)
            {
                    GetNextWaypoint();
            }
            else
            {
                Vector3 direction = target.position - transform.position;
                transform.Translate(direction.normalized * enemyData.currentMovSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    //Pega o próximo waypoint
    void GetNextWaypoint()
    {
        if (waypointIndex >= waypointsParent.waypoints.Length - 1)
        {
            if (!reachedStatue)
            {
                myAnimator.SetBool("isAttacking", true);
                reachedStatue = true;
            }            
        }
        else
        {
            waypointIndex++;
            waypointTarget = waypointsParent.waypoints[waypointIndex];
            //Animate(waypointTarget);
        }
        isMoving = false;
    }

    //Indica a posição onde o inimigo será instanciado
    public void SpawnPos()
    {
        transform.position = waypointsArray[pathIndex].position;
    }

    //Causa dano a estátua de Iemanjá
    private void AttackIemanja()
    {
        GameManager.Life -= enemyData.currentPower * enemyData.currentAtkSpeed * Time.deltaTime;

        //Método alternativo de causar dano, deixei comentado porque posso reutilizar no futuro

        //if (atkCooldown > 0)
        //{
        //    atkCooldown -= enemyData.atkSpeed * Time.deltaTime;
        //}
        //else
        //{
        //    GameManager.Life -= enemyData.power;
        //    atkCooldown = atkTime;
        //}
    }

    //Regulador das animações
    public void Animate(Transform destination)
    {
        //Calcula posição em que o inimigo deve se virar
        float distX = destination.position.x - transform.position.x;
        bool negativeX = false;
        float distY = destination.position.y - transform.position.y;
        bool negativeY = false;

        if (distX < 0) //Se for negativo
        {
            negativeX = true;
            distX = distX * (-1);
        }
        else
        {
            negativeX = false;
        }

        if (distY < 0) //Se for negativo
        {
            negativeY = true;
            distY = distY * (-1);
        }
        else
        {
            negativeY = false;
        }


        if (gameObject.transform.position.y > destination.transform.position.y || gameObject.transform.position.y < destination.transform.position.y && gameObject.transform.position.x > destination.transform.position.x || gameObject.transform.position.x < destination.transform.position.x)
        {
            //Está movendo em ambos os eixos simultâneamente
            if (distX > distY)
            {
                //Está movendo majoritariamente no eixo horizontal
                myAnimator.SetInteger("Vertical", 0);
                if (negativeX)
                {
                    myAnimator.SetInteger("Horizontal", -1);
                }
                else
                {
                    myAnimator.SetInteger("Horizontal", 1);
                }
            }
            else
            {
                //Está movendo majoritariamente no eixo vertical
                myAnimator.SetInteger("Horizontal", 0);
                if (negativeY)
                {
                    myAnimator.SetInteger("Vertical", -1);
                }
                else
                {
                    myAnimator.SetInteger("Vertical", 1);
                }
            }
        }
        else if (gameObject.transform.position.y == destination.transform.position.y)
        {
            if (gameObject.transform.position.x > destination.transform.position.x)
            {
                //Está movendo para esquerda
                myAnimator.SetInteger("Horizontal", -1);
                myAnimator.SetInteger("Vertical", 0);
            }
            else if (gameObject.transform.position.x < destination.transform.position.x)
            {
                //Está movendo para direita
                myAnimator.SetInteger("Horizontal", 1);
                myAnimator.SetInteger("Vertical", 0);
            }
        }
        else if (gameObject.transform.position.x == destination.transform.position.x)
        {
            if (gameObject.transform.position.y > destination.transform.position.y)
            {
                //Está movendo para baixo
                myAnimator.SetInteger("Horizontal", 0);
                myAnimator.SetInteger("Vertical", -1);
            }
            else if (gameObject.transform.position.y < destination.transform.position.y)
            {
                //Está movendo para cima
                myAnimator.SetInteger("Horizontal", 0);
                myAnimator.SetInteger("Vertical", 1);
            }
        }
    }

}
