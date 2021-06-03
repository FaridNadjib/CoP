using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Starsign { None, Mercury, Sun, Moon, Venus, Mars, Jupiter, Saturn, Uranus, Pluto, Neptun }



public class Champion : MonoBehaviour
{
    [Header("Indicators:")]
    [SerializeField] GameObject selectionIndicator;
    [SerializeField] GameObject targetIndicator;
    // Team:
    bool isPlayer;

    [Header("Champion Stats:")]
    // Name:
    [SerializeField] string championName;
    [SerializeField] string title;
    [SerializeField] string description;

    // Sprites:
    [SerializeField] SpriteRenderer championSprite;
    [SerializeField] SpriteRenderer championBorder;

    [Header("Health, Energy, Exp, Speed:")]
    // Health:
    [SerializeField] float maxHealthBase;
    float maxHealthCurrent;
    float currentHealth;

    [SerializeField] float healthRecoveryBase;
    float healthRecoveryCurrent;
    float healthRecoveryBuff;
    float healthRecovery;
    int healthRecBuffDuration;

    // Energy:
    [SerializeField] float maxEnergyBase;
    float maxEnergyCurrent;
    float currentEnergy;

    [SerializeField] float energyRecoveryBase;
    float energyRecoveryCurrent;
    float energyRecoveryBuff;
    float energyRecovery;
    int energyRecBuffDuration;

    [Header("Experience and Levelup related:")]
    // Experience:
    [SerializeField] float maxExp;
    [SerializeField] float expReward;
    [SerializeField] int goldReward;
    [SerializeField] MightCrystalLevel[] crystalReward;
    float currentExp;
    float expBuff;
    // exppenalty: expreward - championlevel - enemyLevel * 0.1 (check if reward is positive, if not give some bas xp).
    [SerializeField] int level;
    int skillPoints;
    [SerializeField] int healthGrowRate;
    [SerializeField] int energyGrowRate;

    // Speed:
    [SerializeField] int initiativeBase;
    int initiativeCurrent;
    int initiativeBuff;
    int initiative;
    int initiativeBuffDuration;

    [Header("Devotion:")]
    // Starsigns / Devotion:
    [SerializeField] Starsign[] possibleDevotions;
    Starsign devotion;
    int devotionAmount;
    float devotionBonus;

    // Mightlevels:
    [SerializeField] MightCrystalLevel[] mightLevels;
    //int mightLevel;
    //int alignmentLevel;
    //int fireLevel;
    //int iceLevel;
    //int lightningLevel;
    //int windLevel;
    //int destructionLevel;
    //int holyLevel;
    //int hunterLevel;
    //int seadragonLevel;

    [Header("Block and Critical:")]
    [SerializeField] float evasionChanceBase;
    float evasionChanceCurrent;
    //float evasionChance;
    [SerializeField] float dmgReductionBase;
    float dmgReductionCurrent;
    //float dmgReduction;
    [SerializeField] float criticalChanceBase;
    float criticalChanceCurrent;
    //float criticalChance;
    [SerializeField] float criticalMultBase;
    float criticalMultCurrent;
    //float criticalMultiplier;

    #region Resistances
    [Header("Resistances:")]
    // Resistances:
    [SerializeField] ResistanceBonus[] resistancesBase;
    [SerializeField] ResistanceBonus[] resistancesCurrent;
    [SerializeField] ResistanceBonus[] resistancesBuff;
    [SerializeField] ResistanceBonus[] resistances;
    //[SerializeField] float bladeResBase;
    //float bladeResCurrent;
    //float bladeResBuff;
    //float bladeResistance;
    //[SerializeField] float pierceResBase;
    //float pierceResCurrent;
    //float pierceResBuff;
    //float pierceResistance;
    //[SerializeField] float impactResBase;
    //float impactResCurrent;
    //float impactResBuff;
    //float impactResistance;
    //[SerializeField] float arcaneResBase;
    //float arcaneResCurrent;
    //float arcaneResBuff;
    //float arcaneResistance;
    //[SerializeField] float fireResBase;
    //float fireResCurrent;
    //float fireResBuff;
    //float fireResistance;
    //[SerializeField] float iceResBase;
    //float iceResCurrent;
    //float iceResBuff;
    //float iceResistance;
    //[SerializeField] float thunderResBase;
    //float thunderResCurrent;
    //float thunderResBuff;
    //float thunderResistance;
    //[SerializeField] float stormResBase;
    //float stormResCurrent;
    //float stormResBuff;
    //float stormResistance;
    int resistanceBuffDuration;
    #endregion

