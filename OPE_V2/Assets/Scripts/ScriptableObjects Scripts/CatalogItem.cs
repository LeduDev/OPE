using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Catalog Item", menuName = "New Catalog Item")]
public class CatalogItem : ScriptableObject
{
    public Sprite itemSprite;
    //índice 0 = Pt Br e 1 = En
    public string[] itemName = new string[2];
    public string[] itemDescription = new string[2];
    public string[] howToUnlock = new string[2];

    public bool unlocked = false;
}
