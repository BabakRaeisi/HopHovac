using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePoolManager : UPatterns.PoolMonoBehaviour<Collectable>
{
    [SerializeField] private GridSystem gridSystem;

    private float spawnInterval = 4f;

    private void Start()
    {
        // Start spawning collectables at intervals
        InvokeRepeating(nameof(SpawnCollectable), 2f, spawnInterval);  // Start spawning after 2 seconds
    }
    private void SpawnCollectable()
    {
        // Get an available collectable from the pool
        Collectable collectable = GetActive;  // Get an active collectable from the pool
        if (collectable != null)
        {
            // Find a random unoccupied tile from the grid
            Node randomNode = gridSystem.GetRandomUnoccupiedNode();
            if (randomNode != null)
            {
                // Place the collectable on the selected tile
                collectable.transform.position = gridSystem.GetWorldPositionFromGrid(randomNode.Coordinates);

                // Initialize the collectable
                collectable.Initialize(randomNode, this);
            }
        }
    }
    public void ReturnCollectable(Collectable collectable)
    {
        collectable.gameObject.SetActive(false);  // Deactivate the collectable
    }
}
