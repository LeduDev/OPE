using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardTag
{
    buildingCard, garbageCard, offeringCard 
};

[CreateAssetMenu(fileName ="New Card", menuName ="New Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public CardTag cardTag;
    public Tower cardTower;
    public Sprite cardSprite;
    public int dropRate;
}
