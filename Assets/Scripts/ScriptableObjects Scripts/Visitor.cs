using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Visitor", menuName ="New Visitor")]
public class Visitor : ScriptableObject
{
    [Header("Visitor Information")]
    public string visitorName;

    [Header("Visuals")]
    public Sprite sprite;
    public RuntimeAnimatorController visitorAnimControl;
}
