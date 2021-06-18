using UnityEngine;

/// <summary>
/// This class will control the playermovement and enviorment interaction.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Private Fields

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    private float currentSpeed;
    [SerializeField] private float sprintSpeed;
    private float horizontalMovement;
    private float verticalMovement;
    [SerializeField] private Animator anim;

    private bool canMove = true;
    private bool isJumping;
    private Vector2 jumpTargetPosition;

    private Quaternion rotation = new Quaternion();
    private Vector3 angle = new Vector3();

    [Tooltip("Which actions the player can perform, smaller numbers can be performed as well. 0 = normal jumps, 1 = canSprint, 2 = large jump, 3 = powerjump.")]
    [SerializeField] private byte bootsLevel;

    [Tooltip("Which actions the player can perform, smaller numbers can be performed as well. 0 = push small objects, 1 = push large objects, 2 = push heavy objects.")]
    [SerializeField] private byte gloveGripLevel;

    [Tooltip("The scale of the spritemask sphere. 1.5f is default which means blind in the dark. 8 - 9 seems good max value.")]
    [SerializeField] private float playerSpriteMaskSize = 1.5f;

    [Tooltip("The spritemaskobject.")]
    [SerializeField] private Transform playerSpriteMask;

    [SerializeField] private Transform emotionHolder;
    [SerializeField] private Transform itemIndicator;
    [SerializeField] private Transform interactionTrigger;

    private Vector3 teleportLocation;
    private Vector3 safeSpotLocation;

    private bool isInteracting;
    private Direction facingDirection;

    private bool isFighting;

    #endregion Private Fields

    #region Properties

    public bool IsJumping => isJumping;
    public float HorizontalFacing => horizontalMovement;
    public float VerticalFacing => verticalMovement;
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public byte BootsLevel { get { return bootsLevel; } set { bootsLevel = value; } }
    public byte GloveGripLevel { get { return gloveGripLevel; } set { gloveGripLevel = value; } }
    public float PlayerSpriteMaskSize { get { return playerSpriteMaskSize; } set { playerSpriteMaskSize = value; ChangePlayerSight(); } }

    public bool IsInteracting { get => isInteracting; set => isInteracting = value; }
    public Direction FacingDirection { get => facingDirection; set => facingDirection = value; }
    public Transform EmotionHolder { get => emotionHolder; set => emotionHolder = value; }
    public Transform ItemIndicator { get => itemIndicator; set => itemIndicator = value; }
    public Vector3 TeleportLocation { get => teleportLocation; set => teleportLocation = value; }
    public bool IsFighting { get => isFighting; set => isFighting = value; }
    public Vector3 SafeSpotLocation { get => safeSpotLocation; set => safeSpotLocation = value; }

    #endregion Properties

    public delegate void Interaction();

    public Interaction OnInteracting;

    #region Singleton

    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion Singleton

    // Start is called before the first frame update
    private void Start()
    {
        // Set default values.
        currentSpeed = speed;
        ChangePlayerSight();
    }

    // Update is called once per frame
    private void Update()
    {
        // If the player can move, get his input and set his facing.
        if (canMove)
        {
            if (verticalMovement == 0)
            {
                horizontalMovement = Input.GetAxisRaw("Horizontal");
                anim.SetFloat("xMovement", horizontalMovement);
                if (horizontalMovement == 1)
                {
                    angle.z = 90;
                    rotation.eulerAngles = angle;
                    interactionTrigger.rotation = rotation;
                }
                else if (horizontalMovement == -1)
                {
                    angle.z = -90;
                    rotation.eulerAngles = angle;
                    interactionTrigger.rotation = rotation;
                }
            }
            if (horizontalMovement == 0)
            {
                verticalMovement = Input.GetAxisRaw("Vertical");
                anim.SetFloat("yMovement", verticalMovement);
                if (verticalMovement == 1)
                {
                    angle.z = 180;
                    rotation.eulerAngles = angle;
                    interactionTrigger.rotation = rotation;
                }
                else if (verticalMovement == -1)
                {
                    angle.z = 0;
                    rotation.eulerAngles = angle;
                    interactionTrigger.rotation = rotation;
                }
            }

            if (horizontalMovement != 0 || verticalMovement != 0)
            {
                anim.SetFloat("horizontalFacing", horizontalMovement);
                anim.SetFloat("verticalFacing", verticalMovement);
                SaveFaceDirection();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnInteracting?.Invoke();
            }

            // Open the menu.
            if (Input.GetKeyDown(KeyCode.Tab))
                UIManager.Instance.ToggleChampionMenu(true);
        }
        else if (!CanMove && !IsFighting && Input.GetKeyDown(KeyCode.Tab))
            UIManager.Instance.ToggleChampionMenu(false);

        // Set sprintspeed.
        if (Input.GetKeyDown(KeyCode.LeftShift) && bootsLevel >= 1)
        {
            currentSpeed += sprintSpeed;
            anim.speed = 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && bootsLevel >= 1)
        {
            currentSpeed -= sprintSpeed;
            anim.speed = 1f;
        }

        // Perform jump.
        if (isJumping)
        {
            transform.position = Vector3.MoveTowards(transform.position, jumpTargetPosition, currentSpeed * 0.7f * Time.deltaTime);
            // Reached the target? Change values back then.
            if (Vector2.SqrMagnitude(jumpTargetPosition - (Vector2)transform.position) < 0.00001)
            {
                isJumping = false;
                canMove = true;
                Physics2D.IgnoreLayerCollision(8, 9, false);
            }
        }

        // For testing.
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerInventory.Instance.Cheat();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        // If the player can move set its velocity.
        if (canMove)
            rb.velocity = new Vector2(horizontalMovement, verticalMovement).normalized * currentSpeed;
    }

    /// <summary>
    /// Start the jump action.
    /// </summary>
    /// <param name="targetPos">Target direction of the jump.</param>
    public void Jump(Vector2 targetPos)
    {
        // Ignore layercollision while jumping. Set other values.
        Physics2D.IgnoreLayerCollision(8, 9, true);
        isJumping = true;
        canMove = false;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("playerJump");
        jumpTargetPosition = targetPos;
    }

    /// <summary>
    /// Scales the players vision field.
    /// </summary>
    public void ChangePlayerSight()
    {
        playerSpriteMask.localScale *= playerSpriteMaskSize;
    }

    /// <summary>
    /// Saves the current facing direction of the player, in opposite. So i can compare if he has same direction as npc who looks at him.
    /// </summary>
    public void SaveFaceDirection()
    {
        if (horizontalMovement == 1)
            facingDirection = Direction.Left;
        else if (horizontalMovement == -1)
            facingDirection = Direction.Right;
        if (verticalMovement == 1)
            facingDirection = Direction.Down;
        else if (verticalMovement == -1)
            facingDirection = Direction.Up;
    }

    /// <summary>
    /// Makes the player stop moving and look into specific direction.
    /// </summary>
    /// <param name="dir">The direction to look at.</param>
    public void GetAttention(Direction dir)
    {
        // Make him uncontrolable and zero his movement.
        canMove = false;
        horizontalMovement = 0;
        verticalMovement = 0;
        rb.velocity = Vector2.zero;
        anim.SetFloat("xMovement", 0f);
        anim.SetFloat("yMovement", 0f);

        // Set right animation.
        switch (dir)
        {
            case Direction.Right:
                anim.SetFloat("horizontalFacing", -1f);
                break;

            case Direction.Left:
                anim.SetFloat("horizontalFacing", 1f);
                break;

            case Direction.Up:
                anim.SetFloat("verticalFacing", -1f);
                break;

            case Direction.Down:
                anim.SetFloat("verticalFacing", 1f);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Makes the player unable to move.
    /// </summary>
    public void StandStill()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        anim.SetFloat("xMovement", 0f);
        anim.SetFloat("yMovement", 0f);
    }

    /// <summary>
    /// Saves the latest respawn position.
    /// </summary>
    /// <param name="pos">The position to spawn the player next time.</param>
    public void SetSafeSpotLocation(Vector3 pos)
    {
        SafeSpotLocation = pos;
    }
}