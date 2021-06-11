using UnityEngine;

/// <summary>
/// Class for moveable grass.
/// </summary>
public class GrassTrigger : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Move the grass.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (transform.position.x < PlayerController.Instance.transform.position.x)
            {
                anim.SetInteger("PlayerPos", -1);
            }
            else if (transform.position.x > PlayerController.Instance.transform.position.x)
                anim.SetInteger("PlayerPos", 1);
            else if (transform.position.x == PlayerController.Instance.transform.position.x)
                anim.SetInteger("PlayerPos", 0);
        }
    }

    /// <summary>
    /// Player left trigger.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            anim.SetInteger("PlayerPos", 0);
    }
}