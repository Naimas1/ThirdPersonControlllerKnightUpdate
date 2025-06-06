using UnityEngine;
using UnityEngine.AI;

public class AiLoc2 : MonoBehaviour
{
    public Transform player;
    public float sightRange = 12f;
    public float viewAngle = 120f;
    public float attackRange = 2.2f;
    public float wanderRadius = 15f;
    public float wanderTimer = 5f;
    public float attackCooldown = 1.5f;
    public LayerMask obstructionMask;

    private NavMeshAgent agent;
    private Animator _anim;

    private float timer;
    private float lastAttackTime;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        timer = wanderTimer;
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        bool playerInSight = CanSeePlayer();
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (playerInSight)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > sightRange + 2f)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            agent.SetDestination(player.position);
            _anim.SetFloat("Speed", agent.velocity.magnitude);

            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        _anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    void Attack()
    {
        agent.isStopped = true;
        _anim.SetFloat("Speed", 0f);
        _anim.SetTrigger("Attack");

        lastAttackTime = Time.time;

        Invoke(nameof(ResumeMovement), 1.0f);
    }

    void ResumeMovement()
    {
        agent.isStopped = false;
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > sightRange)
            return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle < viewAngle / 2f)
        {
            // Raycast на наявність перешкод
            if (!Physics.Raycast(transform.position + Vector3.up, directionToPlayer, distanceToPlayer, obstructionMask))
            {
                return true;
            }
        }

        return false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
