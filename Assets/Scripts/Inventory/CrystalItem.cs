using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the crystal items.
/// </summary>
[CreateAssetMenu(menuName ="Create Crystal")]
public class CrystalItem : ScriptableObject
{
    [SerializeField] Crystal type;
    [SerializeField] string crystalName;
    [SerializeField] int crystalPrice;
    [SerializeField] Sprite crystalSprite;

    public Crystal Type { get => type;}
    public string CrystalName { get => crystalName;}
    public int CrystalPrice { get => crystalPrice;}
    public Sprite CrystalSprite { get => crystalSprite;}
}
