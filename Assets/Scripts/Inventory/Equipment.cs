using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StarSignBonus
{
    [SerializeField] Starsign devotion;
    [SerializeField] int devotionAmount;

    public Starsign Devotion { get => devotion;}
    public int DevotionAmount { get => devotionAmount;}
}
[System.Serializable]
public struct ResistanceBonus
{
    [SerializeField] DamageType resistanceType;
    [SerializeField] float resistanceAmount;

    public DamageType ResistanceType { get => resistanceType; }
    public float ResistanceAmount { get => resistanceAmount; set => resistanceAmount = value; }
}
[System.Serializable]
public struct DefenseBonus
{
    [SerializeField] DefenseType defenseType;
    [SerializeField] float defenseAmount;

    public DefenseType DefenseType { get => defenseType;}
    public float DefenseAmount { get => defenseAmount; set => defenseAmount = value; }
}
[System.Serializable]
public struct StatsBuffBonus
{
    [SerializeField] StatsBuffType statsBuffType;
    [SerializeField] float statsAmount;

    public StatsBuffType StatsBuffType { get => statsBuffType;}
    public float StatsAmount { get => statsAmount; set => statsAmount = value; }
}

[CreateAssetMenu(menuName ="Create Equipment")]
public class Equipment : ScriptableObject
{
    [Header("Basic Values:")]
    [SerializeField] EquipmentType type;
    [SerializeField] string itemName;
    [SerializeField] Sprite itemIcon;
    [SerializeField] int itemID;
    [SerializeField] int price;
    int numberOfItems;

    [Header("Special Values:")]
    [SerializeField] StarSignBonus[] devotionBonus;
    [SerializeField] StatsBuffBonus[] statsBonus;
    [SerializeField] ResistanceBonus[] resistanceBonus;
    [SerializeField] DefenseBonus[] defenseMainBonus;
    [SerializeField] DefenseBonus[] defenseSubBonus;

    public int ItemID { get => itemID;}
    public int NumberOfItems { get => numberOfItems; set => numberOfItems = value; }
    public EquipmentType Type { get => type;}
    public StarSignBonus[] DevotionBonus { get => devotionBonus;}
    public StatsBuffBonus[] StatsBonus { get => statsBonus; set => statsBonus = value; }
    public ResistanceBonus[] ResistanceBonus { get => resistanceBonus; set => resistanceBonus = value; }
    public DefenseBonus[] DefenseMainBonus { get => defenseMainBonus; set => defenseMainBonus = value; }
    public DefenseBonus[] DefenseSubBonus { get => defenseSubBonus; set => defenseSubBonus = value; }
}
