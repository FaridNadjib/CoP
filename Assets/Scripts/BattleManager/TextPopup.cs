using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A class which represents an animated popupText.
/// </summary>
public class TextPopup : MonoBehaviour
{
    #region Fields
    [SerializeField] float fadeTime;
    float timer;

    [SerializeField] TextMeshProUGUI damageNumber;
    [SerializeField] Color[] damageColors;
    [SerializeField] int[] fontSizes;
    #endregion

    private void OnEnable()
    {
        // Since it gets pooled reset the timer here.
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Make it go back to pool after delay.
        if(timer < fadeTime)

            timer += Time.deltaTime;
        else
            ObjectPool.Instance.AddToPool(gameObject, "textPopup");
    }

    /// <summary>
    /// Shows the text and changes color and size depending on damagetype.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageType"></param>
    public void ShowDamage(float damage, int damageType)
    {
        damageNumber.color = damageColors[damageType];
        damageNumber.fontSize = fontSizes[damageType];
        if((int)damage != 0)
            damageNumber.text = $"{(int)damage}";
    }
}
