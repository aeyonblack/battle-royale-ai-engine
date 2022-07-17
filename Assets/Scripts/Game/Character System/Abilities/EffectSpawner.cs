using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public FixedButton SpawnButton;
    public GameObject Effect;
    public GameObject Smoke;

    private void Start()
    {
        SpawnButton.ButtonPressed += () =>
        {
            StartCoroutine(SpawnEffect());
        };
    }

    private IEnumerator SpawnEffect()
    {
        var smoke = Poolable.TryGetPoolable<Poolable>(Smoke);
        smoke.transform.position = transform.position;
        yield return new WaitForSeconds(0.15f);
        Instantiate(Effect, transform.position, Quaternion.identity);
    }
}
