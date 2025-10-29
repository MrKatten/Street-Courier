using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("Points")]
    public Transform[] patrolPoints;

    [Header("Settings")]
    public float waitTime = 2f;
    public bool loop = true;

    private NavMeshAgent agent;
    private int currentPointIndex = 0;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
        {
            MoveToPoint(0);
        }
    }

    void Update()
    {
        if (isWaiting || patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                StartCoroutine(WaitAndMoveNext());
            }
        }
    }

    IEnumerator WaitAndMoveNext()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        currentPointIndex++;

        if (currentPointIndex >= patrolPoints.Length)
        {
            if (loop)
            {
                currentPointIndex = 0;
            }
            else
            {
                isWaiting = true;
                yield break;
            }
        }

        MoveToPoint(currentPointIndex);
        isWaiting = false;
    }

    void MoveToPoint(int index)
    {
        if (patrolPoints[index] != null)
        {
            agent.SetDestination(patrolPoints[index].position);
        }
    }

    // For debugging
    void OnDrawGizmosSelected()
    {
        if (patrolPoints == null) return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] != null)
            {
                Gizmos.DrawSphere(patrolPoints[i].position, 0.3f);

                if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                }
            }
        }
    }
}