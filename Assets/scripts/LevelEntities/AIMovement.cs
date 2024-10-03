using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private Movement movement; // Shared Movement logic

    private Vector2Int currentPos; // Current grid position

    public PlayerData aiData; // AI's data (name, speed, etc.)

    private void Awake()
    {
        movement = GetComponent<Movement>(); // Reference to the shared Movement script
    }

    private void Start()
    {
        movement.InitializePosition(this.transform); // Initialize the AI's position using Movement
    }

    private void Update()
    {
        if (!movement.IsMoving())
        {
            FindNextTile(); // AI decides where to move
        }
        else
        {
            movement.SmoothMove(transform); // Continue moving towards the target position
        }
    }

    private void FindNextTile()
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int nextDirection = directions[Random.Range(0, directions.Length)];
        Vector2Int newPos = currentPos + nextDirection;

        Vector3 targetPosition = movement.GetGridAdjustedPosition(newPos);

        // Check if the target position is valid before updating the AI's position
        if (movement.IsValidPosition(targetPosition))
        {
            currentPos = newPos;
            movement.SetTarget(targetPosition); // Move the AI to the valid new position
        }
        else
        {
            Debug.Log("AI attempted to move outside of grid to: " + newPos);
        }
    }
}
