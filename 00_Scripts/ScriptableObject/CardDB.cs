using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SelectCard
{
    public CardDB db;
    public int Level;
}

[System.Serializable]
public class StatusEffectClass
{
    public Effect_Status status;
    public float value;
}
public enum CardState { Active, Passive, None }
[CreateAssetMenu(fileName = "Scriptable", menuName = "DB/Card", order = int.MaxValue)]
public class CardDB : ScriptableObject
{
    public string id;
    public string className;
    [Space(20f)]
    [TextArea(1 , 10)]
    public string description;
    public CardState state;

    [Space(20f)]
    [Header("스킬 데이터")]
    public float baseCooldown;
    public float baseDamage;
    public float cooldownPerLevel;
    public float damagePerLevel;

    public List<StatusEffectClass> effects = new List<StatusEffectClass>();
}
