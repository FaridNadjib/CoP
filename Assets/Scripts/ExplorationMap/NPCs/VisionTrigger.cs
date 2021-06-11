using UnityEngine;

/// <summary>
/// This class handles the FoV from an npc.
/// </summary>
public class VisionTrigger : MonoBehaviour
{
    #region Fields

    [Tooltip("The SO Editorsettings, for global gizmo settings.")]
    [SerializeField] private EditorSettings settings;

    [Tooltip("The npc this trigger belongs too.")]
    [SerializeField] private NpcManager npc;

    [SerializeField] private BoxCollider2D col;

    #endregion Fields

    private void OnTriggerEnter2D(Collider2D collision)
    {
        col = GetComponent<BoxCollider2D>();
        if (collision.CompareTag("Player") && npc.CanFight && PlayerController.Instance.CanMove)
        {
            npc.EngagePlayer();
        }
    }

    /// <summary>
    /// Draw gizmos to visualize the moveable object areas.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (settings != null && settings.ShowNpcTriggers && transform.parent != null)
        {
            Gizmos.color = settings.NpcTriggerColor;
            // Draw triggerarea.
            Vector3 scale = new Vector3(col.size.x * transform.parent.localScale.x, col.size.y * transform.parent.localScale.y, 1f);
            Vector3 offset = transform.position + transform.rotation * ((Vector3)col.offset);

            Gizmos.DrawCube(offset, transform.rotation * scale);
        }
    }
}