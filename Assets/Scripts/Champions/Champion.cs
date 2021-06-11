using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The available starsigns.
/// </summary>
public enum Starsign { None, Mercury, Sun, Moon, Venus, Mars, Jupiter, Saturn, Uranus, Pluto, Neptun }

/// <summary>
/// This class represents the champions.
/// </summary>
public class Champion : MonoBehaviour
{
    #region Private fields.

    [Header("Indicators:")]
    [SerializeField] private GameObject selectionIndicator;

    [SerializeField] private GameObject targetIndicator;
    [SerializeField] private GameObject targetTrigger;

    // Team:
    private bool isPlayer;

    [Header("Champion Stats:")]
    // Name:
    [SerializeField] private string championName;

    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private int price;

    // Sprites:
    [SerializeField] private SpriteRenderer championSprite;

    [SerializeField] private SpriteRenderer championBorder;

    [Header("Health, Energy, Exp, Speed:")]
    private
    // Health:
    bool isAlive = true;

    [SerializeField] private float maxHealthBase;
    private float maxHealthCurrent;
    private float currentHealth;
    private float maxHealth;

    [SerializeField] private float healthRecoveryBase;
    private float healthRecoveryCurrent;
    private float healthRecoveryBuff;
    private float healthRecovery;
    private int healthRecBuffDuration;

    // Energy:
    [SerializeField] private float maxEnergyBase;

    private float maxEnergyCurrent;
    private float currentEnergy;
    private float maxEnergy;

    [SerializeField] private float energyRecoveryBase;
    private float energyRecoveryCurrent;
    private float energyRecoveryBuff;
    private float energyRecovery;
    private int energyRecBuffDuration;

    [Header("Experience and Levelup related:")]
    // Experience:
    [SerializeField] private float maxExp;

    [SerializeField] private float expReward;
    [SerializeField] private int goldReward;
    [SerializeField] private MightCrystalLevel[] crystalReward;
    private float currentExp;
    private float expBuff;

    // exppenalty: expreward - championlevel - enemyLevel * 0.1 (check if reward is positive, if not give some bas xp).
    [SerializeField] private int level;

    [SerializeField] private SpecialUpgradeChances[] specialUpgradeTypes;
    private List<SpecialUpgrades> possibleUpgradeChoices;
    private int spuPoints;
    private int skillPoints;
    [SerializeField] private int skillPointsPerLevel;
    [SerializeField] private float healthGrowRate;
    [SerializeField] private float energyGrowRate;

    // Speed:
    [SerializeField] private int initiativeBase;

    private int initiativeCurrent;
    private int initiativeBuff;
    private int initiative;
    private int initiativeBuffDuration;

    [Header("Devotion:")]
    // Starsigns / Devotion:
    [SerializeField] private Starsign[] possibleDevotions;

    private Starsign devotion;
    private int devotionAmount;
    private float devotionBonus;

    // Mightlevels:
    [SerializeField] private MightCrystalLevel[] mightLevels;

    [Header("Block and Critical:")]
    [SerializeField] private float evasionChanceBase;

    private float evasionChanceCurrent;
    private float evasionChance;
    [SerializeField] private float dmgReductionBase;
    private float dmgReductionCurrent;
    private float dmgReduction;
    [SerializeField] private float criticalChanceBase;
    private float criticalChanceCurrent;
    private float criticalChance;
    [SerializeField] private float criticalMultBase;
    private float criticalMultCurrent;
    private float criticalMultiplier;

    #region Resistances

    [Header("Resistances:")]
    // Resistances:
    [SerializeField] private ResistanceBonus[] resistancesBase;

    [SerializeField] private ResistanceBonus[] resistancesCurrent;
    [SerializeField] private ResistanceBonus[] resistancesBuff;
    [SerializeField] private ResistanceBonus[] resistances;

    private int resistanceBuffDuration;

    #endregion Resistances

    #region Defenses

    [Header("Defenses:")]
    // Defenses:
    [SerializeField] private DefenseBonus[] defenseMainBase;

    [SerializeField] private DefenseBonus[] defenseSubBase;
    [SerializeField] private DefenseBonus[] defenseMainCurrent;
    [SerializeField] private DefenseBonus[] defenseSubCurrent;

    private float defense;
    private float defenseBuff;
    private int defenseBuffDuration;

    #endregion Defenses

    [Header("Equipment:")]
    // Equipment:
    //[SerializeField] Equipment head;
    //[SerializeField] Equipment decoration;
    //[SerializeField] Equipment armor;
    //[SerializeField] Equipment legs;
    [SerializeField] private Equipment[] equipment;

    [Header("Weapons:")]
    // Weapons:
    [SerializeField] private WeaponType[] usableWeapons;

    [SerializeField] private Weapon[] weapons;
    // weapons...

    [SerializeField] private Weapon specialWeapon;
    private SpecialRequirement currentSpecialReq;
    private float specialReqMultiplier = 1.0f;
    private float currentSpecialMeter;

    private bool attackFinished;

    [Header("Possible Evolutions:")]
    [SerializeField] private BaseChampion[] possibleEvolutions;

    [SerializeField] private float attack;

    #endregion Private fields.

    #region Delegates.

    public delegate void BarsChanged(float f);

    public event BarsChanged OnHealthChanged;

    public event BarsChanged OnEnergyChanged;

    #endregion Delegates.

