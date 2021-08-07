using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Specialty {None, Food, Faith, Recreation, Cleanliness, Love, Offering} //None is for empty areas for construction. Love is for Imenajá's Intervention Skill.

[CreateAssetMenu(fileName = "New Tower", menuName ="New Tower")]
public class Tower : ScriptableObject
{
    [Header("Tower Attributes")]
    //0 = Pt | 1 = En
    public string[] towerName = new string[2];
    public Specialty specialty;
    public int cost;
    public float power;
    public float life;
    public float range;
    public float energy;

    [Header("Tower Display")]
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;

    [Header("Tower Card")]
    public Sprite cardSprite;
}
