using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TelekineticFreeze : MonoBehaviour
{
    public FixedButton AbilityButton;
    public GameObject ShockWave;
    public float Duration;
    public float effectRadius;

    private Collider character;

    private void Start()
    {
        AbilityButton.ButtonPressed += Activate;
    }

    public void Activate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, effectRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Character") // let's make this more accurate
                && !isEqual(hit))
            {
                Freeze(hit);
            }
        }
    }

    private bool isEqual(Collider target)
    {
        bool equal = false;
        var character = target.GetComponent<CharacterData>();
        if (character == null)
        {
            if (target.transform.parent.name == gameObject.name) equal = true;
        }
        else
        {
            if (target.name == gameObject.name) equal = true;
        }
        return equal;
    }

    public void Freeze(Collider character)
    {
        this.character = character;
        var wave = Poolable.TryGetPoolable<Poolable>(ShockWave);
        Vector3 position = transform.position;
        position.y = 1;
        wave.transform.position = position;
        StartCoroutine(Freeze());
    }

    private IEnumerator Freeze()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1f)); 

        var navAgent = character.GetComponent<NavMeshAgent>();
        var anim = character.GetComponent<Animator>();
        PauseMovement(navAgent, anim, true);

        yield return new WaitForSeconds(Duration);

        PauseMovement(navAgent, anim, false);
    }

    private void PauseMovement(NavMeshAgent nav, Animator anim, bool freeze)
    {
        nav.enabled = !freeze;
        anim.enabled = !freeze;
        character.GetComponent<GoapAgent>().Frozen = freeze;
    }
}