    #region Properties.

    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public float ExpReward { get => expReward; }
    public int GoldReward { get => goldReward; }
    public MightCrystalLevel[] CrystalReward { get => crystalReward; }
    public int Level { get => level; set => level = value; }
    public int Initiative { get => initiative; set => initiative = value; }
    public Starsign Devotion { get => devotion; }
    public int DevotionAmount { get => devotionAmount; }
    public string ChampionName { get => championName; set => championName = value; }
    public string Title { get => title; set => title = value; }
    public Weapon[] Weapons { get => weapons; set => weapons = value; }
    public Weapon SpecialWeapon { get => specialWeapon; set => specialWeapon = value; }
    public float CurrentSpecialMeter { get => currentSpecialMeter; set => currentSpecialMeter = value; }
    public float CurrentEnergy { get => currentEnergy; set { currentEnergy = value; if (currentEnergy > MaxEnergy) currentEnergy = maxEnergy; OnEnergyChanged?.Invoke(currentEnergy / maxEnergy); } }
    public ResistanceBonus[] ResistancesBuff { get => resistancesBuff; set => resistancesBuff = value; }
    public ResistanceBonus[] Resistances { get => resistances; set => resistances = value; }
    public int ResistanceBuffDuration { get => resistanceBuffDuration; set => resistanceBuffDuration = value; }
    public float Defense { get => defense; set => defense = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set { currentHealth = (value > maxHealth) ? MaxHealth : value; IsAlive = !(CurrentHealth <= 0); OnHealthChanged?.Invoke(currentHealth / maxHealth); } }
    public float MaxEnergy { get => maxEnergy; set => maxEnergy = value; }
    public float HealthRecovery { get => healthRecovery; set => healthRecovery = value; }
    public float HealthRecoveryBuff { get => healthRecoveryBuff; set => healthRecoveryBuff = value; }
    public int HealthRecBuffDuration { get => healthRecBuffDuration; set => healthRecBuffDuration = value; }
    public float EnergyRecovery { get => energyRecovery; set => energyRecovery = value; }
    public float EnergyRecoveryBuff { get => energyRecoveryBuff; set => energyRecoveryBuff = value; }
    public int EnergyRecBuffDuration { get => energyRecBuffDuration; set => energyRecBuffDuration = value; }
    public int InitiativeBuff { get => initiativeBuff; set => initiativeBuff = value; }
    public int InitiativeBuffDuration { get => initiativeBuffDuration; set => initiativeBuffDuration = value; }
    public float EvasionChance { get => evasionChance; set => evasionChance = value; }
    public float DmgReduction { get => dmgReduction; set => dmgReduction = value; }
    public float CriticalChance { get => criticalChance; set => criticalChance = value; }
    public float CriticalMultiplier { get => criticalMultiplier; set => criticalMultiplier = value; }
    public float DefenseBuff { get => defenseBuff; set => defenseBuff = value; }
    public int DefenseBuffDuration { get => defenseBuffDuration; set => defenseBuffDuration = value; }
    public SpriteRenderer ChampionSprite { get => championSprite; set => championSprite = value; }
    public SpriteRenderer ChampionBorder { get => championBorder; set => championBorder = value; }
    public float CurrentExp { get => currentExp; set => currentExp = value; }
    public float MaxExp { get => maxExp; set => maxExp = value; }
    public float ExpBuff { get => expBuff; set => expBuff = value; }
    public int Price { get => price; set => price = value; }

    #endregion Properties.

    #region Initialization related.

    /// <summary>
    /// Initializes the champion and sets his base values.
    /// </summary>
    public void InitializeChampion()
    {
        UpdateBorder();
        SetChampionValues();
        FullHealChampion();
    }

    /// <summary>
    /// Flips the championsprite.
    /// </summary>
    public void FlipChampion()
    {
        championSprite.flipX = true;
    }

    /// <summary>
    /// Updates the champions border based on his devotion.
    /// </summary>
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
        for (int i = 0; i < equipment.Length; i++)
            devotionAmount1 += CalculateEquipmentDevotion(equipment[i], possibleDevotions[0]);
        for (int i = 0; i < equipment.Length; i++)
            devotionAmount2 += CalculateEquipmentDevotion(equipment[i], possibleDevotions[1]);

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

    /// <summary>
    /// Calculates the devotion gotten by equipment.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="sign"></param>
    /// <returns>The devotionamount.</returns>
    private int CalculateEquipmentDevotion(Equipment type, Starsign sign)
    {
        int tmp = 0;
        if (type == null)
            return tmp;
        // Get the devotion of an equipment piece.
        for (int i = 0; i < type.DevotionBonus.Length; i++)
        {
            if (type.DevotionBonus[i].Devotion == sign)
            {
                tmp += type.DevotionBonus[i].DevotionAmount;
                break;
            }
        }
        return tmp;
    }

    /// <summary>
    /// Calculates the devotion bonus.
    /// </summary>
    /// <param name="other">The other champ to check against.</param>
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

    /// <summary>
    /// Fully heals the champion.
    /// </summary>
    public void FullHealChampion()
    {
        // Replenish his health and energy, then set all the other values to default.
        MaxHealth = maxHealthBase + maxHealthCurrent;
        CurrentHealth = maxHealthBase + maxHealthCurrent;
        MaxEnergy = maxEnergyBase + maxEnergyCurrent;
        CurrentEnergy = maxEnergyBase + maxEnergyCurrent;

        // Reset the specialattack meter.
        currentSpecialMeter = 0.0f;

        ResetChampion();
    }

