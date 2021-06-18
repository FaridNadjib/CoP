using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// How good weapons will scale with crystalmightlevel.
/// </summary>
public enum ParameterScale { None, S, A, B, C, D, E }

/// <summary>
/// Scalelevel, crystal type and scalemodifier.
/// </summary>
[System.Serializable]
public struct MightCrystalScale
{
    [SerializeField] Crystal mightCrystal;
    [SerializeField] ParameterScale scale;

    public Crystal MightCrystal { get => mightCrystal;}
    public ParameterScale Scale { get => scale;}
}
/// <summary>
/// How good chance for weapon is to manifest crystals, crystaltype and chance.
/// </summary>
[System.Serializable]
public struct CrystalManifestation
{
    [SerializeField] Crystal mightCrystal;
    [SerializeField, Range(0.0f,1.0f)] float manifestationChance;

    public Crystal MightCrystal { get => mightCrystal; }
    public float ManifestationChance { get => manifestationChance; set => manifestationChance = value; }
}
/// <summary>
/// Weaponeffecttype, type, buff and duration.
/// </summary>
[System.Serializable]
public struct WeaponSpecial
{
    [SerializeField] WeaponEffect effectType;
    [SerializeField] float buff;
    [SerializeField] int duration;

    public WeaponEffect EffectType { get => effectType;}
    public float Buff { get => buff;}
    public int Duration { get => duration;}
}

/// <summary>
/// Weapon buffs for resistance.
/// </summary>
[System.Serializable]
public struct WeaponResistanceBuff
{
    [SerializeField] DamageType resistanceType;
    [SerializeField] float buff;
    [SerializeField] int duration;

    public DamageType ResistanceType { get => resistanceType; set => resistanceType = value; }
    public float Buff { get => buff; }
    public int Duration { get => duration; }
}

/// <summary>
/// This class represents a weapon.
/// </summary>
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
    [SerializeField] int numberOfAttacks;

    [Header("Weapon Specials:")]
    [SerializeField] CrystalManifestation[] crystalManifestations;
    [SerializeField] WeaponSpecial[] weaponSpecials;
    [SerializeField] WeaponResistanceBuff[] resistancesBuffs;
    [SerializeField] SpecialRequirement specialRequirement;
    [SerializeField] float maxSpecialMeter;

    [Header("Effect related:")]
    [SerializeField] BattleEffects battleEffectSelf;
    [SerializeField] BattleEffects battleEffectTarget;
    [SerializeField] bool randomEffectPositions;
    [SerializeField] AudioClip castSound;

    [SerializeField] float startDelay;
    [SerializeField] float attackDelay;
    [SerializeField] float endDelay;

    public WeaponType Type { get => type; }
    public int ItemID { get => itemID; }
    public int NumberOfItems { get => numberOfItems; set => numberOfItems = value; }
    public float MaxSpecialMeter { get => maxSpecialMeter;}
    public WeaponTarget TargetType { get => targetType;}
    public DamageType[] DamageTypes { get => damageTypes;}
    public MightCrystalScale[] ScaleLevels { get => scaleLevels;}
    public float EnergyCost { get => energyCost;}
    public float BaseAttack { get => baseAttack;}
    public int NumberOfAttacks { get => numberOfAttacks;}
    public CrystalManifestation[] CrystalManifestations { get => crystalManifestations; set => crystalManifestations = value; }
    public WeaponSpecial[] WeaponSpecials { get => weaponSpecials;}
    public WeaponResistanceBuff[] ResistancesBuffs { get => resistancesBuffs;}
    public SpecialRequirement SpecialRequirement { get => specialRequirement;}
    public string ItemName { get => itemName; set => itemName = value; }
    public float AttackDelay { get => attackDelay; set => attackDelay = value; }
    public int Price { get => price; set => price = value; }
    public Sprite ItemIcon { get => itemIcon; set => itemIcon = value; }
    public BattleEffects BattleEffectSelf { get => battleEffectSelf; set => battleEffectSelf = value; }
    public BattleEffects BattleEffectTarget { get => battleEffectTarget; set => battleEffectTarget = value; }
    public AudioClip CastSound { get => castSound; set => castSound = value; }
    public float StartDelay { get => startDelay; set => startDelay = value; }
    public bool RandomEffectPositions { get => randomEffectPositions;}
    public float EndDelay { get => endDelay; set => endDelay = value; }
}
