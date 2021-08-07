using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPick : MonoBehaviour
{
    //Define se é possível pegar a carta
    private void OnMouseEnter()
    {
        if (gameObject.CompareTag("cardTrack"))
        {
            GameManager.canPickCard = true;
        }
        else
        {
            GameManager.canPickCard = false;
        }
    }
}
