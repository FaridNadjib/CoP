using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows me in which direction an action should be performed.
/// </summary>
public enum Direction { Right, Left, Up, Down, None }

/// <summary>
/// This class manages triggerareas from where the player can jump.
/// </summary>
public class JumpTrigger : MonoBehaviour
{

    #region Fields
    [Tooltip("The SO Editorsettings, for global gizmo settings.")]
    [SerializeField] EditorSettings settings;
    [Tooltip("The direction the action should be performed.")]
    [SerializeField] Direction direction;
    [Tooltip("How far the player should jump.")]
    [SerializeField] float jumpRange = 1f;
    [Tooltip("How depth the jump goes, only useful for vertical jumps.")]
    [SerializeField] float jumpDepth = 0f;
    [Tooltip("The players bootslevel has to be greater equal than this to be able to perform this action. 0 = default jump, 2 = large jump, 3 = powerjump.")]
    [SerializeField] byte requiredBootsLevel = 0;
    Vector2 targetPosition;
    #endregion

    /// <summary>
    /// Checks if player enters the trigger and presses the right button to perform a jump action.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            // Is the player not already jumping and does he have the right shoes?
            if (!player.IsJumping && player.BootsLevel >= requiredBootsLevel)
            {
                // Depending direction calculate target position and let the player jump.
                switch (direction)
                {
                    case Direction.Right:
                        if (player.HorizontalFacing == 1)
                        {
                            targetPosition = new Vector2(player.transform.position.x + jumpRange, player.transform.position.y + jumpDepth);
                            player.Jump(targetPosition);
                        }
                        break;
                    case Direction.Left:
                        if (player.HorizontalFacing == -1)
                        {
                            targetPosition = new Vector2(player.transform.position.x - jumpRange, player.transform.position.y + jumpDepth);
                            player.Jump(targetPosition);
                        }
                        break;
                    case Direction.Up:
                        if (player.VerticalFacing == 1)
                        {
                            targetPosition = new Vector2(player.transform.position.x, player.transform.position.y + jumpRange + jumpDepth);
                            player.Jump(targetPosition);
                        }
                        break;
                    case Direction.Down:
                        if (player.VerticalFacing == -1)
                        {
                            targetPosition = new Vector2(player.transform.position.x, player.transform.position.y - jumpRange + jumpDepth);
                            player.Jump(targetPosition);
                        }
                        break;
                    default:
                        break;
                }
            }            
        }
    }

    /// <summary>
    /// Draw gizmos to visualize the jump areas.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (settings != null && settings.ShowJumpTriggers)
        {
            Gizmos.color = settings.JumpTriggerColor;
            // Draw triggerarea.
            Gizmos.DrawCube(transform.position, transform.localScale);
            // Show from where the action should be performed.
            Vector3 actionIndicator = transform.position;
            Vector3 targetPos = transform.position;
            switch (direction)
            {
                case Direction.Right:
                    actionIndicator.x -= 0.1f;
                    targetPos.x += jumpRange;
                    targetPos.y += jumpDepth;
                    break;
                case Direction.Left:
                    actionIndicator.x += 0.1f;
                    targetPos.x -= jumpRange;
                    targetPos.y += jumpDepth;
                    break;
                case Direction.Up:
                    actionIndicator.y -= 0.1f;
                    targetPos.y += jumpRange + jumpDepth;
                    break;
                case Direction.Down:
                    actionIndicator.y += 0.1f;
                    targetPos.y -= jumpRange;
                    targetPos.y += jumpDepth;
                    break;
                default:
                    break;
            }
            Gizmos.DrawSphere(actionIndicator, 0.05f);
            // Draw the target of the jump.
            Gizmos.DrawSphere(targetPos, 0.08f);

        }      
    }
}
