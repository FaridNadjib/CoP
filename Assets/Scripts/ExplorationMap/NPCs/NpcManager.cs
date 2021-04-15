using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Emotion { None, Angry, Attention, GoodMood, Kill, Love, Sleepy, Talking, Waiting }

/// <summary>
/// This class handles all the npc interaction and movement.
/// </summary>
public class NpcManager : MonoBehaviour
{
    [Tooltip("The SO Editorsettings, for global gizmo settings.")]
    [SerializeField] EditorSettings settings;
    [SerializeField] Animator anim;
    [SerializeField] bool isPatrolling;
    [SerializeField] bool isLooping;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed;
    [SerializeField] bool isLookingAround;
    [SerializeField] Direction initialFacing;
    [SerializeField] Direction facingRestriction1;
    [SerializeField] Direction facingRestriction2;
    [SerializeField] float maxTimeBetweenFacingChange;
    float changeTimer;
    float currentMaxChangeTime;

    [SerializeField] bool canRestBetweenWaypoints;
    [SerializeField] float maxBreakBetweenWaypoints;
    float breakTimer;
    float currentMaxBreakTime;

    bool isMoving;
    bool isResting;

    [SerializeField] Transform visionTrigger;
    [SerializeField] bool canFight;
    Quaternion rotation = new Quaternion();
    Vector3 angle = new Vector3();

    Queue<Transform> waypointQueue;
    bool reverseWaypoints;
    int waypointIndex;
    Transform currentWaypoint;
    Vector3 lastWaypoint;

    Direction currentFacing;

    bool isFighting;
    bool engagedInFight;
    bool isTalking;

    [Tooltip("Put here the GO with the dialogmanager.")]
    [SerializeField] GameObject dialog;
    [Tooltip("Will be anchor for any emojis.")]
    [SerializeField] Transform emojiHolder;
    Emotion activeEmotion;

    public bool IsFighting { get { return isFighting; } set { isFighting = value; } }
    public bool IsTalking { get { return isTalking; } set { isTalking = value; } }
    public bool WayBlocked { get; set; }
    public Direction BlockDirection { get; set; }
    public Direction CurrentFacing { get => currentFacing; set => currentFacing = value; }
    public bool IsMoving { get => isMoving; }
    public bool CanFight { get => canFight; set => canFight = value; }


