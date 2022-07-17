using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles all basic controls [movement and attacking]
/// for the Orc character
/// </summary>
public class OrcController : PlayerController
{
    public FixedButton AttackButton;
    public float MoveSpeedMultiplier;
    public float AttackDistance = 5f;
    public LayerMask targetMask;

    private FieldOfView fov;

    private GameObject target;

    protected override void Awake()
    {
        base.Awake();
        AttackButton.ButtonPressed += Attack;
    }

    protected override void Start()
    {
        base.Start();
        fov = GetComponent<FieldOfView>();
        animator.SetFloat("move-speed-multiplier", MoveSpeedMultiplier);
    }

    protected override void Animate()
    {
        bool moving = horizontalMovement != 0 || verticalMovement != 0;
        animator.SetBool("moving", moving);
    }

    public void Attack()
    {
        Queue<GameObject> targets = fov.FindVisibleTargets(targetMask, true);
        if (targets.Count > 0)
        {
            foreach (var t in targets)
            {
                if (t.tag != gameObject.tag)
                {
                    target = t.gameObject;
                    break;
                }
            }
            if (target) LookAt(target.transform.position);
            animator.SetTrigger("attack");
        } 
        else
        {
            animator.SetTrigger("attack");
        }
    }

    public void ExecuteAttack()
    {
        if (target )
        {
            Debug.Log(target);
            target.GetComponent<Health>()?.TakeDamage(Random.Range(10, 30));
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
            Time.deltaTime * RotationSpeed);
    }
}
