using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class initializes the UI of a shop equipment button.
/// </summary>
public class ButtonEquipment : MonoBehaviour
{
    [Header("Euipment UI general related:")]
    [SerializeField] private TextMeshProUGUI itemName;

    [SerializeField] private Image[] devotionImage;
    [SerializeField] private TextMeshProUGUI[] devotionAmountText;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Image equipmentIcon;

    [Header("Stats related:")]
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private TextMeshProUGUI healthRecText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI energyRecText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI critChanceText;
    [SerializeField] private TextMeshProUGUI critMultText;
    [SerializeField] private TextMeshProUGUI dmgReducText;
    [SerializeField] private TextMeshProUGUI evasionText;
    [SerializeField] private TextMeshProUGUI initiativeText;

    [SerializeField] private TextMeshProUGUI[] resistancesText;
    [SerializeField] private TextMeshProUGUI[] defensesText;

    [SerializeField] GameObject hidePanel;
    [SerializeField] TextMeshProUGUI numberOfItems;

    /// <summary>
    /// Sets the UI of the button.
    /// </summary>
    /// <param name="eq"></param>
    public void InitializeEquipmentButton(Equipment eq)
    {
        if (eq != null)
            hidePanel.SetActive(false);
        else
        {
            hidePanel.SetActive(true);
            return;
        }

        if (numberOfItems != null)
            numberOfItems.text = $"{InventoryManager.Instance.GetNumberOfItems(eq)}";


        itemName.text = eq.ItemName;
        // Eventually reset them, so they started out zerod.
        for (int i = 0; i < devotionImage.Length; i++)
        {
            devotionImage[i].gameObject.SetActive(false);
            devotionAmountText[i].text = "";
        }
        for (int i = 0; i < eq.DevotionBonus.Length; i++)
        {
            if (eq.DevotionBonus[i].Devotion != Starsign.None)
            {
                devotionImage[i].gameObject.SetActive(true);
                devotionImage[i].sprite = UIManager.Instance.StarSigns[(int)eq.DevotionBonus[i].Devotion];
                devotionImage[i].color = UIManager.Instance.StarsSignColors[(int)eq.DevotionBonus[i].Devotion];
                devotionAmountText[i].text = $"x{eq.DevotionBonus[i].DevotionAmount}";
                devotionAmountText[i].color = UIManager.Instance.StarsSignColors[(int)eq.DevotionBonus[i].Devotion];
            }
        }

        if(price != null)
            price.text = $"{eq.Price} Gold";
        equipmentIcon.sprite = eq.ItemIcon;

        // All stats related things.
        healthText.gameObject.SetActive(false);
        healthRecText.gameObject.SetActive(false);
        energyText.gameObject.SetActive(false);
        energyRecText.gameObject.SetActive(false);
        expText.gameObject.SetActive(false);
        critChanceText.gameObject.SetActive(false);
        critMultText.gameObject.SetActive(false);
        dmgReducText.gameObject.SetActive(false);
        evasionText.gameObject.SetActive(false);
        initiativeText.gameObject.SetActive(false);
        for (int i = 0; i < eq.StatsBonus.Length; i++)
        {
            switch (eq.StatsBonus[i].StatsBuffType)
            {
                case StatsBuffType.None:
                    break;

                case StatsBuffType.Health:
                    healthText.text = $"Hp:+{eq.StatsBonus[i].StatsAmount}";
                    healthText.gameObject.SetActive(true);
                    break;

                case StatsBuffType.HealthRecovery:
                    healthRecText.text = $"Hp Rec:+{eq.StatsBonus[i].StatsAmount}";
                    healthRecText.gameObject.SetActive(true);
                    break;

                case StatsBuffType.Energy:
                    energyText.text = $"Energie:+{eq.StatsBonus[i].StatsAmount}";
                    energyText.gameObject.SetActive(true);
                    break;

                case StatsBuffType.EnergyRecovery:
                    energyRecText.text = $"Energie Rec:+{eq.StatsBonus[i].StatsAmount}";
                    energyRecText.gameObject.SetActive(true);
                    break;

                case StatsBuffType.BonusExp:
                    expText.text = $"Bonus Exp:+{(int)(eq.StatsBonus[i].StatsAmount * 100)}%";
                    expText.gameObject.SetActive(true);
                    break;

                case StatsBuffType.CritChance:
                    critChanceText.text = $"Krit Chance:+{(int)(eq.StatsBonus[i].StatsAmount * 100)}%";
                    critChanceText.gameObject.SetActive(true);
                    break;

                case StatsBuffType.CritMult:
                    critMultText.text = $"Krit Schaden:+{(int)(eq.StatsBonus[i].StatsAmount * 100)}%";
                    critMultText.gameObject.SetActive(true);
                    break;

                case StatsBuffType.DmgReduction:
                    dmgReducText.text = $"Blocken:+{(int)(eq.StatsBonus[i].StatsAmount * 100)}%";
                    dmgReducText.gameObject.SetActive(true);
                    break;

                case StatsBuffType.EvasionChance:
                    evasionText.text = $"Ausweichen:+{(int)(eq.StatsBonus[i].StatsAmount * 100)}%";
                    evasionText.gameObject.SetActive(true);
                    break;

                case StatsBuffType.Initiative:
                    initiativeText.text = $"Initiative:+{eq.StatsBonus[i].StatsAmount}";
                    initiativeText.gameObject.SetActive(true);
                    break;

                default:
                    break;
            }
        }

        // Resistances.
        for (int i = 0; i < resistancesText.Length; i++)
            resistancesText[i].gameObject.SetActive(false);
        for (int i = 0; i < eq.ResistanceBonus.Length; i++)
        {
            resistancesText[i].gameObject.SetActive(true);
            switch (eq.ResistanceBonus[i].ResistanceType)
            {
                case DamageType.None:
                    break;

                case DamageType.Blade:
                    resistancesText[i].text = $"Schwert: {(int)(eq.ResistanceBonus[i].ResistanceAmount * 100)}%";
                    break;

                case DamageType.Pierce:
                    resistancesText[i].text = $"Stich: {(int)(eq.ResistanceBonus[i].ResistanceAmount * 100)}%";
                    break;

                case DamageType.Impact:
                    resistancesText[i].text = $"Wucht: {(int)(eq.ResistanceBonus[i].ResistanceAmount * 100)}%";
                    break;

                case DamageType.Arcane:
                    resistancesText[i].text = $"Arkan: {(int)(eq.ResistanceBonus[i].ResistanceAmount * 100)}%";
                    break;

                case DamageType.Fire:
                    resistancesText[i].text = $"Feuer: {(int)(eq.ResistanceBonus[i].ResistanceAmount * 100)}%";
                    break;

                case DamageType.Ice:
                    resistancesText[i].text = $"Eis: {(int)(eq.ResistanceBonus[i].ResistanceAmount * 100)}%";
                    break;

                case DamageType.Thunder:
                    resistancesText[i].text = $"Donner: {(int)(eq.ResistanceBonus[i].ResistanceAmount * 100)}%";
                    break;

                case DamageType.Storm:
                    resistancesText[i].text = $"Sturm: {(int)(eq.ResistanceBonus[i].ResistanceAmount * 100)}%";
                    break;

                default:
                    break;
            }
        }
        // Defenses.
        for (int i = 0; i < defensesText.Length; i++)
            defensesText[i].gameObject.SetActive(false);
        for (int i = 0; i < eq.DefenseMainBonus.Length; i++)
        {
            switch (eq.DefenseMainBonus[i].DefenseType)
            {
                case DefenseType.None:
                    break;

                case DefenseType.Paved:
                    defensesText[i].text = $"Befestigt: {(int)(eq.DefenseMainBonus[i].DefenseAmount * 100)}%({(int)(eq.DefenseSubBonus[i].DefenseAmount * 100)}%)";
                    break;

                case DefenseType.Desert:
                    defensesText[i].text = $"Wüste: {(int)(eq.DefenseMainBonus[i].DefenseAmount * 100)}%({(int)(eq.DefenseSubBonus[i].DefenseAmount * 100)}%)";
                    break;

                case DefenseType.Grass:
                    defensesText[i].text = $"Grasland: {(int)(eq.DefenseMainBonus[i].DefenseAmount * 100)}%({(int)(eq.DefenseSubBonus[i].DefenseAmount * 100)}%)";
                    break;

                case DefenseType.Ocean:
                    defensesText[i].text = $"Ozean: {(int)(eq.DefenseMainBonus[i].DefenseAmount * 100)}%({(int)(eq.DefenseSubBonus[i].DefenseAmount * 100)}%)";
                    break;

                case DefenseType.Ice:
                    defensesText[i].text = $"Eiswüste: {(int)(eq.DefenseMainBonus[i].DefenseAmount * 100)}%({(int)(eq.DefenseSubBonus[i].DefenseAmount * 100)}%)";
                    break;

                case DefenseType.Forest:
                    defensesText[i].text = $"Wald: {(int)(eq.DefenseMainBonus[i].DefenseAmount * 100)}%({(int)(eq.DefenseSubBonus[i].DefenseAmount * 100)}%)";
                    break;

                case DefenseType.Swamp:
                    defensesText[i].text = $"Sumpf: {(int)(eq.DefenseMainBonus[i].DefenseAmount * 100)}%({(int)(eq.DefenseSubBonus[i].DefenseAmount * 100)}%)";
                    break;

                case DefenseType.Mountain:
                    defensesText[i].text = $"Dungeon: {(int)(eq.DefenseMainBonus[i].DefenseAmount * 100)}%({(int)(eq.DefenseSubBonus[i].DefenseAmount * 100)}%)";
                    break;

                default:
                    break;
            }
            defensesText[i].gameObject.SetActive(true);
        }
    }

    public void UpdateItemNumbers(Equipment eq)
    {
        if (numberOfItems != null)
            numberOfItems.text = $"{InventoryManager.Instance.GetNumberOfItems(eq)}";
    }
}