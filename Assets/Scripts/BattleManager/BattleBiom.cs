using UnityEngine;

/// <summary>
/// This enum represents all possible bioms.
/// </summary>
public enum Biom
{
    None, PavedArea1, PavedArea2, Desert1, Desert2, Grassland1, GrassLand2, Ocean1, Ocean2, Ice1, Ice2, Forest1, Forest2, Swamp1, Swamp2,
    Mountain1, Mountain2, Mountain3, Mountain4
}

/// <summary>
/// This class represents a biom.
/// </summary>
public class BattleBiom : MonoBehaviour
{
    [SerializeField] private Biom map;
    [SerializeField] private DefenseType mainBiom;
    [SerializeField] private DefenseType subBiom;

    public Biom Map { get => map; }
    public DefenseType MainBiom { get => mainBiom; }
    public DefenseType SubBiom { get => subBiom; }
}