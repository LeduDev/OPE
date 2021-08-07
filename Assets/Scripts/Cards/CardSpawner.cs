using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cardTypes {Tower, Offering, Garbage} //Tipos de cartas: Torre (instalação), Oferenda ou Lixo

public class CardSpawner : MonoBehaviour
{
    [SerializeField]
    private Card[] cards; //Todas as cartas

    public Transform spawnPos; //Transform do objeto q informa o ponto de spawn da carta
    public Transform destroyPos;

    [SerializeField]
    private Transform cardList; //Transform do game object vazio que armazena as cartas

    [SerializeField]
    private GameObject cardToSpawn; //A carta a ser criada
    //private CardData cardData;

    private float spawnTime; //Tempo de intervalo entre a instância de cada carta
    private float currentTime; //Tempo atual

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 2f;
        currentTime = spawnTime;

        //cardData = cardToSpawn.GetComponentInChildren<CardData>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused && !GameManager.Instance.gameOver)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                SpawnCard();
            }
        }
    }

    //Gera uma nova carta aleatóriamente
    private void SpawnCard()
    {
        Card selectedCard = null;

        int cardDrop = Random.Range(1, SumCardsDropRate(cards) + 1);
        for (int i = 0; i < cards.Length; i++)
        {
            if (cardDrop <= cards[i].dropRate)
            {
                selectedCard = cards[i];
                break;
            }
            cardDrop -= cards[i].dropRate;
        }

        CardData cardData = cardToSpawn.GetComponentInChildren<CardData>();
        cardData.card = selectedCard;
        cardData.towerData = selectedCard.cardTower;

        //Adiciona o tipo de carta sorteado a contagem geral de cartas se for do tipo correspondente
        if (cardData.towerData.specialty == Specialty.Cleanliness)
        {
            GameManager.Instance.garbageSpawned++;
        }
        else if (cardData.towerData.specialty == Specialty.Offering)
        {
            GameManager.Instance.offeringsSpawned++;
        }

        GameObject newCard = Instantiate(cardToSpawn, cardList, false);
        CardSlotMov cardSlotMov = newCard.GetComponent<CardSlotMov>();
        cardSlotMov.destroyPoint = destroyPos;
        cardSlotMov.SetCardPosition(spawnPos);
        newCard.GetComponentInChildren<CardData>().SetCardTag();

        //int cardIndex = Random.Range(0, towersSO.Length);
        //CardData cardData = cardToSpawn.GetComponentInChildren<CardData>();
        //cardData.towerData = towersSO[cardIndex];
        //GameObject newCard = Instantiate(cardToSpawn, cardList, false);
        //CardSlotMov cardSlotMov = newCard.GetComponent<CardSlotMov>();
        //cardSlotMov.destroyPoint = destroyPos;
        //cardSlotMov.SetCardPosition(spawnPos);
        //if (cardData.towerData.name == "Garbage")
        //{
        //    newCard.transform.GetChild(0).gameObject.tag = "garbageCard";
        //}
        //if (cardData.towerData.name == "Offering")
        //{
        //    newCard.transform.GetChild(0).gameObject.tag = "offeringCard";
        //}

        //reinicia contador
        currentTime = spawnTime;
    }

    public int SumCardsDropRate(Card[] toSum)
    {
        int dropRateSum = 0;

        foreach (Card card in toSum)
        {
            dropRateSum += card.dropRate;
        }

        return dropRateSum;
    }
}
