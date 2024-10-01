using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Movement movement; // Reference to the Movement component
    private Vector2Int currentPos; // Current grid position of the player
    private Vector3 targetPos; // Target position in world space
    private bool isMoving = false; // Track if the player is currently moving
    private GridSystem gridSystem; // Reference to the GridSystem

    void Awake()
    {
        movement = gameObject.GetComponent<Movement>(); // Add Movement component
        gridSystem = FindObjectOfType<GridSystem>(); // Get the GridSystem instance
        InitializePosition(); // Initialize the player's position

        PlayerInput playerInput = GetComponent<PlayerInput>(); // Get the PlayerInput component
        playerInput.actions["Move"].performed += OnMove; // Handle movement input
        playerInput.actions["Move"].canceled += OnMoveCanceled; // Stop movement on key release
    }

    void Update()
    {
        if (isMoving) // Only handle movement if the player is currently moving
        {
            SmoothMove(); // Call SmoothMove to move the player towards the target
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        HandleMovement(input); // Handle movement based on input
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        isMoving = false; // Stop movement when the input is canceled
    }

    private void HandleMovement(Vector2 input)
    {
        Vector2Int moveDirection = new Vector2Int(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y));

        // Debug: Check the input values
        Debug.Log($"Input received: {input}, Move Direction: {moveDirection}");

        // Check if there's a valid move direction and the player is not currently moving
        if (moveDirection != Vector2Int.zero && !isMoving)
        {
            Vector2Int newPos = currentPos + moveDirection;

            if (IsValidPosition(newPos))
            {
                currentPos = newPos;
                targetPos = new Vector3(currentPos.x * gridSystem.UnityGridSize, 0, currentPos.y * gridSystem.UnityGridSize);
                isMoving = true; // Start moving towards the new position
                Debug.Log($"Moving to new position: {targetPos}"); // Debug: Check target position
            }
        }
    }

    private void SmoothMove()
    {
        movement.MoveTo(targetPos, transform); // Move the player towards the target position

        // Check if the player has reached the target position
        if (movement.IsAtTarget(targetPos, transform))
        {
            UpdateNodeColor(); // Update node ownership when reaching the new tile
            isMoving = false; // Stop moving
        }

        // Smoothly rotate the player towards the target direction
        Vector3 direction = targetPos - transform.position;
        movement.RotateTowards(direction, transform); // Rotate towards the target
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
