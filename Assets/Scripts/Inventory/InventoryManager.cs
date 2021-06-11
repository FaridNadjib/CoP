using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Head, Decoration, Armor, Leg }
public enum DamageType { None, Blade, Pierce, Impact, Arcane, Fire, Ice, Thunder, Storm }
public enum DefenseType { None, Paved, Desert, Grass, Ocean, Ice, Forest, Swamp, Mountain }
public enum StatsBuffType { None, Health, HealthRecovery, Energy, EnergyRecovery, BonusExp, CritChance, CritMult, DmgReduction, EvasionChance, Initiative }
public enum WeaponType { All, Weapon, Magic, Claw, Tooth, Tail, Wing, Body, Special }
public enum WeaponTarget { All, Ally, Allys, Enemy, Enemies }
public enum WeaponEffect { Lifeleech, Energyleech, HealthRecovery, EnergyRecovery, Initiative, Defense}
public enum SpecialRequirement { None, Health, Energy, HealthRec, EnergyRec, XpGain, DamageDealt }


public class InventoryManager : MonoBehaviour
{
    [SerializeField] Equipment[] headEquipments;
    [SerializeField] Equipment[] decorationEquipments;
    [SerializeField] Equipment[] armorEquipments;
    [SerializeField] Equipment[] legEquipments;

    Dictionary<int, Equipment> headDictionary;
    Dictionary<int, Equipment> decorationDictionary;
    Dictionary<int, Equipment> armorDictionary;
    Dictionary<int, Equipment> legsDictionary;


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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateEquipmentDatabase()
    {
        headDictionary = new Dictionary<int, Equipment>();
        decorationDictionary = new Dictionary<int, Equipment>();
        armorDictionary = new Dictionary<int, Equipment>();
        legsDictionary = new Dictionary<int, Equipment>();

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


    }

    public void AddEquipment(Equipment eq)
    {
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
}
