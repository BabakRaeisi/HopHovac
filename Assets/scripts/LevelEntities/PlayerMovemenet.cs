
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GridSystem gridSystem; // Reference to the GridSystem
    private Movement movement; // Movement logic handling

    private Vector2Int currentPos; // Current grid position
    private Vector3 targetPos; // Target position in world space

    private bool isMoving = false; // Is the player currently moving
    private Vector2Int inputDirection = Vector2Int.zero; // The direction the player is moving in
    public PlayerData playerData; // Player data including speed, points, etc.

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
            HandleInput(); // Only accept new input when not moving
        }
        else
        {
            SmoothMove(); // Continue moving towards the target position
        }
    }

    private void HandleInput()
    {
        inputDirection = Vector2Int.zero;

        // Capture input for directional movement
        if (Input.GetKey(KeyCode.W))
        {
            inputDirection = Vector2Int.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputDirection = Vector2Int.down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputDirection = Vector2Int.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputDirection = Vector2Int.right;
        }

        if (inputDirection != Vector2Int.zero)
        {
            Vector2Int newPos = currentPos + inputDirection;
            if (IsValidPosition(newPos))
            {
                currentPos = newPos;
                targetPos = new Vector3(currentPos.x * gridSystem.UnityGridSize, 0, currentPos.y * gridSystem.UnityGridSize);
                isMoving = true; // Start moving
            }
        }
    }

    private void SmoothMove()
    {
        // Move the player towards the target position
        movement.MoveTo(targetPos, transform);

        // Check if the player has reached the target position
        if (movement.IsAtTarget(targetPos, transform))
        {
            UpdateNodeColor(); // Update node ownership when reaching the new tile
            isMoving = false; // Stop moving

            // Immediately check if the key is still pressed to continue moving
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                HandleInput(); // Continue moving in the direction if the key is held
            }
        }

        // Smoothly rotate the player towards the target direction
        Vector3 direction = targetPos - transform.position;
        movement.RotateTowards(direction, transform);
    }

    private bool IsValidPosition(Vector2Int position)
    {
        return gridSystem.GetNodeAtPosition(position) != null; // Check if the position is valid within the grid
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
        UpdateNodeColor();
    }

    private void UpdateNodeColor()
    {
        Node currentNode = gridSystem.GetNodeAtPosition(currentPos);
        if (currentNode != null)
        {
            currentNode.SetOwner(movement.playerData); // Update node color and ownership
        }
    }
}

 
