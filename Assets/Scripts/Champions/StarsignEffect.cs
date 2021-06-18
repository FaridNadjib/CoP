using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class actavates and deactivates the starbuff.
/// </summary>
public class StarsignEffect : MonoBehaviour
{

    [SerializeField] GameObject buffEffectffect;
    [SerializeField] GameObject debuffEffectffect;

    [SerializeField] AudioClip debuffSound;
    [SerializeField] AudioClip buffSound;

    [SerializeField] float deactivationTime;
    float timer;
    bool isInactive;


    // Update is called once per frame
    void Update()
    {
        if (!isInactive)
        {
            if (timer < deactivationTime)
                timer += Time.deltaTime;
            else
            {
                buffEffectffect.SetActive(false);
                debuffEffectffect.SetActive(false);
                timer = 0.0f;
                isInactive = true;
            }
        }
    }

    /// <summary>
    /// Activates the buff or debuff based on devotionbonus.
    /// </summary>
    /// <param name="bonus"></param>
    public void ShowSatrsignBonus(float bonus)
    {
        isInactive = false;
        if (bonus == 0.0f)
            return;
        else if(bonus < 0.0f)
        {
            AudioManager.PlayClipOnce(debuffSound);
            debuffEffectffect.SetActive(true);
        }
        else
        {
            AudioManager.PlayClipOnce(buffSound);
            buffEffectffect.SetActive(true);
        }
    }
}
