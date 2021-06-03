using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    [SerializeField] GameObject pavedArea1;
    [SerializeField] GameObject pavedArea2;
    [SerializeField] GameObject desert1;
    [SerializeField] GameObject desert2;
    [SerializeField] GameObject grassland1;
    [SerializeField] GameObject grassland2;
    [SerializeField] GameObject ocean1;
    [SerializeField] GameObject ocean2;
    [SerializeField] GameObject ice1;
    [SerializeField] GameObject ice2;
    [SerializeField] GameObject forest1;
    [SerializeField] GameObject forest2;
    [SerializeField] GameObject swamp1;
    [SerializeField] GameObject swamp2;
    [SerializeField] GameObject mountain1;
    [SerializeField] GameObject mountain2;
    [SerializeField] GameObject mountain3;
    [SerializeField] GameObject mountain4;

    [SerializeField] Transform enemyHolder;

    DefenseType currentMainBiom;
    DefenseType currentSubBiom;
    Transform[] spawnPositionsLeft;
    Transform[] spawnPositionsRight;

    List<GameObject> enemyTeam;
    List<Champion> championsInBattle;

    // Reward related:
    int battleRewardGold;
    float battleRewardExp;
    float levelDifference;
    List<MightCrystalLevel> battleRewardCrystals;

    bool inCombat;
    int currentTurn = 0;

    #region Singleton
    public static BattleManager Instance;

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
        enemyTeam = new List<GameObject>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // The actual battleloop: is continues til one team is dead.
        if (inCombat)
        {
            Debug.Log("In combat now.");
            championsInBattle[currentTurn].SelectChampion(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                championsInBattle[currentTurn].SelectChampion(false);
                currentTurn++;
            }
        }
    }

    public void StartCombat(Biom[] possibleBioms, GameObject[] enemyTeam)
    {
        // Set the map. Eventually check for null otherwise error.
        int r = Random.Range(0, possibleBioms.Length);       
        SetBattleMap(possibleBioms[r]);

        // Place the playerchampions.
        if(PlayerInventory.Instance.CurrentChampions.Count == 2)
        {
            PlayerInventory.Instance.CurrentChampions[0].transform.position = spawnPositionsLeft[1].position;
            PlayerInventory.Instance.CurrentChampions[1].transform.position = spawnPositionsLeft[2].position;
        }
        else
        {
            for (int i = 0; i < PlayerInventory.Instance.CurrentChampions.Count; i++)
            {
                PlayerInventory.Instance.CurrentChampions[i].transform.position = spawnPositionsLeft[i].position;
            }
        }
        // Instantiate and place the enemy champions.
        this.enemyTeam.Clear();
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            GameObject tmp = Instantiate(enemyTeam[i], enemyHolder);
            this.enemyTeam.Add(tmp);
            
        }
        if (this.enemyTeam.Count == 2)
        {
            this.enemyTeam[0].transform.position = spawnPositionsRight[1].position;
            this.enemyTeam[1].transform.position = spawnPositionsRight[2].position;
        }
        else
        {
            for (int i = 0; i < this.enemyTeam.Count; i++)
            {
                this.enemyTeam[i].transform.position = spawnPositionsRight[i].position;
            }
        }

        

        // Get all participating champions in a sorted list, then start the battle loop.
        GetChampionsInBattle();

    }




    private void SetBattleMap(Biom biom)
    {
        // Set a default value for the map.
        BattleBiom tmpBiom = grassland1.GetComponent<BattleBiom>();

        switch (biom)
        {
            case Biom.PavedArea1:
                tmpBiom = pavedArea1.GetComponent<BattleBiom>();
                pavedArea1.SetActive(true);
                break;
            case Biom.PavedArea2:
                tmpBiom = pavedArea2.GetComponent<BattleBiom>();
                pavedArea2.SetActive(true);
                break;
            case Biom.Desert1:
                tmpBiom = desert1.GetComponent<BattleBiom>();
                desert1.SetActive(true);
                break;
            case Biom.Desert2:
                tmpBiom = desert2.GetComponent<BattleBiom>();
                desert2.SetActive(true);
                break;
            case Biom.Grassland1:
                tmpBiom = grassland1.GetComponent<BattleBiom>();
                grassland1.SetActive(true);
                break;
            case Biom.GrassLand2:
                tmpBiom = grassland2.GetComponent<BattleBiom>();
                grassland2.SetActive(true);
                break;
            case Biom.Ocean1:
                tmpBiom = ocean1.GetComponent<BattleBiom>();
                ocean1.SetActive(true);
                break;
            case Biom.Ocean2:
                tmpBiom = ocean2.GetComponent<BattleBiom>();
                ocean2.SetActive(true);
                break;
            case Biom.Ice1:
                tmpBiom = ice1.GetComponent<BattleBiom>();
                ice1.SetActive(true);
                break;
            case Biom.Ice2:
                tmpBiom = ice2.GetComponent<BattleBiom>();
                ice2.SetActive(true);
                break;
            case Biom.Forest1:
                tmpBiom = forest1.GetComponent<BattleBiom>();
                forest1.SetActive(true);
                break;
            case Biom.Forest2:
                tmpBiom = forest2.GetComponent<BattleBiom>();
                forest2.SetActive(true);
                break;
            case Biom.Swamp1:
                tmpBiom = swamp1.GetComponent<BattleBiom>();
                swamp1.SetActive(true);
                break;
            case Biom.Swamp2:
                tmpBiom = swamp2.GetComponent<BattleBiom>();
                swamp2.SetActive(true);
                break;
            case Biom.Mountain1:
                tmpBiom = mountain1.GetComponent<BattleBiom>();
                mountain1.SetActive(true);
                break;
            case Biom.Mountain2:
                tmpBiom = mountain2.GetComponent<BattleBiom>();
                mountain2.SetActive(true);
                break;
            case Biom.Mountain3:
                tmpBiom = mountain3.GetComponent<BattleBiom>();
                mountain3.SetActive(true);
                break;
            case Biom.Mountain4:
                tmpBiom = mountain4.GetComponent<BattleBiom>();
                mountain4.SetActive(true);
                break;
            default:
                break;

        }
        currentMainBiom = tmpBiom.MainBiom;
        currentSubBiom = tmpBiom.SubBiom;
        spawnPositionsLeft = tmpBiom.LeftSpawnPositions;
        spawnPositionsRight = tmpBiom.RightSpawnPositions;
    }

    private void DeactivateBattleMaps()
    {
        pavedArea1.SetActive(false);
        pavedArea2.SetActive(false);
        desert1.SetActive(false);
        desert2.SetActive(false);
        grassland1.SetActive(false);
        grassland2.SetActive(false);
        ocean1.SetActive(false);
        ocean2.SetActive(false);
        ice1.SetActive(false);
        ice2.SetActive(false);
        forest1.SetActive(false);
        forest2.SetActive(false);
        swamp1.SetActive(false);
        swamp2.SetActive(false);
        mountain1.SetActive(false);
        mountain2.SetActive(false);
        mountain3.SetActive(false);
        mountain4.SetActive(false);
    }


    private void GetChampionsInBattle()
    {
        championsInBattle = new List<Champion>();
        
        // Save the champion scripts of all battle participants in one list for easy access.
        for (int i = 0; i < PlayerInventory.Instance.CurrentChampions.Count; i++)
        {
            championsInBattle.Add(PlayerInventory.Instance.CurrentChampions[i].GetComponent<Champion>());
        }
        for (int i = 0; i < enemyTeam.Count; i++)
        {
            championsInBattle.Add(enemyTeam[i].GetComponent<Champion>());
        }
        // Flip the facing of enemies.
        for (int i = 0; i < championsInBattle.Count; i++)
        {
            if (!championsInBattle[i].IsPlayer)
                championsInBattle[i].FlipChampion();           
        }
        // Sort the current list by the champions initiative.
        List<Champion> sortetChampions = championsInBattle.OrderByDescending(champ => champ.Initiative).ToList();
        championsInBattle = sortetChampions;

        // Set the defense of all champions based on the current biome.
        for (int i = 0; i < championsInBattle.Count; i++)
            championsInBattle[i].SetChampionDefense(currentMainBiom, currentSubBiom);

        // Calculate the battlereward.


        // Everything prepared, set in combat to true to start the main battleloop in update. Reset the turn counter.
        inCombat = true;
        currentTurn = 0;
    }

    private void SortChampions()
    {
        // Sort the current list by the champions initiative.
        List<Champion> sortetChampions = championsInBattle.OrderByDescending(champ => champ.Initiative).ToList();
        championsInBattle = sortetChampions;
    }

    private void CalculateBattleReward()
    {
        battleRewardGold = 0;
        battleRewardExp = 0.0f;
        battleRewardCrystals = new List<MightCrystalLevel>();
        bool inList;
        
        for (int i = 0; i < championsInBattle.Count; i++)
        {
            if (!championsInBattle[i].IsPlayer)
            {
                battleRewardGold += championsInBattle[i].GoldReward;
                battleRewardExp += championsInBattle[i].ExpReward;
                //inList = false;
                //for (int j = 0; j < championsInBattle[i].CrystalReward.Length; j++)
                //{
                //    for (int k = 0; k < battleRewardCrystals.Count; k++)
                //    {
                //        if (championsInBattle[i].CrystalReward[j].MightCrystal == battleRewardCrystals[k].MightCrystal)
                //        {
                //            inList = true;
                //            battleRewardCrystals[k].Amount += championsInBattle[i].CrystalReward[j].Amount;
                //        }

                //    }
                //}
            }
        }
    }

}
