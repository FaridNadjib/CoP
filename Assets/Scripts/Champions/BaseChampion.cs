using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create BaseChampion")]
public class BaseChampion : ScriptableObject
{
    [Header("Base Stats:")]
    [SerializeField] string championName;
    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] Sprite championSprite;

    [Header("Hp and energy:")]
    [SerializeField] float maxHealthBase;
    [SerializeField] float healthRecoveryBase;
    [SerializeField] int healthGrowRate;

    [SerializeField] float maxEnergyBase;
    [SerializeField] float energyRecoveryBase;
    [SerializeField] int energyGrowRate;

    [Header("Speed and Devotion:")]
    [SerializeField] int initiativeBase;
    [SerializeField] Starsign[] possibleDevotions;

    [Header("Block and Critical:")]
    [SerializeField] float evasionChanceBase;
    [SerializeField] float dmgReductionBase;
    [SerializeField] float criticalChanceBase;
    [SerializeField] float criticalMultBase;

    #region Resistances
    [Header("Resistances:")]
    [SerializeField] ResistanceBonus[] resistancesBase;
    //[SerializeField] float bladeResBase;
    //[SerializeField] float pierceResBase;
    //[SerializeField] float impactResBase;
    //[SerializeField] float arcaneResBase;
    //[SerializeField] float fireResBase;
    //[SerializeField] float iceResBase;
    //[SerializeField] float thunderResBase;
    //[SerializeField] float stormResBase;
    #endregion

    #region Defenses
    [Header("Defenses:")]
    [SerializeField] DefenseBonus[] defenseMainBase;
    [SerializeField] DefenseBonus[] defenseSubBase;
    //[SerializeField] float pavedDefMainBase;
    //[SerializeField] float pavedDefSubBase;
    //[SerializeField] float desertDefMainBase;
    //[SerializeField] float desertDefSubBase;
    //[SerializeField] float grassDefMainBase;
    //[SerializeField] float grassDefSubBase;
    //[SerializeField] float oceanDefMainBase;
    //[SerializeField] float oceanDefSubBase;
    //[SerializeField] float iceDefMainBase;
    //[SerializeField] float iceDefSubBase;
    //[SerializeField] float forestDefMainBase;
    //[SerializeField] float forestDefSubBase;
    //[SerializeField] float swampDefMainBase;
    //[SerializeField] float swampDefSubBase;
    //[SerializeField] float mountainDefMainBase;
    //[SerializeField] float mountainDefSubBase;
    #endregion

    #region Mightlevels
    [Header("Mighlevelcaps needed to evolve:")]
    [SerializeField] MightCrystalLevel[] crystalLevelRequirements;
    [SerializeField] MightCrystalLevel[] crystalLevelBoni;

    //[SerializeField] int mightLevel;
    //[SerializeField] int alignmentLevel;
    //[SerializeField] int fireLevel;
    //[SerializeField] int iceLevel;
    //[SerializeField] int lightningLevel;
    //[SerializeField] int windLevel;
    //[SerializeField] int destructionLevel;
    //[SerializeField] int holyLevel;
    //[SerializeField] int hunterLevel;
    //[SerializeField] int seadragonLevel;

    //[Header("Mighlevelbonus when evolved:")]
    //[SerializeField] int mightLevelBonus;
    //[SerializeField] int alignmentLevelBonus;
    //[SerializeField] int fireLevelBonus;
    //[SerializeField] int iceLevelBonus;
    //[SerializeField] int lightningLevelBonus;
    //[SerializeField] int windLevelBonus;
    //[SerializeField] int destructionLevelBonus;
    //[SerializeField] int holyLevelBonus;
    //[SerializeField] int hunterLevelBonus;
    //[SerializeField] int seadragonLevelBonus;
    #endregion

    [Header("Possible Evolutions:")]
    [SerializeField] BaseChampion[] possibleEvolutions;

    #region Properties
    public string ChampionName { get => championName;}
    public string Title { get => title;}
    public string Description { get => description;}
    public Sprite ChampionSprite { get => championSprite;}
    public float MaxHealthBase { get => maxHealthBase;}
    public float HealthRecoveryBase { get => healthRecoveryBase;}
    public int HealthGrowRate { get => healthGrowRate;}
    public float MaxEnergyBase { get => maxEnergyBase;}
    public float EnergyRecoveryBase { get => energyRecoveryBase;}
    public int EnergyGrowRate { get => energyGrowRate;}
    public int InitiativeBase { get => initiativeBase;}
    public Starsign[] PossibleDevotions { get => possibleDevotions;}
    public float EvasionChanceBase { get => evasionChanceBase;}
    public float DmgReductionBase { get => dmgReductionBase;}
    public float CriticalChanceBase { get => criticalChanceBase;}
    public float CriticalMultBase { get => criticalMultBase;}
    public ResistanceBonus[] ResistancesBase { get => resistancesBase;}
    public DefenseBonus[] DefenseMainBase { get => defenseMainBase;}
    public DefenseBonus[] DefenseSubBase { get => defenseSubBase;}
    public MightCrystalLevel[] CrystalLevelRequirements { get => crystalLevelRequirements;}
    public MightCrystalLevel[] CrystalLevelBoni { get => crystalLevelBoni;}
    public BaseChampion[] PossibleEvolutions { get => possibleEvolutions;}
    #endregion
}

[System.Serializable]
public struct MightCrystalLevel
{
    [SerializeField] Crystals mightCrystal;
    [SerializeField] int amount;

    public Crystals MightCrystal { get => mightCrystal; }
    public int Amount { get => amount; set => amount = value; }
}
