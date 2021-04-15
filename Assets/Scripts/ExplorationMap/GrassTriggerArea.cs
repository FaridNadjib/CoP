using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTriggerArea : MonoBehaviour
{
    [SerializeField] float occuranceIntervall;
    [SerializeField] float occuranceChance;
    float timer;
    float rng;
    bool searchRift;

    // brauche hier eine map typ und eine liste von möglichen encountern, die werden an den battlemanager übergeben.
    // eventuell mit rooster klasse, eine liste aus champions, die dazu noch einen zufallswert hat, der kann dann hier abgeglichen werden.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (searchRift)
        {
            if (timer < occuranceIntervall)
                timer += Time.deltaTime;
            else
            {
                rng = Random.Range(0f, 1f);
                if(rng <= occuranceChance)
                {
                    Debug.Log("Start Encounter");
                    // Rng out which of the groups from this triggerarea should be in the fight, then start fight over battlemanager with the troops and map passed.
                    // DisablePlayermovement.
                }
                else
                {
                    Debug.Log("DodgedEncounter");
                }
                timer = 0f;
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
