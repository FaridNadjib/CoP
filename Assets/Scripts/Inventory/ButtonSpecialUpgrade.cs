using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSpecialUpgrade : MonoBehaviour
{
    [Header("The special upgrade values displayed on the button:")]
    [SerializeField] private TextMeshProUGUI des;


    /// <summary>
    /// Sets the shop button UI.
    /// </summary>
    /// <param name="champ"></param>
    public void InitializeButton(Champion champ, SpecialUpgrades spec)
    {
        int index = 0;
        for (int i = 0; i < champ.SpecialUpgradeTypes.Length; i++)
        {
            if (champ.SpecialUpgradeTypes[i].SpecialUpgradeType == spec)
            {
                index = i;
                break;
            }
        }


        switch (spec)
        {
            case SpecialUpgrades.Health:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"HP + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"HP + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.HealthRecov:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Lebensregeneration + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Lebensregeneration + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.HealthGR:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Lebenszuwachs pro Levelaufstieg + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Lebenszuwachs pro Levelaufstieg + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.Energy:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Energie + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Energie + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.EnergyRecov:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Energieregeneration + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Energieregeneration + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.EneryGR:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Energiezuwachs pro Levelaufstieg + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Energiezuwachs pro Levelaufstieg + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.CritMulti:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Kritischer Schadensmultiplier + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Kritischer Schadensmultiplier + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.CitChance:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Kritische Trefferchance + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Kritische Trefferchance + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.DmgReduction:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Schadensreduktion bei erfolgreichem Block + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Schadensreduktion bei erfolgreichem Block + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.EvasionChance:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Ausweichchance + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Ausweichchance + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.Initiative:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Initiative + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Initiative + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.Defense:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Verteidigung in allen Biomen (Main und Sub) + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Verteidigung in allen Biomen (Main und Sub) + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.Resistance:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Alle Resistenzen + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Alle Resistenzen + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.LessXp:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Weniger XP benötigt - {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Weniger XP benötigt - {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.MoreSkillPoints:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Mehr Skillpunkte pro Levelaufstieg + {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Mehr Skillpunkte pro Levelaufstieg + {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            case SpecialUpgrades.LessSpecialRequirement:
                if (champ.SpecialUpgradeTypes[index].IsPercentage)
                    des.text = $"Geringeres Spezialmeter - {(int)(champ.SpecialUpgradeTypes[index].Amount * 100)}% .";
                else
                    des.text = $"Geringeres Spezialmeter - {(int)champ.SpecialUpgradeTypes[index].Amount}.";
                break;
            default:
                break;
        }
        
    }
}
