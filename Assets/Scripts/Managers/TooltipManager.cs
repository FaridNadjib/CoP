using UnityEngine;

/// <summary>
/// This class shows and handles the tooltips. Its basically from this tutorial, no really need to change anythign there:
/// https://www.youtube.com/watch?v=HXFoUGw7eKk&list=LL&index=2&t=2s
/// </summary>
public class TooltipManager : MonoBehaviour
{
    [SerializeField] private Tooltip tooltip;

    #region Singleton

    private static TooltipManager current;

    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion Singleton

    /// <summary>
    /// Shows the tooltip.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="header"></param>
    public static void Show(string content, string header = "")
    {
        if(current.tooltip != null)
        {
            current.tooltip.SetText(content, header);
            current.tooltip.gameObject.SetActive(true);
        }
        
    }

    /// <summary>
    /// Hides the tooltip.
    /// </summary>
    public static void Hide()
    {
        if(current.tooltip != null)
            current.tooltip.gameObject.SetActive(false);
    }
}