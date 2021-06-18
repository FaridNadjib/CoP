using UnityEngine;

/// <summary>
/// Starsignbonus, which sign and how much devotion to it.
/// </summary>
[System.Serializable]
public struct StarSignBonus
{
    [SerializeField] private Starsign devotion;
    [SerializeField] private int devotionAmount;

    public Starsign Devotion { get => devotion; }
    public int DevotionAmount { get => devotionAmount; }
}

/// <summary>
/// Resiboni, type and amount.
/// </summary>
[System.Serializable]
public struct ResistanceBonus
{
    [SerializeField] private DamageType resistanceType;
    [SerializeField] private float resistanceAmount;

    public DamageType ResistanceType { get => resistanceType; }
    public float ResistanceAmount { get => resistanceAmount; set => resistanceAmount = value; }
}

/// <summary>
/// Defense boni, type and amount.
/// </summary>
[System.Serializable]
public struct DefenseBonus
{
    [SerializeField] private DefenseType defenseType;
    [SerializeField] private float defenseAmount;

    public DefenseType DefenseType { get => defenseType; }
    public float DefenseAmount { get => defenseAmount; set => defenseAmount = value; }
}

/// <summary>
/// Stats buff boni type and amount.
/// </summary>
[System.Serializable]
public struct StatsBuffBonus
{
    [SerializeField] private StatsBuffType statsBuffType;
    [SerializeField] private float statsAmount;

    public StatsBuffType StatsBuffType { get => statsBuffType; }
    public float StatsAmount { get => statsAmount; set => statsAmount = value; }
}

/// <summary>
/// This class represents an item in form of armor.
/// </summary>
[CreateAssetMenu(menuName = "Create Equipment")]
public class Equipment : ScriptableObject
{
    [Header("Basic Values:")]
    [SerializeField] private EquipmentType type;

    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int itemID;
    [SerializeField] private int price;
    private int numberOfItems;

    [Header("Special Values:")]
    [SerializeField] private StarSignBonus[] devotionBonus;

    [SerializeField] private StatsBuffBonus[] statsBonus;
    [SerializeField] private ResistanceBonus[] resistanceBonus;
    [SerializeField] private DefenseBonus[] defenseMainBonus;
    [SerializeField] private DefenseBonus[] defenseSubBonus;

    public int ItemID { get => itemID; }
    public int NumberOfItems { get => numberOfItems; set => numberOfItems = value; }
    public EquipmentType Type { get => type; }
    public StarSignBonus[] DevotionBonus { get => devotionBonus; }
    public StatsBuffBonus[] StatsBonus { get => statsBonus; set => statsBonus = value; }
    public ResistanceBonus[] ResistanceBonus { get => resistanceBonus; set => resistanceBonus = value; }
    public DefenseBonus[] DefenseMainBonus { get => defenseMainBonus; set => defenseMainBonus = value; }
    public DefenseBonus[] DefenseSubBonus { get => defenseSubBonus; set => defenseSubBonus = value; }
    public string ItemName { get => itemName; set => itemName = value; }
    public Sprite ItemIcon { get => itemIcon; set => itemIcon = value; }
    public int Price { get => price; set => price = value; }
}