using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArmySpawner : MonoBehaviour
{
    /// <summary>
    /// A model of the skeletons to spawn
    /// </summary>
    public GameObject SkeletonPrefab;

    public GameObject SpawnEffect;

    /// <summary>
    /// Number of skeletons to spawn
    /// </summary>
    public int ArmySize = 3;

    /// <summary>
    /// Area to spawn the army
    /// </summary>
    public float SpawnArea = 5f;

    public FixedButton SpawnButton;

    private void Start()
    {
        SpawnButton.ButtonPressed += SpawnArmy;
    }

    private void SpawnArmy()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        for (int i = 0; i < ArmySize; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * SpawnArea;
            spawnPosition.y = transform.position.y;
            var effect = Poolable.TryGetPoolable<Poolable>(SpawnEffect);
            effect.transform.position = spawnPosition + new Vector3(0,-spawnPosition.y, 0);
            yield return new WaitForSeconds(0.2f);
            Instantiate(SkeletonPrefab, spawnPosition, transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
