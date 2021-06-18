using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class handles the display of the various ui screens.
/// </summary>
public class UIManager : MonoBehaviour
{
    // A reference to the current champ that should be displayed.
    Champion currentChamp;
    Weapon weapon;

    [Header("Dialog System:")]
    [SerializeField] GameObject dialogScreen;
    [SerializeField] TextMeshProUGUI dialogTextField;

    [Header("Starsigns base sprites:")]
    [SerializeField] Sprite[] starSigns;
    [SerializeField] Color[] starsSignColors;
    [Header("Crystal images:")]
    [SerializeField] Sprite[] crystals;

    [Header("BattleScreenRelated:")]
    [SerializeField] GameObject battleScreenUIPlayer;
    [SerializeField] TextMeshProUGUI champName;
    [SerializeField] Image devotionImage;
    [SerializeField] TextMeshProUGUI devotionAmount;
    [SerializeField] TextMeshProUGUI[] attackNames;
    [SerializeField] TextMeshProUGUI[] damageTypes;
    [SerializeField] TextMeshProUGUI[] targets;
    [SerializeField] TextMeshProUGUI[] energyCosts;
    [SerializeField] TextMeshProUGUI[] powers;
    [SerializeField] Image specialFillImage;
    [SerializeField] TextMeshProUGUI fillText;
    [SerializeField] Button[] attackButtons;
    [SerializeField] GameObject[] disableImages;
    [SerializeField] Sprite[] disabledButtonImages;
    [SerializeField] SpriteState[] disabledSprites;