    #region Defenses
    [Header("Defenses:")]
    // Defenses:
    [SerializeField] DefenseBonus[] defenseMainBase;
    [SerializeField] DefenseBonus[] defenseSubBase;
    [SerializeField] DefenseBonus[] defenseMainCurrent;
    [SerializeField] DefenseBonus[] defenseSubCurrent;
    //[SerializeField] float pavedDefMainBase;
    //[SerializeField] float pavedDefSubBase;
    //float pavedDefMainCurrent;
    //float pavedDefSubCurrent;
    //[SerializeField] float desertDefMainBase;
    //[SerializeField] float desertDefSubBase;
    //float desertDefMainCurrent;
    //float desertDefSubCurrent;
    //[SerializeField] float grassDefMainBase;
    //[SerializeField] float grassDefSubBase;
    //float grassDefMainCurrent;
    //float grassDefSubCurrent;
    //[SerializeField] float oceanDefMainBase;
    //[SerializeField] float oceanDefSubBase;
    //float oceanDefMainCurrent;
    //float oceanDefSubCurrent;
    //[SerializeField] float iceDefMainBase;
    //[SerializeField] float iceDefSubBase;
    //float iceDefMainCurrent;
    //float iceDefSubCurrent;
    //[SerializeField] float forestDefMainBase;
    //[SerializeField] float forestDefSubBase;
    //float forestDefMainCurrent;
    //float forestDefSubCurrent;
    //[SerializeField] float swampDefMainBase;
    //[SerializeField] float swampDefSubBase;
    //float swampDefMainCurrent;
    //float swampDefSubCurrent;
    //[SerializeField] float mountainDefMainBase;
    //[SerializeField] float mountainDefSubBase;
    //float mountainDefMainCurrent;
    //float mountainDefSubCurrent;
    float defense;
    float defenseBuff;
    int defenseBuffDuration;
    #endregion

    [Header("Equipment:")]
    // Equipment:
    [SerializeField] Equipment head;
    [SerializeField] Equipment decoration;
    [SerializeField] Equipment armor;
    [SerializeField] Equipment legs;

    [Header("Weapons:")]
    // Weapons:
    [SerializeField] WeaponType[] usableWeapons;
    // weapons...

    [Header("Possible Evolutions:")]
    [SerializeField] BaseChampion[] possibleEvolutions;

