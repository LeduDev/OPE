using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateTower : MonoBehaviour
{
    private SpriteRenderer mySprite;

    [SerializeField]
    private Transform towers;

    [SerializeField]
    private Transform spawnPoint;

    private bool isOccupied = false;
    private bool canBuild = false;

    DragObj towerCard; //informações da carta contidas no script DragObj
    TowerData towerInfo; //informações da torre contidas no script TowerData alocado no GameObject TowerPrefab

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.GetChild(0).GetComponent<Transform>();
        mySprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "buildingCard") //Se colidiu com carta de construção, passa os dados dos scripts
        {
            towerCard = collision.collider.GetComponent<DragObj>();
            //towerInfo = towerCard.towerPrefab.GetComponent<TowerData>();
            towerCard.isColliding = true;
            if (isOccupied == false)
            {
                canBuild = true;
            }
        }
    }

    //Constrói a torre (instalação) se todas as condições foram cumpridas, caso contrário, volta a carta para sua vaga ou desaparece
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "buildingCard")
        {
            if (towerCard.isColliding == true && towerCard.isBeingHeld == false)
            {
                if (canBuild == true)
                {
                    if(axeOrbs.Instance.currentOrbs >= towerInfo.axeCost)
                    {
                        axeOrbs.Instance.BuyTower(towerInfo.axeCost);
                        //Instantiate(towerCard.towerPrefab, spawnPoint.transform.position, Quaternion.identity).transform.SetParent(towers);                    
                        Destroy(collision.gameObject);
                        isOccupied = true;
                        canBuild = false;
                        mySprite.enabled = false;
                        SoundManager.Instance.PlaySFX("Build");
                    }
                    else
                    {
                        towerCard.ReturnCard();
                    }
                }
                else
                {
                    towerCard.ReturnCard();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canBuild = false;
        towerCard.isColliding = false;
        towerCard = null;
        towerInfo = null;
    }
}
