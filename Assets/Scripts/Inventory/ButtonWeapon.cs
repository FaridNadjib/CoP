using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class will initialize the generated weapon buttons UI.
/// </summary>
public class ButtonWeapon : MonoBehaviour
{
    [Header("Base weapon Stats:")]
    [SerializeField] private TextMeshProUGUI itemName;

    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Image itemIcon;

    [SerializeField] private TextMeshProUGUI damageType;
    [SerializeField] private TextMeshProUGUI targetType;
    [SerializeField] private TextMeshProUGUI[] scaleText;
    [SerializeField] private Image[] scaleImage;
    [SerializeField] private TextMeshProUGUI energyCost;
    [SerializeField] private TextMeshProUGUI attacks;

    [SerializeField] private TextMeshProUGUI[] manifestationText;
    [SerializeField] private Image[] manifestationImage;
    [SerializeField] private TextMeshProUGUI[] specials;
    [SerializeField] private TextMeshProUGUI[] resistances;
    [SerializeField] private TextMeshProUGUI specialReq;

    [SerializeField] GameObject hidePanel;
    [SerializeField] TextMeshProUGUI numberOfItems;

    [SerializeField] GameObject specialMeter;
    [SerializeField] Image specialFill;
    [SerializeField] TextMeshProUGUI specialText;

