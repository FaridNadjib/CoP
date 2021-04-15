using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class checks if the player is able to move a cabinet for example.
/// Things like constrains and drag have to be set directly in the rigidbody 2d.
/// Set this as child of the object you want to move.
/// </summary>
public class MovingObjectTrigger : MonoBehaviour
{
    #region Fields
    [Tooltip("The SO Editorsettings, for global gizmo settings.")]
    [SerializeField] EditorSettings settings;
    Rigidbody2D rb;
    [Tooltip("Can the player move this? 0 = push small objects, 1 = push large objects, 2 = push heavy objects.")]
    [SerializeField] byte requiredGripLevel;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    /// <summary>
    /// Checks if the player has the skills to make the object moveable.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if(player.GloveGripLevel >= requiredGripLevel)
                rb.isKinematic = false;
        }
    }

    /// <summary>
    /// Draw gizmos to visualize the moveable object areas.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (settings != null && settings.ShowMoveableObjectTriggers && transform.parent != null)
        {
            Gizmos.color = settings.MoveableObjectTriggerColor;
            // Draw triggerarea.
            Vector3 scale = new Vector3(transform.localScale.x * transform.parent.localScale.x, transform.localScale.y * transform.parent.localScale.y, 1f);
            Gizmos.DrawCube(transform.position, scale);
        }
    }
}
