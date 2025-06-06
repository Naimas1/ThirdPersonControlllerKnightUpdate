using UnityEngine;
using UnityEngine.AI;

public class DebugNavMeshAgent : MonoBehaviour
{
    [Header("Debug Options")]
    public bool showVelocity = false;
    public bool showDesiredVelocity = false;
    public bool showPath = false;

    private NavMeshAgent agent;

    void Start()
    {
        // Перевірка наявності NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent не знайдено на цьому об'єкті!");
        }
    }

    void OnDrawGizmos()
    {
        // Перевірка наявності агента
        if (agent == null) return;

        // Відображення вектора поточної швидкості
        if (showVelocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }

        // Відображення бажаної швидкості
        if (showDesiredVelocity)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
        }

        // Відображення шляху
        if (showPath)
        {
            Gizmos.color = Color.black;
            var agentPath = agent.path;

            // Перевірка наявності шляху
            if (agentPath != null && agentPath.corners.Length > 0)
            {
                Vector3 prevCorner = transform.position;

                foreach (var corner in agentPath.corners)
                {
                    Gizmos.DrawLine(prevCorner, corner);
                    Gizmos.DrawSphere(corner, 0.1f);
                    prevCorner = corner;
                }
            }
            else
            {
                Debug.LogWarning("NavMeshAgent не має шляху для відображення.");
            }
        }
    }
}
