using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class lets the player specify targets for selection.
/// </summary>
public class ChampTargetTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Champion champ;


    void Awake()
    {
        champ = GetComponentInParent<Champion>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.SetChampionTarget(gameObject.GetComponentInParent<Champion>());
        // Once the selection is done disable the targeting.
        BattleManager.Instance.DisableTargeting();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        champ.TargetChampion(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        champ.TargetChampion(false);
    }
}
