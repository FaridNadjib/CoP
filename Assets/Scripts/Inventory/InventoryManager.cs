using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The type of items, Head, Decos, Armor or legs.
/// </summary>
public enum EquipmentType { Head, Decoration, Armor, Leg }
/// <summary>
/// Type of damage, from blade, pierce to fire and ice.
/// </summary>
public enum DamageType { None, Blade, Pierce, Impact, Arcane, Fire, Ice, Thunder, Storm }
/// <summary>
/// Defensetype aka the terraindefense, like desert, swamp forest and so on.
/// </summary>
public enum DefenseType { None, Paved, Desert, Grass, Ocean, Ice, Forest, Swamp, Mountain }
/// <summary>
/// The type of stats that can get changed, like hp energy critchances and so on.
/// </summary>
public enum StatsBuffType { None, Health, HealthRecovery, Energy, EnergyRecovery, BonusExp, CritChance, CritMult, DmgReduction, EvasionChance, Initiative }
/// <summary>
/// Not all champions can use all weapon types. Claws weapons magic and so on.
/// </summary>
public enum WeaponType { All, Weapon, Magic, Claw, Tooth, Tail, Wing, Body, Special }
/// <summary>
/// The targets a weapon can aim, from all to single to enemy to allys.
/// </summary>
public enum WeaponTarget { All, Ally, Allys, Enemy, Enemies }
/// <summary>
/// Special Weapon Debuff Effectbonis for enemies. Like hp regen debuff for enemies.
/// </summary>
public enum WeaponEffect { Lifeleech, Energyleech, HealthRecovery, EnergyRecovery, Initiative, Defense}
/// <summary>
/// For special Weapons the requirement to use them.
/// </summary>
public enum SpecialRequirement { None, Health, Energy, HealthRec, EnergyRec, XpGain, DamageDealt }

/// <summary>
/// This enum contains all battleffects to access them via objectpooler.
/// </summary>
public enum BattleEffects { None, Dark1, Dark2, Dark3, Dark4, Dark5, Dark6, Dark7, Dark8, Earth1, Earth2, Earth3, Earth4, Earth5, Earth6, Earth7, Earth8,
Thunder1, Thunder2, Thunder3, Thunder4, Thunder5, Thunder6, Thunder7, Thunder8, Water1, Water2, Water3, Water4, Water5, Water6, Water7, Water8,
Thauma1, Thauma2, Thauma3, Thauma4, Thauma5, Thauma6, Thauma7, Thauma8, Thauma9, Quantum1, Quantum2, Quantum3, Quantum4, Quantum5, Quantum6, Quantum7, Quantum8,
    Quantum9, Quantum10, Quantum11, Quantum12, Quantum13, Quantum14, Quantum15, Quantum16, Quantum17, Quantum18, Quantum19, Quantum20, Quantum21, Quantum22, Quantum23,
    Quantum24, Quantum25,
}

/// <summary>
/// This class manages the inventory database.
/// </summary>
public class InventoryManager : MonoBehaviour
{ 
    [Header("Inventory Database:")]
    [SerializeField] Equipment[] headEquipments;
    [SerializeField] Equipment[] decorationEquipments;
    [SerializeField] Equipment[] armorEquipments;
    [SerializeField] Equipment[] legEquipments;
    [SerializeField] Weapon[] weapons;

    Dictionary<int, Equipment> headDictionary;
    Dictionary<int, Equipment> decorationDictionary;
    Dictionary<int, Equipment> armorDictionary;
    Dictionary<int, Equipment> legsDictionary;
    Dictionary<int, Weapon> weaponDictionary;
    public Dictionary<int, Weapon> WeaponDictionary { get => weaponDictionary; set => weaponDictionary = value; }

    [Header("Tier Database.")]
    [SerializeField] Equipment[] t1H;
    [SerializeField] Equipment[] t2H;
    [SerializeField] Equipment[] t3H;
    [SerializeField] Equipment[] t1D;
    [SerializeField] Equipment[] t2D;
    [SerializeField] Equipment[] t3D;
    [SerializeField] Equipment[] t1A;
    [SerializeField] Equipment[] t2A;
    [SerializeField] Equipment[] t3A;
    [SerializeField] Equipment[] t1B;
    [SerializeField] Equipment[] t2B;
    [SerializeField] Equipment[] t3B;
    [SerializeField] Weapon[] t1W;
    [SerializeField] Weapon[] t2W;


