using UnityEngine;
using System;
using System.Collections.Generic;

public class GhostSpawner : MonoBehaviour
{
    [Serializable]
    public class GhostSpawnPoint
    {
        public Vector3 position;
    }

    public GameObject ghostPrefab;
    public Vector3 ghostScale = Vector3.one;
    public List<GhostSpawnPoint> spawnPoints = new List<GhostSpawnPoint>();
    public float moveSpeed = 2f;
    public Transform player;
    public LayerMask playerLayer;
    public int ghostLayer = 8; // Default to layer 8, change this in the inspector if needed

    void Start()
    {
        Debug.Log("GhostSpawner Start method called");
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("Player not found. Make sure your player has the 'Player' tag.");
            }
        }
        SpawnGhosts();
    }

    void SpawnGhosts()
    {
        Debug.Log($"Attempting to spawn {spawnPoints.Count} ghosts");
        foreach (var spawnPoint in spawnPoints)
        {
            GameObject spawnedGhost = Instantiate(ghostPrefab, spawnPoint.position, Quaternion.identity);
            if (spawnedGhost != null)
            {
                spawnedGhost.transform.localScale = ghostScale;
                GhostMover mover = spawnedGhost.AddComponent<GhostMover>();
                mover.Initialize(player, moveSpeed, playerLayer, ghostLayer);
                Debug.Log($"Ghost spawned at position: {spawnPoint.position}, with scale: {ghostScale}");
            }
            else
            {
                Debug.LogError("Failed to instantiate ghost prefab");
            }
        }
    }
}

public class GhostMover : MonoBehaviour
{
    private Transform player;
    private float speed;
    private Rigidbody rb;
    private LayerMask playerLayer;

    public void Initialize(Transform playerTransform, float moveSpeed, LayerMask plLayer, int ghostLayerNumber)
    {
        player = playerTransform;
        speed = moveSpeed;
        playerLayer = plLayer;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configure Rigidbody
        rb.useGravity = false;
        rb.isKinematic = true; // This prevents physics from affecting the ghost
        rb.interpolation = RigidbodyInterpolation.Interpolate; // This can help smooth out movement

        // Add a collider if it doesn't exist
        if (GetComponent<Collider>() == null)
        {
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true; // This allows the ghost to pass through objects
        }

        // Set the ghost's layer
        gameObject.layer = ghostLayerNumber;

        // Ignore collisions with everything except the player
        for (int i = 0; i < 32; i++)
        {
            if (i != ghostLayerNumber && ((1 << i) & playerLayer.value) == 0)
            {
                Physics.IgnoreLayerCollision(ghostLayerNumber, i);
            }
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Remove vertical component to keep ghosts at the same height
            Vector3 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
            newPosition.y = transform.position.y; // Maintain the original y position
            rb.MovePosition(newPosition);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
        {
            Debug.Log("Ghost touched the player!");
            // Add your player interaction logic here
        }
    }
}