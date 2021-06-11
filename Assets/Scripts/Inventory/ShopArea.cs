using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopArea : MonoBehaviour
{
    [SerializeField] string description;

    [SerializeField] Champion[] champions;

    [SerializeField] Equipment[] equipment;

    [SerializeField] Weapon[] weapons;

    public string Description { get => description; set => description = value; }
    public Champion[] Champions { get => champions; set => champions = value; }
    public Equipment[] Equipment { get => equipment; set => equipment = value; }
    public Weapon[] Weapons { get => weapons; set => weapons = value; }

    public void OpenShop()
    {
        Debug.Log("Shop is open now...");
        PlayerController.Instance.StandStill();
        UIManager.Instance.ShowShopScreen(this);

    }
}
