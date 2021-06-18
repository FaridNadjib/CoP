using UnityEngine;

/// <summary>
/// The possible enemies the player can face here.
/// </summary>
[System.Serializable]
public struct Encounters
{
    
    [Space, SerializeField] int[] levels;
    [Space, SerializeField] private GameObject[] encounter;
    [Space, SerializeField] BaseChampion[] evolved;  

    public GameObject[] Encounter { get => encounter; }
    public BaseChampion[] Evolved { get => evolved; set => evolved = value; }
    public int[] Levels { get => levels; set => levels = value; }
}

/// <summary>
/// This class represents a grass area.
/// </summary>
public class GrassTriggerArea : MonoBehaviour
{
    [SerializeField] private float occuranceIntervall;
    [SerializeField] private float occuranceChance;
    private float timer;
    private float rng;
    private bool searchRift;
    bool isActive;

    private bool readyToBattle;

    [SerializeField] private Biom[] possibleBioms;
    [SerializeField] private Transitions transitionType;
    [SerializeField] private Encounters[] possibleEncounters;

    // Start is called before the first frame update
    private void Start()
    {
        TransitionManager.Instance.OnFinishedFading += (status) => { readyToBattle = status; };
    }

    // Update is called once per frame
    private void Update()
    {
        if (searchRift && PlayerController.Instance.IsFighting == false)
        {
            if (timer < occuranceIntervall)
                timer += Time.deltaTime;
            else
            {
                rng = Random.Range(0f, 1f);
                if (rng <= occuranceChance)
                {
                    readyToBattle = false;
                    AudioManager.Instance.PlayEffectClip(10);
                    // Starts the encounter.
                    PlayerController.Instance.IsFighting = true;
                    PlayerController.Instance.StandStill();
                    TransitionManager.Instance.BattleFade(transitionType);
                }
                timer = 0f;
            }
        }
        else if (PlayerController.Instance.IsFighting == true)
        {
            if (readyToBattle && BattleManager.Instance.NotInBattle && isActive)
            {
                isActive = false;
                BattleManager.Instance.NotInBattle = false;
                
                // Rng which encounter the player will face.
                int r = Random.Range(0, possibleEncounters.Length);
                //Start the combat via the battlemanager, pass the possible maps and the encounters.
                BattleManager.Instance.StartCombat(possibleBioms, possibleEncounters[r]);
            }
        }
    }

    /// <summary>
    /// Start to check for encounters.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.CanMove && (PlayerController.Instance.HorizontalFacing != 0 || PlayerController.Instance.VerticalFacing != 0))
            {
                searchRift = true;
                isActive = true;
            }
            else
            {
                searchRift = false;
            }
        }
    }

    /// <summary>
    /// Player left area.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            searchRift = false;
            isActive = false;
        }
    }
}