using UnityEngine;

/// <summary>
/// A simple class to deactivate a battle effect after an amount of time.
/// </summary>
public class BattleEffect : MonoBehaviour
{
    [SerializeField] private BattleEffects effectName;
    [SerializeField] private float deactivationTime;
    [SerializeField] AudioClip effectClip;
    [SerializeField] bool playSound;
    [SerializeField] bool playOnce;

    private float timer;

    private void OnEnable()
    {
        timer = 0.0f;      
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer < deactivationTime)
            timer += Time.deltaTime;
        else
            ObjectPool.Instance.AddToPool(this.gameObject, effectName.ToString());
        

    }

    public void PlaySound()
    {
        if(playSound)
            if (playOnce)
                AudioManager.PlayClipOnce(effectClip);
            else
                AudioManager.PlayClip(effectClip);
    }

}