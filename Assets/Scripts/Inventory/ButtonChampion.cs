using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class sets the UI for champion shop button.
/// </summary>
public class ButtonChampion : MonoBehaviour
{
    [Header("The champion values displayed in shop:")]
    [SerializeField] private TextMeshProUGUI championName;

    [SerializeField] private TextMeshProUGUI championPrice;
    [SerializeField] private TextMeshProUGUI championDescription;
    [SerializeField] private Image championSprite;
    [SerializeField] private Image championBorder;

    /// <summary>
    /// Sets the shop button UI.
    /// </summary>
    /// <param name="champ"></param>
    public void InitializeButton(Champion champ)
    {
        championName.text = $"{champ.ChampionName} Lvl:{champ.Level}";
        if(championPrice != null)
            championPrice.text = $"{champ.Price} Gold";
        championDescription.text = $"{champ.Description}";
        championSprite.sprite = champ.ChampionSprite.sprite;
        championBorder.sprite = ChampionManager.Instance.GetChampionBorder(Starsign.None);
    }
    
}