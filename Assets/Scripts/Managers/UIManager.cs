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
    [SerializeField] Button buttonPrefab;
    [SerializeField] TextMeshProUGUI shopDescription;
    [SerializeField] TextMeshProUGUI shopGold;
    [SerializeField] Transform shopButtonHolder;


    [Header("Champion menu related:")]
    [SerializeField] GameObject championMenuScreen;


    #region Singleton
    public static UIManager Instance;

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
            devotionImage.sprite = starSigns[(int)champ.Devotion];
            devotionImage.color = starsSignColors[(int)champ.Devotion];
            devotionAmount.text = $"x{champ.DevotionAmount}";
            devotionAmount.color = starsSignColors[(int)champ.Devotion];
        }
        else
        {
            devotionImage.sprite = null;
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
                tmp = "Health lost:";
                break;
            case SpecialRequirement.Energy:
                tmp = "Energy used:";
                break;
            case SpecialRequirement.HealthRec:
                tmp = "Health recovered:";
                break;
            case SpecialRequirement.EnergyRec:
                tmp = "Energy recovered:";
                break;
            case SpecialRequirement.XpGain:
                tmp = "XP gained:";
                break;
            case SpecialRequirement.DamageDealt:
                tmp = "Damage dealt:";
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
        healthRegen.text = $"Hp Regeneration: {currentChamp.HealthRecovery}+({currentChamp.HealthRecoveryBuff}): {currentChamp.HealthRecBuffDuration}";
        // Set energy details.
        energy.text = $"{(int)currentChamp.CurrentEnergy}/{(int)currentChamp.MaxEnergy}";
        energyFill.fillAmount = currentChamp.CurrentEnergy / currentChamp.MaxEnergy;
        energyRegen.text = $"Energy Regeneration: {currentChamp.EnergyRecovery}+({currentChamp.EnergyRecoveryBuff}): {currentChamp.EnergyRecBuffDuration}";
        // Set resistance details.
        for (int i = 0; i < resis.Length; i++)
            resis[i].text = $"{currentChamp.Resistances[i].ResistanceAmount*100}% +({currentChamp.ResistancesBuff[i].ResistanceAmount*100}%) :{currentChamp.ResistanceBuffDuration}";
        // Set defense and acc details.
        defense.text = $"Defense: {(int)(currentChamp.Defense* 100)}% <color=green>+({currentChamp.DefenseBuff})</color> : {currentChamp.DefenseBuffDuration}";
        evade.text = $"Block: {currentChamp .DmgReduction* 100}%, Dodge: {currentChamp .EvasionChance* 100}%";
        crit.text = $"Krit: {currentChamp .CriticalChance* 100}%, ({currentChamp.CriticalMultiplier * 100}%)";

        statsScreen.SetActive(true);
        statsScreenButton.gameObject.SetActive(false);
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
            devotionImage.sprite = starSigns[(int)champ.Devotion];
            devotionImage.color = starsSignColors[(int)champ.Devotion];
            devotionAmount.text = $"x{champ.DevotionAmount}";
            devotionAmount.color = starsSignColors[(int)champ.Devotion];
        }
        else
        {
            devotionImage.sprite = null;
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
    }

    public void ShowContinueButton(bool status)
    {
        continueButton.SetActive(status);
    }

    public void AttackButtons(int weaponIndex)
    {
        DiableAllAttackButtons();
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

    }

    public void SetChampionTarget(Champion target)
    {
        BattleManager.Instance.AttackSingle(target, weapon);
    }

    public void HideBattleUIScreen()
    {
        battleScreenUIPlayer.SetActive(false);
        rewardScreen.SetActive(false);
    }

    public void ShowRewardScreen(List<Champion> champs, int gold, float xp, int[]crystalReward)
    {
        battleScreenUIPlayer.SetActive(false);
        rewardScreen.SetActive(true);

        for (int i = 0; i < champs.Count; i++)
        {
            headers[i].text = $"{champs[i].ChampionName}, Lvl:{champs[i].Level} ";
            if (!champs[i].IsAlive)
                headers[i].text += $"<color=red>(Dead)</color>";
            championImages[i].sprite = champs[i].ChampionSprite.sprite;
            championBorders[i].sprite = champs[i].ChampionBorder.sprite;
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
    }


    public void ShowShopScreen(ShopArea shop)
    {
        shopScreen.SetActive(true);

        for (int i = shopButtonHolder.childCount - 1; i > 0; i--)
        {
            Destroy(shopButtonHolder.GetChild(i));
        }

        for (int i = 0; i < 15; i++)
        {
            Button tmp = Instantiate(buttonPrefab, shopButtonHolder);
            //tmp.onClick.AddListener(Tst);
            //tmp.onClick.AddListener(delegate { Tst(i); });
            //delegate { SwitchButtonHandler(0); });
            Debug.Log("how often?");
            //tmp.onClick.AddListener(delegate { Tst(i); });
            tmp.GetComponent<GeneratedButton>().ButtonIndex = i;

            tmp.onClick.AddListener(() => Tst(tmp.GetComponent<GeneratedButton>()));
            //tmp.onClick.AddListener(Tst);
        }




    }

    public void CloseShopScreen()
    {
        shopScreen.SetActive(false);
        PlayerController.Instance.CanMove = true;
    }

    public void ToggleChampionMenu(bool status)
    {
        championMenuScreen.SetActive(status);
        PlayerController.Instance.CanMove = !status;
    }


    public void Tst(GeneratedButton btn)
    {
        

        Debug.Log("Testsign" + btn.ButtonIndex);
    }
}
