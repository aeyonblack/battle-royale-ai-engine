using System.Collections;
using UnityEngine;


public class PlayerWeapon : WeaponController
{
    [Header("Settings")]
    public Joystick FireButton;
    public AimingHelper WeaponAim;
    public Transform PlayerForward;
    public LayerMask TargetMask;
    public Weapon weaponData;
    public Clip clip;
   
    [Header("CameraShaking")]
    public float ShakeStrength;
    public float ShakeDuration;

    private Animator weaponAnimator;

    private WeaponState state = WeaponState.IDLING;


    private void Awake()
    {
        if (GetComponent<Animator>())
        {
            weaponAnimator = GetComponent<Animator>();
        }

        FireButton.Attack = false;
    }

    private void OnEnable()
    {
        FireButton.OnAttack += StartDelayAttack;
        FireButton.OnStopAttack += StopAttack;
        FireButton.OnHandleAtCenter += ToggleAim;
        FireButton.OnHandleNotAtCenter += ToggleAim;
    }

    private void Update()
    {
        UpdateControllerState();
        TryDrawPath();
    }

    private void StartDelayAttack()
    {
        StartCoroutine("DelayAttack");
    }

    private void StopAttack()
    {
        PlayerController.instance.Animator.SetBool("attacking", false);
        StopCoroutine("DelayAttack");
    }

    private IEnumerator DelayAttack()
    {
        PlayerController.instance.Animator.SetBool("attacking", true);
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            ShootProjectile();
            yield return new WaitForSeconds(weaponData.fireRate);
        }
    }

    /// <summary>
    /// Triggered externally to launch the actual shooting coroutine
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="fireRate"></param>
    public void HeavyFire(float duration, float fireRate)
    {
        StartCoroutine(HeavyFireCoroutine());
    }

    /// <summary>
    /// Ensures that the firing is done automatically 
    /// by pulling the fire stick once rather than continuous
    /// tapping and pressing
    /// </summary>
    private IEnumerator HeavyFireCoroutine()
    {
        PlayerController.instance.Animator.SetBool("attacking", true);
        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// Main function for firing
    /// </summary>
    /// <param name="damage"></param>
    public void ShootProjectile(float damage = 0)
    {
        if (state != WeaponState.IDLING || clip.rounds == 0)
            return;

        state = WeaponState.SHOOTING;

        //CameraShaker.instance.Shake(ShakeStrength, ShakeDuration);
        //AttackFeedback?.PlayFeedbacks();

        if (weaponAnimator) weaponAnimator.SetTrigger("fire");

        var projectile = Poolable.TryGetPoolable<Poolable>(Projectile);
        var bullet = projectile.GetComponent<Bullet>();
        if (bullet) bullet.Launch(FirePoint, damage == 0 ? weaponData.damage : damage);

        AlertTarget();

        clip.rounds--;
    }

    /// <summary>
    /// Used for throwing an equipped grenade
    /// </summary>
    public void ThrowGrenade()
    {
        var projectile = Poolable.TryGetPoolable<Poolable>(Projectile);
        projectile.transform.position = FirePoint.position;
        projectile.transform.rotation = FirePoint.rotation;
        var grenade = projectile.GetComponent<Grenade>();

        if (grenade)
        {
            grenade.Launch(this);
        }
    }

    /// <summary>
    /// Reloading the weapon is triggered automatically when 
    /// bullets run out
    /// </summary>
    public void Reload()
    {
        if (state != WeaponState.IDLING) return;
        if (clip.ammo == 0) return;
        state = WeaponState.RELOADING;
        int roundsToLoad = (int)Mathf.Min(clip.ammo, clip.size - clip.rounds);
        clip.rounds += roundsToLoad;
        clip.ModifyAmmo(-roundsToLoad);
        PlayerController.instance.Animator.SetTrigger("reload");
    }

    /// <summary>
    /// The weapon can only be in one of three states 
    /// Idling, Shooting and Reloading
    /// This function switches between those states depending on current user input
    /// </summary>
    private void UpdateControllerState()
    {
        var info = PlayerController.instance.Animator.GetCurrentAnimatorStateInfo(0);
        WeaponState newState;

        if (info.shortNameHash == fireNameHash)
        {
            newState = WeaponState.SHOOTING;
        }
        else if (info.shortNameHash == reloadNameHash)
        {
            newState = WeaponState.RELOADING;
        }
        else
        {
            newState = WeaponState.IDLING;
        }

        if (newState != state)
        {
            var oldState = state;
            state = newState;
            if (oldState == WeaponState.SHOOTING)
            {
                if (clip.rounds == 0)
                {
                    StopAttack();
                    Reload();
                }
            }
        }
    }

    /// <summary>
    /// This is a helper function used to alert 
    /// enemies that they are being attacked and should do something
    /// </summary>
    private void AlertTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerForward.position, PlayerForward
            .TransformDirection(Vector3.forward), out hit, TargetMask))
        {
            if (hit.collider.gameObject.GetComponent<GoapLegend>())
            {
                hit.collider.gameObject.GetComponent<GoapLegend>().OnAttacked();
            }
        }
    }

    /// <summary>
    /// Used to visualize the projectile path of a grenade to 
    /// enhance aiming
    /// </summary>
    private void TryDrawPath()
    {
        if (WeaponAim.isProjection && WeaponAim.isEnabled)
        {
            DrawProjection projection = WeaponAim as DrawProjection;
            projection.DrawPath(this);
        }
    }

    /// <summary>
    /// Draws the aiming arrow for visual aid
    /// </summary>
    private void ToggleAim()
    {
        if (FireButton.HandleAtCenter)
        {
            WeaponAim.Disable();
        }
        else
        {
            WeaponAim.Enable();
        }
    }

    /// <summary>
    /// Unsubscribe to all events and actions
    /// </summary>
    private void OnDisable()
    {
        FireButton.OnAttack -= StartDelayAttack;
        FireButton.OnStopAttack -= StopAttack;
        FireButton.OnHandleAtCenter -= ToggleAim;
        FireButton.OnHandleNotAtCenter -= ToggleAim;
    }
}
