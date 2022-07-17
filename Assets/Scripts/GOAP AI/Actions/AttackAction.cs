using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : GoapAction
{
    private bool attacking = false;
    private float range;
    private float defaultRange;

    public override void DoReset()
    {
        if (attacking) AbortAttack();
       
        StopAllCoroutines();
        running = false;
    }

    public override bool PrePerform()
    {
        range = Random.Range(10, 15);
        defaultRange = range;
        agent.navAgent.stoppingDistance = range - 2;

        FindEnemy();
        StartCoroutine(PerformAction());
        StartCoroutine(GetClosestEnemy());

        return true;
    }

    private IEnumerator PerformAction()
    {
        while (true)
        {
            if (target == null)
            {
                if (attacking) AbortAttack();
                FindEnemy();
            }
            else
            {
                if (agent.Frozen)
                {
                    if (attacking) AbortAttack();
                }
                else FollowEnemy();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator GetClosestEnemy()
    {
        while (true)
        {
            float delay = Random.Range(1, 8);

            yield return new WaitForSeconds(delay);

            Queue<GameObject> enemies = fov.FindVisibleTargets(targetMask, true);
            if (enemies.Count > 0) target = enemies.Dequeue();
        }
    }

    private void FindEnemy()
    {
        Queue<GameObject> enemies = fov.FindVisibleTargets(targetMask, true);
        target = enemies.Count == 0 ? GetRandomTarget() : enemies.Dequeue();
    }

    private void FollowEnemy()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (target.layer != LayerMask.NameToLayer("Character"))
        {
            if (attacking) AbortAttack();
            SearchArea();
        }
        else
        {
            Attack();
        }
        agent.Move(this);
    }

    private void SearchArea()
    {
        if (distanceToTarget < range) FindEnemy();
        else
        {
            Queue<GameObject> enemies = fov.FindVisibleTargets(targetMask, true);
            if (enemies.Count > 0) target = enemies.Dequeue();
        }
    }

    private void Attack()
    {
        if (distanceToTarget <= range)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (Physics.Raycast(transform.position, dirToTarget, distanceToTarget, fov.obstacleMask))
            {
                range = 5f;
                agent.navAgent.stoppingDistance = range - 2;
                if (attacking) AbortAttack();
            }
            else
            {
                ResetAttackRange();
                LookAtTarget();
                if (!attacking)
                {
                    attacking = true;
                    agent.Weapon.Fire(target);
                }
            }
        }
        else
        {
            if (attacking) AbortAttack();
        }
    }

    private void ResetAttackRange()
    {
        if (range != defaultRange)
        {
            range = defaultRange;
            agent.navAgent.stoppingDistance = range - 2;
        }
    }
         
    private void AbortAttack()
    {
        agent.Weapon.HoldFire();
        attacking = false;
    }

    private void LookAtTarget()
    {
        CharacterData targetCharacter = target.GetComponent<CharacterData>() != null ? 
            target.GetComponent<CharacterData>() : target.GetComponentInParent<CharacterData>();

        Vector3 targetPosition = targetCharacter.velocity > 1f && targetCharacter.OffsetTarget ? 
            targetCharacter.OffsetTarget.position : target.transform.position;

        Vector3 direction = (targetPosition - transform.position).normalized;

        Quaternion lookRotation = Quaternion.identity;

        if (direction != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 
            Time.deltaTime * agent.navAgent.angularSpeed);
    }

}
