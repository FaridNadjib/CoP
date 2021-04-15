using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Crystals { Gold, Attack, Alignment, Fire, Ice, Lightning, Wind, Destruction, Holy, Hunter, Seadragon, None}

public class PlayerInventory : MonoBehaviour
{
    int gold;
    int attackCrystals;
    int alignmentCrystals;
    int fireCrystals;
    int iceCrystals;
    int lightningCrystals;
    int windCrystals;
    int destructionCrystals;
    int holyCrystals;
    int hunterCrystals;
    int seadrragonCrystals;


    #region Singleton
    public static PlayerInventory Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCrystals(Crystals crystal, int amount)
    {
        switch (crystal)
        {
            case Crystals.Gold:
                gold += amount;
                break;
            case Crystals.Attack:
                attackCrystals += amount;
                break;
            case Crystals.Alignment:
                alignmentCrystals += amount;
                break;
            case Crystals.Fire:
                fireCrystals += amount;
                break;
            case Crystals.Ice:
                iceCrystals += amount;
                break;
            case Crystals.Lightning:
                lightningCrystals += amount;
                break;
            case Crystals.Wind:
                windCrystals += amount;
                break;
            case Crystals.Destruction:
                destructionCrystals += amount;
                break;
            case Crystals.Holy:
                holyCrystals += amount;
                break;
            case Crystals.Hunter:
                hunterCrystals += amount;
                break;
            case Crystals.Seadragon:
                seadrragonCrystals += amount;
                break;
            default:
                break;
        }
    }
}