    /// <summary>
    /// Removes all buffs from the champion.
    /// </summary>
    public void ResetChampion()
    {
        // Reset all champion values to default.
        //healthRecovery = healthRecoveryCurrent;
        healthRecoveryBuff = 0.0f;
        healthRecBuffDuration = 0;

        //energyRecovery = energyRecoveryCurrent;
        energyRecoveryBuff = 0.0f;
        energyRecBuffDuration = 0;

        //initiative = initiativeCurrent;
        initiativeBuff = 0;
        initiativeBuffDuration = 0;

        //evasionChance = evasionChanceCurrent;
        //dmgReduction = dmgReductionCurrent;
        //criticalChance = criticalChanceCurrent;
        //criticalMultiplier = criticalMultCurrent;

        // Reset the resistances.
        //for (int i = 0; i < resistances.Length; i++)
        //    for (int j = 0; j < resistancesCurrent.Length; j++)
        //        if (resistances[i].ResistanceType == resistancesCurrent[j].ResistanceType)
        //            resistances[i].ResistanceAmount = resistancesCurrent[j].ResistanceAmount;
        //for (int i = 0; i < resistancesBuff.Length; i++)
        //    resistancesBuff[i].ResistanceAmount = 0.0f;
        resistanceBuffDuration = 0;

        // Reset defenses.
        //defense = 0.0f;
        defenseBuff = 0.0f;
        defenseBuffDuration = 0;
    }

    /// <summary>
    /// Sets the defense of the champion.
    /// </summary>
    /// <param name="main"></param>
    /// <param name="sub"></param>
    public void SetChampionDefense(DefenseType main, DefenseType sub)
    {
        defense = 0.0f;
        for (int i = 0; i < defenseMainBase.Length; i++)
            if (main == defenseMainBase[i].DefenseType)
            {
                defense += defenseMainBase[i].DefenseAmount;
                defense += defenseMainCurrent[i].DefenseAmount;
            }
        for (int i = 0; i < defenseSubBase.Length; i++)
            if (sub == defenseSubBase[i].DefenseType)
            {
                defense += defenseSubBase[i].DefenseAmount;
                defense += defenseSubCurrent[i].DefenseAmount;
                break;
            }
    }

    /// <summary>
    /// Sets the base champion values.
    /// </summary>
    public void SetChampionValues()
    {
        for (int i = 0; i < equipment.Length; i++)
            EquipItem(equipment[i], true);

        maxHealth = maxHealthBase + maxHealthCurrent;
        CurrentHealth = maxHealth;
        MaxEnergy = maxEnergyBase + maxEnergyCurrent;
        CurrentEnergy = MaxEnergy;
        healthRecovery = healthRecoveryBase + healthRecoveryCurrent;
        energyRecovery = energyRecoveryBase + energyRecoveryCurrent;

        initiative = initiativeBase + initiativeCurrent;

        evasionChance = evasionChanceBase + evasionChanceCurrent;
        dmgReduction = dmgReductionBase + dmgReductionCurrent;
        criticalChance = criticalChanceBase + criticalChanceCurrent;
        criticalMultiplier = criticalMultBase + criticalMultCurrent;

        for (int i = 0; i < resistances.Length; i++)
            resistances[i].ResistanceAmount = resistancesBase[i].ResistanceAmount + resistancesCurrent[i].ResistanceAmount;
    }

    #endregion Initialization related.

    #region Targeting related.

    /// <summary>
    /// Toggles championselection.
    /// </summary>
    /// <param name="status"></param>
    public void SelectChampion(bool status)
    {
        selectionIndicator.SetActive(status);
    }

    /// <summary>
    /// Toggles champion targeting.
    /// </summary>
    /// <param name="status"></param>
    public void TargetChampion(bool status)
    {
        targetIndicator.SetActive(status);
    }

    /// <summary>
    /// Toggles if targeting is enabled or not.
    /// </summary>
    /// <param name="status"></param>
    public void TargetingEnabled(bool status)
    {
        targetTrigger.SetActive(status);
        if (status == false)
            targetIndicator.SetActive(false);
    }

    #endregion Targeting related.

    /// <summary>
    /// Equips or unequips an item.
    /// </summary>
    /// <param name="item">The item to equip.</param>
    /// <param name="equip">True euip, false unequip.</param>
    public void EquipItem(Equipment item, bool equip)
    {
        if (item == null)
            return;

        int onOrOff = 1;
        if (equip)
            onOrOff = 1;
        else
            onOrOff = -1;
        // Adds statsboni.
        for (int i = 0; i < item.StatsBonus.Length; i++)
        {
            switch (item.StatsBonus[i].StatsBuffType)
            {
                case StatsBuffType.Health:
                    maxHealthCurrent += item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                case StatsBuffType.HealthRecovery:
                    healthRecoveryCurrent += item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                case StatsBuffType.Energy:
                    maxEnergyCurrent += item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                case StatsBuffType.EnergyRecovery:
                    energyRecoveryCurrent += item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                case StatsBuffType.BonusExp:
                    expBuff += item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                case StatsBuffType.CritChance:
                    criticalChanceCurrent += item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                case StatsBuffType.CritMult:
                    criticalMultCurrent += item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                case StatsBuffType.DmgReduction:
                    dmgReductionCurrent += item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                case StatsBuffType.EvasionChance:
                    evasionChanceCurrent += item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                case StatsBuffType.Initiative:
                    initiativeCurrent += (int)item.StatsBonus[i].StatsAmount * onOrOff;
                    break;

                default:
                    break;
            }
        }
        // Add resistance bonus from item.
        for (int i = 0; i < item.ResistanceBonus.Length; i++)
            for (int j = 0; j < resistancesCurrent.Length; j++)
                if (item.ResistanceBonus[i].ResistanceType == resistancesCurrent[j].ResistanceType)
                {
                    resistancesCurrent[j].ResistanceAmount = item.ResistanceBonus[i].ResistanceAmount;
                    break;
                }
        // Add defense.
        for (int i = 0; i < item.DefenseMainBonus.Length; i++)
            for (int j = 0; j < defenseMainCurrent.Length; j++)
                if (item.DefenseMainBonus[i].DefenseType == defenseMainCurrent[j].DefenseType)
                {
                    defenseMainCurrent[j].DefenseAmount = item.DefenseMainBonus[i].DefenseAmount;
                    break;
                }
        for (int i = 0; i < item.DefenseSubBonus.Length; i++)
            for (int j = 0; j < defenseSubCurrent.Length; j++)
                if (item.DefenseSubBonus[i].DefenseType == defenseSubCurrent[j].DefenseType)
                {
                    defenseSubCurrent[j].DefenseAmount = item.DefenseSubBonus[i].DefenseAmount;
                    break;
                }
    }

