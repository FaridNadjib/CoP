using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AreaTransitionTrigger : MonoBehaviour
{
    [Tooltip("This will tell if the player is just teleportet to target, or if a new scene has to be loaded. If unchecked sceneToLoad can stay empty.")]
    [SerializeField] bool isTeleporter;
    [SerializeField] string sceneToLoad;
    [SerializeField] Transform entrancePoint;
    [SerializeField] Transitions transitionType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TransitionManager.Instance.TeleportPlayer(sceneToLoad, entrancePoint.position, isTeleporter, transitionType);
        }
    }
}
