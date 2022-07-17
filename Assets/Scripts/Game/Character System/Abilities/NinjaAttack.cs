using System.Collections;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections.Generic;

public class NinjaAttack : MonoBehaviour
{
    public FixedButton AttackButton;

    public float CooldownDuration = 0.1f;

    [Header("Feedbacks")]
    public MMFeedbacks AttackStartFeedback;
    public MMFeedbacks IndividualAttackFeedback;
    public MMFeedbacks DeniedFeedback;

    /// <summary>
    /// This should be a scriptable object
    /// to make it upgradeable
    /// </summary>
    [Header("Attack Settings")]
    public MMTween.MMTweenCurve AttackCurve = MMTween.MMTweenCurve.EaseOutElastic;
    public float AttackDuration = 2.5f;
    public float AttackPositionOffset = 0.15f;
    public float IntervalDecrement = 0.04f;
    public float ReachRadius = 5f;

    private float timeSinceLastAttack = -100f;
    private List<Vector3> targets;
    private Vector3 initialPosition;
    private Vector3 initialLookAtTarget;

    private Health enemy;
    private bool attacking = false;

    private void Awake()
    {
        AttackButton.ButtonPressed += Attack;
    }

    private void Attack()
    {
        if (Time.time - timeSinceLastAttack < CooldownDuration + AttackDuration)
        {
            DeniedFeedback?.PlayFeedbacks();
        }
        else
        {
            initialPosition = transform.position;
            initialLookAtTarget = transform.position + transform.forward * 10f;
            AcquireTargets();
            StartCoroutine(AttackCoroutine());
            timeSinceLastAttack = Time.time;
        }
    }

    private void AcquireTargets()
    {
        targets = new List<Vector3>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, ReachRadius);
        foreach (var collider in colliders)
        {
            Vector3 enemyPosition = collider.transform.position;
            Vector3 direction = transform.position - enemyPosition;
            if (collider.gameObject.layer == LayerMask.NameToLayer("Character") && 
                collider.tag != "Player")
            {
                targets.Add(enemyPosition + direction * AttackPositionOffset);
            }
        }
        targets.MMShuffle();
    }

    private IEnumerator AttackCoroutine()
    {
        if (targets.Count > 0)
        {
            attacking = true;
            float intervalDuration = AttackDuration / targets.Count;
            AttackStartFeedback?.PlayFeedbacks();
            int enemyCounter = 0;
            foreach (Vector3 destination in targets)
            {
                IndividualAttackFeedback?.PlayFeedbacks();
                MMTween.MoveTransform(this, transform, transform.position, destination,
                    null, 0, intervalDuration, AttackCurve);
                transform.LookAt(5f * destination - 5f * initialPosition);
                yield return MMCoroutine.WaitFor(intervalDuration - enemyCounter * IntervalDecrement);
                enemyCounter++;
            }
            MMTween.MoveTransform(this, transform, transform.position, initialPosition,
                null, 0, intervalDuration, AttackCurve);
            transform.LookAt(6f * initialLookAtTarget - 5f * initialPosition);
        }
        attacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character") && 
            other.tag != "Player" && attacking)
        {
            enemy = other.GetComponent<Health>();
            enemy?.TakeDamage(Random.Range(15, 30));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (attacking) print(collision.gameObject.name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, ReachRadius);
    }

}
