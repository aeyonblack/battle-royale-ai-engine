using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class BowAttack : MonoBehaviour
{
    public FixedButton FireButton;
    public PlayerWeapon CrossBow;
    public float FireRate = 0.15f;
    public float ReachRadius = 10;
    public int NumberOfShots = 5;
    public float MinDamage = 5;
    public float MaxDamage = 8;
    public MMFeedbacks AttackFeedbacks;

    private CharacterData character;
    private List<Vector3> targets;
    private Weapon defaultWeapon;

    private void Start()
    {
        character = GetComponent<CharacterData>();
        FireButton.ButtonPressed += AcquireTargets;
    }

    private void AcquireTargets()
    {
        targets = new List<Vector3>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, ReachRadius);
        foreach (var c in colliders)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Character") && 
                c.tag != "Player")
            {
                print(c.gameObject.name);
                targets.Add(c.transform.position);
            }
        }
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        defaultWeapon = character.CurrentWeapon; // get the current default weapon immediately before attack 
        if (targets.Count > 0)
        {
            if (!character.Equip(CrossBow.weaponData)) yield break;
            
            if (targets.Count == 1)
            {
                var target = targets[0];
                yield return KillShot(target);
            }
            else
            {
                targets.MMShuffle();
            }
        }
        EquipDefaultWeapon();
    }

    private IEnumerator KillShot(Vector3 target)
    {
        LookAt(target);
        PlayerController.instance.Animator.SetBool("attacking", true);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < NumberOfShots; i++)
        {
            LookAt(target);
            CrossBow.ShootProjectile(Random.Range(MinDamage,MaxDamage));
            yield return new WaitForSeconds(FireRate);
        }
        PlayerController.instance.Animator.SetBool("attacking", false);
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

    private void EquipDefaultWeapon()
    {
        var weapon = defaultWeapon == null ? null : defaultWeapon;
        character.Equip(weapon);
    }
}
