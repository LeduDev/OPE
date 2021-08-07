using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tutorial Page", menuName ="New Tutorial Page")]
public class TutorialPage : ScriptableObject
{
    public Sprite tutorialImage;
    //0 = PT-BR | 1 = EN
    public string[] tutorialTitle = new string[2];
    public string[] tutorialInstructions = new string[2];
}
