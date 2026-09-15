using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(TargetHealth))]
public sealed class EnemyAI : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform player;
    [SerializeField, Min(0f)] private float detectionRange = 30f;

    [Header("Attack Settings")]
    [SerializeField, Min(0f)] private float attackDistance = 1.7f;
    [SerializeField, Min(0f)] private float attackDamage = 10f;
    [SerializeField, Min(0.05f)] private float attackInterval = 1f;

    [Header("Rotation Settings")]
    [SerializeField, Min(0f)] private float rotationSpeed = 8f;

    private NavMeshAgent agent;
    private PlayerHealth playerHealth;
    private float nextAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = Mathf.Max(0.1f, attackDistance - 0.2f);
        agent.updateRotation = false;
    }

    private void Start()
    {
        FindPlayer();
    }

    public void ConfigureForWave(int waveNumber)
    {
        float waveScale = Mathf.Max(0, waveNumber - 1);
        attackDamage *= 1f + waveScale * 0.08f;
        agent.speed *= 1f + waveScale * 0.04f;
        GetComponent<TargetHealth>().Configure(1f + waveScale * 0.12f);
    }

    private void Update()
    {
        if (GameRuntime.IsPaused) return;

        if (player == null || playerHealth == null) FindPlayer();
        if (player == null || playerHealth == null || playerHealth.IsDead || !agent.isOnNavMesh)
        {
            StopMoving();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > detectionRange) StopMoving();
        else if (distance > attackDistance) ChasePlayer();
        else
        {
            StopMoving();
            FacePlayer();
            AttackPlayer();
        }
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return;

        player = playerObject.transform;
        playerHealth = playerObject.GetComponent<PlayerHealth>();
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        FaceDirection(agent.desiredVelocity);
    }

    private void StopMoving()
    {
        if (agent == null || !agent.isOnNavMesh) return;
        agent.isStopped = true;
        if (agent.hasPath) agent.ResetPath();
    }

    private void FacePlayer()
    {
        FaceDirection(player.position - transform.position);
    }

    private void FaceDirection(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude <= 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackInterval;
        playerHealth.TakeDamage(attackDamage);
    }

    private void OnValidate()
    {
        detectionRange = Mathf.Max(0f, detectionRange);
        attackDistance = Mathf.Clamp(attackDistance, 0f, detectionRange);
        attackDamage = Mathf.Max(0f, attackDamage);
        attackInterval = Mathf.Max(0.05f, attackInterval);
        rotationSpeed = Mathf.Max(0f, rotationSpeed);
    }
}
