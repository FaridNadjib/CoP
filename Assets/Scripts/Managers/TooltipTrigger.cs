using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Add this component to any UI to enable the tooltip.
/// </summary>
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Fields

    [SerializeField] private string header;
    [SerializeField] private string content;
    private bool hide;

    #endregion Fields

    /// <summary>
    /// Interface for pointer enter.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(ShowDelayedTooltip());
        hide = false;
    }

    /// <summary>
    /// Interface for pointer exit.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(ShowDelayedTooltip());
        TooltipManager.Hide();
        hide = true;
    }

    /// <summary>
    /// To make sure tooltip is destroy when other go is deactiaved or destroyed.
    /// </summary>
    private void OnDisable()
    {
        StopCoroutine(ShowDelayedTooltip());
        TooltipManager.Hide();
        hide = true;
    }

    /// <summary>
    /// To make sure tooltip is destroy when other go is deactiaved or destroyed.
    /// </summary>
    private void OnDestroy()
    {
        StopCoroutine(ShowDelayedTooltip());
        TooltipManager.Hide();
        hide = true;
    }

    /// <summary>
    /// Coroutine for tip delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowDelayedTooltip()
    {
        yield return new WaitForSeconds(0.5f);
        if (!hide)
            TooltipManager.Show(content, header);
    }

    #region For GOs you need RB and Col

    public void OnMouseEnter()
    {
        StartCoroutine(ShowDelayedTooltip());
        hide = false;
    }

    public void OnMouseExit()
    {
        StopCoroutine(ShowDelayedTooltip());
        TooltipManager.Hide();
        hide = true;
    }

    #endregion For GOs you need RB and Col
}