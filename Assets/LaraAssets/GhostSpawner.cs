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
    public int ghostLayer = 8;

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
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = player.position.y; // Set the ghost's Y position to match the player's

            GameObject spawnedGhost = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
            if (spawnedGhost != null)
            {
                spawnedGhost.transform.localScale = ghostScale;
                GhostMover mover = spawnedGhost.AddComponent<GhostMover>();
                mover.Initialize(player, moveSpeed, playerLayer, ghostLayer);
                Debug.Log($"Ghost spawned at position: {spawnPosition}, with scale: {ghostScale}");
            }
            else
            {
                Debug.LogError("Failed to instantiate ghost prefab");
            }
        }
    }
}