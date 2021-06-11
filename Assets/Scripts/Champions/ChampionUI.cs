using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class to show champions healthbars.
/// </summary>
public class ChampionUI : MonoBehaviour
{
    private Champion champ;
    [SerializeField] private Image healthFill;
    [SerializeField] private Image energyFill;

    // Start is called before the first frame update
    private void Awake()
    {
        champ = GetComponentInParent<Champion>();
        // SubScribe to the event from the champion this is attached to, to update the bar.
        champ.OnHealthChanged += (float ratio) => { healthFill.fillAmount = ratio; };
        champ.OnEnergyChanged += (float ratio) => { energyFill.fillAmount = ratio; };
    }
}