    [Header("Statsscreen (in Battle):")]
    [SerializeField] GameObject statsScreen;
    [SerializeField] Button statsScreenButton;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] Image healthFill;
    [SerializeField] TextMeshProUGUI healthRegen;
    [SerializeField] TextMeshProUGUI energy;
    [SerializeField] Image energyFill;
    [SerializeField] TextMeshProUGUI energyRegen;
    [SerializeField] TextMeshProUGUI[] resis;
    [SerializeField] TextMeshProUGUI defense;
    [SerializeField] TextMeshProUGUI crit;
    [SerializeField] TextMeshProUGUI evade;

    [SerializeField] Button skipButton;
    
    [Header("Enemyscreen related:")]
    [SerializeField] GameObject battleScreenUIEnemy;
    [SerializeField] TextMeshProUGUI enemyAction;
    [SerializeField] GameObject continueButton;


    [Header("Reward related:")]
    [SerializeField] GameObject rewardScreen;
    [SerializeField] TextMeshProUGUI[] headers;
    [SerializeField] Image[] championImages;
    [SerializeField] Image[] championBorders;
    [SerializeField] Image[] xpFills;
    [SerializeField] TextMeshProUGUI[] xpTexts;

    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] Image[] cIms;
    [SerializeField] TextMeshProUGUI[] cTexts;

    [Header("Shop related:")]
    [SerializeField] GameObject shopScreen;
    [SerializeField] Button buttonChampionPrefab;
    [SerializeField] Button buttonEquipmentPrefab;
    [SerializeField] Button buttonWeaponPrefab;
    [SerializeField] Button buttonCrystalPrefab;
    [SerializeField] TextMeshProUGUI shopDescription;
    [SerializeField] TextMeshProUGUI shopGold;
    [SerializeField] Transform shopButtonHolder;


    [Header("Champion menu related:")]
    [SerializeField] GameObject championMenuScreen;

    [SerializeField] GameObject equipPanel;
    [SerializeField] GameObject statsPanel;
    [SerializeField] GameObject evolvePanel;

    [SerializeField] GameObject[] disableChampionButtons;
    [SerializeField] Button[] championButtons;
    [SerializeField] Button[] sideButtons;

    [Header("Left Panel related:")]
    [SerializeField] TextMeshProUGUI championName;
    [SerializeField] TextMeshProUGUI hp;
    [SerializeField] TextMeshProUGUI hpRec;
    [SerializeField] Image hpFill;
    [SerializeField] TextMeshProUGUI en;
    [SerializeField] TextMeshProUGUI enRec;
    [SerializeField] Image enFill;
    [SerializeField] TextMeshProUGUI xp;
    [SerializeField] TextMeshProUGUI bonusXp;
    [SerializeField] Image xpFill;
    [Header("Special Upgrade related:")]
    [SerializeField] Button specUpgradeButton;
    [SerializeField] GameObject specUpgradePanel;
    [SerializeField] Button specUpgradeButtonPrefab;
    [SerializeField] Transform specialUpBtnHolder;
    List<SpecialUpgrades> specUp;

    [SerializeField] Image devotion;
    [SerializeField] TextMeshProUGUI devAmount;
    [SerializeField] Image devotion1;
    [SerializeField] Image devotion2;

    [SerializeField] Image championImage;
    [SerializeField] Image championBorder;

    [SerializeField] TextMeshProUGUI sp;
    [SerializeField] TextMeshProUGUI[] mightLevels;

    [Header("Equipment panel related:")]
    [SerializeField] ButtonEquipment[] equipmentButtons;
    [SerializeField] ButtonWeapon[] weaponButtons;
    [SerializeField] GameObject itemPickerWindow;
    [SerializeField] Button buttonEquipmentPrefab2;
    [SerializeField] Button buttonWeaponPrefab2;
    [SerializeField] Button buttonChampionPrefab2;
    [SerializeField] Transform itemPrefabHolder;


    [Header("Evolution panel related:")]
    [SerializeField] GameObject[] evos;
    [SerializeField] Image[] evoCImages;
    [SerializeField] Button[] mightLevelButtons;
    [SerializeField] Button[] evolutionButtons;

    [Header("Stats panel related:")]
    [SerializeField] TextMeshProUGUI[] inventoryCrystals;
    [SerializeField] TextMeshProUGUI playerGold;

    [SerializeField] TextMeshProUGUI criticalStats;
    [SerializeField] TextMeshProUGUI blockStats;
    [SerializeField] TextMeshProUGUI initativeStats;
    [SerializeField] TextMeshProUGUI[] resistancesStats;
    [SerializeField] TextMeshProUGUI[] defenseStats;

    [Header("Healing Panel related:")]
    [SerializeField] GameObject healingPanel;

    // Temps:
    int champIndex;
    int sideIndex;
    int evoIndex;
    int itemIndex;

    List<Equipment> tmpEq;
    List<Weapon> tmpW;


    #region Singleton
    public static UIManager Instance;

    public Sprite[] StarSigns { get => starSigns; set => starSigns = value; }
    public Color[] StarsSignColors { get => starsSignColors; set => starsSignColors = value; }
    public Sprite[] Crystals { get => crystals;}

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    #region DialogScreenRelated

    /// <summary>
    /// Toggles the dialogscreen on and off.
    /// </summary>
    /// <param name="status"></param>
    public void ShowDialogScreen(bool status)
    {
        dialogScreen.SetActive(status);
    }

    /// <summary>
    /// Returns the dialogtextfield to the one who requests it.
    /// </summary>
    /// <returns></returns>
    public TextMeshProUGUI GetDialogTextField()
    {
        return dialogTextField;
    }

    #endregion

    #region BattleScreenUI related

    /// <summary>
    /// Shows the battle screen.
    /// </summary>
    /// <param name="champ">The current champion.</param>
    public void ShowBattleScreenUIPlayer(Champion champ)
    {
        currentChamp = champ;
        statsScreenButton.gameObject.SetActive(true);
        statsScreen.SetActive(false);
        battleScreenUIEnemy.SetActive(false);
        battleScreenUIPlayer.SetActive(true);

        champName.text = $"{champ.ChampionName}, {champ.Title}";
        if (champ.Devotion != Starsign.None)
        {
            devotionImage.enabled = true;
            devotionImage.sprite = starSigns[(int)champ.Devotion];
            devotionImage.color = starsSignColors[(int)champ.Devotion];
            devotionAmount.text = $"x{champ.DevotionAmount}";
            devotionAmount.color = starsSignColors[(int)champ.Devotion];
        }
        else
        {
            devotionImage.enabled = false;
            devotionAmount.text = "";
        }

        // Enable all buttons.
        EnableAllAttackButtons();

        for (int i = 0; i < champ.Weapons.Length; i++)
        {
            if (champ.Weapons[i] != null)
            {
                attackNames[i].text = champ.Weapons[i].ItemName;
                for (int j = 0; j < champ.Weapons[i].DamageTypes.Length; j++)
                {
                    if (j != 0)
                        damageTypes[i].text += $", {champ.Weapons[i].DamageTypes[j]}";
                    else
                        damageTypes[i].text = champ.Weapons[i].DamageTypes[j].ToString();
                }

                targets[i].text = $"Target: {champ.Weapons[i].TargetType}";
                energyCosts[i].text = champ.Weapons[i].EnergyCost.ToString();
                powers[i].text = $"{champ.Weapons[i].NumberOfAttacks}x{(int)champ.GetWeaponPower(champ.Weapons[i])}";
                // Disable buttons the player cant use atm.
                if (champ.CurrentEnergy < champ.Weapons[i].EnergyCost)
                    attackButtons[i].interactable = false;
            }
            else
                HideAttackButton(i);
        }

        if (champ.SpecialWeapon != null)
        {
            attackNames[4].text = champ.SpecialWeapon.ItemName;
            for (int i = 0; i < champ.SpecialWeapon.DamageTypes.Length; i++)
            {
                if (i != 0)
                    damageTypes[4].text += $", {champ.SpecialWeapon.DamageTypes[i]}";
                else
                    damageTypes[4].text = champ.SpecialWeapon.DamageTypes[i].ToString();
            }
            targets[4].text = $"Target: {champ.SpecialWeapon.TargetType}";
            energyCosts[4].text = champ.SpecialWeapon.EnergyCost.ToString();
            powers[4].text = $"{champ.SpecialWeapon.NumberOfAttacks}x{(int)champ.GetWeaponPower(champ.SpecialWeapon)}";
            specialFillImage.fillAmount = champ.GetSpecialMeterRatio();
            fillText.text = $"{SpecialTypeToString(champ.SpecialWeapon.SpecialRequirement)} {(int)champ.CurrentSpecialMeter}/{(int)champ.SpecialWeapon.MaxSpecialMeter} ";
            if (champ.CurrentSpecialMeter < champ.SpecialWeapon.MaxSpecialMeter || champ.CurrentEnergy < champ.SpecialWeapon.EnergyCost)
                attackButtons[4].interactable = false;
        }
        else
            HideAttackButton(4);

        
        // Remember to check if he  cant do anything to immediately skip.

        battleScreenUIPlayer.SetActive(true);
    }

    private string SpecialTypeToString(SpecialRequirement spec)
    {
        string tmp = "";
        switch (spec)
        {
            case SpecialRequirement.Health:
                tmp = "HP Verlust:";
                break;
            case SpecialRequirement.Energy:
                tmp = "E genutzt:";
                break;
            case SpecialRequirement.HealthRec:
                tmp = "HP Recover:";
                break;
            case SpecialRequirement.EnergyRec:
                tmp = "E Recover:";
                break;
            case SpecialRequirement.XpGain:
                tmp = "XP gewonnen:";
                break;
            case SpecialRequirement.DamageDealt:
                tmp = "Schaden:";
                break;
            default:
                break;
        }
        return tmp;
    }


    private void HideAttackButton(int index)
    {
        attackButtons[index].interactable = false;
        disableImages[index].SetActive(true);
    }

    private void EnableAllAttackButtons()
    {
        for (int i = 0; i < attackButtons.Length; i++)
        {
            attackButtons[i].gameObject.SetActive(true);
            attackButtons[i].interactable = true;
            disableImages[i].SetActive(false);

            attackButtons[i].spriteState = disabledSprites[0];
        }
        skipButton.interactable = true;
    }

    private void DiableAllAttackButtons()
    {
        for (int i = 0; i < attackButtons.Length; i++)
        {
            attackButtons[i].interactable = false;            
        }
        skipButton.interactable = false;
    }


    public void ShowStatsScreen()
    {
        // Set health details.
        health.text = $"{(int)currentChamp.CurrentHealth}/{(int)currentChamp.MaxHealth}";
        healthFill.fillAmount = currentChamp.CurrentHealth / currentChamp.MaxHealth;
        healthRegen.text = $"Hp Regeneration: {currentChamp.HealthRecovery} ({currentChamp.HealthRecoveryBuff}): {currentChamp.HealthRecBuffDuration}";
        // Set energy details.
        energy.text = $"{(int)currentChamp.CurrentEnergy}/{(int)currentChamp.MaxEnergy}";
        energyFill.fillAmount = currentChamp.CurrentEnergy / currentChamp.MaxEnergy;
        energyRegen.text = $"Energie Regeneration: {currentChamp.EnergyRecovery} ({currentChamp.EnergyRecoveryBuff}): {currentChamp.EnergyRecBuffDuration}";
        // Set resistance details.
        for (int i = 0; i < resis.Length; i++)
            resis[i].text = $"{currentChamp.Resistances[i].ResistanceAmount*100}% ({currentChamp.ResistancesBuff[i].ResistanceAmount*100}%) :{currentChamp.ResistanceBuffDuration}";
        // Set defense and acc details.
        defense.text = $"Defense: {(int)(currentChamp.Defense* 100)}% ({(int)(currentChamp.DefenseBuff*100)}%) : {currentChamp.DefenseBuffDuration}";
        evade.text = $"Block: {currentChamp .DmgReduction* 100}%, Dodge: {currentChamp .EvasionChance* 100}%";
        crit.text = $"Krit: {(int)(currentChamp .CriticalChance * 100)}%, ({(int)(currentChamp.CriticalMultiplier * 100)}%)";

        statsScreen.SetActive(true);
        statsScreenButton.gameObject.SetActive(false);
        AudioManager.Instance.PlayEffectClip(5);
    }

    private string DamageTypeToString(DamageType resis)
    {
        string tmp = "";
        switch (resis)
        {
            case DamageType.Blade:
                tmp = "Schwert: ";
                break;
            case DamageType.Pierce:
                tmp = "Stich: ";
                break;
            case DamageType.Impact:
                tmp = "Wucht: ";
                break;
            case DamageType.Arcane:
                tmp = "Arkan: ";
                break;
            case DamageType.Fire:
                tmp = "Feuer: ";
                break;
            case DamageType.Ice:
                tmp = "Eis: ";
                break;
            case DamageType.Thunder:
                tmp = "Donner: ";
                break;
            case DamageType.Storm:
                tmp = "Sturm: ";
                break;
            default:
                break;
        }
        return tmp;
    }

    public void HideStatsScreen()
    {
        statsScreen.SetActive(false);
        statsScreenButton.gameObject.SetActive(true);
        AudioManager.Instance.PlayEffectClip(5);
    }


    public void ShowBattleScreenUIEnemy(Champion champ)
    {
        battleScreenUIPlayer.SetActive(true);
        battleScreenUIEnemy.SetActive(true);
        continueButton.SetActive(false);

        // Set the name and devotion from the enemy.
        champName.text = $"{champ.ChampionName}, {champ.Title}";
        if (champ.Devotion != Starsign.None)
        {
            devotionImage.enabled = true;
            devotionImage.sprite = starSigns[(int)champ.Devotion];
            devotionImage.color = starsSignColors[(int)champ.Devotion];
            devotionAmount.text = $"x{champ.DevotionAmount}";
            devotionAmount.color = starsSignColors[(int)champ.Devotion];
        }
        else
        {
            devotionImage.enabled = false;
            devotionAmount.text = "";
        }
    }

    public void ShowEnemyAction(string message)
    {
        enemyAction.text = "";
        enemyAction.text = message;
    }

    public void OnPassButtonClick()
    {
        BattleManager.Instance.EndTurn();
        AudioManager.Instance.PlayEffectClip(5);
    }

    /// <summary>
    /// Activates the continue button.
    /// </summary>
    /// <param name="status"></param>
    public void ShowContinueButton(bool status)
    {
        continueButton.SetActive(status);
    }

    /// <summary>
    /// Getting called from the attackbuttons in game.
    /// </summary>
    /// <param name="weaponIndex">The index of the button.</param>
    public void AttackButtons(int weaponIndex)
    {
        DiableAllAttackButtons();
        attackButtons[weaponIndex].spriteState = disabledSprites[1];
        WeaponTarget target;
        if(weaponIndex == 4)
        {
            weapon = currentChamp.SpecialWeapon;
            // Reset the special meter progress.
            currentChamp.CurrentSpecialMeter = 0.0f;
        }
        else
            weapon = currentChamp.Weapons[weaponIndex];

        target = weapon.TargetType;

        if (target == WeaponTarget.Ally || target == WeaponTarget.Enemy)
            BattleManager.Instance.EnableTargeting(target);
        else
            BattleManager.Instance.AttackMultiple(weapon);

        AudioManager.Instance.PlayEffectClip(5);

    }

    public void SetChampionTarget(Champion target)
    {
        BattleManager.Instance.AttackSingle(target, weapon);
        AudioManager.Instance.PlayEffectClip(5);
    }

    public void HideBattleUIScreen()
    {
        battleScreenUIPlayer.SetActive(false);
        rewardScreen.SetActive(false);

        for (int i = 0; i < championImages.Length; i++)
        {
            championImages[i].gameObject.SetActive(false);
            championBorders[i].gameObject.SetActive(false);
            headers[i].gameObject.SetActive(false);
        }

    }

    public void ShowRewardScreen(List<Champion> champs, int gold, float xp, int[]crystalReward)
    {
        battleScreenUIPlayer.SetActive(false);
        rewardScreen.SetActive(true);
        AudioManager.Instance.PlayEffectClip(2);

        for (int i = 0; i < champs.Count; i++)
        {
            headers[i].text = $"{champs[i].ChampionName}, Lvl:{champs[i].Level} ";
            headers[i].gameObject.SetActive(true);
            if (!champs[i].IsAlive)
                headers[i].text += $"<color=red>(Dead)</color>";
            championImages[i].sprite = champs[i].ChampionSprite.sprite;
            championImages[i].gameObject.SetActive(true);
            championBorders[i].sprite = champs[i].ChampionBorder.sprite;
            championBorders[i].gameObject.SetActive(true);
            xpFills[i].fillAmount = champs[i].CurrentExp / champs[i].MaxExp;
            if(champs[i].IsAlive)
                xpTexts[i].text = $"+{(int)xp}";
            else
                xpTexts[i].text = $"+{(int)(xp*0.5f)}";
        }

        goldText.text = $"Gold: {PlayerInventory.Instance.InventoryCrystals[0]} +({gold})";

        for (int i = 2; i < crystalReward.Length; i++)
        {
            if (crystalReward[i - 1] != 0)
            {
                cIms[i - 2].sprite = crystals[i-2];
                cIms[i - 2].gameObject.SetActive(true);
                cTexts[i - 2].text = $"{PlayerInventory.Instance.InventoryCrystals[i - 1]}+({crystalReward[i - 1]})";
                cTexts[i - 2].gameObject.SetActive(true);
            }
            else
            {
                cIms[i - 2].gameObject.SetActive(false);
                cTexts[i - 2].gameObject.SetActive(false);
            }
        }

    }

    public void OnEndBattleButton()
    {
        BattleManager.Instance.EndBattle();
        AudioManager.Instance.PlayEffectClip(0);
    }

    #endregion

    #region Shop related

    /// <summary>
    /// Shows the shopscreen and instantiates the interaction buttons.
    /// </summary>
    /// <param name="shop"></param>
    public void ShowShopScreen(ShopArea shop)
    {
        shopScreen.SetActive(true);

        // Show shop description and player gold.
        shopDescription.text = $"{shop.Description}";       
        shopGold.text = $"{PlayerInventory.Instance.InventoryCrystals[0]} Gold";

        // Initialize championbuttons.
        for (int i = 0; i < shop.Champions.Length; i++)
        {
            Button tmp = Instantiate(buttonChampionPrefab, shopButtonHolder);
            tmp.GetComponent<GeneratedButton>().ButtonIndex = i;
            tmp.onClick.AddListener(() => OnBuyButtonClick(tmp.GetComponent<GeneratedButton>(), shop));
            tmp.GetComponent<ButtonChampion>().InitializeButton(shop.Champions[i].GetComponent<Champion>());
            tmp.interactable = PlayerInventory.Instance.HasCurrency(Crystal.Gold, shop.Champions[i].GetComponent<Champion>().Price);
        }

        // Instantiate equipmenbuttons.
        for (int i = 0; i < shop.Equipment.Length; i++)
        {         
            Button tmp = Instantiate(buttonEquipmentPrefab, shopButtonHolder);
            tmp.GetComponent<GeneratedButton>().ButtonIndex = i;
            tmp.onClick.AddListener(() => OnBuyButtonClick(tmp.GetComponent<GeneratedButton>(), shop));
            tmp.GetComponent<ButtonEquipment>().InitializeEquipmentButton(shop.Equipment[i]);
            tmp.interactable = PlayerInventory.Instance.HasCurrency(Crystal.Gold, shop.Equipment[i].Price);
        }

        // Initialize weaponbuttons.
        for (int i = 0; i < shop.Weapons.Length; i++)
        {
            Button tmp = Instantiate(buttonWeaponPrefab, shopButtonHolder);
            tmp.GetComponent<GeneratedButton>().ButtonIndex = i;
            tmp.onClick.AddListener(() => OnBuyButtonClick(tmp.GetComponent<GeneratedButton>(), shop));
            tmp.GetComponent<ButtonWeapon>().InitializeWeaponButton(shop.Weapons[i]);
            tmp.interactable = PlayerInventory.Instance.HasCurrency(Crystal.Gold, shop.Weapons[i].Price);
        }

        // Init crystalbuttons.
        for (int i = 0; i < shop.Crystals.Length; i++)
        {
            Button tmp = Instantiate(buttonCrystalPrefab, shopButtonHolder);
            tmp.GetComponent<GeneratedButton>().ButtonIndex = i;
            tmp.onClick.AddListener(() => OnBuyButtonClick(tmp.GetComponent<GeneratedButton>(), shop));
            tmp.GetComponent<ButtonCrystal>().InitializeCrystalButton(shop.Crystals[i]);
            tmp.interactable = PlayerInventory.Instance.HasCurrency(Crystal.Gold, shop.Crystals[i].CrystalPrice);
        }
        AudioManager.Instance.PlayEffectClip(1);
    }

    /// <summary>
    /// Refreshes the gold and interactivity after buying.
    /// </summary>
    /// <param name="shop"></param>
    private void UpdateShop(ShopArea shop)
    {
        shopGold.text = $"{PlayerInventory.Instance.InventoryCrystals[0]} Gold";
        int c = 0;
        int e = 0;
        int w = 0;
        int crys = 0;
        for (int i = 0; i < shopButtonHolder.childCount; i++)
        {
            if(shopButtonHolder.GetChild(i).GetComponent<ButtonChampion>() != null)
            {
                shopButtonHolder.GetChild(i).GetComponent<Button>().interactable = PlayerInventory.Instance.HasCurrency(Crystal.Gold, shop.Champions[c].GetComponent<Champion>().Price);
                c++;
            }
            else if(shopButtonHolder.GetChild(i).GetComponent<ButtonEquipment>() != null)
            {
                shopButtonHolder.GetChild(i).GetComponent<Button>().interactable = PlayerInventory.Instance.HasCurrency(Crystal.Gold, shop.Equipment[e].Price);
                e++;
            }
            else if (shopButtonHolder.GetChild(i).GetComponent<ButtonWeapon>() != null)
            {
                shopButtonHolder.GetChild(i).GetComponent<Button>().interactable = PlayerInventory.Instance.HasCurrency(Crystal.Gold, shop.Weapons[w].Price);
                w++;
            }else if (shopButtonHolder.GetChild(i).GetComponent<ButtonCrystal>() != null)
            {
                shopButtonHolder.GetChild(i).GetComponent<Button>().interactable = PlayerInventory.Instance.HasCurrency(Crystal.Gold, shop.Crystals[crys].CrystalPrice);
                crys++;
            }
        }
    }

    /// <summary>
    /// Buys the item on generated button click.
    /// </summary>
    /// <param name="btn">To geth the index of the button.</param>
    /// <param name="shop">The shop the index will be used on.</param>
    public void OnBuyButtonClick(GeneratedButton btn, ShopArea shop)
    {
        if (btn.GetComponent<ButtonEquipment>() != null && PlayerInventory.Instance.HasCurrency(Crystal.Gold, shop.Equipment[btn.ButtonIndex].Price))
        {
            InventoryManager.Instance.AddEquipment(shop.Equipment[btn.ButtonIndex]);
            PlayerInventory.Instance.SpendCurrency(Crystal.Gold, shop.Equipment[btn.ButtonIndex].Price);
            btn.GetComponent<ButtonEquipment>().UpdateItemNumbers(shop.Equipment[btn.ButtonIndex]);
        }
        else if (btn.GetComponent<ButtonWeapon>() != null)
        {
            InventoryManager.Instance.AddWeapon(shop.Weapons[btn.ButtonIndex]);
            PlayerInventory.Instance.SpendCurrency(Crystal.Gold, shop.Weapons[btn.ButtonIndex].Price);
            btn.GetComponent<ButtonWeapon>().UpdateItemNumbers(shop.Weapons[btn.ButtonIndex]);
        }
        else if (btn.GetComponent<ButtonChampion>() != null)
        {
            ChampionManager.Instance.AddChampionToInventory(shop.Champions[btn.ButtonIndex]);
            PlayerInventory.Instance.SpendCurrency(Crystal.Gold, shop.Champions[btn.ButtonIndex].GetComponent<Champion>().Price);
        }
        else if (btn.GetComponent<ButtonCrystal>() != null)
        {
            PlayerInventory.Instance.AddCrystals(shop.Crystals[btn.ButtonIndex].Type, 1);
            PlayerInventory.Instance.SpendCurrency(Crystal.Gold, shop.Crystals[btn.ButtonIndex].CrystalPrice);
        }
        UpdateShop(shop);
        AudioManager.Instance.PlayEffectClip(0);
    }

    
    /// <summary>
    /// Leaves the shop screen.
    /// </summary>
    public void CloseShopScreen()
    {
        for (int i = shopButtonHolder.childCount - 1; i >= 0; i--)
        {
            GameObject tmp = shopButtonHolder.GetChild(i).gameObject;
            tmp.transform.parent = null;
            // remove the listeners.
            Destroy(tmp);
        }
        shopScreen.SetActive(false);
        PlayerController.Instance.CanMove = true;
        PlayerController.Instance.IsFighting = false;
        AudioManager.Instance.PlayEffectClip(3);
    }

    #endregion


    #region ChampionMenu related

    /// <summary>
    /// Toggles the champion menu on and off.
    /// </summary>
    /// <param name="status"></param>
    public void ToggleChampionMenu(bool status)
    {
        
        if (status == true)
        {
            champIndex = 0;
            
            currentChamp = PlayerInventory.Instance.CurrentChampions[champIndex].GetComponent<Champion>();
            UpdateLeftPanel(currentChamp);
            UpdateEquipPanel(currentChamp);
            // champbuttons und disablebuttons davor noch auf false setzen. für später.

            for (int i = 0; i < PlayerInventory.Instance.CurrentChampions.Count; i++)
            {
                championButtons[i].interactable = true;
                championButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{PlayerInventory.Instance.CurrentChampions[i].GetComponent<Champion>().ChampionName}, Lvl: {PlayerInventory.Instance.CurrentChampions[i].GetComponent<Champion>().Level}";
                disableChampionButtons[i].SetActive(false);
            }
            // Always show the equipment screen of the first champion.
            
            sideIndex = 0;
            championButtons[0].interactable = false;
            statsPanel.SetActive(false);
            evolvePanel.SetActive(false);
            equipPanel.SetActive(true);
            for (int i = 0; i < sideButtons.Length; i++)
                sideButtons[i].interactable = true;
            sideButtons[0].interactable = false;
            AudioManager.Instance.PlayEffectClip(1);
        }
        else
            AudioManager.Instance.PlayEffectClip(3);

        championMenuScreen.SetActive(status);
        PlayerController.Instance.StandStill();
        PlayerController.Instance.CanMove = !status;
        
    }


    public void OnChampionButtonClick(int index)
    {
        champIndex = index;
        currentChamp = PlayerInventory.Instance.CurrentChampions[champIndex].GetComponent<Champion>();
        UpdateLeftPanel(currentChamp);
        if (sideIndex == 0)
            UpdateEquipPanel(currentChamp);
        else if (sideIndex == 1)
            UpdateStatsPanel(currentChamp);
        else if (sideIndex == 2)
            UpdateEvoPanel(currentChamp);

        // Set all buttons interactable to true, then the clicked one to false to make him highlighted.
        for (int i = 0; i < championButtons.Length; i++)
            if (i < PlayerInventory.Instance.CurrentChampions.Count)
                championButtons[i].interactable = true;
        championButtons[index].interactable = false;
        AudioManager.Instance.PlayEffectClip(1);
    }

    /// <summary>
    /// This method will be called by the UI sidebuttons. 
    /// </summary>
    /// <param name="index">Which sidebutton was pressed.</param>
    public void OnSideButtonsClicked(int index)
    {
        equipPanel.SetActive(false);
        statsPanel.SetActive(false);
        evolvePanel.SetActive(false);

        sideIndex = index;
        // Reactivate all sidebuttons except the just pressed one.
        for (int i = 0; i < sideButtons.Length; i++)
            sideButtons[i].interactable = true;
        sideButtons[index].interactable = false;
        // Depending on Sideindex update the ui.
        if (sideIndex == 0)
        {
            UpdateEquipPanel(currentChamp);
            equipPanel.SetActive(true);
            UpdateMightLevels(currentChamp);
        }
        else if (sideIndex == 1)
        {
            UpdateStatsPanel(currentChamp);
            statsPanel.SetActive(true);
            UpdateMightLevels(currentChamp);
        }
        else if (sideIndex == 2)
        {
            UpdateEvoPanel(currentChamp);
            evolvePanel.SetActive(true);
            UpdateMightLevels(currentChamp);
        }
        AudioManager.Instance.PlayEffectClip(1);
    }

    /// <summary>
    /// Updates the left side of the champion menu.
    /// </summary>
    /// <param name="c">The champ to show his information.</param>
    private void UpdateLeftPanel(Champion c)
    {
        // Name.
        championName.text = $"{currentChamp.ChampionName}, Lvl: {currentChamp.Level}";
        // Set health details.
        hp.text = $"{(int)currentChamp.CurrentHealth}/{(int)currentChamp.MaxHealth}";
        hpFill.fillAmount = currentChamp.CurrentHealth / currentChamp.MaxHealth;
        hpRec.text = $"(+{currentChamp.HealthRecovery})";
        // Set energy details.
        en.text = $"{(int)currentChamp.CurrentEnergy}/{(int)currentChamp.MaxEnergy}";
        enFill.fillAmount = currentChamp.CurrentEnergy / currentChamp.MaxEnergy;
        enRec.text = $"(+{currentChamp.EnergyRecovery})";
        // Set xp details.
        xp.text = $"{(int)currentChamp.CurrentExp}/{(int)currentChamp.MaxExp}";
        xpFill.fillAmount = currentChamp.CurrentExp / currentChamp.MaxExp;
        bonusXp.text = $"(+{(int)(currentChamp.ExpBuff * 100)}%)";
        // Devotion.
        if (currentChamp.Devotion != Starsign.None)
        {
            devotion.enabled = true;
            devotion.sprite = starSigns[(int)currentChamp.Devotion];
            devotion.color = starsSignColors[(int)currentChamp.Devotion];
            devAmount.text = $"x{currentChamp.DevotionAmount}";
            devAmount.color = starsSignColors[(int)currentChamp.Devotion];
        }
        else
        {
            devotion.enabled = false;
            devAmount.text = "";
        }
        devotion1.sprite = starSigns[(int)currentChamp.PossibleDevotions[0]];
        devotion2.sprite = starSigns[(int)currentChamp.PossibleDevotions[1]];
        // Special Upgrade Button.
        if (currentChamp.SpuPoints > 0)
            specUpgradeButton.gameObject.SetActive(true);

        // Championimage.
        championImage.sprite = currentChamp.ChampionSprite.sprite;
        currentChamp.UpdateBorder();
        championBorder.sprite = currentChamp.ChampionBorder.sprite;

        //Mightlevels.
        sp.text = $"({currentChamp.SkillPoints}SP)";
        UpdateMightLevels(currentChamp);
    }

    /// <summary>
    /// Will get the mightlevel data and color them in case i check for evolutions.
    /// </summary>
    /// <param name="current">Current champ.</param>
    /// <param name="other">The champion i want to check for evolution.</param>
    private void UpdateMightLevels(Champion current, BaseChampion other = null)
    {
        // Get the data.
        for (int i = 0; i < mightLevels.Length; i++)
            mightLevels[i].text = $"{currentChamp.MightLevels[i].Amount}";
        sp.text = $"({currentChamp.SkillPoints}SP)";

        // Color the data in case i check for evolution.
        if (other != null)
            for (int i = 0; i < other.CrystalLevelRequirements.Length; i++)
                for (int j = 0; j < currentChamp.MightLevels.Length; j++)
                    if (other.CrystalLevelRequirements[i].MightCrystal == currentChamp.MightLevels[j].MightCrystal)
                    {
                        if (other.CrystalLevelRequirements[i].Amount <= currentChamp.MightLevels[j].Amount)
                            mightLevels[j].text = $"<color=green>{currentChamp.MightLevels[j].Amount}</color>";
                        else
                            mightLevels[j].text = $"<color=red>{currentChamp.MightLevels[j].Amount}</color>";
                    }

        // Check if to eneable the evolve buttons.
        if(other != null)
        {
            for (int i = 0; i < currentChamp.PossibleEvolutions.Length; i++)
            {
                if (currentChamp.EvolveRequirementsMet(currentChamp.PossibleEvolutions[i]))
                {
                    evolutionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentChamp.PossibleEvolutions[i].ChampionName;
                    evolutionButtons[i].interactable = true;
                }
            }
        }
    }

    /// <summary>
    /// This method is called by the mightupgrade buttons in championmenu. It upgrades the mightlevels.
    /// </summary>
    /// <param name="index">Index to upgrade which mightlevel.</param>
    public void OnMightUpgradeClick(int index)
    {
        if (currentChamp.SkillPoints > 0 && PlayerInventory.Instance.HasCurrency((Crystal)(index +1),1))
        {
            currentChamp.UpgradeMightLevel(index);
            PlayerInventory.Instance.SpendCurrency((Crystal)(index + 1), 1);
            AudioManager.Instance.PlayEffectClip(2);

            if (sideIndex == 2)
            {
                if(evoIndex < currentChamp.PossibleEvolutions.Length)
                    UpdateMightLevels(currentChamp, currentChamp.PossibleEvolutions[evoIndex]);
                else
                    UpdateMightLevels(currentChamp);
            }
            else
                UpdateMightLevels(currentChamp);
        }
        else
        {
            // Sound it didnt worked.
            AudioManager.Instance.PlayEffectClip(4);
        }
        if (sideIndex == 1)
            UpdateStatsPanel(currentChamp);

        
    }

    /// <summary>
    /// For buttons to span particles at their pos.
    /// </summary>
    /// <param name="pos"></param>
    public void SpawnParticle(Transform pos)
    {
        if(currentChamp.SkillPoints > 0)
        {
            GameObject tmp = ObjectPool.Instance.GetFromPool("UIPart1");
            tmp.transform.position = pos.position;
            tmp.SetActive(true);
        }      
    }

    private void UpdateEquipPanel(Champion c)
    {
        for (int i = 0; i < equipmentButtons.Length; i++)
            equipmentButtons[i].InitializeEquipmentButton(currentChamp.Equipment[i]);

        for (int i = 0; i < weaponButtons.Length; i++)
        {
            if (i != 4)
                weaponButtons[i].InitializeWeaponButton(currentChamp.Weapons[i], currentChamp);
            else
                weaponButtons[i].InitializeWeaponButton(currentChamp.SpecialWeapon, currentChamp);
        }
        
    }
    #region Equip Item related.
    /// <summary>
    /// For the euqipmentpanel buttons.
    /// </summary>
    /// <param name="index"></param>
    public void OnEquipButtonClick(int index)
    {
        itemIndex = index;
        ShowItemPickerWindow();
        AudioManager.Instance.PlayEffectClip(1);
    }

    private void ShowItemPickerWindow()
    {
        tmpEq = new List<Equipment>();
        if(itemIndex == 0)
            tmpEq = InventoryManager.Instance.GetEquipmentList(EquipmentType.Head);
        else if(itemIndex == 1)
            tmpEq = InventoryManager.Instance.GetEquipmentList(EquipmentType.Decoration);
        else if (itemIndex == 2)
            tmpEq = InventoryManager.Instance.GetEquipmentList(EquipmentType.Armor);
        else if (itemIndex == 3)
            tmpEq = InventoryManager.Instance.GetEquipmentList(EquipmentType.Leg);

        // Instantiate equipmenbuttons.
        for (int i = 0; i < tmpEq.Count; i++)
        {
            Button tmp = Instantiate(buttonEquipmentPrefab2, itemPrefabHolder);
            tmp.GetComponent<GeneratedButton>().ButtonIndex = i;
            tmp.onClick.AddListener(() => OnEquipItemClick(tmp.GetComponent<GeneratedButton>()));
            tmp.GetComponent<ButtonEquipment>().InitializeEquipmentButton(tmpEq[i]);
        }

        itemPickerWindow.SetActive(true);
    }

    public void OnEquipItemClick(GeneratedButton btn)
    {
        InventoryManager.Instance.AddEquipment(currentChamp.Equipment[itemIndex]);
        currentChamp.Equipment[itemIndex] = tmpEq[btn.ButtonIndex];
        InventoryManager.Instance.RemoveEquipment(currentChamp.Equipment[itemIndex]);
        currentChamp.UpdateChampionValues();        
        currentChamp.UpdateBorder();

        UpdateLeftPanel(currentChamp);
        UpdateEquipPanel(currentChamp);
        OnCloseItemPickerWindow();
        AudioManager.Instance.PlayEffectClip(0);
    }
    #endregion
    #region Equip Weapon related
    /// <summary>
    /// For the euqipmentpanel weapon buttons.
    /// </summary>
    /// <param name="index"></param>
    public void OnWeaponButtonClick(int index)
    {
        itemIndex = index;
        ShowWeaponPickerWindow();
        AudioManager.Instance.PlayEffectClip(1);
    }

    private void ShowWeaponPickerWindow()
    {
        tmpW = new List<Weapon>();
        
        if(itemIndex != 4)
            tmpW = InventoryManager.Instance.GetWeaponList(currentChamp.UsableWeapons);
        else
        {
            WeaponType[] t = new WeaponType[] { WeaponType.Special };
            tmpW = InventoryManager.Instance.GetWeaponList(t);
        }

        // Instantiate weaponbuttons.
        for (int i = 0; i < tmpW.Count; i++)
        {
            Button tmp = Instantiate(buttonWeaponPrefab2, itemPrefabHolder);
            tmp.GetComponent<GeneratedButton>().ButtonIndex = i;
            tmp.onClick.AddListener(() => OnWeaponItemClick(tmp.GetComponent<GeneratedButton>()));
            tmp.GetComponent<ButtonWeapon>().InitializeWeaponButton(tmpW[i], currentChamp);
        }
        itemPickerWindow.SetActive(true);
    }

    public void OnWeaponItemClick(GeneratedButton btn)
    {
        if(itemIndex != 4)
        {
            InventoryManager.Instance.AddWeapon(currentChamp.Weapons[itemIndex]);
            currentChamp.Weapons[itemIndex] = tmpW[btn.ButtonIndex];
            InventoryManager.Instance.RemoveWeapon(currentChamp.Weapons[itemIndex]);
        }else if(itemIndex == 4)
        {
            InventoryManager.Instance.AddWeapon(currentChamp.SpecialWeapon);
            currentChamp.SpecialWeapon = tmpW[btn.ButtonIndex];
            InventoryManager.Instance.RemoveWeapon(currentChamp.SpecialWeapon);
            currentChamp.SetSpecialRequirement();
        }
        UpdateEquipPanel(currentChamp);
        OnCloseItemPickerWindow();
        AudioManager.Instance.PlayEffectClip(0);
    }
    #endregion
    // FOr champion change button.
    #region ChampionChange related.
    /// <summary>
    /// For the champion change buttons.
    /// </summary>
    public void OnChampionButtonClick()
    {
        ShowChampionPickerWindow();
        AudioManager.Instance.PlayEffectClip(1);
    }

    private void ShowChampionPickerWindow()
    {
        // Instantiate championbuttons.
        for (int i = 0; i < PlayerInventory.Instance.AllChampions.Count; i++)
        {
            Button tmp = Instantiate(buttonChampionPrefab2, itemPrefabHolder);
            tmp.GetComponent<GeneratedButton>().ButtonIndex = i;
            tmp.onClick.AddListener(() => OnChampionClick(tmp.GetComponent<GeneratedButton>()));
            tmp.GetComponent<ButtonChampion>().InitializeButton(PlayerInventory.Instance.AllChampions[i].GetComponent<Champion>());
        }
        itemPickerWindow.SetActive(true);
    }

    public void OnChampionClick(GeneratedButton btn)
    {

        GameObject tmp = currentChamp.gameObject;
        GameObject tmp2 = PlayerInventory.Instance.AllChampions[btn.ButtonIndex];
        PlayerInventory.Instance.CurrentChampions.Insert(champIndex, PlayerInventory.Instance.AllChampions[btn.ButtonIndex]);
        PlayerInventory.Instance.AllChampions.Add(tmp);
        PlayerInventory.Instance.CurrentChampions.Remove(tmp);
        PlayerInventory.Instance.AllChampions.Remove(tmp2);
        
        AudioManager.Instance.PlayEffectClip(0);
        OnCloseItemPickerWindow();
    }
    #endregion

    public void OnCloseItemPickerWindow()
    {
        for (int i = itemPrefabHolder.childCount - 1; i >= 0; i--)
        {
            GameObject tmp = itemPrefabHolder.GetChild(i).gameObject;
            tmp.transform.parent = null;
            Destroy(tmp);
        }
        itemPickerWindow.SetActive(false);
        currentChamp = PlayerInventory.Instance.CurrentChampions[champIndex].GetComponent<Champion>();
        championButtons[champIndex].GetComponentInChildren<TextMeshProUGUI>().text = $"{currentChamp.ChampionName} Lvl: {currentChamp.Level}";
        UpdateLeftPanel(currentChamp);
        UpdateEquipPanel(currentChamp);
        AudioManager.Instance.PlayEffectClip(3,false);
    }

    #region StatsPanel related
    private void UpdateStatsPanel(Champion c)
    {
        for (int i = 0; i < inventoryCrystals.Length; i++)
        {
            inventoryCrystals[i].text = $": {PlayerInventory.Instance.InventoryCrystals[i+1]}";
        }

        playerGold.text = $"Gold: {PlayerInventory.Instance.InventoryCrystals[0]}";
        criticalStats.text = $"Kritische Trefferchance: {(int)(currentChamp.CriticalChance*100)}%,  Kritischer Schaden: {(int)(currentChamp.CriticalMultiplier * 100)}%";
        blockStats.text = $"Ausweichchance: {(int)(currentChamp.EvasionChance * 100)}%,  Schadensminderung beim Blocken: {(int)(currentChamp.DmgReduction * 100)}%";
        initativeStats.text = $"Initiativewert: {currentChamp.Initiative}";
        for (int i = 0; i < resistancesStats.Length; i++)
            resistancesStats[i].text = $" {(int)(currentChamp.Resistances[i].ResistanceAmount*100)}%";
        for (int i = 0; i < defenseStats.Length; i++)
            defenseStats[i].text = $" {(int)(currentChamp.DefenseMainBase[i].DefenseAmount * 100)+(int)(currentChamp.DefenseMainCurrent[i].DefenseAmount * 100)}% ({(int)(currentChamp.DefenseSubBase[i].DefenseAmount * 100) + (int)(currentChamp.DefenseSubCurrent[i].DefenseAmount * 100)}%)";
    }

    #endregion

    private void UpdateEvoPanel(Champion c)
    {
        evoIndex = 0;
        // Only show the placeholders needed.
        for (int i = 0; i < evos.Length; i++)
        {
            mightLevelButtons[i].interactable = true;
            evolutionButtons[i].interactable = false;
            evos[i].SetActive(false);
        }
        for (int i = 0; i < currentChamp.PossibleEvolutions.Length; i++)
        {
            evoCImages[i].sprite = currentChamp.PossibleEvolutions[i].ChampionSprite;

            if (currentChamp.EvolveRequirementsMet(currentChamp.PossibleEvolutions[i]))
            {
                evolutionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentChamp.PossibleEvolutions[i].ChampionName;
                evolutionButtons[i].interactable = true;
            }
            else
            {
                evolutionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "???";
            }

            evos[i].SetActive(true);
        }

    }

    public void OnMightLevelButtonClicked(int index)
    {
        evoIndex = index;
        for (int i = 0; i < mightLevelButtons.Length; i++)
            mightLevelButtons[i].interactable = true;
        mightLevelButtons[evoIndex].interactable = false;
        UpdateMightLevels(currentChamp, currentChamp.PossibleEvolutions[evoIndex]);
        AudioManager.Instance.PlayEffectClip(1);

    }

    public void OnEvolutionButtonClicked(int index)
    {
        currentChamp.EvolveChampion(currentChamp.PossibleEvolutions[evoIndex]);
        currentChamp.UpdateChampionValues();
        currentChamp.UpdateBorder();
        UpdateEvoPanel(currentChamp);
        UpdateLeftPanel(currentChamp);
        championButtons[champIndex].GetComponentInChildren<TextMeshProUGUI>().text = $"{currentChamp.ChampionName}, Lvl: {currentChamp.Level}";
        AudioManager.Instance.PlayEffectClip(2);
        // Spawn particles.
    }

    #region SpecialUpgrades related

    public void ShowSpecialUpgradeScreen()
    {
        specUp = new List<SpecialUpgrades>();
        specUp = currentChamp.GetRandomSpecialUpgrades();
        if(specUp.Count == 0)
        {
            specUpgradePanel.SetActive(false);
            return;
        }
        for (int i = specialUpBtnHolder.childCount - 1; i >= 0; i--)
        {
            GameObject tmp = specialUpBtnHolder.GetChild(i).gameObject;
            tmp.transform.parent = null;
            Destroy(tmp);
        }


        // Initialize upgradebuttons.
        for (int i = 0; i < specUp.Count; i++)
        {
            Button tmp = Instantiate(specUpgradeButtonPrefab, specialUpBtnHolder);
            tmp.GetComponent<GeneratedButton>().ButtonIndex = i;
            tmp.onClick.AddListener(() => OnSpecialUpgradeClick(tmp.GetComponent<GeneratedButton>()));
            tmp.GetComponent<ButtonSpecialUpgrade>().InitializeButton(currentChamp, specUp[i]);
        }
        AudioManager.Instance.PlayEffectClip(5);
        specUpgradePanel.SetActive(true);

    }

    public void OnSpecialUpgradeClick(GeneratedButton btn)
    {
        currentChamp.AddSpecialUpgrade(specUp[btn.ButtonIndex]);
        UpdateLeftPanel(currentChamp);
        if (sideIndex == 1)
            UpdateStatsPanel(currentChamp);
        specUpgradePanel.SetActive(false);

        AudioManager.Instance.PlayEffectClip(2);
    }


    #endregion


    #endregion

    /// <summary>
    /// Opens the healing panel.
    /// </summary>
    /// <param name="status"></param>
    public void ShowHealingPanel(bool status)
    {
        if (!status)
        {
            PlayerController.Instance.CanMove = true;
            AudioManager.Instance.PlayEffectClip(2);
        }
        healingPanel.SetActive(status);
    }



    public Sprite GetCrystalSprite(Crystal crystal)
    {
        switch (crystal)
        {
            case Crystal.Attack:
                return Crystals[0];

            case Crystal.Darkness:
                return Crystals[1];
            case Crystal.Fire:
                return Crystals[2];
            case Crystal.Ice:
                return Crystals[3];
            case Crystal.Lightning:
                return Crystals[4];
            case Crystal.Wind:
                return Crystals[5];
            case Crystal.Destruction:
                return Crystals[6];
            case Crystal.Holy:
                return Crystals[7];
            case Crystal.Hunter:
                return Crystals[8];
            case Crystal.Seadragon:
                return Crystals[9];
            default:
                return null;
        }
    }
}
