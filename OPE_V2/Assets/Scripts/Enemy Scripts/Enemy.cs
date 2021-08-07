using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBehavior { Direct, Destructive }

[CreateAssetMenu(fileName ="New Enemy", menuName = "New Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Attributes")]
    public string enemyName;
    public float life;
    public float power;
    public float resistance;
    public float movSpeed;
    public float atkSpeed;
    public EnemyBehavior enemyBehavior;
    public int attackChance; //Chance em porcentagem (de 1 a 100) do inimigo atacar a torre próxima a ele/ela

    [Header("Graphics")]
    public Sprite sprite;
    public RuntimeAnimatorController movAnimController;
    public RuntimeAnimatorController atkAnimController;
}
