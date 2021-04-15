using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Transitions { Random, Default, Checker, Checker2, Snail, Stripes}


public class TransitionManager : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject def;
    [SerializeField] GameObject checker;
    [SerializeField] GameObject stripes;
    [SerializeField] float waitingTime;

    Vector3 targetPosition;
    bool isTeleporting;

    Transitions activeTransition;

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
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += PlacePlayer;
    }


    public void TeleportPlayer(string sceneName, Vector3 targetPos, bool shouldTeleport, Transitions trans)
    {
        PlayerController.Instance.StandStill();
        
        targetPosition = targetPos;
        isTeleporting = shouldTeleport;
        activeTransition = trans;
        StartCoroutine("FadeOut", sceneName);
    }

    IEnumerator FadeOut(string sceneName)
    {
        PlayFadeOutAnimation();
        yield return new WaitForSeconds(waitingTime);
        if (isTeleporting)
        {
            PlayerController.Instance.transform.position = targetPosition;
            PlayFadeInAnimation();
            yield return new WaitForSeconds(waitingTime);
            DeactivateLayers();
            PlayerController.Instance.CanMove = true;
        }
        else
        {
            SceneManager.LoadScene(sceneName);

        }

        
    }

    private void PlayFadeOutAnimation()
    {
        if(activeTransition == Transitions.Random)
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

    private void DeactivateLayers()
    {
        def.SetActive(false);
        checker.SetActive(false);
        stripes.SetActive(false);
    }

    private void PlacePlayer(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine("InitPlayerPosition");
        Debug.Log("Success");
    }

    IEnumerator InitPlayerPosition()
    {
        PlayerController.Instance.transform.position = targetPosition;
        PlayFadeInAnimation();
        yield return new WaitForSeconds(waitingTime);
        DeactivateLayers();
        PlayerController.Instance.CanMove = true;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PlacePlayer;
    }
}
