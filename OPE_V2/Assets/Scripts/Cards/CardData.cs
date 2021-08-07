using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    public Tower towerData;
    public Text costTxt;
    public Card card;
    [SerializeField]
    private Transform offeringTxtPos; //Posição onde deve aparecer o texto da carta caso seja uma oferenda
    [SerializeField]
    private Sprite[] offeringSprites;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = card.cardName;
        //towerData = card.cardTower;
        if (this.gameObject.CompareTag("offeringCard"))
        {
            if (offeringSprites.Length > 0)
            {
                int spriteId = (int)Random.Range(0, offeringSprites.Length);
                gameObject.GetComponent<Image>().sprite = offeringSprites[spriteId];
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = card.cardSprite;
            }
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = card.cardSprite;
        }

        //GameObject costTextObj = gameObject.transform.GetChild(0).gameObject;
    }

    public void SetCardTag()
    {
        if (this.towerData != null)
        {
            costTxt = gameObject.GetComponentInChildren<Text>();
            if (costTxt != null)
            {
                costTxt.text = towerData.cost.ToString();
            }

            if (this.towerData.specialty == Specialty.Cleanliness)
            {
                this.gameObject.tag = "garbageCard";
            }
            else if (this.towerData.specialty == Specialty.Offering)
            {
                this.gameObject.tag = "offeringCard";
                if (offeringTxtPos != null)
                {
                    costTxt.transform.position = offeringTxtPos.transform.position;
                }
            }
            else
            {
                this.gameObject.tag = "buildingCard";
            }

        }
    }
}
