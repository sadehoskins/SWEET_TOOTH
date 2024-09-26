using UnityEngine;
using System.Collections;

public class GhostMover : MonoBehaviour
{
    private Transform player;
    private float speed;
    private int playerLayer;
    private Vector3 originalPosition;
    private bool isDead = false;

    [SerializeField] private float obstacleAvoidanceDistance = 1f;
    [SerializeField] private float obstacleAvoidanceForce = 2f;
    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private float killDistance = 1f; // Distance at which the ghost can "kill" the player

    private Renderer[] renderers;
    private Collider ghostCollider;

    public void Initialize(Transform playerTransform, float moveSpeed, LayerMask plLayer, int ghostLayerNumber)
    {
        player = playerTransform;
        speed = moveSpeed;
        playerLayer = plLayer;
        gameObject.layer = ghostLayerNumber;
        originalPosition = transform.position;

        renderers = GetComponentsInChildren<Renderer>();

        ghostCollider = GetComponent<Collider>();
        if (ghostCollider == null)
        {
            ghostCollider = gameObject.AddComponent<SphereCollider>();
        }
        ghostCollider.isTrigger = true;

        Debug.Log($"Ghost initialized. Player Layer: {playerLayer}, Ghost Layer: {ghostLayerNumber}");
    }

    void Update()
    {
        if (player != null && !isDead)
        {
            MoveTowardsPlayer();
            CheckPlayerDistance();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        Vector3 avoidanceForce = ObstacleAvoidance();
        direction += avoidanceForce;
        direction.Normalize();

        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
        newPosition.y = player.position.y;
        transform.position = newPosition;

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.deltaTime);
        }
    }

    Vector3 ObstacleAvoidance()
    {
        // ... (keep the existing ObstacleAvoidance code)
        return Vector3.zero; // Placeholder return
    }

    void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= killDistance)
        {
            Debug.Log($"Ghost {gameObject.name} is within kill distance ({distanceToPlayer}). Initiating death.");
            DieAndRespawn();
            // Placeholder: Player takes damage
            // PlayerHealth.TakeDamage(10);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by {other.gameObject.name}. Other layer: {other.gameObject.layer}, Player layer: {playerLayer}");
        if (((1 << other.gameObject.layer) & playerLayer) != 0 && !isDead)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            Debug.Log($"Player collision detected for ghost {gameObject.name}. Distance to player: {distanceToPlayer}");
            if (distanceToPlayer <= killDistance)
            {
                Debug.Log($"Ghost {gameObject.name} is within kill distance. Initiating death.");
                DieAndRespawn();
                // Placeholder: Player takes damage
                // PlayerHealth.TakeDamage(10);
            }
            else
            {
                Debug.Log($"Ghost {gameObject.name} is too far from player to die. Distance: {distanceToPlayer}, Required: {killDistance}");
            }
        }
    }

    public void OnRangedAttackHit()
    {
        if (!isDead)
        {
            DieAndRespawn();
        }
    }

    void DieAndRespawn()
    {
        StartCoroutine(DieAndRespawnCoroutine());
    }

    IEnumerator DieAndRespawnCoroutine()
    {
        isDead = true;
        Debug.Log($"Ghost {gameObject.name} died. Respawning in {respawnTime} seconds.");

        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }

        if (ghostCollider != null)
        {
            ghostCollider.enabled = false;
        }

        yield return new WaitForSeconds(respawnTime);

        transform.position = originalPosition;

        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
        }

        if (ghostCollider != null)
        {
            ghostCollider.enabled = true;
        }

        isDead = false;
        Debug.Log($"Ghost {gameObject.name} respawned at {originalPosition}");
    }
}