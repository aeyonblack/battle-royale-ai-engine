using UnityEngine;
using UnityEngine.AI;
using System;
using MoreMountains.Feedbacks;

/// <summary>
/// All character stats, abilities and attributes are defined here
/// </summary>
public class CharacterData : MonoBehaviour
{

    /// <summary>
    /// Optional character name attribute
    /// </summary>
    public string characterName;

    /// <summary>
    /// Parent transform holding all possible weapons for this character
    /// </summary>
    public Transform WeaponPlace;

    /// <summary>
    /// Transform this into a custom feedback
    /// </summary>
    public MMFeedbacks HealingFeedbacks;

    /// <summary>
    /// The time it takes a character to loot an item
    /// </summary>
    public float lootSpeed;

    /// <summary>
    /// used by ai agents for special targeting
    /// </summary>
    public Transform OffsetTarget;

    /// <summary>
    /// Main inventory defining all the items the player is currently holding
    /// and all the max items the player can carry at any one time
    /// </summary>
    public Inventory Backpack = new Backpack();

    /// <summary>
    /// Manages the player's health and death
    /// </summary>
    [HideInInspector]
    public Health health;

    /// <summary>
    /// Current velocity of the player
    /// </summary>
    [HideInInspector]
    public float velocity;

    /// <summary>
    /// The current equipped weapon
    /// </summary>
    [HideInInspector]
    public Weapon CurrentWeapon
    {
        get;
        private set;
    }

    /// <summary>
    /// The weapon gameobject
    /// </summary>
    [HideInInspector]
    public GameObject WeaponPrefab { get; private set; }

    /// <summary>
    /// Unique identifier for the current equipped weapon
    /// This is just the weapon's position under the [WeaponPlace]
    /// </summary>
    [HideInInspector]
    public int CurrentWeaponId;

    /// <summary>
    /// Invoked when the current weapon is updated or a new weapon is
    /// equipped
    /// </summary>
    public Action weaponUpdated;

    /// <summary>
    /// Default animation controller name
    /// </summary>
    private string baseController = "_BaseController";

    /// <summary>
    /// Initialize all dependent components
    /// </summary>
    public void Init()
    {
        Backpack.Init(this);
        health = GetComponent<Health>();
    }

    /// <summary>
    /// Equip new weapon by activating its gameobject
    /// </summary>
    /// <param name="weapon">The weapon to equip</param>
    /// <returns>true if the weapon was equipped successfully</returns>
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

    /// <summary>
    /// UnEquip the current weapon by deactivating it's gameobject
    /// </summary>
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
                .Load("Art/Animators/Player/" + (CurrentWeapon ? CurrentWeapon?.controllerName : baseController))
                as RuntimeAnimatorController;
        }
        else
        {
            animator.runtimeAnimatorController = Resources
                .Load("Art/Animators/Legends/" + CurrentWeapon?.controllerName)
                as RuntimeAnimatorController;
        }
    }

    /// <summary>
    /// Find the weapon prefab with [CurrentWeaponId] where id
    /// is its position relative to others under the [WeaponPlace]
    /// </summary>
    /// <returns>
    /// A gameobject of the weapon if one is found
    /// otherwise null
    /// </returns>
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

    /// <summary>
    /// Get attached animator
    /// </summary>
    /// <returns></returns>
    public Animator GetAnimator()
    {
        return GetComponent<Animator>();
    }

    /// <summary>
    /// Get attached [NavAgent] if one is attached for bots
    /// </summary>
    /// <returns></returns>
    public NavMeshAgent GetNavAgent()
    {
        return GetComponent<NavMeshAgent>();
    }

}
