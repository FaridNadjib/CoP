using UnityEngine;

/// <summary>
/// This class manages things in the players interaction trigger area.
/// </summary>
public class PlayerInteractionTrigger : MonoBehaviour
{
    #region Private Fields

    private Interactable target;
    private ShopArea targetShop;
    private GameObject tmp;

    #endregion Private Fields

    // Start is called before the first frame update
    private void Start()
    {
        // These are just tmps.
        target = new Interactable();
        targetShop = new ShopArea();
    }

    /// <summary>
    /// Checks if some interactable or shop is in his trigger.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the right component to start the interaction.
        if (collision.GetComponent<Interactable>() != null && collision.GetComponent<Interactable>().IsInteractable || collision.GetComponent<ShopArea>() != null)
        {
            PlayerController.Instance.OnInteracting += StartInteraction;
            if (collision.GetComponent<Interactable>() != null)
                target = collision.GetComponent<Interactable>();
            else if (collision.GetComponent<ShopArea>() != null)
                targetShop = collision.GetComponent<ShopArea>();

            tmp = ObjectPool.Instance.GetFromPool(Emotion.Attention.ToString());
            tmp.transform.parent = PlayerController.Instance.EmotionHolder;
            tmp.transform.localPosition = Vector3.zero;
            tmp.SetActive(true);
        }
    }

    /// <summary>
    /// Checks if the interaction left the trigger.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target != null || targetShop != null)
        {
            // Desubscribe to the event of player hitting action button.
            PlayerController.Instance.OnInteracting -= StartInteraction;
            target = null;
            targetShop = null;

            if (PlayerController.Instance.EmotionHolder.childCount > 0)
            {
                GameObject tmp = PlayerController.Instance.EmotionHolder.GetChild(0).gameObject;
                ObjectPool.Instance.AddToPool(tmp, Emotion.Attention.ToString());
            }
        }
    }

    /// <summary>
    /// Start the interaction from the object in trigger.
    /// </summary>
    private void StartInteraction()
    {
        if (target != null && target.IsInteractable)
            target.StartInteraction();
        else if (targetShop != null)
            targetShop.OpenShop();
    }
}