using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{

    public Joystick MovementJoystick;
    public Joystick RotationJoystick;
    public Transform CameraTransform;
    public float MoveSpeed;
    public float DashSpeed;
    public float RotationSpeed;

    public CharacterData Character => character;

    public Animator Animator => animator;

    [HideInInspector]
    public CharacterController controller;

    [HideInInspector]
    public Vector3 moveDirection;

    protected float horizontalMovement;
    protected float verticalMovement;

    protected CharacterData character;
    protected Animator animator;

    protected bool isTurning = false;

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

    private void Update()
    {
        MovePlayer();
        TurnPlayer();
        Animate();
    }

    private void IgnoreCollisions()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Character"),
            LayerMask.NameToLayer("Collectable"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("LootBox"), 
            LayerMask.NameToLayer("Collectable"));
        /*Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Character"),
            LayerMask.NameToLayer("Character"));*/
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Collectable"), 
            LayerMask.NameToLayer("Collectable"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Character"),
            LayerMask.NameToLayer("LootBox"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Gas"),
            LayerMask.NameToLayer("Projectile"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"),
            LayerMask.NameToLayer("Projectile"));
    }

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
