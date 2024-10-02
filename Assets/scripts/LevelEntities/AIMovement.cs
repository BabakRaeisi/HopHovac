using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private GridSystem gridSystem; // Reference to the GridSystem
    private Movement movement; // Shared Movement logic

    private Vector2Int currentPos; // Current grid position
    private Vector3 targetPos; // Target position in world space

    private bool isMoving = false; // Is the AI currently moving
    public PlayerData aiData; // AI's data (name, speed, etc.)

    private void Awake()
    {
        movement = GetComponent<Movement>();
        gridSystem = FindObjectOfType<GridSystem>(); // Get the GridSystem instance
        InitializePosition(); // Set the initial position on the grid
    }

    private void Update()
    {
        if (!isMoving)
        {
            FindNextTile(); // AI decides where to move
        }
        else
        {
            SmoothMove(); // Continue moving towards the target position
        }
    }

    private void FindNextTile()
    {
        // Here, implement AI logic to find the next tile
        // For simplicity, we can move in a random direction, but this can be improved with pathfinding or specific logic
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int nextDirection = directions[Random.Range(0, directions.Length)];
        Vector2Int newPos = currentPos + nextDirection;

        if (gridSystem.IsValidPosition(newPos))
        {
            currentPos = newPos;
            targetPos = new Vector3(currentPos.x * gridSystem.UnityGridSize, 0, currentPos.y * gridSystem.UnityGridSize);
            isMoving = true; // Start moving
        }
    }

    private void SmoothMove()
    {
        // Move the AI towards the target position
        movement.MoveTo(targetPos, transform);

        // Check if the AI has reached the target position
        if (movement.IsAtTarget(targetPos, transform))
        {
            FlipTile(); // Flip the tile when reaching the new tile
            isMoving = false; // Stop moving
        }

        // Smoothly rotate the AI towards the target direction
        Vector3 direction = targetPos - transform.position;
        movement.RotateTowards(direction, transform);
    }

    

    private void InitializePosition()
    {
        Vector3 initialPosition = transform.position;
        currentPos = new Vector2Int(
            Mathf.RoundToInt(initialPosition.x / gridSystem.UnityGridSize),
            Mathf.RoundToInt(initialPosition.z / gridSystem.UnityGridSize)
        );
        targetPos = new Vector3(currentPos.x * gridSystem.UnityGridSize, 0, currentPos.y * gridSystem.UnityGridSize);
        transform.position = targetPos; // Set the initial position
        FlipTile();
    }

    private void FlipTile()
    {
        // Get the current tile and flip its ownership if necessary
        Node currentNode = gridSystem.GetNodeAtPosition(currentPos);
        if (currentNode != null)
        {
            // Check if the tile is owned by another character, and if so, flip it to the AI's ownership
            if (currentNode.Owner != aiData)
            {
                currentNode.SetOwner(aiData); // Change ownership of the tile to the AI
            }
        }
    }
}
