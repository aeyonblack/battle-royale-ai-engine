using UnityEngine;
using UnityEngine.AI;
using System;
using MoreMountains.Feedbacks;

/// <summary>
/// All character stats, abilities and attributes are defined here
/// </summary>
public class CharacterData : MonoBehaviour
{
    public string characterName;

    public Transform WeaponPlace;

    public MMFeedbacks HealingFeedbacks;

    /// <summary>
    /// The time it takes a character to loot an item
    /// </summary>
    public float lootSpeed;

    /// <summary>
    /// used by ai agents for special targeting
    /// </summary>
    public Transform OffsetTarget;

    public Inventory Backpack = new Backpack();

    [HideInInspector]
    public Health health;

    [HideInInspector]
    public float velocity;

    [HideInInspector]
    public Weapon CurrentWeapon
    {
        get;
        private set;
    }

    [HideInInspector]
    public GameObject WeaponPrefab { get; private set; }

    [HideInInspector]
    public int CurrentWeaponId;

    public Action weaponUpdated;

    private string baseController = "_BaseController";

    public void Init()
    {
        Backpack.Init(this);
        health = GetComponent<Health>();
    }

    public bool Equip(Weapon weapon)
    {
        bool weaponEquipped = false;
        UnEquip();
        CurrentWeapon = weapon;
        GameObject weaponPrefab = FindWeaponInParent();
        if (weaponPrefab)
        {
            WeaponPrefab = weaponPrefab;
            weaponPrefab.SetActive(true);
            weaponUpdated?.Invoke();
            weaponEquipped = true;
        }
        ChangeAnimationController();
        return weaponEquipped;
    }

    public void UnEquip()
    {
        if (CurrentWeapon)
        {
            GameObject weaponPrefab = FindWeaponInParent();
            weaponPrefab.SetActive(false);
        }
    }

    /// <summary>
    /// The idea is to have separate animation controllers for the player and AI 
    /// because the player requires a more complex controller with a blendtree for
    /// locomotion 
    /// </summary>
    private void ChangeAnimationController()
    {
        Animator animator = GetAnimator();
        if (gameObject.tag == "Player")
        {
            animator.runtimeAnimatorController = Resources
                .Load("Animators/Characters/Player/" + (CurrentWeapon ? CurrentWeapon?.controllerName : baseController))
                as RuntimeAnimatorController;
        }
        else
        {
            animator.runtimeAnimatorController = Resources
                .Load("Animators/Characters/Legends/" + CurrentWeapon?.controllerName)
                as RuntimeAnimatorController;
        }
    }

    private GameObject FindWeaponInParent()
    {
        for (int i = 0; i < WeaponPlace.childCount; i++)
        {
            Transform weapon = WeaponPlace.GetChild(i);
            if (weapon.name == CurrentWeapon?.worldObjectPrefab.name)
            {
                CurrentWeaponId = i;
                return weapon.gameObject;
            }
        }
        return null;
    }

    public Animator GetAnimator()
    {
        return GetComponent<Animator>();
    }

    public NavMeshAgent GetNavAgent()
    {
        return GetComponent<NavMeshAgent>();
    }

}
