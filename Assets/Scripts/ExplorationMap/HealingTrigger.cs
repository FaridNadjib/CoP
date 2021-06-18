using UnityEngine;

/// <summary>
/// Represents a healing area.
/// </summary>
public class HealingTrigger : MonoBehaviour
{
    /// <summary>
    /// Heals all champions and sets the new safelocation.
    /// </summary>
    public void StartHealing()
    {
        for (int i = 0; i < PlayerInventory.Instance.CurrentChampions.Count; i++)
            PlayerInventory.Instance.CurrentChampions[i].GetComponent<Champion>().FullHealChampion();

        PlayerController.Instance.SetSafeSpotLocation(transform.position);

        PlayerController.Instance.StandStill();
        UIManager.Instance.ShowHealingPanel(true);
    }
}