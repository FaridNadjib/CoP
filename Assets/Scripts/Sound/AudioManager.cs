using UnityEngine;

/// <summary>
/// This class will handle my sounds.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;

    [SerializeField] AudioClip[] effects;
    [SerializeField] AudioClip[] musics;

    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion Singleton

    /// <summary>
    /// Stops the music source and then plays a new clip.
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayMusicClip(AudioClip clip)
    {
        if (clip != null)
        {
            Instance.musicSource.Stop();
            Instance.musicSource.clip = clip;
            Instance.musicSource.Play();
        }
    }

    /// <summary>
    /// Plays the clip once, no matter if effect source is playing something..
    /// </summary>
    /// <param name="clip">The clip to play.</param>
    public static void PlayClipOnce(AudioClip clip)
    {
        if (clip != null)
            Instance.effectSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Checks if no effects are playing then plays the clip once.
    /// </summary>
    /// <param name="clip">The clip to play.</param>
    public static void PlayClip(AudioClip clip)
    {
        if (clip != null && !Instance.effectSource.isPlaying)
            Instance.effectSource.PlayOneShot(clip);
    }

    public void StopEffectSound()
    {
        effectSource.Stop();
    }

    public void PlayEffectClip(int index, bool playOnce = true)
    {
        if (playOnce)
        {
            if (index < effects.Length && effects[index] != null)
                Instance.effectSource.PlayOneShot(effects[index]);
        }
        else
        {
            if (index < effects.Length && effects[index] != null && !Instance.effectSource.isPlaying)
                Instance.effectSource.PlayOneShot(effects[index]);
        }
        
    }

    /// <summary>
    /// Plays music at given index.
    /// </summary>
    /// <param name="index"></param>
    public void PlayMusic(int index)
    {
        if(musics[index] != null)
        {
            musicSource.Stop();
            musicSource.clip = musics[index];
            musicSource.Play();
        }
    }
}