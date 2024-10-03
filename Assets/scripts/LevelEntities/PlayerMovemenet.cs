using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Movement movement; // Movement logic handling

    private Vector2Int currentPos; // Current grid position
    private Vector3 targetPos; // Target position in world space

    private Vector2Int inputDirection = Vector2Int.zero; // The direction the player is moving in
    public PlayerData playerData; // Player data including speed, points, etc.

    private void Awake()
    {
        movement = GetComponent<Movement>(); // Get the Movement component
        
    }
    private void Start()
    {
     movement.InitializePosition(this.transform);
    }

    private void Update()
    {
        if (!movement.IsMoving())
        {
            HandleInput(); // Only accept new input when not moving
        }
        else
        {
            
            movement.SmoothMove(transform.position); // Continue moving towards the target position
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
            // Calculate the next grid position
            Vector2Int newPos = currentPos + inputDirection;
            Vector3 targetPosition = movement.GetGridAdjustedPosition(newPos);

            // Now check if the position is valid using Movement's IsValidPosition
            if (movement.IsValidPosition(targetPosition))
            {
                currentPos = newPos; // Only update currentPos if the move is valid
                movement.SetTarget(targetPosition); // Move to the new valid position
            }
            else
            {
                Debug.Log("Invalid move to position: " + newPos);
            }
        }
    }


}
