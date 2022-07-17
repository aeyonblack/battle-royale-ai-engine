using UnityEngine;
using UnityEngine.AI;

public class Golem : MonoBehaviour
{
    private GameObject master;
    private NavMeshAgent agent;

    private void Start()
    {
        master = GameObject.FindGameObjectWithTag("CompanionTarget");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (master)
        {
            agent.SetDestination(master.transform.position);
        }
    }

}
