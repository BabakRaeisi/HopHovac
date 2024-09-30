using UnityEngine;

public class PlayerMovemenet : MonoBehaviour
{
    private Vector2Int currentPos; // Current grid position of the player
    private Vector3 targetPos; // Target position in world space
    private bool isMoving = false; // Is the player currently moving?
    private Node currentNode; // Current node the player is on
    private Vector3 targetDirection; // Direction the player is facing

     
    public PlayerData player; // Simplified PlayerData

    [SerializeField] private ColorManager colorManager; // Reference to color manager
    public int playerIndex; // Index for the player's color

    [SerializeField] private GridManager gridManager; // Reference to the grid manager

    private float moveDelay = 0.5f; // Delay between movements
    private float moveTimer = 0f; // Timer for movement delay

    void Awake()
    {
       
        InitializeManagers();
        InitializePosition();
        InitializeCurrentNode();
    }

    private void InitializePlayerData(string name)
    {
        player = GetComponent<PlayerData>();
    }

    void InitializeManagers()
    {
        if (colorManager == null)
        {
            colorManager = FindObjectOfType<ColorManager>();
        }

        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }

        if (colorManager == null || gridManager == null)
        {
            Debug.LogError("Failed to initialize ColorManager or GridManager.");
        }
    }

    void InitializePosition()
    {
        Vector3 initialPosition = transform.position;
        currentPos = new Vector2Int(
            Mathf.RoundToInt(initialPosition.x / gridManager.UnityGridSize),
            Mathf.RoundToInt(initialPosition.z / gridManager.UnityGridSize)
        );
        targetPos = new Vector3(currentPos.x * gridManager.UnityGridSize, 0, currentPos.y * gridManager.UnityGridSize);
        transform.position = targetPos; // Set initial position
    }

    void InitializeCurrentNode()
    {
        currentNode = gridManager.GetNodeAtPosition(currentPos);
        if (currentNode != null)
        {
            UpdateNodeColor();
        }
        else
        {
            Debug.LogError("Current node is null at start.");
        }
    }

    void Update()
    {
        HandleInput(); // Check for player input
        SmoothMove(); // Handle smooth movement
    }

    private void HandleInput()
    {
        if (isMoving || moveTimer > 0) return; // Don't process input if already moving or in delay

        // Handle movement input
        if (Input.GetKey(KeyCode.W))
        {
            MoveTo(currentPos + Vector2Int.up); // Move up
            targetDirection = Vector3.forward; // Set target direction
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MoveTo(currentPos + Vector2Int.down); // Move down
            targetDirection = Vector3.back; // Set target direction
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveTo(currentPos + Vector2Int.left); // Move left
            targetDirection = Vector3.left; // Set target direction
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveTo(currentPos + Vector2Int.right); // Move right
            targetDirection = Vector3.right; // Set target direction
        }
    }

    private void MoveTo(Vector2Int newPosition)
    {
        if (IsValidPosition(newPosition))
        {
            currentPos = newPosition; // Update current position
            targetPos = new Vector3(currentPos.x * gridManager.UnityGridSize, 0, currentPos.y * gridManager.UnityGridSize); // Update target position
            isMoving = true; // Start moving
            UpdateNodeColor(); // Update node ownership
        }
    }

    void UpdateNodeColor()
    {
        currentNode = gridManager.GetNodeAtPosition(currentPos);
        if (currentNode != null)
        {
            currentNode.SetOwner(player); // Update node color and ownership
        }
    }

    void SmoothMove()
    {
        if (isMoving)
        {
            float moveSpeed = player.Speed; // Get speed from PlayerData
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                transform.position = targetPos; // Snap to target position
                isMoving = false; // Stop moving
                moveTimer = moveDelay; // Start movement delay
            }

            RotatePlayer(); // Rotate the player toward the target direction
        }
        else if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime; // Decrease the timer
        }
    }

    void RotatePlayer()
    {
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * player.RotationSpeed);
        }
    }

    bool IsValidPosition(Vector2Int position)
    {
        bool isValid = gridManager.GetNodeAtPosition(position) != null; // Check if position is valid
        if (!isValid)
        {
            Debug.LogError("Position is not valid in the grid: " + position);
        }
        return isValid;
    }
}
