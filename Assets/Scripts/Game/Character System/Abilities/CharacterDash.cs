using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class CharacterDash : MonoBehaviour
{
    public FixedButton ninjaAttackButton;
    public float attackRange;
    public float reachRadius;
    public float intervalDecrement;
    public float dashDuration;
    public LayerMask targetMask;
    public HandCombat attack;
    public float attackSpeedMultiplier;
    public string attackName;

    [Header("Feedbacks")]
    public MMFeedbacks AttackFeeback;
    public MMFeedbacks DeniedFeedback;
    public MMFeedbacks IndividualAttackFeedback;

    private FieldOfView fov;
    private NavMeshAgent agent;
    //private float remainingDistance;
    //private bool attacking;
    private GameObject target;

    private Vector3 initialPosition;
    private List<Vector3> targets;

    private Animator animator;

    private void Start()
    {
        //fov = GetComponent<FieldOfView>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //animator.SetFloat("attack-speed", attackSpeedMultiplier);
        ninjaAttackButton.ButtonPressed += ExecuteAttack;
    }

    private void Update()
    {
        /*if (target)
        {
            if (agent.enabled)
            {
                agent.SetDestination(target.transform.position);
            }
            remainingDistance = Vector3.Distance(transform.position, target.transform.position);
            if (remainingDistance <= attackRange)
            {
                // Execute the attack;
                if (!attacking)
                {
                    attacking = true;
                    Debug.Log("Attacking Done");
                    PlayerController.instance.controller.enabled = true;
                    agent.enabled = false;
                    animator.SetBool("ninja-run", false);
                    animator.SetLayerWeight(1, 1);
                    animator.SetTrigger(attackName);
                    
                }
            }
            else
            {
                if (agent.enabled) Animate();
            }
        }*/
        if (agent.enabled) Animate();
    } 

    /// <summary>
    /// Called by animator
    /// </summary>
    public void NinjaAttack()
    {
        attack.Execute(transform, target.transform);
    }

    private void ExecuteAttack()
    {
        //attacking = false;
        initialPosition = transform.position;
        //target = GetNearestTarget();
        //Debug.Log(target.name);
        PlayerController.instance.controller.enabled = false;
        agent.enabled = true;
        AcquireTargets();
        StartCoroutine(AttackCoroutine());
    }

    private void AcquireTargets()
    {
        Debug.Log("Acquiring target");
        targets = new List<Vector3>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, reachRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                targets.Add(collider.transform.position);
            }
        }
        targets.MMShuffle();
    }

    private IEnumerator AttackCoroutine()
    {
        Debug.Log("attacking");
        float intervalDuration = dashDuration / targets.Count;
        AttackFeeback?.PlayFeedbacks();

        //int enemyCount = 0;
        foreach (Vector3 destination in targets)
        {
            IndividualAttackFeedback?.PlayFeedbacks();
            agent.SetDestination(destination + Random.insideUnitSphere*2f);
            yield return new WaitUntil(() => agent.remainingDistance < 0.1f);
            Debug.Log("I'm here");
            //enemyCount++;
        }
        agent.SetDestination(initialPosition);
        yield return new WaitUntil(() => Mathf.Abs(agent.velocity.magnitude) < 0.1f);
        PlayerController.instance.controller.enabled = true;
        agent.enabled = false;

    }

    private GameObject GetNearestTarget()
    {
        var targets = fov.FindVisibleTargets(targetMask, true);
        if (targets.Count == 0)
        {
            // Deny attack; Play denied feedbacks
            Debug.Log("No targets nearby");
            return null;
        }
        return targets.Dequeue();
    }

    private void Animate()
    {
        bool moving = agent.velocity.magnitude != 0;
        //animator.SetBool("ninja-run", moving);
        float w = moving ? 1 : 0;
        animator.SetLayerWeight(1, w); 
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, reachRadius);
    }
}
