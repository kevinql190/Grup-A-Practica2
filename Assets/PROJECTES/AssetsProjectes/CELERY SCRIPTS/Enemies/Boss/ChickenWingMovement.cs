using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenWingMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 movementDirection;
    private NavMeshAgent agent;
    private Vector3 currentDirection;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        currentDirection = movementDirection;
    }

    void Update()
    {
        transform.Rotate(60f * Time.deltaTime * Vector3.up);
        if (!NavMesh.SamplePosition(transform.position + currentDirection.normalized - Vector3.up * agent.height/2, out _, 1.0f, NavMesh.AllAreas))
        {
            if(!NavMesh.SamplePosition(transform.position + Quaternion.Euler(0, 90, 0) * currentDirection.normalized - Vector3.up * agent.height / 2, out _, 1.0f, NavMesh.AllAreas))
            {
                currentDirection = Quaternion.Euler(0, -90, 0) * currentDirection;
            }
            if (!NavMesh.SamplePosition(transform.position + Quaternion.Euler(0, -90, 0) * currentDirection.normalized - Vector3.up * agent.height / 2, out _, 1.0f, NavMesh.AllAreas))
            {
                currentDirection = Quaternion.Euler(0, 90, 0) * currentDirection;
            }
        }
        else
        {
            // Mueve al agente hacia la direcci�n
            agent.SetDestination(transform.position + currentDirection - Vector3.up * agent.height / 2);
        }
    }
}
