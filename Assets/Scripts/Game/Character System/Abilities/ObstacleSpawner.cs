using UnityEngine;


/// <summary>
/// Spawns an obstacle to slow down and block enemy shots
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    public FixedButton SpawnButton;
    public GameObject Obstacle;

    private void Start()
    {
        SpawnButton.ButtonPressed += () =>
        {
            var obstacle = Poolable.TryGetPoolable<Poolable>(Obstacle);
            Vector3 pos = transform.position + transform.forward * 2f;
            obstacle.transform.position = pos;
            obstacle.transform.rotation = transform.rotation;
        };
    }
}
