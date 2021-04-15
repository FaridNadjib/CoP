using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool IsInteractable { get; set; }
    public Crystals Crystals { get => crystals; set => crystals = value; }

    [SerializeField] Transform emojiHolder;
    [SerializeField] GameObject dialog;
    [SerializeField] Crystals crystals;
    [SerializeField] int amount;
    Emotion activeEmotion;

    // Start is called before the first frame update
    void Start()
    {
        IsInteractable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartInteraction()
    {
        IsInteractable = false;
        PlayerController.Instance.CanMove = false;
        dialog.SetActive(true);
        if (PlayerController.Instance.EmotionHolder.childCount > 0)
        {
            GameObject tmp = PlayerController.Instance.EmotionHolder.GetChild(0).gameObject;
            ObjectPool.Instance.AddToPool(tmp, Emotion.Attention.ToString());
        }
    }

    private void ClearEmoji()
    {
        if (emojiHolder.childCount > 0)
        {
            GameObject tmp = emojiHolder.GetChild(0).gameObject;
            switch (activeEmotion)
            {
                case Emotion.None:
                    break;
                case Emotion.Angry:
                    ObjectPool.Instance.AddToPool(tmp, Emotion.Angry.ToString());
                    break;
                case Emotion.Attention:
                    ObjectPool.Instance.AddToPool(tmp, Emotion.Attention.ToString());
                    break;
                case Emotion.GoodMood:
                    ObjectPool.Instance.AddToPool(tmp, Emotion.GoodMood.ToString());
                    break;
                case Emotion.Kill:
                    ObjectPool.Instance.AddToPool(tmp, Emotion.Kill.ToString());
                    break;
                case Emotion.Love:
                    ObjectPool.Instance.AddToPool(tmp, Emotion.Love.ToString());
                    break;
                case Emotion.Sleepy:
                    ObjectPool.Instance.AddToPool(tmp, Emotion.Sleepy.ToString());
                    break;
                case Emotion.Talking:
                    ObjectPool.Instance.AddToPool(tmp, Emotion.Talking.ToString());
                    break;
                case Emotion.Waiting:
                    ObjectPool.Instance.AddToPool(tmp, Emotion.Waiting.ToString());
                    break;
                default:
                    break;
            }
        }
        activeEmotion = Emotion.None;
    }

    public void ShowEmotion(Emotion emo)
    {
        if (activeEmotion == emo)
            return;
        ClearEmoji();
        GameObject tmp = null;

        //GameObject tmp = new GameObject();
        switch (emo)
        {
            case Emotion.Angry:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Angry.ToString());
                break;
            case Emotion.Attention:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Attention.ToString());
                break;
            case Emotion.GoodMood:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.GoodMood.ToString());
                break;
            case Emotion.Kill:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Kill.ToString());
                break;
            case Emotion.Love:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Love.ToString());
                break;
            case Emotion.Sleepy:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Sleepy.ToString());
                break;
            case Emotion.Talking:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Talking.ToString());
                break;
            case Emotion.Waiting:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Waiting.ToString());
                break;
            case Emotion.None:
                ClearEmoji();
                break;
            default:
                break;
        }
        if (tmp != null)
        {
            tmp.transform.parent = emojiHolder;
            tmp.transform.localPosition = Vector3.zero;
            tmp.SetActive(true);
        }
        activeEmotion = emo;
    }

    public void FinishedInteraction()
    {
        dialog.SetActive(false);
        PlayerInventory.Instance.AddCrystals(crystals, amount);
        GameObject tmp = ObjectPool.Instance.GetFromPool(crystals.ToString());
        tmp.transform.parent = PlayerController.Instance.ItemIndicator;
        tmp.transform.localPosition = Vector3.zero;
        tmp.SetActive(true);
        PlayerController.Instance.CanMove = true;
    }
}
