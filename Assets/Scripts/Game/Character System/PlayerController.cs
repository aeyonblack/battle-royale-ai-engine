using UnityEngine;

/// <summary>
/// Main player controller component
/// </summary>
public class PlayerController : Singleton<PlayerController>
{

    /// <summary>
    /// Main joystick used for movement
    /// </summary>
    public Joystick MovementJoystick;

    /// <summary>
    /// Main joystick used for rotation
    /// </summary>
    public Joystick RotationJoystick;

    /// <summary>
    /// Main Game camera
    /// </summary>
    public Transform CameraTransform;

    /// <summary>
    /// Speed of player in world space
    /// </summary>
    public float MoveSpeed;

    /// <summary>
    /// Dash speed of player in world space
    /// </summary>
    public float DashSpeed;

    /// <summary>
    /// Determines the speed of rotation in world space 
    /// </summary>
    public float RotationSpeed;

    /// <summary>
    /// Exposed reference to the character controller
    /// accessible to other game components attached to this object or
    /// otherwise
    /// </summary>
    public CharacterData Character => character;

    /// <summary>
    /// Main animator used to animate player motion and other actions
    /// </summary>
    public Animator Animator => animator;

    /// <summary>
    /// Collision object with position and velocity information
    /// </summary>
    [HideInInspector]
    public CharacterController controller;

    /// <summary>
    /// Direction the player is moving in with relation
    /// to the world space
    /// </summary>
    [HideInInspector]
    public Vector3 moveDirection;

    /// <summary>
    /// Magnitude of horizontal motion derived from the movement joystick
    /// i.e how far left or right is the movement joystick dragged
    /// </summary>
    protected float horizontalMovement;

    /// <summary>
    /// Magnitude of vertical motion derived from the movement joystic
    /// i.e how far up or down is the movement joystick dragged
    /// </summary>
    protected float verticalMovement;

    /// <summary>
    /// Holds subjective data about the character including the animator
    /// as well as inventory items and health etc.
    /// </summary>
    protected CharacterData character;

    /// <summary>
    /// References the animator attached to this player object
    /// </summary>
    protected Animator animator;

    /// <summary>
    /// True when the rotation joystic is moving and outside the dead zone
    /// </summary>
    protected bool isTurning = false;

    /// <summary>
    /// Main entry point for component initialization
    /// This is the only place and time when all components
    /// attached to the player object are initialized
    /// </summary>
    protected virtual void Start()
    {
        Application.targetFrameRate = 60;

        controller = GetComponent<CharacterController>();
        character = GetComponent<CharacterData>();
        character.Init();
        animator = character.GetAnimator();

        horizontalMovement = 0;
        verticalMovement = 0;

        IgnoreCollisions();
    }

    /// <summary>
    /// Listen to movement and turn events on every frame and
    /// perform the necessary updates
    /// </summary>
    private void Update()
    {
        MovePlayer();
        TurnPlayer();
        Animate();
    }

    /// <summary>
    /// Ignore collisions between objects on the specified layers
    /// </summary>
    private void IgnoreCollisions()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Character"),
            LayerMask.NameToLayer("Collectable"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("LootBox"), 
            LayerMask.NameToLayer("Collectable"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Collectable"), 
            LayerMask.NameToLayer("Collectable"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Character"),
            LayerMask.NameToLayer("LootBox"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Gas"),
            LayerMask.NameToLayer("Projectile"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"),
            LayerMask.NameToLayer("Projectile"));
    }

    /// <summary>
    /// Move the player object in relation to the movement joystic
    /// </summary>
    private void MovePlayer()
    {
        horizontalMovement = MovementJoystick.Horizontal;
        verticalMovement = MovementJoystick.Vertical;

        Quaternion rotation = Quaternion.Euler(0f, CameraTransform.eulerAngles.y, 0f);

        Vector3 horizontalMoveScreen = rotation * Vector3.right;
        Vector3 verticalMoveScreen = rotation * Vector3.forward;

        moveDirection = horizontalMoveScreen * horizontalMovement + verticalMoveScreen * verticalMovement;
        if (controller.enabled)
        {
            controller.SimpleMove(moveDirection * MoveSpeed);
        }

        if (!isTurning)
        {
            if (moveDirection != Vector3.zero)
            {
                Quaternion characterRotation = Quaternion.LookRotation(moveDirection);
                controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation,
                    characterRotation, RotationSpeed * Time.deltaTime);
            }
        }

        character.velocity = controller.velocity.magnitude;
    }

    /// <summary>
    /// Turn player in relation to the rotation joystick
    /// </summary>
    private void TurnPlayer()
    {
        float horizontalMovement = RotationJoystick.Horizontal;
        float verticalMovement = RotationJoystick.Vertical;

        if (Mathf.Abs(horizontalMovement) <= 0.01f || Mathf.Abs(verticalMovement) <= 0.01f)
        {
            isTurning = false;
            return;
        }
        else
        {
            isTurning = true;
            Quaternion rotation = Quaternion.Euler(0f, CameraTransform.eulerAngles.y, 0f);

            Vector3 horizontalMoveScreen = rotation * Vector3.right;
            Vector3 verticalMoveScreen = rotation * Vector3.forward;

            Vector3 moveDirection = horizontalMoveScreen * horizontalMovement + verticalMoveScreen * verticalMovement;

            if (moveDirection != Vector3.zero)
            {
                Quaternion characterRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    characterRotation, 180f * Time.deltaTime);
            }
        }

    }

    /// <summary>
    /// Appropriately animate player movement when player is in motion
    /// </summary>
    protected virtual void Animate()
    {
        float velocityZ = Vector3.Dot(moveDirection.normalized, transform.forward);
        float velocityX = Vector3.Dot(moveDirection.normalized, transform.right);

        animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

        bool moving = horizontalMovement != 0 || verticalMovement != 0;
        float weight = moving ? 1 : 0;
        animator.SetLayerWeight(1, weight);
    }

}
