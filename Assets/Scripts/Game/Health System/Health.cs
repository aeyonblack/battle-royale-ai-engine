using System;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using BattleRoyale.Level;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;

    public float startingHealth = 100;

    public float currentHealth;

    //public MMFeedbacks DamageFeedback;

    //public MMFeedbacks DeathFeedbacks;

    //public MMProgressBar HealthBar;

    public TextMeshProUGUI HealthText;

    public GameObject DeathSmoke;

    public GameObject DeathSkull;

    private CharacterData character;

    private LootDrop lootDrop;

    public Action<Loot> Dead;

    public Action LowHealth;

    [HideInInspector]
    public Loot medKit;

    private bool dead = false;

    private void Start()
    {
        character = GetComponent<CharacterData>();
        lootDrop = GetComponent<LootDrop>();
        currentHealth = startingHealth;
        //HealthBar.SetBar(currentHealth, 0, maxHealth);
        HealthText.text = ((int)currentHealth).ToString() + "%";
    }

    public void TakeDamage(float damage)
    {
        ModifyHealth(-damage);

        //DamageFeedback?.PlayFeedbacks(transform.position, (int)damage);

        if (currentHealth == 0)
        {
            if (!dead)
            {
                dead = true;
                CreateDeathFX();
                DropLoot();
                Dead?.Invoke(medKit);
                LevelManager.instance.DecrementNumberOfPlayers();
                Destroy(gameObject);
            }
        }
        else if (currentHealth < 100)
        {
            LowHealth?.Invoke();
        }
    }

    public void ModifyHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthProgress(currentHealth);
        if (currentHealth == 100)
        {
            var legend = GetComponent<GoapLegend>();
            if (legend) legend.beliefs.RemoveState("hasLowHealth");
        }
    }

    /// <summary>
    /// Update the bar's value to be the current health
    /// </summary>
    /// <param name="value"></param>
    private void UpdateHealthProgress(float value)
    {
        //HealthBar.UpdateBar(value, 0, maxHealth);
        //HealthText.text = ((int)currentHealth).ToString() + "%";
    }

    /// <summary>
    /// Drop randomly generated loot items
    /// </summary>
    private void DropLoot()
    {
        lootDrop?.CreateDropEvent(character?.CurrentWeapon, 60);
        lootDrop?.DropLoot();
    }

    private void CreateDeathFX()
    {
        var deathSkull = Poolable.TryGetPoolable<Poolable>(DeathSkull);
        deathSkull.transform.position = new Vector3(transform.position.x, 1.71f, transform.position.z);

        var deathSmoke = Poolable.TryGetPoolable<Poolable>(DeathSmoke);
        deathSmoke.transform.position = transform.position;
    }
}