    [SerializeField] float attack;


    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }
    public float ExpReward { get => expReward;}
    public int GoldReward { get => goldReward;}
    public MightCrystalLevel[] CrystalReward { get => crystalReward;}
    public int Initiative { get => initiative; set => initiative = value; }
    public Starsign Devotion { get => devotion;}
    public int DevotionAmount { get => devotionAmount;}

    // Start is called before the first frame update
    void Start()
    {
        UpdateBorder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectChampion(bool status)
    {
        selectionIndicator.SetActive(status);
    }
    public void TargetChampion(bool status)
    {
        targetIndicator.SetActive(status);
    }
    public void FlipChampion()
    {
        championSprite.flipX = true;
    }

    public void UpdateBorder()
    {
        Starsign index = CalculateChampionDevotion();
        championBorder.sprite = ChampionManager.Instance.GetChampionBorder(index);
    }

    private Starsign CalculateChampionDevotion()
    {
        int devotionAmount1 = 0;
        int devotionAmount2 = 0;

        if (possibleDevotions.Length == 0)
            return Starsign.None;

        // Get the devotion of each equipment and for each possible devotion.
        devotionAmount1 += CalculateEquipmentDevotion(head, possibleDevotions[0]);
        devotionAmount1 += CalculateEquipmentDevotion(decoration, possibleDevotions[0]);
        devotionAmount1 += CalculateEquipmentDevotion(armor, possibleDevotions[0]);
        devotionAmount1 += CalculateEquipmentDevotion(legs, possibleDevotions[0]);

        devotionAmount2 += CalculateEquipmentDevotion(head, possibleDevotions[1]);
        devotionAmount2 += CalculateEquipmentDevotion(decoration, possibleDevotions[1]);
        devotionAmount2 += CalculateEquipmentDevotion(armor, possibleDevotions[1]);
        devotionAmount2 += CalculateEquipmentDevotion(legs, possibleDevotions[1]);

        if (devotionAmount1 == 0 && devotionAmount2 == 0)
        {
            devotion = Starsign.None;
            devotionAmount = 0;
        }
        else if (devotionAmount1 >= devotionAmount2)
        {
            devotion = possibleDevotions[0];
            devotionAmount = devotionAmount1;
        }
        else
        {
            devotion = possibleDevotions[1];
            devotionAmount = devotionAmount2;
        }
        Debug.Log("Devotion amount and sign:" + devotionAmount + devotion.ToString());
        return devotion;
    }

    private int CalculateEquipmentDevotion(Equipment type, Starsign sign)
    {
        int tmp = 0;
        if (type == null)
            return tmp;
        // Get the devotion of an equipment piece.
        for (int i = 0; i < type.DevotionBonus.Length; i++)
        {
            if (type.DevotionBonus[i].Devotion == sign)
                tmp += type.DevotionBonus[i].DevotionAmount;
        }
        return tmp;
    }

    private void CalculateDevotionBonus(Champion other)
    {
        int delta = 0;
        switch (devotion)
        {
            case Starsign.None:
                devotionBonus = 0.0f;
                return;
            case Starsign.Mercury:
                if (other.devotion == Starsign.Sun)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Venus)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            case Starsign.Sun:
                if (other.devotion == Starsign.Moon)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Mercury)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            case Starsign.Moon:
                if (other.devotion == Starsign.Venus)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Sun)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            case Starsign.Venus:
                if (other.devotion == Starsign.Mercury)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Moon)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            case Starsign.Mars:
                if (other.devotion == Starsign.Jupiter)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Neptun)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            case Starsign.Jupiter:
                if (other.devotion == Starsign.Saturn)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Mars)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            case Starsign.Saturn:
                if (other.devotion == Starsign.Uranus)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Jupiter)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            case Starsign.Uranus:
                if (other.devotion == Starsign.Pluto)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Saturn)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            case Starsign.Pluto:
                if (other.devotion == Starsign.Neptun)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Uranus)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            case Starsign.Neptun:
                if (other.devotion == Starsign.Mars)
                    delta = devotionAmount + other.devotionAmount;
                else if (other.devotion == Starsign.Pluto)
                    delta = (devotionAmount + other.devotionAmount) * -1;
                break;
            default:
                break;
        }
        devotionBonus = 0.05f * delta;
    }

    public void FullHealChampion()
    {
        // Replenish his health and energy, then set all the other values to default.
        currentHealth = maxHealthCurrent;
        currentEnergy = maxEnergyCurrent;

        ResetChampion();
    }

    public void ResetChampion()
    {
        // Reset all champion values to default.      
        healthRecovery = healthRecoveryCurrent;
        healthRecoveryBuff = 0.0f;
        healthRecBuffDuration = 0;
        
        energyRecovery = energyRecoveryCurrent;
        energyRecoveryBuff = 0.0f;
        energyRecBuffDuration = 0;

        initiative = initiativeCurrent;
        initiativeBuff = 0;
        initiativeBuffDuration = 0;

        //evasionChance = evasionChanceCurrent;
        //dmgReduction = dmgReductionCurrent;
        //criticalChance = criticalChanceCurrent;
        //criticalMultiplier = criticalMultCurrent;

        // Reset the resistances.
        for (int i = 0; i < resistances.Length; i++)
            for (int j = 0; j < resistancesCurrent.Length; j++)
                if (resistances[i].ResistanceType == resistancesCurrent[j].ResistanceType)
                    resistances[i].ResistanceAmount = resistancesCurrent[j].ResistanceAmount;
        for (int i = 0; i < resistancesBuff.Length; i++)
            resistancesBuff[i].ResistanceAmount = 0.0f;
        resistanceBuffDuration = 0;

        // Reset defenses.
        defense = 0.0f;
        defenseBuff = 0.0f;
        defenseBuffDuration = 0;
    }

    public void SetChampionDefense(DefenseType main, DefenseType sub)
    {
        defense = 0.0f;
        for (int i = 0; i < defenseMainCurrent.Length; i++)
            if (main == defenseMainCurrent[i].DefenseType)
            {
                defense += defenseMainCurrent[i].DefenseAmount;
                break;
            }
        for (int i = 0; i < defenseSubCurrent.Length; i++)
            if (sub == defenseSubCurrent[i].DefenseType)
            {
                defense += defenseSubCurrent[i].DefenseAmount;
                break;
            }
    }

    public void SetChampionCurrentEquipmentValues()
    {

    }

    public void EquipItem(EquipmentType type)
    {

    }
    public void UnequipItem(EquipmentType type)
    {

    }

    public bool EvolveRequirementsMet(BaseChampion evo)
    {
        //for (int i = 0; i < mightLevels.Length; i++)
        //{
        //    for (int j = 0; j < evo.CrystalLevelRequirements.Length; j++)
        //    {
        //        if (mightLevels[i].MightCrystal == evo.CrystalLevelRequirements[j].MightCrystal)
        //        {
        //            if (mightLevels[i].Amount < evo.CrystalLevelRequirements[j].Amount)
        //            {
        //                return false;
        //            }
        //            break;
        //        }
        //    }
        //}

        for (int i = 0; i < evo.CrystalLevelRequirements.Length; i++)
        {
            for (int j = 0; j < mightLevels.Length; j++)
            {
                if (evo.CrystalLevelRequirements[i].MightCrystal == mightLevels[j].MightCrystal)
                {
                    if (evo.CrystalLevelRequirements[i].Amount > mightLevels[j].Amount)
                    {
                        return false;
                    }
                    break;
                }
            }
        }

        return true;
    }

    public void EvolveChampion(BaseChampion evo)
    {
        championName = evo.ChampionName;
        title = evo.Title;
        description = evo.Description;
        championSprite.sprite = evo.ChampionSprite;

        maxHealthBase += evo.MaxHealthBase;
        healthRecoveryBase = evo.HealthRecoveryBase;
        healthGrowRate = evo.HealthGrowRate;
        maxEnergyBase += evo.MaxEnergyBase;
        energyRecoveryBase = evo.EnergyRecoveryBase;
        energyGrowRate = evo.EnergyGrowRate;

        initiativeBase = evo.InitiativeBase;

        possibleDevotions = evo.PossibleDevotions;

        evasionChanceBase = evo.EvasionChanceBase;
        dmgReductionBase = evo.DmgReductionBase;
        criticalChanceBase = evo.CriticalChanceBase;
        criticalMultBase = evo.CriticalMultBase;

        for (int i = 0; i < resistancesBase.Length; i++)
            for (int j = 0; j < evo.ResistancesBase.Length; j++)
                if (resistancesBase[i].ResistanceType == evo.ResistancesBase[j].ResistanceType)
                {
                    resistancesBase[i].ResistanceAmount = evo.ResistancesBase[j].ResistanceAmount;
                    break;
                }

        for (int i = 0; i < defenseMainBase.Length; i++)
            for (int j = 0; j < evo.DefenseMainBase.Length; j++)
                if (defenseMainBase[i].DefenseType == evo.DefenseMainBase[j].DefenseType)
                {
                    defenseMainBase[i].DefenseAmount = evo.DefenseMainBase[j].DefenseAmount;
                    break;
                }
        for (int i = 0; i < defenseSubBase.Length; i++)
            for (int j = 0; j < evo.DefenseSubBase.Length; j++)
                if (defenseSubBase[i].DefenseType == evo.DefenseSubBase[j].DefenseType)
                {
                    defenseSubBase[i].DefenseAmount = evo.DefenseSubBase[j].DefenseAmount;
                    break;
                }

        for (int i = 0; i < evo.CrystalLevelBoni.Length; i++)
            for (int j = 0; j < mightLevels.Length; j++)
                if (evo.CrystalLevelBoni[i].MightCrystal == mightLevels[j].MightCrystal)
                {
                    mightLevels[j].Amount += evo.CrystalLevelBoni[i].Amount;
                    break;
                }

        possibleEvolutions = evo.PossibleEvolutions;
    }

    private void AddExperience(float amount)
    {
        int divident = 0;
        currentExp += amount + (amount * expBuff);
        if (currentExp >= maxExp)
        {
            divident = (int)(currentExp / maxExp);
            currentExp -= (maxExp * divident);
            level += divident;
            skillPoints += (3 * divident);
        }
    }
}