    #region Singleton
    public static InventoryManager Instance;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);

        // Create the data base.
        CreateEquipmentDatabase();
    }
    #endregion

    /// <summary>
    /// Adds all items to the database.
    /// </summary>
    private void CreateEquipmentDatabase()
    {
        headDictionary = new Dictionary<int, Equipment>();
        decorationDictionary = new Dictionary<int, Equipment>();
        armorDictionary = new Dictionary<int, Equipment>();
        legsDictionary = new Dictionary<int, Equipment>();
        weaponDictionary = new Dictionary<int, Weapon>();

        for (int i = 0; i < headEquipments.Length; i++)
            if (!headDictionary.ContainsKey(headEquipments[i].ItemID))
                headDictionary.Add(headEquipments[i].ItemID, headEquipments[i]);

        for (int i = 0; i < decorationEquipments.Length; i++)
            if (!decorationDictionary.ContainsKey(decorationEquipments[i].ItemID))
                decorationDictionary.Add(decorationEquipments[i].ItemID, decorationEquipments[i]);

        for (int i = 0; i < armorEquipments.Length; i++)
            if (!armorDictionary.ContainsKey(armorEquipments[i].ItemID))
                armorDictionary.Add(armorEquipments[i].ItemID, armorEquipments[i]);

        for (int i = 0; i < legEquipments.Length; i++)
            if (!legsDictionary.ContainsKey(legEquipments[i].ItemID))
                legsDictionary.Add(legEquipments[i].ItemID, legEquipments[i]);

        for (int i = 0; i < weapons.Length; i++)
            if (!weaponDictionary.ContainsKey(weapons[i].ItemID))
                weaponDictionary.Add(weapons[i].ItemID, weapons[i]);
    }

    /// <summary>
    /// Adds equipment base on id.
    /// </summary>
    /// <param name="eq">The equipment to add.</param>
    public void AddEquipment(Equipment eq)
    {
        if (eq == null)
            return;
        EquipmentType type = eq.Type;
        switch (type)
        {
            case EquipmentType.Head:
                if (headDictionary.ContainsKey(eq.ItemID))
                    headDictionary[eq.ItemID].NumberOfItems++;
                break;
            case EquipmentType.Decoration:
                if (decorationDictionary.ContainsKey(eq.ItemID))
                    decorationDictionary[eq.ItemID].NumberOfItems++;
                break;
            case EquipmentType.Armor:
                if (armorDictionary.ContainsKey(eq.ItemID))
                    armorDictionary[eq.ItemID].NumberOfItems++;
                break;
            case EquipmentType.Leg:
                if (legsDictionary.ContainsKey(eq.ItemID))
                    legsDictionary[eq.ItemID].NumberOfItems++;
                break;
            default:
                break;
        }     
    }
    /// <summary>
    /// Removes the number of items from an equipment.
    /// </summary>
    /// <param name="eq"></param>
    public void RemoveEquipment(Equipment eq)
    {
        
        EquipmentType type = eq.Type;
        switch (type)
        {
            case EquipmentType.Head:
                if (headDictionary.ContainsKey(eq.ItemID))
                    headDictionary[eq.ItemID].NumberOfItems--;
                break;
            case EquipmentType.Decoration:
                if (decorationDictionary.ContainsKey(eq.ItemID))
                    decorationDictionary[eq.ItemID].NumberOfItems--;
                break;
            case EquipmentType.Armor:
                if (armorDictionary.ContainsKey(eq.ItemID))
                    armorDictionary[eq.ItemID].NumberOfItems--;
                break;
            case EquipmentType.Leg:
                if (legsDictionary.ContainsKey(eq.ItemID))
                    legsDictionary[eq.ItemID].NumberOfItems--;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Gives a list with all items available to the player.
    /// </summary>
    /// <param name="type">The type of equipment you need.</param>
    /// <returns>The list of items the palyer already ownes.</returns>
    public List<Equipment> GetEquipmentList(EquipmentType type)
    {
        List<Equipment> tmp = new List<Equipment>();

        switch (type)
        {
            case EquipmentType.Head:
                for (int i = 0; i < headEquipments.Length; i++)
                    if (headEquipments[i].NumberOfItems > 0)
                        tmp.Add(headEquipments[i]);
                break;
            case EquipmentType.Decoration:
                for (int i = 0; i < decorationEquipments.Length; i++)
                    if (decorationEquipments[i].NumberOfItems > 0)
                        tmp.Add(decorationEquipments[i]);
                break;
            case EquipmentType.Armor:
                for (int i = 0; i < armorEquipments.Length; i++)
                    if (armorEquipments[i].NumberOfItems > 0)
                        tmp.Add(armorEquipments[i]);
                break;
            case EquipmentType.Leg:
                for (int i = 0; i < legEquipments.Length; i++)
                    if (legEquipments[i].NumberOfItems > 0)
                        tmp.Add(legEquipments[i]);
                break;
            default:
                break;
        }
        return tmp;
    }


    public int GetNumberOfItems(Equipment eq, Weapon w = null)
    {
        if (w != null)
        {
            if (WeaponDictionary.ContainsKey(w.ItemID))
                return WeaponDictionary[w.ItemID].NumberOfItems;
            else
                return 0;
        }
        else
        {
            switch (eq.Type)
            {
                case EquipmentType.Head:
                    if (headDictionary.ContainsKey(eq.ItemID))
                        return headDictionary[eq.ItemID].NumberOfItems;
                    break;
                case EquipmentType.Decoration:
                    if (decorationDictionary.ContainsKey(eq.ItemID))
                        return decorationDictionary[eq.ItemID].NumberOfItems;
                    break;
                case EquipmentType.Armor:
                    if (armorDictionary.ContainsKey(eq.ItemID))
                        return armorDictionary[eq.ItemID].NumberOfItems;
                    break;
                case EquipmentType.Leg:
                    if (legsDictionary.ContainsKey(eq.ItemID))
                        return legsDictionary[eq.ItemID].NumberOfItems;
                    break;
                default:
                    break;
            }
            return 0;
        }
        
    }

    /// <summary>
    /// Adds a weapon.
    /// </summary>
    /// <param name="w"></param>
    public void AddWeapon(Weapon w)
    {
        if (weaponDictionary.ContainsKey(w.ItemID))
            weaponDictionary[w.ItemID].NumberOfItems++;
    }

    /// <summary>
    /// Removes a weapon.
    /// </summary>
    /// <param name="w"></param>
    public void RemoveWeapon(Weapon w)
    {
        if (weaponDictionary.ContainsKey(w.ItemID))
            weaponDictionary[w.ItemID].NumberOfItems--;
    }

    public List<Weapon> GetWeaponList(WeaponType[] type)
    {
        List<Weapon> tmp = new List<Weapon>();

        for (int i = 0; i < type.Length; i++)
        {
            for (int j = 0; j < weapons.Length; j++)
            {
                if(weapons[j].Type == type[i] && weaponDictionary.ContainsKey(weapons[j].ItemID) && weaponDictionary[weapons[j].ItemID].NumberOfItems > 0)
                    tmp.Add(weapons[j]);
            }
        }
        return tmp;
    }

    public Equipment GetRandomEquipment(int type, int tier)
    {
        switch (type)
        {
            case 0:
                if (tier == 1)
                    return t1H[Random.Range(0, t1H.Length)];
                else if(tier == 2)
                    return t2H[Random.Range(0, t2H.Length)];
                else if (tier == 3)
                    return t3H[Random.Range(0, t3H.Length)];
                break;
            case 1:
                if (tier == 1)
                    return t1D[Random.Range(0, t1D.Length)];
                else if (tier == 2)
                    return t2D[Random.Range(0, t2D.Length)];
                else if (tier == 3)
                    return t3D[Random.Range(0, t3D.Length)];
                break;
            case 2:
                if (tier == 1)
                    return t1A[Random.Range(0, t1A.Length)];
                else if (tier == 2)
                    return t2A[Random.Range(0, t2A.Length)];
                else if (tier == 3)
                    return t3A[Random.Range(0, t3A.Length)];
                break;
            case 3:
                if (tier == 1)
                    return t1B[Random.Range(0, t1B.Length)];
                else if (tier == 2)
                    return t2B[Random.Range(0, t2B.Length)];
                else if (tier == 3)
                    return t3B[Random.Range(0, t3B.Length)];
                break;
            default:
                break;
        }
        Debug.Log("No eqipment?");
        return null;
    }

    public Weapon GetRandomWeapon(int tier)
    {
        if (tier == 1)
            return t1W[Random.Range(0, t1W.Length)];
        else
            return t2W[Random.Range(0, t2W.Length)];
    }

}