    #region Evolution and levelup related.

    /// <summary>
    /// Checks if the champ can evolve.
    /// </summary>
    /// <param name="evo">The evolution to check against.</param>
    /// <returns>True if he can evolve.</returns>
    public bool EvolveRequirementsMet(BaseChampion evo)
    {
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

    /// <summary>
    /// Adds the stats from evolution to the champion.
    /// </summary>
    /// <param name="evo">The evolved form.</param>
    public void EvolveChampion(BaseChampion evo)
    {
        championName = evo.ChampionName;
        title = evo.Title;
        description = evo.Description;
        championSprite.sprite = evo.ChampionSprite;

        maxHealthBase += evo.MaxHealthBase;
        healthRecoveryBase += evo.HealthRecoveryBase;
        healthGrowRate += evo.HealthGrowRate;
        maxEnergyBase += evo.MaxEnergyBase;
        energyRecoveryBase += evo.EnergyRecoveryBase;
        energyGrowRate += evo.EnergyGrowRate;

        initiativeBase += evo.InitiativeBase;

        possibleDevotions = evo.PossibleDevotions;

        evasionChanceBase += evo.EvasionChanceBase;
        dmgReductionBase += evo.DmgReductionBase;
        criticalChanceBase += evo.CriticalChanceBase;
        criticalMultBase += evo.CriticalMultBase;

        for (int i = 0; i < resistancesBase.Length; i++)
            for (int j = 0; j < evo.ResistancesBase.Length; j++)
                if (resistancesBase[i].ResistanceType == evo.ResistancesBase[j].ResistanceType)
                {
                    resistancesBase[i].ResistanceAmount += evo.ResistancesBase[j].ResistanceAmount;
                    break;
                }

        for (int i = 0; i < defenseMainBase.Length; i++)
            for (int j = 0; j < evo.DefenseMainBase.Length; j++)
                if (defenseMainBase[i].DefenseType == evo.DefenseMainBase[j].DefenseType)
                {
                    defenseMainBase[i].DefenseAmount += evo.DefenseMainBase[j].DefenseAmount;
                    break;
                }
        for (int i = 0; i < defenseSubBase.Length; i++)
            for (int j = 0; j < evo.DefenseSubBase.Length; j++)
                if (defenseSubBase[i].DefenseType == evo.DefenseSubBase[j].DefenseType)
                {
                    defenseSubBase[i].DefenseAmount += evo.DefenseSubBase[j].DefenseAmount;
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

    /// <summary>
    /// Adds experience.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public void AddExperience(float amount)
    {
        // Add experience and check if the champion leveld up.
        int divident = 0;
        currentExp += amount + (amount * expBuff);

        // Check for specialrequirement.
        if (currentSpecialReq == SpecialRequirement.XpGain)
        {
            currentSpecialMeter += amount;
            if (specialWeapon != null && currentSpecialMeter >= specialWeapon.MaxSpecialMeter * specialReqMultiplier)
                currentSpecialMeter = specialWeapon.MaxSpecialMeter * specialReqMultiplier;
        }

        if (currentExp >= maxExp)
        {
            // Check if the champion leveld up more than once.
            divident = (int)(currentExp / maxExp);
            currentExp -= (maxExp * divident);
            level += divident;
            skillPoints += (skillPointsPerLevel * divident);

            // Check if the champion leveld up a certain amount of times, if enable special upgrade.
            if (level % 4 == 0)
            {
                spuPoints++;
            }
        }
    }

    /// <summary>
    /// For upgrade button.
    /// </summary>
    /// <param name="index"></param>
    public void OnSpendUpgradePointsButton(int index)
    {
        AddSpecialUpgrade(possibleUpgradeChoices[index]);
    }

    /// <summary>
    /// Creates a list with possible upgrades.
    /// </summary>
    private void GetRandomSpecialUpgrades()
    {
        List<SpecialUpgrades> possibleUpgradeChoices = new List<SpecialUpgrades>();
        bool finished = false;
        float r = 0;
        for (int i = 0; i < 100; i++)
        {
            r = Random.Range(0.0f, 1.0f);
            for (int j = 0; j < specialUpgradeTypes.Length; j++)
            {
                if (r >= specialUpgradeTypes[j].ChanceFrom && r <= specialUpgradeTypes[j].ChanceTo)
                {
                    if (!possibleUpgradeChoices.Contains(specialUpgradeTypes[j].SpecialUpgradeType))
                    {
                        possibleUpgradeChoices.Add(specialUpgradeTypes[j].SpecialUpgradeType);
                        if (possibleUpgradeChoices.Count >= 3)
                        {
                            finished = true;
                            break;
                        }
                    }
                }
            }
            if (finished)
                break;
        }
    }

    /// <summary>
    /// Adds the special upgrade to the champion stats.
    /// </summary>
    /// <param name="type">The type.</param>
    private void AddSpecialUpgrade(SpecialUpgrades type)
    {
        SpecialUpgradeChances special = specialUpgradeTypes[0];
        for (int i = 0; i < specialUpgradeTypes.Length; i++)
        {
            if (type == specialUpgradeTypes[i].SpecialUpgradeType)
            {
                special = specialUpgradeTypes[i];
                break;
            }
        }

        switch (special.SpecialUpgradeType)
        {
            case SpecialUpgrades.Health:
                if (special.IsPercentage)
                    maxHealthBase += maxHealthBase * special.Amount;
                else
                    maxHealthBase += special.Amount;
                break;

            case SpecialUpgrades.HealthRecov:
                if (special.IsPercentage)
                    healthRecoveryBase += healthRecoveryBase * special.Amount;
                else
                    healthRecoveryBase += special.Amount;
                break;

            case SpecialUpgrades.HealthGR:
                if (special.IsPercentage)
                    healthGrowRate += healthGrowRate * special.Amount;
                else
                    healthGrowRate += special.Amount;
                break;

            case SpecialUpgrades.Energy:
                if (special.IsPercentage)
                    maxEnergyBase += maxEnergyBase * special.Amount;
                else
                    maxEnergyBase += special.Amount;
                break;

            case SpecialUpgrades.EnergyRecov:
                if (special.IsPercentage)
                    energyRecoveryBase += energyRecoveryBase * special.Amount;
                else
                    energyRecoveryBase += special.Amount;
                break;

            case SpecialUpgrades.EneryGR:
                if (special.IsPercentage)
                    energyGrowRate += energyGrowRate * special.Amount;
                else
                    energyGrowRate += special.Amount;
                break;

            case SpecialUpgrades.CritMulti:
                if (special.IsPercentage)
                    criticalMultBase += criticalMultBase * special.Amount;
                else
                    criticalMultBase += special.Amount;
                break;

            case SpecialUpgrades.CitChance:
                if (special.IsPercentage)
                    criticalChanceBase += criticalChanceBase * special.Amount;
                else
                    criticalChanceBase += special.Amount;
                break;

            case SpecialUpgrades.DmgReduction:
                if (special.IsPercentage)
                    dmgReductionBase += dmgReductionBase * special.Amount;
                else
                    dmgReductionBase += special.Amount;
                break;

            case SpecialUpgrades.EvasionChance:
                if (special.IsPercentage)
                    evasionChanceBase += evasionChanceBase * special.Amount;
                else
                    evasionChanceBase += special.Amount;
                break;

            case SpecialUpgrades.Initiative:
                if (special.IsPercentage)
                    initiativeBase += (int)(initiativeBase * special.Amount);
                else
                    initiativeBase += (int)special.Amount;
                break;

            case SpecialUpgrades.Defense:
                if (special.IsPercentage)
                {
                    // Eventually cap resistance and def to a max of 0.9f or so.
                    for (int i = 0; i < defenseMainBase.Length; i++)
                        defenseMainBase[i].DefenseAmount += defenseMainBase[i].DefenseAmount * special.Amount;
                    for (int i = 0; i < defenseSubBase.Length; i++)
                        defenseSubBase[i].DefenseAmount += defenseSubBase[i].DefenseAmount * special.Amount;
                }
                else
                {
                    for (int i = 0; i < resistancesBase.Length; i++)
                        defenseMainBase[i].DefenseAmount += special.Amount;
                    for (int i = 0; i < defenseSubBase.Length; i++)
                        defenseSubBase[i].DefenseAmount += special.Amount;
                }
                break;

            case SpecialUpgrades.Resistance:
                if (special.IsPercentage)
                    for (int i = 0; i < resistancesBase.Length; i++)
                        resistancesBase[i].ResistanceAmount += resistancesBase[i].ResistanceAmount * special.Amount;
                else
                    for (int i = 0; i < resistancesBase.Length; i++)
                        resistancesBase[i].ResistanceAmount += special.Amount;
                break;

            case SpecialUpgrades.LessXp:
                if (special.IsPercentage)
                    maxExp -= maxExp * special.Amount;
                else
                    maxExp -= special.Amount;
                break;

            case SpecialUpgrades.MoreSkillPoints:
                if (special.IsPercentage)
                    skillPointsPerLevel += (int)(skillPointsPerLevel * special.Amount);
                else
                    skillPointsPerLevel += (int)special.Amount;
                break;

            case SpecialUpgrades.LessSpecialRequirement:
                if (special.IsPercentage)
                    specialReqMultiplier -= specialReqMultiplier * special.Amount;
                else
                    specialReqMultiplier -= special.Amount;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Upgrade button method.
    /// </summary>
    /// <param name="index"></param>
    public void OnUpgradeMightLevelButton(int index)
    {
        mightLevels[index].Amount += 3;
    }

    #endregion Evolution and levelup related.

    #region Combat related.

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        if (!IsAlive)
        {
            // Call the die method.
            Die();
        }
        if (currentSpecialReq == SpecialRequirement.Health)
        {
            currentSpecialMeter += amount;
            if (specialWeapon != null && currentSpecialMeter >= specialWeapon.MaxSpecialMeter * specialReqMultiplier)
                currentSpecialMeter = specialWeapon.MaxSpecialMeter * specialReqMultiplier;
        }
    }

    private void Die()
    {
        // Spawn die particles.
    }

    public void SpendEnergy(float amount)
    {
        CurrentEnergy -= amount;
        if (currentSpecialReq == SpecialRequirement.Energy)
        {
            currentSpecialMeter += amount;
            if (specialWeapon != null && currentSpecialMeter >= specialWeapon.MaxSpecialMeter * specialReqMultiplier)
                currentSpecialMeter = specialWeapon.MaxSpecialMeter * specialReqMultiplier;
        }
    }

    /// <summary>
    /// Gets called when player attacks multiple champions at once.
    /// </summary>
    /// <param name="other">Target champion.</param>
    /// <param name="w">The weapon he uses.</param>
    public void AttackMultiple(Champion other, Weapon w, bool notAI = true)
    {
        // Calculare the weapon power.
        float power = GetWeaponPower(w);
        float finalPower = 0.0f;
        CalculateDevotionBonus(other);
        float resistanceMultiplier = CalculateResistanceMultiplier(other, w);
        finalPower = power + power * devotionBonus;
        finalPower -= finalPower * resistanceMultiplier;
        // Start the Coroutine with the calculated value, it will not end the turn on its own.
        StartCoroutine(MultipleAttacksDelayed(finalPower, w, other, notAI));
    }

    /// <summary>
    /// This coroutine will attack but not end the turn on its own, since it attacks multiple targets.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="w"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    private IEnumerator MultipleAttacksDelayed(float damage, Weapon w, Champion other, bool notAI)
    {
        for (int i = 0; i < w.NumberOfAttacks; i++)
        {
            float finalPower = damage;
            GameObject tmp;
            if (w.TargetEffect != null)
            {
                tmp = Instantiate(w.TargetEffect, transform.position, Quaternion.identity);
                Destroy(tmp, w.AttackDelay);
            }
            // Wait before applying the damage.
            yield return new WaitForSeconds(w.AttackDelay);

            // Calculate the final damage.
            int hittype = 0;
            float r = Random.Range(0.0f, 1.0f);
            if (r <= (other.defense + other.defenseBuff))
            {
                r = Random.Range(0.0f, 1.0f);
                if (r <= other.evasionChance)
                    finalPower = 0.0f;
                else
                {
                    finalPower -= (finalPower * other.dmgReduction);
                    hittype = 1;
                }
            }
            else
            {
                r = Random.Range(0.0f, 1.0f);
                if (r <= other.criticalChance)
                {
                    finalPower *= criticalMultiplier;
                    hittype = 2;
                }
            }

            // Update the specialweaponmeter.
            if (currentSpecialReq == SpecialRequirement.DamageDealt)
            {
                currentSpecialMeter += finalPower;
                if (specialWeapon != null && currentSpecialMeter >= specialWeapon.MaxSpecialMeter * specialReqMultiplier)
                    currentSpecialMeter = specialWeapon.MaxSpecialMeter * specialReqMultiplier;
            }

            // Apply Damage.
            float luck = Random.Range(0.96f, 1.04f);
            other.TakeDamage(finalPower * luck);

            // Show damagenumber.
            GameObject tmpText = null;
            tmpText = ObjectPool.Instance.GetFromPool("textPopup");
            if (tmpText != null)
            {
                tmpText.transform.parent = BattleManager.Instance.AttackEffectsHolder;
                tmpText.transform.position = new Vector2(Random.Range((other.transform.position.x - 1.8f), (other.transform.position.x + 1.8f)), Random.Range((other.transform.position.y - 1.0f), (other.transform.position.y + 1.5f)));
                tmpText.GetComponent<TextPopup>().ShowDamage(finalPower * luck, hittype);
                tmpText.SetActive(true);
            }

            // Apply weaponeffects.
            ApplyWeaponBuffs(other, w, finalPower * luck);
            if (notAI)
                CrystalManifestation(w);

            // If the target is dead, cancel this method.
            if (!other.IsAlive)
                yield break;
            //else if(other.isAlive && !notAI)
            //{
            //    UIManager.Instance.ShowContinueButton(true);
            //    yield break;
            //}
        }
        //if (!notAI)
        //    UIManager.Instance.ShowContinueButton(true);
    }

    /// <summary>
    /// This method starts the coroutine that will end the turn in case a champion attacked multiple targets at once.
    /// </summary>
    /// <param name="w">The weapon.</param>
    public void AttackMultipleDelay(Weapon w, bool notAI = true)
    {
        StartCoroutine(WaitMultipleAttacks(w, notAI));
    }

    /// <summary>
    /// This coroutine will end the turn in case a champion attacked multiple targets at once.
    /// </summary>
    /// <param name="w">The weapon.</param>
    /// <returns></returns>
    private IEnumerator WaitMultipleAttacks(Weapon w, bool notAI)
    {
        yield return new WaitForSeconds((float)w.NumberOfAttacks * w.AttackDelay + 0.2f);
        // Tell battleManager the attack is finished.
        if (notAI)
            BattleManager.Instance.EndTurn();
        else
            UIManager.Instance.ShowContinueButton(true);
    }

    /// <summary>
    /// This method calls the coroutine for singletarget enemies.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="w"></param>
    public void Attack(Champion other, Weapon w, bool notAI = true)
    {
        SpendEnergy(w.EnergyCost);

        // Calculate the base damage.
        float power = GetWeaponPower(w);
        Debug.Log("WeaponPower with Mightlevel: " + power);
        float finalPower = 0.0f;
        CalculateDevotionBonus(other);
        Debug.Log("Devotionbonus :" + devotionBonus);
        float resistanceMultiplier = CalculateResistanceMultiplier(other, w);
        Debug.Log("Resmultipler :" + resistanceMultiplier);
        finalPower = power + power * devotionBonus;
        Debug.Log("Final power after devotion :" + finalPower);
        finalPower -= finalPower * resistanceMultiplier;
        Debug.Log("Final power after resistance:" + finalPower);

        StartCoroutine(SingleAttackDelayed(finalPower, w, other, notAI));
    }

    private IEnumerator SingleAttackDelayed(float damage, Weapon w, Champion other, bool notAI)
    {
        for (int i = 0; i < w.NumberOfAttacks; i++)
        {
            float finalPower = damage;
            GameObject tmp;
            if (w.TargetEffect != null)
            {
                tmp = Instantiate(w.TargetEffect, transform.position, Quaternion.identity);
                Destroy(tmp, w.AttackDelay);
            }
            // Wait before applying the damage.
            yield return new WaitForSeconds(w.AttackDelay);

            // Calculate the final damage.
            int hittype = 0;
            float r = Random.Range(0.0f, 1.0f);
            if (r <= (other.defense + other.defenseBuff))
            {
                r = Random.Range(0.0f, 1.0f);
                if (r <= other.evasionChance)
                    finalPower = 0.0f;
                else
                {
                    finalPower -= (finalPower * other.dmgReduction);
                    hittype = 1;
                }
            }
            else
            {
                r = Random.Range(0.0f, 1.0f);
                if (r <= other.criticalChance)
                {
                    finalPower *= criticalMultiplier;
                    hittype = 2;
                }
            }

            // Check for specialweapon requirement.
            if (currentSpecialReq == SpecialRequirement.DamageDealt)
            {
                currentSpecialMeter += finalPower;
                if (specialWeapon != null && currentSpecialMeter >= specialWeapon.MaxSpecialMeter * specialReqMultiplier)
                    currentSpecialMeter = specialWeapon.MaxSpecialMeter * specialReqMultiplier;
            }
            // Deal damage.
            float luck = Random.Range(0.96f, 1.04f);
            other.TakeDamage(finalPower * luck);

            // Show damagenumber.
            GameObject tmpText = null;
            tmpText = ObjectPool.Instance.GetFromPool("textPopup");
            if (tmpText != null)
            {
                tmpText.transform.parent = BattleManager.Instance.AttackEffectsHolder;
                tmpText.transform.position = new Vector2(Random.Range((other.transform.position.x - 1.8f), (other.transform.position.x + 1.8f)), Random.Range((other.transform.position.y - 1.0f), (other.transform.position.y + 1.5f)));
                tmpText.GetComponent<TextPopup>().ShowDamage(finalPower * luck, hittype);
                tmpText.SetActive(true);
            }

            // Apply buffs.
            ApplyWeaponBuffs(other, w, finalPower * luck);
            if (notAI)
                CrystalManifestation(w);

            // If target is dead end the turn and coroutine.
            if (!other.IsAlive && notAI)
            {
                BattleManager.Instance.EndTurn();
                yield break;
            }
            else if (!other.IsAlive)
            {
                string m = $"{championName} hat sein Ziel getoetet.";
                UIManager.Instance.ShowEnemyAction(m);
                UIManager.Instance.ShowContinueButton(true);
                yield break;
            }
        }

        if (notAI)
            // Tell battleManager the attack is finished.
            BattleManager.Instance.EndTurn();
        else
            UIManager.Instance.ShowContinueButton(true);
    }

    /// <summary>
    /// Apply the weapon effects after damage.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="w"></param>
    /// <param name="damage"></param>
    private void ApplyWeaponBuffs(Champion other, Weapon w, float damage)
    {
        for (int i = 0; i < w.WeaponSpecials.Length; i++)
        {
            switch (w.WeaponSpecials[i].EffectType)
            {
                case WeaponEffect.Lifeleech:
                    CurrentHealth += damage * w.WeaponSpecials[i].Buff;
                    break;

                case WeaponEffect.Energyleech:
                    CurrentEnergy += damage * w.WeaponSpecials[i].Buff;
                    break;

                case WeaponEffect.HealthRecovery:
                    other.healthRecoveryBuff = w.WeaponSpecials[i].Buff;
                    other.healthRecBuffDuration = w.WeaponSpecials[i].Duration;
                    break;

                case WeaponEffect.EnergyRecovery:
                    other.energyRecoveryBuff = w.WeaponSpecials[i].Buff;
                    other.energyRecBuffDuration = w.WeaponSpecials[i].Duration;
                    break;

                case WeaponEffect.Initiative:
                    other.initiativeBuff = (int)w.WeaponSpecials[i].Buff;
                    other.initiativeBuffDuration = w.WeaponSpecials[i].Duration;
                    break;

                case WeaponEffect.Defense:
                    other.defenseBuff = w.WeaponSpecials[i].Buff;
                    other.defenseBuffDuration = w.WeaponSpecials[i].Duration;
                    break;

                default:
                    break;
            }
        }
        // Resistances:
        for (int i = 0; i < w.ResistancesBuffs.Length; i++)
        {
            for (int j = 0; j < other.resistancesBuff.Length; j++)
            {
                if (w.ResistancesBuffs[i].ResistanceType == other.resistancesBuff[j].ResistanceType)
                {
                    other.resistancesBuff[j].ResistanceAmount = w.ResistancesBuffs[i].Buff;
                    other.resistanceBuffDuration = w.ResistancesBuffs[i].Duration;
                }
            }
        }
    }

    /// <summary>
    /// Apply the crystalmanifestation effects.
    /// </summary>
    /// <param name="w"></param>
    private void CrystalManifestation(Weapon w)
    {
        for (int i = 0; i < w.CrystalManifestations.Length; i++)
        {
            float r = Random.Range(0.0f, 1.0f);
            if (r <= w.CrystalManifestations[i].ManifestationChance)
            {
                Debug.Log(w.CrystalManifestations[i].MightCrystal + " added times: ");
                BattleManager.Instance.AddCrystalRewards((int)(w.CrystalManifestations[i].MightCrystal), 1);
            }
        }
    }

    /// <summary>
    /// Calculates the resistance multiplier based on weapon used and target champions resistance.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    private float CalculateResistanceMultiplier(Champion other, Weapon w)
    {
        float numberOfTypes = 0.0f;
        float tmpResistance = 0.0f;
        for (int i = 0; i < w.DamageTypes.Length; i++)
        {
            for (int j = 0; j < other.resistances.Length; j++)
            {
                if (w.DamageTypes[i] == other.resistances[j].ResistanceType)
                {
                    tmpResistance += other.resistances[j].ResistanceAmount;
                    tmpResistance += other.resistancesBuff[j].ResistanceAmount;
                    numberOfTypes++;
                    break;
                }
            }
        }
        if ((tmpResistance / numberOfTypes) > 0.9f)
            return 0.9f;
        else
            return tmpResistance / numberOfTypes;
    }

    public void Regenerate()
    {
        // Regenerate HP.
        CurrentHealth += healthRecoveryBase + healthRecoveryCurrent + healthRecoveryBuff;

        // Health special meter.
        if (currentSpecialReq == SpecialRequirement.HealthRec)
        {
            currentSpecialMeter += healthRecoveryBase + healthRecoveryCurrent + healthRecoveryBuff;
            if (specialWeapon != null && currentSpecialMeter >= specialWeapon.MaxSpecialMeter * specialReqMultiplier)
                currentSpecialMeter = specialWeapon.MaxSpecialMeter * specialReqMultiplier;
        }
        if (healthRecBuffDuration >= 1)
        {
            healthRecBuffDuration--;
            if (healthRecBuffDuration == 0)
                healthRecoveryBuff = 0.0f;
        }
        // Regenerate EP.
        CurrentEnergy += energyRecoveryBase + energyRecoveryCurrent + energyRecoveryBuff;
        if (currentEnergy >= maxEnergyBase + maxEnergyCurrent)
            CurrentEnergy = maxEnergyBase + maxEnergyCurrent;
        if (currentSpecialReq == SpecialRequirement.EnergyRec)
        {
            currentSpecialMeter += energyRecoveryBase + energyRecoveryCurrent + energyRecoveryBuff;
            if (specialWeapon != null && currentSpecialMeter >= specialWeapon.MaxSpecialMeter * specialReqMultiplier)
                currentSpecialMeter = specialWeapon.MaxSpecialMeter * specialReqMultiplier;
        }
        if (energyRecBuffDuration >= 1)
        {
            energyRecBuffDuration--;
            if (energyRecBuffDuration == 0)
                energyRecoveryBuff = 0.0f;
        }

        // Initiative.
        if (initiativeBuffDuration >= 1)
        {
            initiativeBuffDuration--;
            if (initiativeBuffDuration == 0)
                initiativeBuff = 0;
        }
        // Resistance.
        if (resistanceBuffDuration >= 1)
        {
            resistanceBuffDuration--;
            if (resistanceBuffDuration == 0)
                for (int i = 0; i < resistancesBuff.Length; i++)
                    resistancesBuff[i].ResistanceAmount = 0.0f;
        }
        // Defense.
        if (defenseBuffDuration >= 1)
        {
            defenseBuffDuration--;
            if (defenseBuffDuration == 0)
                defenseBuff = 0.0f;
        }
    }

    /// <summary>
    /// Returns the power of a weapon taking the champions mightlevels into concern.
    /// </summary>
    /// <param name="w">The weapon to check the power.</param>
    /// <returns>The power the weapon has when this champion wields it.</returns>
    public float GetWeaponPower(Weapon w)
    {
        int numOfParams = 0;
        float tmpMod = 0.0f;
        // Store all the weapons scalemods in one value then the divide it by the number of mods. This * the base power will make the power.
        for (int i = 0; i < w.ScaleLevels.Length; i++)
        {
            for (int j = 0; j < mightLevels.Length; j++)
            {
                if (w.ScaleLevels[i].MightCrystal == mightLevels[j].MightCrystal)
                {
                    tmpMod += GetScaleModifier(w.ScaleLevels[i].Scale) * (mightLevels[j].Amount / 100.0f);
                    numOfParams++;
                }
            }
        }
        if (numOfParams == 0)
            return w.BaseAttack;
        else
            return w.BaseAttack + w.BaseAttack * (tmpMod / numOfParams);
    }

    /// <summary>
    /// Converts the parameter scale into an modifier i can calculate with.
    /// </summary>
    /// <param name="s">The parameter modifier as enum.</param>
    /// <returns>The modifier as float.</returns>
    private float GetScaleModifier(ParameterScale s)
    {
        switch (s)
        {
            case ParameterScale.S:
                return 1.25f;

            case ParameterScale.A:
                return 1.0f;

            case ParameterScale.B:
                return 0.8f;

            case ParameterScale.C:
                return 0.6f;

            case ParameterScale.D:
                return 0.4f;

            case ParameterScale.E:
                return 0.25f;

            default:
                break;
        }
        return 0.0f;
    }

    /// <summary>
    /// Checks if the champion can perform any move.
    /// </summary>
    /// <returns></returns>
    public bool HasMoveAvailable()
    {
        for (int i = 0; i < weapons.Length; i++)
            if (weapons[i].EnergyCost <= currentEnergy)
                return true;

        if (specialWeapon.EnergyCost <= currentEnergy)
            return true;

        return false;
    }

    #endregion Combat related.

    /// <summary>
    /// Returns the specialmeter fill ratio.
    /// </summary>
    /// <returns></returns>
    public float GetSpecialMeterRatio()
    {
        return currentSpecialMeter / specialWeapon.MaxSpecialMeter;
    }
}