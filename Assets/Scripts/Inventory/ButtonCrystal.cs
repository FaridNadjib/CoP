using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class sets the UI for generated CrystalItem buttons.
/// </summary>
public class ButtonCrystal : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI crystalName;
    [SerializeField] TextMeshProUGUI crystalPrice;
    [SerializeField] Image crystalImage;

    /// <summary>
    /// Inits the button UI.
    /// </summary>
    /// <param name="c">The crystal to show.</param>
    public void InitializeCrystalButton(CrystalItem c)
    {
        crystalName.text = c.CrystalName;
        crystalPrice.text = $"{c.CrystalPrice} Gold";
        crystalImage.sprite = c.CrystalSprite;
    }

}
