using UnityEngine;

/// <summary>
/// This class represents a shop.
/// </summary>
public class ShopArea : MonoBehaviour
{
    [SerializeField] private string description;

    [SerializeField] private GameObject[] champions;

    [SerializeField] private Equipment[] equipment;

    [SerializeField] private Weapon[] weapons;

    [SerializeField] private CrystalItem[] crystals;

    public string Description { get => description;}
    public GameObject[] Champions { get => champions;}
    public Equipment[] Equipment { get => equipment;}
    public Weapon[] Weapons { get => weapons;}
    public CrystalItem[] Crystals { get => crystals;}

    /// <summary>
    /// Called once the shop is activated.
    /// </summary>
    public void OpenShop()
    {
        PlayerController.Instance.StandStill();
        PlayerController.Instance.IsFighting = true;
        UIManager.Instance.ShowShopScreen(this);
    }
}