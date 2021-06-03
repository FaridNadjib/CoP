using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Encounters
{
    [SerializeField] GameObject[] encounter;

    public GameObject[] Encounter { get => encounter;}
}

public class GrassTriggerArea : MonoBehaviour
{
    [SerializeField] float occuranceIntervall;
    [SerializeField] float occuranceChance;
    float timer;
    float rng;
    bool searchRift;

    bool readyToBattle;

    [SerializeField] Biom[] possibleBioms;
    [SerializeField] Transitions transitionType;

    [SerializeField] Encounters[] possibleEncounters;

    // brauche hier eine map typ und eine liste von möglichen encountern, die werden an den battlemanager übergeben.
    // eventuell mit rooster klasse, eine liste aus champions, die dazu noch einen zufallswert hat, der kann dann hier abgeglichen werden.
    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.Instance.OnFinishedFading += (status) => { readyToBattle = status;};

    }

    // Update is called once per frame
    void Update()
    {
        if (searchRift && PlayerController.Instance.IsFighting == false)
        {
            if (timer < occuranceIntervall)
                timer += Time.deltaTime;
            else
            {
                rng = Random.Range(0f, 1f);
                if(rng <= occuranceChance)
                {
                    Debug.Log("Start Encounter");
                    PlayerController.Instance.IsFighting = true;
                    PlayerController.Instance.StandStill();
                    TransitionManager.Instance.BattleFade(transitionType);

                    

                    // Rng out which of the groups from this triggerarea should be in the fight, then start fight over battlemanager with the troops and map passed.
                    // DisablePlayermovement.
                }
                else
                {
                    Debug.Log("DodgedEncounter");
                }
                timer = 0f;
            }
        }else if(PlayerController.Instance.IsFighting == true)
        {
            if (readyToBattle)
            {
                readyToBattle = false;
                // Rng which encounter the player will face.
                int r = Random.Range(0, possibleEncounters.Length);
                //Start the combat via the battlemanager, pass the possible maps and the encounters.
                BattleManager.Instance.StartCombat(possibleBioms, possibleEncounters[r].Encounter);

            }

        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.CanMove && (PlayerController.Instance.HorizontalFacing != 0 || PlayerController.Instance.VerticalFacing != 0))
            {
                searchRift = true;
            }
            else
            {
                searchRift = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            searchRift = false;
    }
}
