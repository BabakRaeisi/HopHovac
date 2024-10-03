using UnityEngine;

public class Movement : MonoBehaviour
{
    private GridSystem gridSystem; // Reference to the GridSystem
    public PlayerData playerData; // Reference to PlayerData

    private Vector3 targetPos; // Target position to move towards
    private bool isMoving = false; // Track whether the character is currently moving
    float gridSize;

    private void Awake()
    {
        gridSystem = FindObjectOfType<GridSystem>(); // Get the GridSystem instance
        gridSize = gridSystem.UnityGridSize;
    }
    
    public void MoveTo(Vector3 targetPos, Transform transform)
    {
        if (gridSystem.isValidPosition(targetPos))
        {
            // Proceed with movement if the position is valid
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * playerData.Speed);
        }
        else
        {
            // If the position is invalid, reset the movement state
            Debug.Log("Invalid target position: " + targetPos);
            isMoving = false;  // Stop the movement to allow new input
            return;  // Exit the function early to avoid further calculations
        }
    }
    public bool IsValidPosition(Vector3 position)
    {
        return gridSystem.isValidPosition(position); // Calls the GridSystem's isValidPosition method
    }
    public void InitializePosition(Transform transform)
    {
        Vector3 initialPosition = transform.position;
        Vector2Int gridPos = GetGridPosition(initialPosition);

        // Set the transform's position based on the grid-aligned coordinates
        targetPos = new Vector3(gridPos.x * gridSystem.UnityGridSize, 0, gridPos.y * gridSystem.UnityGridSize);
        transform.position = targetPos;  // Align the object with the grid

        // Mark the initial node as occupied
        Node currentNode = gridSystem.GetNodeAtPosition(gridPos);
        if (currentNode != null)
        {
            currentNode.SetOccupied(true);
            UpdateNodeColor(gridPos); // Update the tile's color to reflect player ownership
        }

        Debug.Log($"Initialized at grid position {gridPos} and world position {targetPos}");
    }

    public bool IsAtTarget(Vector3 targetPos, Transform transform)
    {
        // Check if the current position is close enough to the target position
        return Vector3.Distance(transform.position, targetPos) < 0.1f;
    }

    public void RotateTowards(Vector3 targetDirection, Transform transform)
    {
        // Smoothly rotate towards the specified direction using rotationSpeed from PlayerData
        if (targetDirection != Vector3.zero && playerData != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * playerData.RotationSpeed);
        }
    }
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / gridSystem.UnityGridSize),
            Mathf.RoundToInt(worldPosition.z / gridSystem.UnityGridSize)
        );
    }
    public void SmoothMove(Transform transform)
    {
        MoveTo(targetPos, transform);

        if (IsAtTarget(targetPos, transform))
        {
            // Get the grid position based on the transform position
            Vector2Int currentPos = GetGridPosition(transform.position);

            // Mark the new node as occupied by this player
            gridSystem.GetNodeAtPosition(currentPos)?.SetOccupied(true);

            // Handle tile color flipping, ownership, etc.
            UpdateNodeColor(currentPos);

            isMoving = false; // Stop moving
        }

        Vector3 direction = targetPos - transform.position;
        RotateTowards(direction, transform);
    }

    public void SetTarget(Vector3 newTargetPos)
    {
        targetPos = newTargetPos;
        isMoving = true;
        Debug.Log("Set new target position: " + newTargetPos);
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public Vector3 GetGridAdjustedPosition(Vector2Int gridPosition)
    {
        // Assume UnityGridSize is managed in GridSystem
        return new Vector3(gridPosition.x * gridSize, 0, gridPosition.y * gridSize); // Adjust position based on the grid size
    }
    public void UpdateNodeColor(Vector2Int currentPos)
    {
        Node currentNode = gridSystem.GetNodeAtPosition(currentPos);
        if (currentNode != null)
        {
            currentNode.SetOwner(playerData); // Update node color and ownership
            Debug.Log("Tile at " + currentPos + " now belongs to: " + playerData.PlayerColor);
        }
    }
}
