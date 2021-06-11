using UnityEngine;

/// <summary>
/// Special upgradetype.
/// </summary>
public enum SpecialUpgrades { Health, HealthRecov, HealthGR, Energy, EnergyRecov, EneryGR, CritMulti, CitChance, DmgReduction, EvasionChance, Initiative, Defense, Resistance, LessXp, MoreSkillPoints, LessSpecialRequirement }

/// <summary>
/// The chances a special upgrade gets available.
/// </summary>
[System.Serializable]
public struct SpecialUpgradeChances
{
    [SerializeField] private SpecialUpgrades specialUpgradeType;
    [SerializeField] private float chanceFrom;
    [SerializeField] private float chanceTo;
    [SerializeField] private float amount;
    [SerializeField] private bool isPercentage;

    public SpecialUpgrades SpecialUpgradeType { get => specialUpgradeType; }
    public float ChanceFrom { get => chanceFrom; }
    public float ChanceTo { get => chanceTo; }
    public float Amount { get => amount; set => amount = value; }
    public bool IsPercentage { get => isPercentage; }
}

/// <summary>
/// This class manages some base values of all champions as well as initializing them.
/// </summary>
public class ChampionManager : MonoBehaviour
{
    [Header("ChampionBorders:")]
    [Tooltip("Have to be in the same order like Starsign enum.")]
    [SerializeField] private Sprite[] borders;

    #region Singleton

    public static ChampionManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion Singleton

    /// <summary>
    /// Adds champion to player inventory as well as init him.
    /// </summary>
    /// <param name="champ">The champ to add.</param>
    public void AddChampionToInventory(GameObject champ)
    {
        GameObject tmp = Instantiate(champ, PlayerInventory.Instance.ChampionHolder);
        tmp.GetComponent<Champion>().IsPlayer = true;
        tmp.GetComponent<Champion>().InitializeChampion();
        tmp.SetActive(false);
        PlayerInventory.Instance.AllChampions.Add(tmp);
    }

    /// <summary>
    /// Gets the right border sprite based on devotion.
    /// </summary>
    /// <param name="sign"></param>
    /// <returns>The border sprite.</returns>
    public Sprite GetChampionBorder(Starsign sign)
    {
        return borders[(int)sign];
    }
}