using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class will control how the battles will run, sort the enemys and watches the battleloop.
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region BattleMaps

    [Header("Battlemaps:")]
    [SerializeField] private GameObject[] battlemaps;

    private DefenseType currentMainBiom;
    private DefenseType currentSubBiom;
    [SerializeField] private Transform[] spawnPositionsLeft;
    [SerializeField] private Transform[] spawnPositionsRight;

    #endregion BattleMaps

    [Header("EnemyHolder:")]
    [SerializeField] private Transform enemyHolder;

    [SerializeField] private Transform attackEffectsHolder;

    [Header("EnemyHolder:")]
    [SerializeField] private string[] enemyActions;

    private NpcManager currentNpc;
    private List<GameObject> enemyTeam;
    private List<Champion> championsInBattle;

    // Reward related:
    private int battleRewardGold;

    private float battleRewardExp;
    private float levelDifference;
    private float levelPenalty = 0.13f;
    private int[] battleRewardCrystals;

    // Battleloop related:
    private bool inCombat;

    private int currentTurn = 0;
    private bool isWaiting;

    private bool playerWon;

    // Test.
    bool notInBattle = true;

    #region Properties

    public Transform AttackEffectsHolder { get => attackEffectsHolder; }
    public bool NotInBattle { get => notInBattle; set => notInBattle = value; }

    #endregion Properties

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

    #endregion Singleton

    // Start is called before the first frame update
    private void Start()
    {
        // Setup.
        enemyTeam = new List<GameObject>();
        battleRewardCrystals = new int[System.Enum.GetNames(typeof(Crystal)).Length];

        TransitionManager.Instance.OnBattleEndFading += (bool status) => { DeactivateBattleMaps(); };
    }

    // Update is called once per frame
    private void Update()
    {
        // The actual battleloop: is continues til one team is dead.
        if (inCombat)
        {
            if (!isWaiting)
            {
                if (championsInBattle[currentTurn].IsAlive)
                {
                    // Regenerate the champion.
                    championsInBattle[currentTurn].Regenerate();
                    // Did the champion died after regeneration?
                    if (!championsInBattle[currentTurn].IsAlive)
                    {
                        UIManager.Instance.ShowBattleScreenUIEnemy(championsInBattle[currentTurn]);
                        string m = $"{championsInBattle[currentTurn].ChampionName} died from poison.";
                        UIManager.Instance.ShowEnemyAction(m);
                        isWaiting = true;
                        UIManager.Instance.ShowContinueButton(true);
                    }
                    else
                    {
                        // Check if champion can make any moves, else skip him.
                        if (championsInBattle[currentTurn].HasMoveAvailable())
                        {
                            // Activate the next champion and ui based on his team.
                            if (championsInBattle[currentTurn].IsPlayer)
                                UIManager.Instance.ShowBattleScreenUIPlayer(championsInBattle[currentTurn]);
                            else
                            {
                                string m = $"{championsInBattle[currentTurn].ChampionName} {enemyActions[Random.Range(1, enemyActions.Length)]}";
                                UIManager.Instance.ShowEnemyAction(m);
                                UIManager.Instance.ShowBattleScreenUIEnemy(championsInBattle[currentTurn]);
                                // Let the ai play the enemys move.
                                AIAttackCalculation();
                            }

                            championsInBattle[currentTurn].SelectChampion(true);
                            isWaiting = true;
                        }
                        else
                        {
                            if (!championsInBattle[currentTurn].IsPlayer)
                            {
                                string m = $"{championsInBattle[currentTurn].ChampionName} muss sich ausruhen.";
                                UIManager.Instance.ShowEnemyAction(m);
                                UIManager.Instance.ShowBattleScreenUIEnemy(championsInBattle[currentTurn]);
                                UIManager.Instance.ShowContinueButton(true);
                                isWaiting = true;
                            }
                            else
                            {
                                // Show the UI though he has no moves, so he has to  skip the turn basically.
                                UIManager.Instance.ShowBattleScreenUIPlayer(championsInBattle[currentTurn]);
                            }
                        }
                    }
                }
                else
                    EndTurn();
            }
        }
    }

    /// <summary>
    /// Ends the turn and checks for victory conditions.
    /// </summary>
    public void EndTurn()
    {
        // Deactivate dead champions.
        for (int i = 0; i < championsInBattle.Count; i++)
            if (!championsInBattle[i].IsAlive)
                championsInBattle[i].gameObject.SetActive(false);
        // Check if battle is over.
        if (CheckBattleInProgress())
        {
            // Update the turncount.
            championsInBattle[currentTurn].SelectChampion(false);
            currentTurn++;
            // Full round complete, resort the list.
            if (currentTurn == championsInBattle.Count)
                currentTurn = 0;

            isWaiting = false;
        }
        else
        {
            // Handle what happens after the battle is over.
            if (playerWon)
            {
                // Calculate the reward.
                CalculateBattleReward();

                // Give the contenders list for ui.
                List<Champion> champs = new List<Champion>();
                for (int i = 0; i < championsInBattle.Count; i++)
                    if (championsInBattle[i].IsPlayer)
                        champs.Add(championsInBattle[i]);
                // Destroy ai champions.
                for (int i = 0; i < championsInBattle.Count; i++)
                    if (!championsInBattle[i].IsPlayer)
                        Destroy(championsInBattle[i].gameObject);
                    else
                        championsInBattle[i].gameObject.SetActive(false);

                // Add the reward to the champs and player.
                for (int i = 0; i < champs.Count; i++)
                {
                    if (champs[i].IsAlive)
                        champs[i].AddExperience(battleRewardExp);
                    else
                        champs[i].AddExperience(battleRewardExp * 0.5f);
                }
                battleRewardCrystals[0] = battleRewardGold;

                for (int i = 0; i < battleRewardCrystals.Length; i++)
                    PlayerInventory.Instance.AddCrystals((Crystal)i, battleRewardCrystals[i]);

                // Show the reward screen.
                UIManager.Instance.ShowRewardScreen(champs, battleRewardGold, battleRewardExp, battleRewardCrystals);
            }
            else
            {
                // The player lost, so heal all his champs and respawn him in last safespot.
                for (int i = 0; i < PlayerInventory.Instance.CurrentChampions.Count; i++)
                    PlayerInventory.Instance.CurrentChampions[i].GetComponent<Champion>().FullHealChampion();

                // Destroy ai champions.
                for (int i = 0; i < championsInBattle.Count; i++)
                    if (!championsInBattle[i].IsPlayer)
                        Destroy(championsInBattle[i].gameObject);
                    else
                        championsInBattle[i].gameObject.SetActive(false);
                // Respawn the player.
                UIManager.Instance.HideBattleUIScreen();
                TransitionManager.Instance.TeleportPlayer("", PlayerController.Instance.SafeSpotLocation, true, Transitions.Random);
                inCombat = false;
                PlayerController.Instance.IsFighting = false;
                NotInBattle = true;
                AudioManager.Instance.PlayMusic(0);
            }
        }
    }

    /// <summary>
    /// Ends the battle and brings player back to exploration map.
    /// </summary>
    public void EndBattle()
    {
        UIManager.Instance.HideBattleUIScreen();
        TransitionManager.Instance.TeleportPlayer("", PlayerController.Instance.gameObject.transform.position, true, Transitions.Random);
        inCombat = false;
        PlayerController.Instance.IsFighting = false;
        NotInBattle = true;
        AudioManager.Instance.PlayMusic(0);
        // Activate the npc if any.
        if (currentNpc != null)
        {
            currentNpc.FinishedCombat();
            currentNpc = null;
        }
    }

    #region AI related.

    /// <summary>
    /// Starts the coroutine for the ais turn.
    /// </summary>
    private void AIAttackCalculation()
    {
        StartCoroutine(AICalculation());
    }

    /// <summary>
    /// Performs the ais turn.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AICalculation()
    {
        yield return new WaitForSeconds(0.9f);

        Weapon w = null;
        Champion c = null;

        // Pick a random Weapon.
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < championsInBattle[currentTurn].Weapons.Length; j++)
            {
                if (championsInBattle[currentTurn].Weapons[j] != null && championsInBattle[currentTurn].Weapons[j].EnergyCost <= championsInBattle[currentTurn].CurrentEnergy)
                {
                    float r = Random.Range(0.0f, 1.0f);
                    if (r <= 0.2f)
                    {
                        w = championsInBattle[currentTurn].Weapons[j];
                        break;
                    }
                }
            }
            if (w != null)
                break;
        }
        // In case i couldnt find a target skip this turn.
        if (w == null)
        {
            UIManager.Instance.ShowEnemyAction("Setzt aus.");
            yield return new WaitForSeconds(1.0f);
            EndTurn();
            yield break;
        }

        // If singletarget pick a random target. Then call the corresponding attack method.
        if (w.TargetType == WeaponTarget.Ally || w.TargetType == WeaponTarget.Enemy)
        {
            if (w.TargetType == WeaponTarget.Ally)
            {
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < championsInBattle.Count; j++)
                    {
                        if (!championsInBattle[j].IsPlayer && championsInBattle[j].IsAlive)
                        {
                            float r = Random.Range(0.0f, 1.0f);
                            if (r <= 0.2f)
                                c = championsInBattle[j];
                            break;
                        }
                    }
                    if (c != null)
                        break;
                }
            }
            else if (w.TargetType == WeaponTarget.Enemy)
            {
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < championsInBattle.Count; j++)
                    {
                        if (championsInBattle[j].IsPlayer && championsInBattle[j].IsAlive)
                        {
                            float r = Random.Range(0.0f, 1.0f);
                            if (r <= 0.2f)
                                c = championsInBattle[j];
                            break;
                        }
                    }
                    if (c != null)
                        break;
                }
            }
            if (c == null)
            {
                UIManager.Instance.ShowEnemyAction("Setzt aus.");
                yield return new WaitForSeconds(1.0f);
                EndTurn();
                yield break;
            }
            // Display what the ai will do.
            string m = $"{championsInBattle[currentTurn].ChampionName} greift <color=blue>{c.ChampionName}</color> mit <color=red><size=40>{w.ItemName}</size></color> an.";
            UIManager.Instance.ShowEnemyAction(m);
            AttackSingle(c, w, false);
        }
        else
        {
            if (w == null)
            {
                UIManager.Instance.ShowEnemyAction("Setzt aus.");
                yield return new WaitForSeconds(1.0f);
                EndTurn();
                yield break;
            }
            // Display what the ai will do.
            string m = $"{championsInBattle[currentTurn].ChampionName} greift mit <color=red><size=40>{w.ItemName}</size></color> an.";
            UIManager.Instance.ShowEnemyAction(m);
            AIAttackMultiple(w);
        }
    }

    /// <summary>
    /// This method calls the champions attack method for multiple targets based on the weapon.
    /// </summary>
    /// <param name="weapon"></param>
    public void AIAttackMultiple(Weapon weapon)
    {
        // Pay energy cost once.
        championsInBattle[currentTurn].SpendEnergy(weapon.EnergyCost);

        // Then apply attack method to all targets.
        if (weapon.TargetType == WeaponTarget.Allys)
        {
            for (int i = 0; i < championsInBattle.Count; i++)
            {
                if (!championsInBattle[i].IsPlayer && championsInBattle[i].IsAlive)
                    championsInBattle[currentTurn].AttackMultiple(championsInBattle[i], weapon, false);
            }
        }
        else if (weapon.TargetType == WeaponTarget.Enemies)
        {
            for (int i = 0; i < championsInBattle.Count; i++)
            {
                if (championsInBattle[i].IsPlayer && championsInBattle[i].IsAlive)
                    championsInBattle[currentTurn].AttackMultiple(championsInBattle[i], weapon, false);
            }
        }
        else if (weapon.TargetType == WeaponTarget.All)
        {
            for (int i = 0; i < championsInBattle.Count; i++)
                if (championsInBattle[i].IsAlive && championsInBattle[i] != championsInBattle[currentTurn])
                    championsInBattle[currentTurn].AttackMultiple(championsInBattle[i], weapon, false);
        }

        // Wait some extra time and tell the player what the ai did.
        championsInBattle[currentTurn].AttackMultipleDelay(weapon, false);
    }

    #endregion AI related.

    /// <summary>
    /// Checks if the battle is going to continue.
    /// </summary>
    /// <returns>True if battle is going on.</returns>
    private bool CheckBattleInProgress()
    {
        bool playersAlive = false;
        bool enemiesAlive = false;
        for (int i = 0; i < championsInBattle.Count; i++)
            if (championsInBattle[i].IsPlayer && championsInBattle[i].IsAlive)
                playersAlive = true;
            else if (championsInBattle[i].IsAlive)
                enemiesAlive = true;

        // If enemies and players are alive, continue battle.
        if (playersAlive && enemiesAlive)
            return true;
        else if (playersAlive && !enemiesAlive)
        {
            // Won the battle.
            playerWon = true;
            return false;
        }
        else
        {
            // Lost the battle.
            playerWon = false;
            return false;
        }
    }

    #region Attack related.

    /// <summary>
    /// This method calls the champions attack methods based on the target type of the used weapn.
    /// </summary>
    /// <param name="weapon">The used weapon for that attack.</param>
    public void AttackMultiple(Weapon weapon)
    {
        // Pay energy cost once.
        championsInBattle[currentTurn].SpendEnergy(weapon.EnergyCost);

        // Then apply attack method to all targets.
        if (weapon.TargetType == WeaponTarget.Allys)
        {
            for (int i = 0; i < championsInBattle.Count; i++)
            {
                if (championsInBattle[i].IsPlayer && championsInBattle[i].IsAlive)
                    championsInBattle[currentTurn].AttackMultiple(championsInBattle[i], weapon);
            }
        }
        else if (weapon.TargetType == WeaponTarget.Enemies)
        {
            for (int i = 0; i < championsInBattle.Count; i++)
            {
                if (!championsInBattle[i].IsPlayer && championsInBattle[i].IsAlive)
                    championsInBattle[currentTurn].AttackMultiple(championsInBattle[i], weapon);
            }
        }
        else if (weapon.TargetType == WeaponTarget.All)
        {
            for (int i = 0; i < championsInBattle.Count; i++)
                if (championsInBattle[i].IsAlive && championsInBattle[i] != championsInBattle[currentTurn])
                    championsInBattle[currentTurn].AttackMultiple(championsInBattle[i], weapon);
        }

        // Inform when attacks will be over.
        championsInBattle[currentTurn].AttackMultipleDelay(weapon);
    }

    /// <summary>
    /// Attacks a single target.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="weapon">The weapon to use.</param>
    /// <param name="notAI">If its ai or not.</param>
    public void AttackSingle(Champion target, Weapon weapon, bool notAI = true)
    {
        championsInBattle[currentTurn].Attack(target, weapon, notAI);
    }

    #endregion Attack related.

    #region Targeting

    /// <summary>
    /// Sets which champions can be targeted.
    /// </summary>
    /// <param name="type"></param>
    public void EnableTargeting(WeaponTarget type)
    {
        if (type == WeaponTarget.Ally)
        {
            for (int i = 0; i < championsInBattle.Count; i++)
                if (championsInBattle[i].IsPlayer && championsInBattle[i].IsAlive)
                    championsInBattle[i].TargetingEnabled(true);
        }
        else if (type == WeaponTarget.Enemy)
        {
            for (int i = 0; i < championsInBattle.Count; i++)
                if (!championsInBattle[i].IsPlayer && championsInBattle[i].IsAlive)
                    championsInBattle[i].TargetingEnabled(true);
        }
    }

    /// <summary>
    /// Disables all targeting.
    /// </summary>
    public void DisableTargeting()
    {
        for (int i = 0; i < championsInBattle.Count; i++)
            championsInBattle[i].TargetingEnabled(false);
    }

    #endregion Targeting

    #region Battlemap related

    /// <summary>
    /// This class sets the battlemap and initializez the right bioms.
    /// </summary>
    /// <param name="biom"></param>
    private void SetBattleMap(Biom biom)
    {
        // Set a default value for the map.
        BattleBiom tmpBiom = battlemaps[(int)biom - 1].GetComponent<BattleBiom>();
        battlemaps[(int)biom - 1].SetActive(true);
        currentMainBiom = tmpBiom.MainBiom;
        currentSubBiom = tmpBiom.SubBiom;
    }

    /// <summary>
    /// This method deactivates all the battlemaps.
    /// </summary>
    private void DeactivateBattleMaps()
    {
        for (int i = 0; i < battlemaps.Length; i++)
            battlemaps[i].SetActive(false);
    }

    #endregion Battlemap related

    #region Battle setup related.

    /// <summary>
    /// This map will be called by triggers in the gameworld to start a battle.
    /// </summary>
    /// <param name="possibleBioms">The possible bioms the battle coult take place.</param>
    /// <param name="enemyTeam">The enemies to spawn.</param>
    public void StartCombat(Biom[] possibleBioms, Encounters enemyTeam, NpcManager npc = null)
    {
        // CHange the music.
        AudioManager.Instance.PlayMusic(1);

        // Save the current npc if any, to tell him when fight is over.
        currentNpc = npc;

        // Reset battlerewardcrystals.
        for (int i = 0; i < battleRewardCrystals.Length; i++)
            battleRewardCrystals[i] = 0;

        // Set the map. Eventually check for null otherwise error.
        int r = Random.Range(0, possibleBioms.Length);
        SetBattleMap(possibleBioms[r]);

        // Instantiate and place the enemy champions.
        //this.enemyTeam.Clear();
        //for (int i = 0; i < enemyTeam.Length; i++)
        //{
        //    GameObject tmp = Instantiate(enemyTeam[i], enemyHolder);
        //    this.enemyTeam.Add(tmp);
        //}
        //if (this.enemyTeam.Count == 2)
        //{
        //    this.enemyTeam[0].transform.position = spawnPositionsRight[1].position;
        //    this.enemyTeam[1].transform.position = spawnPositionsRight[2].position;
        //}
        //else
        //{
        //    for (int i = 0; i < this.enemyTeam.Count; i++)
        //        this.enemyTeam[i].transform.position = spawnPositionsRight[i].position;
        //}

        this.enemyTeam.Clear();
        for (int i = 0; i < enemyTeam.Encounter.Length; i++)
        {
            if(enemyTeam.Encounter[i] != null)
            {
                GameObject tmp = Instantiate(enemyTeam.Encounter[i], enemyHolder);
                Champion tmpChamp = tmp.GetComponent<Champion>();

                // Try to tier balance.
                if(enemyTeam.Levels[i] > 14)
                {
                    tmpChamp.SetBaseAiMightLevel(65);
                    for (int g = 0; g < tmpChamp.Equipment.Length; g++)
                        tmpChamp.Equipment[g] = InventoryManager.Instance.GetRandomEquipment(g, 3);
                    for (int g = 0; g < tmpChamp.Weapons.Length-1; g++)
                        tmpChamp.Weapons[g] = InventoryManager.Instance.GetRandomWeapon(2);

                }
                else if (enemyTeam.Levels[i] > 9)
                {
                    tmpChamp.SetBaseAiMightLevel(45);
                    for (int g = 0; g < tmpChamp.Equipment.Length; g++)
                        tmpChamp.Equipment[g] = InventoryManager.Instance.GetRandomEquipment(g, 2);
                    float rand = Random.Range(0.0f, 1.0f);
                    if (rand < 0.4f)
                    {
                        for (int g = 0; g < tmpChamp.Weapons.Length - 1; g++)
                            tmpChamp.Weapons[g] = InventoryManager.Instance.GetRandomWeapon(2);
                    }
                    else
                    {
                        for (int g = 0; g < tmpChamp.Weapons.Length - 1; g++)
                            tmpChamp.Weapons[g] = InventoryManager.Instance.GetRandomWeapon(1);
                    }
                    
                }
                else if (enemyTeam.Levels[i] > 4)
                {
                    tmpChamp.SetBaseAiMightLevel(25);
                    for (int g = 0; g < tmpChamp.Equipment.Length; g++)
                        tmpChamp.Equipment[g] = InventoryManager.Instance.GetRandomEquipment(g, 1);
                    
                    
                    for (int g = 0; g < tmpChamp.Weapons.Length - 1; g++)
                        tmpChamp.Weapons[g] = InventoryManager.Instance.GetRandomWeapon(1);
                }

                if (enemyTeam.Evolved[i] != null)
                {
                    tmpChamp.EvolveChampion(enemyTeam.Evolved[i], true);
                    if (enemyTeam.Levels[i] > 9)
                        tmpChamp.EvolveChampion(enemyTeam.Evolved[i]);
                    if (enemyTeam.Levels[i] > 14)
                        tmpChamp.EvolveChampion(enemyTeam.Evolved[i]);



                    // Get him random equipment.
                }
                for (int j = 0; j < enemyTeam.Levels[i]; j++)
                {
                    tmpChamp.LevelAI();
                    tmpChamp.GoldReward += Random.Range(7, 15);
                }
                tmpChamp.Level = enemyTeam.Levels[i];
                tmpChamp.UpdateChampionValues();
                this.enemyTeam.Add(tmp);
            }
            


            
        }
        if (this.enemyTeam.Count == 2)
        {
            this.enemyTeam[0].transform.position = spawnPositionsRight[1].position;
            this.enemyTeam[1].transform.position = spawnPositionsRight[2].position;
        }
        else
        {
            for (int i = 0; i < this.enemyTeam.Count; i++)
                this.enemyTeam[i].transform.position = spawnPositionsRight[i].position;
        }

        // Get all participating champions in a sorted list, then start the battle loop.
        GetChampionsInBattle();
    }

    /// <summary>
    /// This class will prepare a sortet list of all champions and the gives the ok to actually start the battle.
    /// </summary>
    private void GetChampionsInBattle()
    {
        championsInBattle = new List<Champion>();
        // Save the champion scripts of all battle participants in one list for easy access.
        for (int i = 0; i < PlayerInventory.Instance.CurrentChampions.Count; i++)
        {
            if (PlayerInventory.Instance.CurrentChampions[i].GetComponent<Champion>().IsAlive)
                championsInBattle.Add(PlayerInventory.Instance.CurrentChampions[i].GetComponent<Champion>());
        }
        for (int i = 0; i < championsInBattle.Count; i++)
            championsInBattle[i].gameObject.SetActive(true);

        // Place the playerchampions.
        if (championsInBattle.Count == 2)
        {
            championsInBattle[0].transform.position = spawnPositionsLeft[1].position;
            championsInBattle[1].transform.position = spawnPositionsLeft[2].position;
        }
        else
        {
            for (int i = 0; i < championsInBattle.Count; i++)
                championsInBattle[i].transform.position = spawnPositionsLeft[i].position;
        }

        for (int i = 0; i < enemyTeam.Count; i++)
        {
            championsInBattle.Add(enemyTeam[i].GetComponent<Champion>());
            // Init the enemies here.
            enemyTeam[i].GetComponent<Champion>().InitializeChampion();
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

        // Everything prepared, set in combat to true to start the main battleloop in update. Reset the turn counter.
        inCombat = true;
        currentTurn = 0;
        isWaiting = false;
        playerWon = false;
    }

    #endregion Battle setup related.

    #region Reward related.

    /// <summary>
    /// This method calculates the battlereward for the player.
    /// </summary>
    private void CalculateBattleReward()
    {
        // Reset the values.
        battleRewardGold = 0;
        battleRewardExp = 0.0f;

        int playerLevel = 0;
        int enemyLevel = 0;

        // Add the new values.
        for (int i = 0; i < championsInBattle.Count; i++)
        {
            if (!championsInBattle[i].IsPlayer)
            {
                battleRewardGold += Random.Range((championsInBattle[i].GoldReward - 20), (championsInBattle[i].GoldReward + 1));
                battleRewardExp += championsInBattle[i].ExpReward;
                for (int j = 0; j < championsInBattle[i].CrystalReward.Length; j++)
                    battleRewardCrystals[(int)championsInBattle[i].CrystalReward[j].MightCrystal] += championsInBattle[i].CrystalReward[j].Amount;

                enemyLevel += championsInBattle[i].Level;
            }
            else
                playerLevel += championsInBattle[i].Level;
        }

        // Calculate level difference.
        levelDifference = enemyLevel - playerLevel;
        battleRewardExp += levelDifference * levelPenalty * battleRewardExp;
        if (battleRewardExp < 100f)
            battleRewardExp = 100f;
    }

    /// <summary>
    /// Adds crystals to the current rewardpool.
    /// </summary>
    /// <param name="crystal">The type.</param>
    /// <param name="amount">The amount.</param>
    public void AddCrystalRewards(int crystal, int amount)
    {
        battleRewardCrystals[crystal] += amount;
    }

    #endregion Reward related.
}