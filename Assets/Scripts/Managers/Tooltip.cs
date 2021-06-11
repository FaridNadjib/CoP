using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Basically from this tutorial, like all 3 tooltip classes:
/// https://www.youtube.com/watch?v=HXFoUGw7eKk&list=LL&index=2&t=2s
/// </summary>
[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI content;

    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int charWrapLimit;

    [SerializeField] private RectTransform rectTransform;

    #endregion Fields

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        // If the tooltip is show, update its position.
        if (gameObject.activeSelf)
        {
            Vector2 position = Input.mousePosition;

            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;

            rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = position;
        }
    }

    /// <summary>
    /// Sets the actual text of the tooltip.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="header"></param>
    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
            this.header.gameObject.SetActive(false);
        else
        {
            this.header.gameObject.SetActive(true);
            this.header.text = header;
        }
        this.content.text = content;

        int headerLength = this.header.text.Length;
        int contentLength = this.content.text.Length;
        layoutElement.enabled = (contentLength > charWrapLimit || headerLength > charWrapLimit) ? true : false;
    }
}