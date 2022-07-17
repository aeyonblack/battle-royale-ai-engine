using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioactiveGasSpawner : MonoBehaviour
{
    public FixedButton AbilityButton;
    public GameObject RadioactiveGas;
    public GameObject Smoke;

    private void Start()
    {
        AbilityButton.ButtonPressed += SpawnGas;
    }

    private void SpawnGas()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        Instantiate(Smoke, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.15f);
        Instantiate(RadioactiveGas, transform.position, transform.rotation);
    }
}
