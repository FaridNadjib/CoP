using UnityEngine;

/// <summary>
/// Will move the player to new location.
/// </summary>
public class AreaTransitionTrigger : MonoBehaviour
{
    [Tooltip("This will tell if the player is just teleportet to target, or if a new scene has to be loaded. If unchecked sceneToLoad can stay empty.")]
    [SerializeField] private bool isTeleporter;

    [SerializeField] private string sceneToLoad;
    [SerializeField] private Transform entrancePoint;
    [SerializeField] private Transitions transitionType;

    /// <summary>
    /// Will move the player to new location.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TransitionManager.Instance.TeleportPlayer(sceneToLoad, entrancePoint.position, isTeleporter, transitionType);
        }
    }
}