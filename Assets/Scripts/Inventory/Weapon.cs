using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParameterScale { None, S, A, B, C, D, E }


[System.Serializable]
public struct MightCrystalScale
{
    [SerializeField] Crystals mightCrystal;
    [SerializeField] ParameterScale scale;

    public Crystals MightCrystal { get => mightCrystal;}
    public ParameterScale Scale { get => scale;}
}

[System.Serializable]
public struct CrystalManifestation
{
    [SerializeField] Crystals mightCrystal;
    [SerializeField] float manifestationChance;

    public Crystals MightCrystal { get => mightCrystal; }
    public float ManifestationChance { get => manifestationChance; set => manifestationChance = value; }
}

[System.Serializable]
public struct WeaponSpecial
{
    [SerializeField] WeaponEffect effectType;
    [SerializeField] float buff;
    [SerializeField] float duration;

    public WeaponEffect EffectType { get => effectType;}
    public float Buff { get => buff;}
    public float Duration { get => duration;}
}

[System.Serializable]
public struct WeaponResistanceBuff
{
    [SerializeField] DamageType resistanceType;
    [SerializeField] float buff;
    [SerializeField] float duration;

    public DamageType ResistanceType { get => resistanceType; set => resistanceType = value; }
    public float Buff { get => buff; }
    public float Duration { get => duration; }
}

[CreateAssetMenu(menuName = "Create Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Basic Values:")]
    [SerializeField] string itemName;
    [SerializeField] Sprite itemIcon;
    [SerializeField] int itemID;
    [SerializeField] int price;
    int numberOfItems;

    [Header("Weapon Values:")]
    [SerializeField] WeaponType type;
    [SerializeField] WeaponTarget targetType;
    [SerializeField] DamageType[] damageTypes;
    [SerializeField] MightCrystalScale[] scaleLevels;
    [SerializeField] float energyCost;
    [SerializeField] float baseAttack;

    [Header("Weapon Specials:")]
    [SerializeField] CrystalManifestation[] crystalManifestations;
    [SerializeField] WeaponSpecial[] weaponSpecials;
    [SerializeField] WeaponResistanceBuff[] resistancesBuffs;
    // ToDO Particles and stuff.


    public WeaponType Type { get => type; }
    public int ItemID { get => itemID; }
    public int NumberOfItems { get => numberOfItems; set => numberOfItems = value; }
}