    /// <summary>
    /// Sets the UI of the button.
    /// </summary>
    /// <param name="w"></param>
    public void InitializeWeaponButton(Weapon w, Champion c = null)
    {
        if (numberOfItems != null)
            numberOfItems.text = $"{InventoryManager.Instance.GetNumberOfItems(null, w)}";

        if (w != null)
            hidePanel.SetActive(false);
        else
        {
            hidePanel.SetActive(true);
            return;
        }


        itemName.text = $"{w.ItemName}";
        if(price != null)
            price.text = $"{w.Price} Gold";
        itemIcon.sprite = w.ItemIcon;

        
        // Damage types.
        for (int i = 0; i < w.DamageTypes.Length; i++)
        {
            if (i == 0)
                damageType.text = "Typ: ";
            switch (w.DamageTypes[i])
            {
                case DamageType.None:
                    damageType.text += "";
                    break;

                case DamageType.Blade:
                    damageType.text += "Schwert ";
                    break;

                case DamageType.Pierce:
                    damageType.text += "Stich ";
                    break;

                case DamageType.Impact:
                    damageType.text += "Wucht ";
                    break;

                case DamageType.Arcane:
                    damageType.text += "Arkan ";
                    break;

                case DamageType.Fire:
                    damageType.text += "Feuer ";
                    break;

                case DamageType.Ice:
                    damageType.text += "Eis ";
                    break;

                case DamageType.Thunder:
                    damageType.text += "Donner ";
                    break;

                case DamageType.Storm:
                    damageType.text += "Sturm ";
                    break;

                default:
                    break;
            }
            damageType.gameObject.SetActive(true);
        }

        // Targettype.
        targetType.text = $"Targets: {w.TargetType.ToString()}";
        targetType.gameObject.SetActive(true);

        // Scalelevels.
        for (int i = 0; i < w.ScaleLevels.Length; i++)
        {
            scaleImage[i].sprite = UIManager.Instance.GetCrystalSprite(w.ScaleLevels[i].MightCrystal);
            scaleImage[i].gameObject.SetActive(true);
            scaleText[i].gameObject.SetActive(true);
            scaleText[i].text = $"{w.ScaleLevels[i].Scale}";
        }

        energyCost.text = $"Energie: {w.EnergyCost}";
        if(c == null)
            attacks.text = $"{w.NumberOfAttacks} x {w.BaseAttack} Pow";
        else
            attacks.text = $"{w.NumberOfAttacks} x {(int)(c.GetWeaponPower(w))} Pow";

        // Crystal manifestations.
        for (int i = 0; i < w.CrystalManifestations.Length; i++)
        {
            manifestationImage[i].sprite = UIManager.Instance.GetCrystalSprite(w.CrystalManifestations[i].MightCrystal);
            manifestationImage[i].gameObject.SetActive(true);
            manifestationText[i].gameObject.SetActive(true);
            manifestationText[i].text = $" :{(int)(w.CrystalManifestations[i].ManifestationChance * 100)}%";
        }

        // Specials.
        for (int i = 0; i < w.WeaponSpecials.Length; i++)
        {
            switch (w.WeaponSpecials[i].EffectType)
            {
                case WeaponEffect.Lifeleech:
                    specials[i].text = $"Lebensentzug: {(int)(w.WeaponSpecials[i].Buff * 100)}%";
                    break;

                case WeaponEffect.Energyleech:
                    specials[i].text = $"Energieentzug: {(int)(w.WeaponSpecials[i].Buff * 100)}%";
                    break;

                case WeaponEffect.HealthRecovery:
                    if (w.WeaponSpecials[i].Buff > 0)
                        specials[i].text = $"Regeneration: {w.WeaponSpecials[i].Buff}HP :{w.WeaponSpecials[i].Duration}";
                    else
                        specials[i].text = $"Vergiftung: {w.WeaponSpecials[i].Buff}HP :{w.WeaponSpecials[i].Duration}";

                    break;

                case WeaponEffect.EnergyRecovery:
                    if (w.WeaponSpecials[i].Buff > 0)
                        specials[i].text = $"Vitalisierung: {w.WeaponSpecials[i].Buff}E :{w.WeaponSpecials[i].Duration}";
                    else
                        specials[i].text = $"Lähmung: {w.WeaponSpecials[i].Buff}E :{w.WeaponSpecials[i].Duration}";
                    break;

                case WeaponEffect.Initiative:
                    if (w.WeaponSpecials[i].Buff > 0)
                        specials[i].text = $"Eile: {w.WeaponSpecials[i].Buff} Init :{w.WeaponSpecials[i].Duration}";
                    else
                        specials[i].text = $"Verwurzeln: {w.WeaponSpecials[i].Buff} Init :{w.WeaponSpecials[i].Duration}";
                    break;

                case WeaponEffect.Defense:
                    specials[i].text = $"Verteidigung: {(int)(w.WeaponSpecials[i].Buff * 100)}% :{w.WeaponSpecials[i].Duration}";
                    break;

                default:
                    break;
            }
            specials[i].gameObject.SetActive(true);
        }

        // Resistance.
        for (int i = 0; i < w.ResistancesBuffs.Length; i++)
        {
            switch (w.ResistancesBuffs[i].ResistanceType)
            {
                case DamageType.None:
                    break;

                case DamageType.Blade:
                    resistances[i].text = $"Schwert: {(int)(w.ResistancesBuffs[i].Buff * 100)}% :{w.ResistancesBuffs[i].Duration}";
                    break;

                case DamageType.Pierce:
                    resistances[i].text = $"Stich: {(int)(w.ResistancesBuffs[i].Buff * 100)}% :{w.ResistancesBuffs[i].Duration}";
                    break;

                case DamageType.Impact:
                    resistances[i].text = $"Wucht: {(int)(w.ResistancesBuffs[i].Buff * 100)}% :{w.ResistancesBuffs[i].Duration}";
                    break;

                case DamageType.Arcane:
                    resistances[i].text = $"Arkan: {(int)(w.ResistancesBuffs[i].Buff * 100)}% :{w.ResistancesBuffs[i].Duration}";
                    break;

                case DamageType.Fire:
                    resistances[i].text = $"Feuer: {(int)(w.ResistancesBuffs[i].Buff * 100)}% :{w.ResistancesBuffs[i].Duration}";
                    break;

                case DamageType.Ice:
                    resistances[i].text = $"Eis: {(int)(w.ResistancesBuffs[i].Buff * 100)}% :{w.ResistancesBuffs[i].Duration}";
                    break;

                case DamageType.Thunder:
                    resistances[i].text = $"Donner: {(int)(w.ResistancesBuffs[i].Buff * 100)}% :{w.ResistancesBuffs[i].Duration}";
                    break;

                case DamageType.Storm:
                    resistances[i].text = $"Sturm: {(int)(w.ResistancesBuffs[i].Buff * 100)}% :{w.ResistancesBuffs[i].Duration}";
                    break;

                default:
                    break;
            }
            resistances[i].gameObject.SetActive(true);
        }

        // Specialreqs.
        
            switch (w.SpecialRequirement)
            {
                case SpecialRequirement.None:
                    specialReq.text = "";
                    break;
                case SpecialRequirement.Health:
                    specialReq.text = $"Spezial: Leben";
                    break;

                case SpecialRequirement.Energy:
                    specialReq.text = $"Spezial: Energie";
                    break;

                case SpecialRequirement.HealthRec:
                    specialReq.text = $"Spezial: Lebens Regeneration";
                    break;

                case SpecialRequirement.EnergyRec:
                    specialReq.text = $"Spezial: Energie Regeneration";
                    break;

                case SpecialRequirement.XpGain:
                    specialReq.text = $"Spezial: EXP";
                    break;

                case SpecialRequirement.DamageDealt:
                    specialReq.text = $"Spezial: Schaden";
                    break;

                default:
                    break;
            }
            specialReq.gameObject.SetActive(true);
        

        // Special meter, only show when weapon equiped.
        if(specialMeter != null && c != null)
        {
            specialMeter.SetActive(true);
            specialFill.fillAmount = c.GetSpecialMeterRatio();
            specialText.text = $"{(int)c.CurrentSpecialMeter} / {(int)(w.MaxSpecialMeter*c.SpecialReqMultiplier)}";
        }
    }

    public void UpdateItemNumbers(Weapon w)
    {
        if (numberOfItems != null)
            numberOfItems.text = $"{InventoryManager.Instance.GetNumberOfItems(null, w)}";
    }
}