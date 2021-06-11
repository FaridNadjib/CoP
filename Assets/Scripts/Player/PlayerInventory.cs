using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// These are the mightcrystals existing in this game. Later added gold too.
/// </summary>
public enum Crystal { Gold, Attack, Darkness, Fire, Ice, Lightning, Wind, Destruction, Holy, Hunter, Seadragon, None }

/// <summary>
/// This class represents what the player has in his inventory.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    #region Fields

    private int[] inventoryCrystals;

    private List<GameObject> allChampions;
    private List<GameObject> currentChampions;

    [SerializeField] private GameObject test;

    [Header("Championholder:")]
    [SerializeField] private Transform championHolder;

    #endregion Fields

    #region Properties

    public List<GameObject> CurrentChampions { get => currentChampions; set => currentChampions = value; }
    public int[] InventoryCrystals { get => inventoryCrystals; set => inventoryCrystals = value; }
    public List<GameObject> AllChampions { get => allChampions; set => allChampions = value; }
    public Transform ChampionHolder { get => championHolder;}

    #endregion Properties

    #region Singleton

    public static PlayerInventory Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion Singleton

    // Start is called before the first frame update
    private void Start()
    {
        // Set the inventory crystals.
        inventoryCrystals = new int[System.Enum.GetNames(typeof(Crystal)).Length];

        currentChampions = new List<GameObject>();

        GameObject tmp = Instantiate(test, championHolder);
        tmp.GetComponent<Champion>().IsPlayer = true;
        tmp.GetComponent<Champion>().InitializeChampion();
        tmp.SetActive(false);
        currentChampions.Add(tmp);

        tmp = Instantiate(test, championHolder);
        tmp.GetComponent<Champion>().IsPlayer = true;
        tmp.GetComponent<Champion>().InitializeChampion();
        tmp.SetActive(false);
        currentChampions.Add(tmp);

        tmp = Instantiate(test, championHolder);
        tmp.GetComponent<Champion>().IsPlayer = true;
        tmp.GetComponent<Champion>().InitializeChampion();
        tmp.SetActive(false);
        currentChampions.Add(tmp);
    }

    /// <summary>
    /// Adds the item to the playerinventory.
    /// </summary>
    /// <param name="crystal">The crystal to add.</param>
    /// <param name="amount">The amount.</param>
    public void AddCrystals(Crystal crystal, int amount)
    {
        inventoryCrystals[(int)crystal] += amount;
    }

    /// <summary>
    /// Checks if the player has enough currency.
    /// </summary>
    /// <param name="amount">The amount to check for.</param>
    /// <returns>True if the player has enough currency.</returns>
    public bool HasCurrency(Crystal crystal, int amount)
    {
        return inventoryCrystals[(int)crystal] >= amount;
    }

    /// <summary>
    /// Spends the currency.
    /// </summary>
    /// <param name="crystal">The type to spend.</param>
    /// <param name="amount">The amount.</param>
    public void SpendCurrency(Crystal crystal, int amount)
    {
        inventoryCrystals[(int)crystal] -= amount;
    }
}