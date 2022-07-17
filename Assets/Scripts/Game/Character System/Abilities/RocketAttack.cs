using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class RocketAttack : MonoBehaviour
{
    public FixedButton AttackButton;

    [Header("Attack Settings")]
    public float ReachRadius = 10f;
    public float AttackDuration = 5f;
    public float MinDamage = 10f;
    public float MaxDamage = 20f;
    public float IntervalDecrement = 0.04f;

    [Header("Launch Settings")]
    public Transform DefaultLaunchPoint;
    public Transform[] LaunchPoints;
    public float LaunchForce;
    public float LaunchRate;
    public GameObject Projectile;
    public float NumberOfShots = 3;
    public bool LowAngle = false;

    private List<Vector3> targets;
    private Golem golem;

    private void Start()
    {
        AttackButton.ButtonPressed += Attack;
        if (gameObject.tag == "Golem") golem = GetComponent<Golem>();
    }

    private void Attack()
    {
        AcquireTargets();
    }

    private void AcquireTargets()
    {
        targets = new List<Vector3>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, ReachRadius);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                if (collider.gameObject.name != gameObject.name && 
                    collider.gameObject.tag != "Player")
                {
                    targets.Add(collider.transform.position);
                }
            }
        }
        targets.MMShuffle();

        if (targets.Count > 0)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        golem.enabled = false;
        // we have more than one target
        if (targets.Count > 1)
        {
            if (LowAngle)
            {
                foreach (var target in targets)
                {
                    yield return KillShot(target);
                }
            }
            else
            {
                foreach (var target in targets)
                {
                    LookAt(target);
                    yield return new WaitForSeconds(0.1f);
                    Launch(target, DefaultLaunchPoint);
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
        else
        {
            // we have one target
            var target = targets[0];
            yield return KillShot(target);
        }
        golem.enabled = true;
    }

    private IEnumerator KillShot(Vector3 target)
    {
        if (target != null)
        {
            LookAt(target);
            for (int i = 0; i < NumberOfShots; i++)
            {
                foreach (var launchPoint in LaunchPoints)
                {
                    Launch(target, launchPoint);
                    yield return new WaitForSeconds(0.15f);
                }
            }
        }
    }

    private void Launch(Vector3 target, Transform launchPoint)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        launchPoint.rotation = Quaternion.Slerp(launchPoint.transform.rotation, 
            lookRotation, Time.deltaTime * 120f);


        float? angle = Ballistics.RotateLaunchPoint(launchPoint, target, LaunchForce, LowAngle);
        if (angle != null)
        {
            var projectile = Poolable.TryGetPoolable<Poolable>(Projectile);
            projectile.transform.position = launchPoint.position;
            projectile.transform.rotation = launchPoint.rotation;
            var rocket = projectile.GetComponent<Rocket>();
            if (rocket != null)
            {
                rocket.Launch(LaunchForce, launchPoint, this);
            }
        }
    }

    private void LookAt(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.identity;
        if (direction != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 
            Time.deltaTime * PlayerController.instance.RotationSpeed);
    }
}
