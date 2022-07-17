using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlames : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Damaging...");
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (!gameObject.activeInHierarchy) return;
        Poolable.TryPool(gameObject);
    }
}
