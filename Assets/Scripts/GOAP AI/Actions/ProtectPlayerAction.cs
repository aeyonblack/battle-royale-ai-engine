using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

/// <summary>
/// TODO - IMPROVE THE SPAWNING EFFECTS OF THE SKELETON ARMY
/// Instantiate Particles with a delay
/// </summary>
public class ProtectPlayerAction : GoapAction
{

    private bool attacking = false;

    private Attacker attacker;

    private Transform playerPosition;

    private float range;

    private float stoppingDistanceFromPlayer;

    public override void DoReset()
    {
        StopAllCoroutines();
        attacker = null;
        attacking = false;
        running = false;
    }

    public override bool PrePerform()
    {
        attacker = GetComponent<Attacker>();
        playerPosition = PlayerController.instance.transform;
        range = attacker.attack.AttackDistance;
        stoppingDistanceFromPlayer = Random.Range(5, 8);
        agent.navAgent.stoppingDistance = stoppingDistanceFromPlayer;
        StartCoroutine(PerformAction());
        return true;
    }

    private IEnumerator PerformAction()
    {
        while (true)
        {
            if (target == null)
            {
                if (attacking) AbortAttack();
                FollowPlayer();
            }
            else
            {
                agent.navAgent.stoppingDistance = range - 1;
                AttackTarget();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void FollowPlayer()
    {
        if (agent.navAgent.stoppingDistance != stoppingDistanceFromPlayer)
            agent.navAgent.stoppingDistance = stoppingDistanceFromPlayer;
        SearchArea();
        agent.MoveTo(playerPosition);
    }

    private void SearchArea()
    {
        Queue<GameObject> targets = GetTargets();
        if (targets.Count > 0) target = targets.Dequeue();
    }

    private void AbortAttack()
    {
        attacker.HoldAttack();
        attacking = false;
    }

    private void AttackTarget()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget <= range)
        {
            LookAtTarget();
            if (!attacking)
            {
                attacking = true;
                attacker.AttackTarget(target);
            }
        }
        else
        {
            if (attacking) AbortAttack();
        }
        agent.MoveTo(target.transform);
    }

    private void LookAtTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.identity;
        if (direction != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
            Time.deltaTime * agent.navAgent.angularSpeed);
    }

    private Queue<GameObject> GetTargets()
    {
        Queue<GameObject> targets = new Queue<GameObject>();
        foreach (var target in fov.FindVisibleTargets(targetMask, true))
        {
            if (target.tag != "Skeleton")
            {
                targets.Enqueue(target);
            }
        }
        return targets;
    }
}