    // Start is called before the first frame update
    void Start()
    {
        // Prepare waypoints depending on the npc settings.
        if (isPatrolling)
        {
            if (isLooping && waypoints.Length > 0)
            {
                waypointQueue = new Queue<Transform>(waypoints);
                currentWaypoint = waypointQueue.Dequeue();

            }
            
        }
        //First waypoint is always where the palyer starts in the world.
        //currentWaypoint = transform.parent.transform;
        currentWaypoint = waypoints[0];
        //currentWaypoint.position = transform.parent.transform.position;

        if (waypoints.Length == 0)
            isPatrolling = false;

        // Calculate random timers for taking a break and for chaging facing direction.
        currentMaxChangeTime = Random.Range(0, maxTimeBetweenFacingChange);
        currentMaxBreakTime = Random.Range(0, maxBreakBetweenWaypoints);

        // Set init facing.
        if(currentFacing != Direction.None)
        {
            currentFacing = initialFacing;
            MoveCharacter(false);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        // Handle npc movement.
        if(!WayBlocked && !isFighting && !isTalking)
        {
            if (isPatrolling && !isResting)
            {
                isMoving = true;

                    // Move the npc.
                    MoveToWaypoint();
                    // If he reached the waypoint, set next waypoint and set its facing direction.
                    if (ReachedWayPoint())
                    {
                        SetNextWaypoint();
                        //MoveCharacter(false);
                    }

            }

            // Is he taking a break?
            if (isLookingAround && !isMoving)
            {
                if (breakTimer < currentMaxBreakTime)
                {
                    breakTimer += Time.deltaTime;

                    if (changeTimer < currentMaxChangeTime)
                    {
                        changeTimer += Time.deltaTime;
                    }
                    else
                    {
                        currentMaxChangeTime = Random.Range(0, maxTimeBetweenFacingChange);
                        currentFacing = (Direction)Random.Range(0, 4);
                        for (int i = 0; i < 10; i++)
                        {
                            if (currentFacing == facingRestriction1 || currentFacing == facingRestriction2)
                                currentFacing = (Direction)Random.Range(0, 4);
                            else
                                break;
                        }
                        MoveCharacter(false);
                        changeTimer = 0;
                    }
                }
                else
                {
                    if (isPatrolling)
                    {
                        
                        isResting = false;
                        currentFacing = CaulculateFacingDirection(currentWaypoint.position);
                        MoveCharacter(true);
                    }
                    breakTimer = 0;
                    currentMaxBreakTime = Random.Range(0, maxBreakBetweenWaypoints);
                }

            }
        }

        // Engaging the player?
        if (isFighting)
        {
            if (!engagedInFight)
            {
                MoveToWaypoint();
                MoveCharacter(true);
                if (ReachedWayPoint())
                {
                    MoveCharacter(false);
                    dialog.SetActive(true);
                    engagedInFight = true;                  
                }

            }
            else
            {

            }
            // Once fight is over go back to old waypoint.

        }

        // Does the player talks to the npc?
        if (isTalking && !dialog.activeSelf)
        {
            dialog.SetActive(true);
        }


    }

    /// <summary>
    /// Checks if the npc reached its destination.
    /// </summary>
    /// <returns>Reached the current waypoint?</returns>
    private bool ReachedWayPoint()
    {
        // Reached the target? 
        if (Vector2.SqrMagnitude((Vector2)currentWaypoint.position - (Vector2)transform.position) < 0.00001)
        {
            isMoving = false;
            if (canRestBetweenWaypoints)
                isResting = true;
            return true;
        }
        else return false;
    }


    /// <summary>
    /// Set next waypoint.
    /// </summary>
    private void SetNextWaypoint()
    {
        if (isLooping)
        {
            waypointIndex++;
            if (waypointIndex == waypoints.Length)
                waypointIndex = 0;
            currentWaypoint = waypoints[waypointIndex];
        }
        else
        {
            if (!reverseWaypoints)
                waypointIndex++;
            else
                waypointIndex--;

            if (waypointIndex == waypoints.Length)
            {
                reverseWaypoints = !reverseWaypoints;
                waypointIndex--;
            }
            else if (waypointIndex == -1)
            {
                reverseWaypoints = !reverseWaypoints;
                waypointIndex++;
            }

            currentWaypoint = waypoints[waypointIndex];

            currentFacing = CaulculateFacingDirection(currentWaypoint.position);
        }
        
    }

    /// <summary>
    /// Moves the npc.
    /// </summary>
    private void MoveToWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);
    }

    private void ResetTriggers()
    {
        anim.ResetTrigger("LookRight");
        anim.ResetTrigger("WalkRight");
        anim.ResetTrigger("LookLeft");
        anim.ResetTrigger("WalkLeft");
        anim.ResetTrigger("LookUp");
        anim.ResetTrigger("WalkUp");
        anim.ResetTrigger("LookDown");
        anim.ResetTrigger("WalkDown");
    }

    /// <summary>
    /// Set the npcs animation and rotate him accordingly.
    /// </summary>
    /// <param name="isWalking">Walk animation?</param>
    public void MoveCharacter(bool isWalking)
    {
        ResetTriggers();
        switch (currentFacing)
        {
            case Direction.Right:
                if (isWalking == false)
                    anim.SetTrigger("LookRight");
                else
                    anim.SetTrigger("WalkRight");
                angle.z = 90;
                rotation.eulerAngles = angle;
                visionTrigger.rotation = rotation;

                break;
            case Direction.Left:
                if (!isWalking)
                    anim.SetTrigger("LookLeft");
                else
                    anim.SetTrigger("WalkLeft");
                angle.z = -90;
                rotation.eulerAngles = angle;
                visionTrigger.rotation = rotation;
                break;
            case Direction.Up:
                if (!isWalking)
                    anim.SetTrigger("LookUp");
                else
                    anim.SetTrigger("WalkUp");
                angle.z = 180;
                rotation.eulerAngles = angle;
                visionTrigger.rotation = rotation;
                break;
            case Direction.Down:
                if (!isWalking)
                    anim.SetTrigger("LookDown");
                else
                    anim.SetTrigger("WalkDown");
                angle.z = 0;
                rotation.eulerAngles = angle;
                visionTrigger.rotation = rotation;
                break;
            default:
                break;
                
        }
    }

    /// <summary>
    /// Calculates the current facing direction based on target.
    /// </summary>
    /// <param name="target">Where to look?</param>
    /// <returns>Direction of the target.</returns>
    public Direction CaulculateFacingDirection(Vector3 target)
    {
        if(Mathf.Abs(target.x - transform.position.x) > Mathf.Abs(target.y - transform.position.y))
        {
            if (target.x < transform.position.x)
                return Direction.Left;
            else
                return  Direction.Right;
        }
        else
        {
            if (target.y < transform.position.y)
                return Direction.Down;
            else
                return Direction.Up;
        }
    }

    /// <summary>
    /// Calculates the waypoint to the player and sets fighting to true.
    /// </summary>
    public void EngagePlayer()
    {
        Debug.Log("CurrentWP:" + currentWaypoint);
        lastWaypoint = waypoints[waypointIndex].position;
        PlayerController.Instance.GetAttention(currentFacing);
        isFighting = true;
        Vector3 tmp = PlayerController.Instance.transform.position;

        if(currentFacing == Direction.Down)
        {
            tmp.y += 1f;
        }else if (currentFacing == Direction.Up)
        {
            tmp.y -= 1f;
        }
        else if (currentFacing == Direction.Left)
        {
            tmp.x += 1f;
        }
        else if (currentFacing == Direction.Right)
        {
            tmp.x -= 1f;
        }
        currentWaypoint.position = tmp;

        ShowEmotion(Emotion.Attention);
    }

    /// <summary>
    /// Draw gizmos to visualize the moveable object areas.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (settings != null && settings.ShowNpcTriggers)
        {
            Gizmos.color = settings.NpcTriggerColor;

            if (isPatrolling)
            {
                if (isLooping)
                {
                    for (int i = 0; i < waypoints.Length - 1; i++)
                        Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                    Gizmos.DrawLine(waypoints[0].position, waypoints[waypoints.Length -1].position);
                }
                else
                    for (int i = 0; i < waypoints.Length - 1; i++)
                        Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }

    public void FinishedPreCombat()
    {
        dialog.SetActive(false);
        ClearEmoji();
        StartCombat();
    }

    private void StartCombat()
    {
        Debug.Log("Combat started");
        // Use battlemanager here.
        // if the player has won, the npc cant fight anymore.
        canFight = false;
        isFighting = false;
        isTalking = true;
        currentWaypoint.position = lastWaypoint;
        dialog.SetActive(true);
    }

    public void FinishedCombat()
    {

    }


    public void FinishedPostCombat()
    {

        isTalking = false;
        ClearEmoji();
        PlayerController.Instance.CanMove = true;
        dialog.SetActive(false);
    }

    private void ClearEmoji()
    {
        if (emojiHolder.childCount > 0)
        {
            GameObject tmp = emojiHolder.GetChild(0).gameObject;
            switch (activeEmotion)
            {
                case Emotion.None:
                    break;
                case Emotion.Angry:
                        ObjectPool.Instance.AddToPool(tmp, Emotion.Angry.ToString());
                    break;
                case Emotion.Attention:
                        ObjectPool.Instance.AddToPool(tmp, Emotion.Attention.ToString());
                    break;
                case Emotion.GoodMood:
                        ObjectPool.Instance.AddToPool(tmp, Emotion.GoodMood.ToString());
                    break;
                case Emotion.Kill:
                        ObjectPool.Instance.AddToPool(tmp, Emotion.Kill.ToString());
                    break;
                case Emotion.Love:
                        ObjectPool.Instance.AddToPool(tmp, Emotion.Love.ToString());
                    break;
                case Emotion.Sleepy:
                        ObjectPool.Instance.AddToPool(tmp, Emotion.Sleepy.ToString());
                    break;
                case Emotion.Talking:
                        ObjectPool.Instance.AddToPool(tmp, Emotion.Talking.ToString());
                    break;
                case Emotion.Waiting:
                        ObjectPool.Instance.AddToPool(tmp, Emotion.Waiting.ToString());
                    break;              
                default:
                    break;
            }
        }      
        activeEmotion = Emotion.None;
    }

    public void ShowEmotion(Emotion emo)
    {
        if (activeEmotion == emo)
            return;
        ClearEmoji();
        GameObject tmp = null;

        //GameObject tmp = new GameObject();
        switch (emo)
        {
            case Emotion.Angry:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Angry.ToString());
                break;
            case Emotion.Attention:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Attention.ToString());
                break;
            case Emotion.GoodMood:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.GoodMood.ToString());
                break;
            case Emotion.Kill:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Kill.ToString());                
                break;
            case Emotion.Love:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Love.ToString());
                break;
            case Emotion.Sleepy:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Sleepy.ToString());
                break;
            case Emotion.Talking:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Talking.ToString());
                break;
            case Emotion.Waiting:
                tmp = ObjectPool.Instance.GetFromPool(Emotion.Waiting.ToString());
                break;
            case Emotion.None:
                ClearEmoji();
                break;
            default:
                break;
        }
        if (tmp != null)
        {
            tmp.transform.parent = emojiHolder;
            tmp.transform.localPosition = Vector3.zero;
            tmp.SetActive(true);
        }
            activeEmotion = emo;
    }

}
