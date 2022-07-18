using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCMovement : MonoBehaviour
{
    [SerializeField]
    Transform destination;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent Attached to Object: " + gameObject.name);
        }
    }

    // Update is called once per frame
}
