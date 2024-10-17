using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Node assignedNode;
    private CollectablePoolManager poolManager;

    private float lifetime = 10f;  // Time before the collectable despawns
    private float despawnTimer;  // Timer for despawning the collectable
    private float rotationSpeed = 100f;
    // Method to initialize the collectable
    public void Initialize(Node node, CollectablePoolManager poolManager)
    {
        this.assignedNode = node;
        this.poolManager = poolManager;
        assignedNode.AssignCollectable(this);  // Assign the collectable to the node

        despawnTimer = lifetime;  // Reset the despawn timer when spawned
    }

    private void Update()
    {

        // Countdown timer for despawning the collectable if not collected
        despawnTimer -= Time.deltaTime;
        RotateCollectable();
        if (despawnTimer <= 0)
        {
            // Timer expired, despawn the collectable
            Collect();
        }
    }

    // Method to handle when the collectable is collected or despawned
    public void Collect()
    {
        if (assignedNode != null)
        {
            assignedNode.ClearCollectable();  // Clear the collectable from the node
        }
        poolManager.ReturnCollectable(this);  // Return the collectable to the pool
    }
    private void RotateCollectable()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);  // Rotate around the Y-axis
    }
}
