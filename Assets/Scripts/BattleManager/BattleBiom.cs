using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biom { None, PavedArea1, PavedArea2, Desert1, Desert2, Grassland1, GrassLand2, Ocean1, Ocean2, Ice1, Ice2, Forest1, Forest2, Swamp1, Swamp2,
Mountain1, Mountain2, Mountain3, Mountain4}

public class BattleBiom : MonoBehaviour
{
    [SerializeField] Biom map;
    [SerializeField] DefenseType mainBiom;
    [SerializeField] DefenseType subBiom;
    [SerializeField] Transform[] leftSpawnPositions;
    [SerializeField] Transform[] rightSpawnPositions;

    public Biom Map { get => map;}
    public DefenseType MainBiom { get => mainBiom;}
    public DefenseType SubBiom { get => subBiom;}
    public Transform[] LeftSpawnPositions { get => leftSpawnPositions;}
    public Transform[] RightSpawnPositions { get => rightSpawnPositions;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
