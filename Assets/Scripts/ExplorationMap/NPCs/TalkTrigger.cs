using UnityEngine;

/// <summary>
/// This class handles the trigger area of an npc, whihc lets the player interact with him.
/// </summary>
public class TalkTrigger : MonoBehaviour
{
    #region Fields

    [Tooltip("The SO Editorsettings, for global gizmo settings.")]
    [SerializeField] private EditorSettings settings;

    [Tooltip("The npc this trigger belongs too.")]
    [SerializeField] private NpcManager npc;

    #endregion Fields

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Subscribe for event that player could start interaction.
        if (collision.CompareTag("Player"))
            PlayerController.Instance.OnInteracting += StartInteraction;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !npc.IsFighting)
        {
            // Does the player stands in the way of the npc? Then stop moving.
            if (npc.CaulculateFacingDirection(PlayerController.Instance.transform.position) == npc.CurrentFacing)
            {
                npc.WayBlocked = true;
                npc.MoveCharacter(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player left trigger so continue your path.
            npc.WayBlocked = false;
            if (npc.IsMoving)
                npc.MoveCharacter(true);

            // Unsubscribe from event.
            PlayerController.Instance.OnInteracting -= StartInteraction;
        }
    }

    /// <summary>
    /// Event method. Can the player start talking to this npc?
    /// </summary>
    private void StartInteraction()
    {
        if (!npc.IsTalking && !npc.IsFighting && npc.CaulculateFacingDirection(PlayerController.Instance.transform.position) == PlayerController.Instance.FacingDirection && !npc.CanFight)
        {
            Debug.Log("chatting");
            PlayerController.Instance.CanMove = false;
            npc.IsTalking = true;
            npc.CurrentFacing = npc.CaulculateFacingDirection(PlayerController.Instance.transform.position);
            npc.MoveCharacter(false);
        }
        else if (!npc.IsTalking && !npc.IsFighting && npc.CaulculateFacingDirection(PlayerController.Instance.transform.position) == PlayerController.Instance.FacingDirection)
        {
            npc.EngagePlayer();
            npc.CurrentFacing = npc.CaulculateFacingDirection(PlayerController.Instance.transform.position);
            npc.MoveCharacter(false);
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
            Vector3 scale = new Vector3(transform.localScale.x * transform.parent.localScale.x, transform.localScale.y * transform.parent.localScale.y, 1f);
            Gizmos.DrawCube(transform.position, scale);
        }
    }
}