using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the activation and deactivation of 
/// a defensive shield
/// </summary>
public class ShieldActivator : MonoBehaviour
{
    /// <summary>
    /// Button used to activate the shield
    /// </summary>
    public FixedButton ActivateShiedButton;

    /// <summary>
    /// Shield prefab to be instantiated
    /// </summary>
    public GameObject ShieldPrefab;

    /// <summary>
    /// The rate at which the shield activates
    /// or the time it takes the shield to fully activate
    /// </summary>
    public float ActivationSpeed = 1.5f;

    /// <summary>
    /// How long the shield stays active | upgradeable property
    /// </summary>
    public float ShieldLife = 5f;

    /// <summary>
    /// The outer fill of the shield that is 
    /// manipulated during activation and deactivation
    /// </summary>
    private float fill = -2;

    /// <summary>
    /// The shield material
    /// </summary>
    private Renderer shieldRenderer;

    private void Start()
    {   
        ActivateShiedButton.ButtonPressed += ActivateShield;
        shieldRenderer = ShieldPrefab.GetComponent<Renderer>();
        ModifyFill(fill);
        if (ShieldPrefab.activeSelf) ShieldPrefab.SetActive(false);
    }

    /// <summary>
    /// Activates the shield
    /// </summary>
    public void ActivateShield()
    {
        ShieldPrefab.SetActive(true);
        StartCoroutine(ActivateShieldCoroutine());
    }

    /// <summary>
    /// Manipulates the fill to activate the shield over time
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActivateShieldCoroutine()
    {
        while (fill < -1)
        {
            fill += Time.deltaTime / ActivationSpeed;
            ModifyFill(fill);
            yield return null;
        }
        yield return DeactivateShield();
    }

    /// <summary>
    /// Reduces the fill to deactivate the shield
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeactivateShield()
    {
        yield return new WaitForSeconds(ShieldLife);
        while (fill >= -2)
        {
            fill -= Time.deltaTime / ActivationSpeed;
            ModifyFill(fill);
            yield return null;
        }
        ShieldPrefab.SetActive(false);
    }

    private void ModifyFill(float fill)
    {
        shieldRenderer.sharedMaterial.SetFloat("fill", fill);
    }
}
