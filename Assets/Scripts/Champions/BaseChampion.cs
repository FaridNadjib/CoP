using UnityEngine;

/// <summary>
/// This class represents the champion base stats.
/// </summary>
[CreateAssetMenu(menuName = "Create BaseChampion")]
public class BaseChampion : ScriptableObject
{
    #region Base stats.

    [Header("Base Stats:")]
    [SerializeField] private string championName;

    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private Sprite championSprite;

    [Header("Hp and energy:")]
    [SerializeField] private float maxHealthBase;

    [SerializeField] private float healthRecoveryBase;
    [SerializeField] private int healthGrowRate;

    [SerializeField] private float maxEnergyBase;
    [SerializeField] private float energyRecoveryBase;
    [SerializeField] private int energyGrowRate;

    [Header("Speed and Devotion:")]
    [SerializeField] private int initiativeBase;

    [SerializeField] private Starsign[] possibleDevotions;

    [Header("Block and Critical:")]
    [SerializeField] private float evasionChanceBase;

    [SerializeField] private float dmgReductionBase;
    [SerializeField] private float criticalChanceBase;
    [SerializeField] private float criticalMultBase;

    #endregion Base stats.

    #region Resistances

    [Header("Resistances:")]
    [SerializeField] private ResistanceBonus[] resistancesBase;

    #endregion Resistances

    #region Defenses

    [Header("Defenses:")]
    [SerializeField] private DefenseBonus[] defenseMainBase;
    [SerializeField] private DefenseBonus[] defenseSubBase;

    #endregion Defenses

    #region Mightlevels

    [Header("Mighlevelcaps needed to evolve:")]
    [SerializeField] private MightCrystalLevel[] crystalLevelRequirements;
    [SerializeField] private MightCrystalLevel[] crystalLevelBoni;

    #endregion Mightlevels

    [Header("Possible Evolutions:")]
    [SerializeField] private BaseChampion[] possibleEvolutions;

    [SerializeField] private SpecialUpgradeChances[] specialUpgradeTypes;


    [SerializeField] WeaponType[] usableWeapons;

    #region Properties

    public string ChampionName { get => championName; }
    public string Title { get => title; }
    public string Description { get => description; }
    public Sprite ChampionSprite { get => championSprite; }
    public float MaxHealthBase { get => maxHealthBase; }
    public float HealthRecoveryBase { get => healthRecoveryBase; }
    public int HealthGrowRate { get => healthGrowRate; }
    public float MaxEnergyBase { get => maxEnergyBase; }
    public float EnergyRecoveryBase { get => energyRecoveryBase; }
    public int EnergyGrowRate { get => energyGrowRate; }
    public int InitiativeBase { get => initiativeBase; }
    public Starsign[] PossibleDevotions { get => possibleDevotions; }
    public float EvasionChanceBase { get => evasionChanceBase; }
    public float DmgReductionBase { get => dmgReductionBase; }
    public float CriticalChanceBase { get => criticalChanceBase; }
    public float CriticalMultBase { get => criticalMultBase; }
    public ResistanceBonus[] ResistancesBase { get => resistancesBase; }
    public DefenseBonus[] DefenseMainBase { get => defenseMainBase; }
    public DefenseBonus[] DefenseSubBase { get => defenseSubBase; }
    public MightCrystalLevel[] CrystalLevelRequirements { get => crystalLevelRequirements; }
    public MightCrystalLevel[] CrystalLevelBoni { get => crystalLevelBoni; }
    public BaseChampion[] PossibleEvolutions { get => possibleEvolutions; }
    public SpecialUpgradeChances[] SpecialUpgradeTypes { get => specialUpgradeTypes; set => specialUpgradeTypes = value; }
    public WeaponType[] UsableWeapons { get => usableWeapons; set => usableWeapons = value; }

    #endregion Properties
}

/// <summary>
/// Used fpr crystal levels.
/// </summary>
[System.Serializable]
public struct MightCrystalLevel
{
    [SerializeField] private Crystal mightCrystal;
    [SerializeField] private int amount;

    public Crystal MightCrystal { get => mightCrystal; }
    public int Amount { get => amount; set => amount = value; }
}