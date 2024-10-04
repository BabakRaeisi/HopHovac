using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private Movement movement; // Shared Movement logic
    private GridSystem gridSystem; // Reference to GridSystem
    private Vector2Int currentPos; // Current grid position
    public PlayerData aiData; // AI's PlayerData (stores position, points, etc.)

    private void Awake()
    {
        movement = GetComponent<Movement>(); // Reference to the shared Movement script
        gridSystem = FindObjectOfType<GridSystem>(); // Reference to GridSystem
    }

    private void Start()
    {
        movement.InitializePosition(this.transform); // Initialize the AI's position using Movement
    }

    private void Update()
    {
        if (!movement.IsMoving())
        {
            DecideNextMove(); // AI decides where to move next
        }
    }

    private void DecideNextMove()
    {
        // List possible directions for movement
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int nextMove = Vector2Int.zero;
        bool foundUnvisitedTile = false;

        // Step 1: Look for unvisited or opponent-owned tiles in all directions
        foreach (Vector2Int direction in directions)
        {
            Vector2Int targetTilePos = currentPos + direction;

            // Check if the tile is valid and accessible
            if (gridSystem.IsValidPosition(targetTilePos))
            {
                Node targetNode = gridSystem.GetNodeAtPosition(targetTilePos);

                // Prioritize unvisited tiles first
                if (targetNode.Owner == null)
                {
                    nextMove = direction;
                    foundUnvisitedTile = true;
                    break; // Prioritize moving to unvisited tiles first
                }
                // If no unvisited tiles are found, consider tiles owned by other players
                else if (targetNode.Owner != aiData)
                {
                    nextMove = direction; // Move to steal a tile from another player
                }
            }
        }

        // Step 2: If a valid direction is found, make the move
        if (nextMove != Vector2Int.zero)
        {
            movement.MoveTo(nextMove); // Move AI in the selected direction
        }
        else
        {
            // AI is stuck, no valid move found
            Debug.Log("No valid moves available for AI.");
        }
    }
}
