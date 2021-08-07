using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotMov : MonoBehaviour
{
    //velocidade de movimento da carta e seu espaço/vaga (slot)
    private float speed = 150f;
    private Transform thisTransform;
    public Transform destroyPoint; //Local onde a carta deve ser destruída

    void Update()
    {
        if (!GameManager.Instance.isPaused && !GameManager.Instance.gameOver)
        {
            //movimento da carta e vaga para a esquerda
            if (gameObject.transform.position.x > destroyPoint.position.x)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            else
            {
                if (gameObject.transform.childCount > 0)
                {
                    GameObject cardObj = gameObject.transform.GetChild(0).gameObject;
                    if (cardObj != null)
                    {
                        if (cardObj.CompareTag("garbageCard"))
                        {
                            //Inserir som de efeito negativo no futuro
                            SoundManager.Instance.PlaySFX("Error");
                            GameManager.Instance.SetSatisfaction(Specialty.Cleanliness, -1);
                        }
                    }
                }
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCardPosition(Transform spawnPos)
    {
        thisTransform = GetComponent<Transform>();
        thisTransform.position = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z);
    }
}
