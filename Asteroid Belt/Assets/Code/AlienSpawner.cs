using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fixed a lot of issues in the original code with merge conflicts fuck man...
// this was after I MADE IT, and the SHOOTING WORKED...
public class AlienSpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    [SerializeField] private Renderer planeRenderer;   // Drag your Plane's MeshRenderer here

    [Header("Aliens")]
    [SerializeField] private GameObject[] alienPrefabs; // Drag 1+ prefabs here

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1f;  // seconds between spawns
    [SerializeField] private float zOffset = -10f;      // how far back in Z to place them

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            SpawnAlien();
            _timer = 0f;
        }
    }

    private void SpawnAlien()
    {
        if (planeRenderer == null || alienPrefabs == null || alienPrefabs.Length == 0)
        {
            return;
        }

        // Get the world-space bounds of the plane
        Bounds bounds = planeRenderer.bounds;

        // Random X and Y inside the plane's bounds
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        // Z is the plane's center.z plus some offset (so they sit "in front of" or "behind" it)
        float z = bounds.center.z + zOffset;

        Vector3 spawnPos = new Vector3(x, y, z);

        // Pick a random alien prefab
        int index = Random.Range(0, alienPrefabs.Length);
        GameObject prefabToSpawn = alienPrefabs[index];

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}