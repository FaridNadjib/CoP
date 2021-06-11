using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The transitiontype.
/// </summary>
public enum Transitions { Random, Default, Checker, Checker2, Snail, Stripes }

/// <summary>
/// Manages the transitions.
/// </summary>
public class TransitionManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private Animator anim;
    [SerializeField] private GameObject def;
    [SerializeField] private GameObject checker;
    [SerializeField] private GameObject stripes;
    [SerializeField] private float waitingTime;

    private Vector3 targetPosition;
    private bool isTeleporting;
    private Transitions activeTransition;

    #endregion Fields

    #region Delegates.

    public delegate void FinishedFading(bool fadeFinished);

    public FinishedFading OnFinishedFading;
    public FinishedFading OnBattleEndFading;

    #endregion Delegates.

    #region Singleton

    public static TransitionManager Instance;

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
    /// Subscribe to on scene loaded.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += PlacePlayer;
    }

    /// <summary>
    /// Moves the palyer to new location.
    /// </summary>
    /// <param name="sceneName">The scene to move him to.</param>
    /// <param name="targetPos">The position.</param>
    /// <param name="shouldTeleport">If yes no scene is needed, will relocate in same scene.</param>
    /// <param name="trans">The transition type.</param>
    public void TeleportPlayer(string sceneName, Vector3 targetPos, bool shouldTeleport, Transitions trans)
    {
        PlayerController.Instance.StandStill();

        targetPosition = targetPos;
        isTeleporting = shouldTeleport;
        activeTransition = trans;
        StartCoroutine("FadeOut", sceneName);
    }

    /// <summary>
    /// Coroutine for fading out.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator FadeOut(string sceneName)
    {
        PlayFadeOutAnimation();
        yield return new WaitForSeconds(waitingTime);
        OnBattleEndFading?.Invoke(true);
        if (isTeleporting)
        {
            PlayerController.Instance.transform.position = targetPosition;
            PlayFadeInAnimation();
            yield return new WaitForSeconds(waitingTime);
            DeactivateLayers();
            PlayerController.Instance.CanMove = true;
        }
        else
            SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Play the fade out animation.
    /// </summary>
    private void PlayFadeOutAnimation()
    {
        if (activeTransition == Transitions.Random)
            activeTransition = (Transitions)Random.Range(1, 6);
        DeactivateLayers();

        switch (activeTransition)
        {
            case Transitions.Random:
                break;

            case Transitions.Default:
                def.SetActive(true);
                anim.Play("DefaultFadeOut");
                break;

            case Transitions.Checker:
                checker.SetActive(true);
                anim.Play("CheckerFadeOut");
                break;

            case Transitions.Checker2:
                checker.SetActive(true);
                anim.Play("Checker2FadeOut");
                break;

            case Transitions.Snail:
                checker.SetActive(true);
                anim.Play("SnailFadeOut");
                break;

            case Transitions.Stripes:
                stripes.SetActive(true);
                anim.Play("StripesFadeOut");
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Player the fade in animations.
    /// </summary>
    private void PlayFadeInAnimation()
    {
        switch (activeTransition)
        {
            case Transitions.Default:
                anim.Play("DefaultFadeIn");
                break;

            case Transitions.Checker:
                anim.Play("CheckerFadeIn");
                break;

            case Transitions.Checker2:
                anim.Play("Checker2FadeIn");
                break;

            case Transitions.Snail:
                anim.Play("SnailFadeIn");
                break;

            case Transitions.Stripes:
                anim.Play("StripesFadeIn");
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Fade to battle.
    /// </summary>
    /// <param name="trans"></param>
    public void BattleFade(Transitions trans)
    {
        activeTransition = trans;
        StartCoroutine("BattleFading");
    }

    /// <summary>
    /// Coroutine for battle fading.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BattleFading()
    {
        PlayFadeOutAnimation();
        yield return new WaitForSeconds(waitingTime);
        OnFinishedFading?.Invoke(true);
        PlayFadeInAnimation();
        yield return new WaitForSeconds(waitingTime);
        DeactivateLayers();
    }

    /// <summary>
    /// Deactivates the transition objects.
    /// </summary>
    private void DeactivateLayers()
    {
        def.SetActive(false);
        checker.SetActive(false);
        stripes.SetActive(false);
    }

    /// <summary>
    /// Places the player.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void PlacePlayer(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine("InitPlayerPosition");
    }

    /// <summary>
    /// Playces the player.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitPlayerPosition()
    {
        PlayerController.Instance.transform.position = targetPosition;
        PlayFadeInAnimation();
        yield return new WaitForSeconds(waitingTime);
        DeactivateLayers();
        PlayerController.Instance.CanMove = true;
    }

    /// <summary>
    /// Desubscribe to on scene loaded.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PlacePlayer;
    }
}