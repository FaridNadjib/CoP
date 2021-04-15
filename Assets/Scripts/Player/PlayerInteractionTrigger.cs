using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionTrigger : MonoBehaviour
{
    Interactable target;
    GameObject tmp;

    // Start is called before the first frame update
    void Start()
    {
        target = new Interactable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactable>() != null && collision.GetComponent<Interactable>().IsInteractable)
        {
            PlayerController.Instance.OnInteracting += StartInteraction;
            target = collision.GetComponent<Interactable>();

            tmp = ObjectPool.Instance.GetFromPool(Emotion.Attention.ToString());
            tmp.transform.parent = PlayerController.Instance.EmotionHolder;
            tmp.transform.localPosition = Vector3.zero;
            tmp.SetActive(true);

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target != null)
        {
            PlayerController.Instance.OnInteracting -= StartInteraction;
            target = null;

            if(PlayerController.Instance.EmotionHolder.childCount > 0)
            {
                GameObject tmp = PlayerController.Instance.EmotionHolder.GetChild(0).gameObject;
                ObjectPool.Instance.AddToPool(tmp, Emotion.Attention.ToString());
            }
        }
    }

    private void StartInteraction()
    {
        if(target != null && target.IsInteractable)
            target.StartInteraction();
    }
}
