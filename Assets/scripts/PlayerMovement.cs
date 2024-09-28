using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2Int currentPos;
    private Vector3 targetPos;
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    private bool isMoving = false;
    private Vector3 targetDirection;
    private Node currentNode;

    [SerializeField] private ColorManager colorManager; // Use the concrete class here
    public int playerIndex;  // Index for the player's color

    // Reference to the GridManager
    [SerializeField] private GridManager gridManager;

    void Start()
    {
        InitializeManagers();
        InitializePosition();
        InitializeCurrentNode();
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
        transform.position = targetPos;
        targetDirection = transform.forward;
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
            Debug.LogError("Current node is null at start. Position: " + currentPos);
        }
    }

    void Update()
    {
        if (!isMoving)
        {
            HandleMovement();
        }
        SmoothMove();
        SmoothRotate();
    }

    void HandleMovement()
    {
        // Here you can handle input for moving the player to a new tile
        // For example, use input to modify currentPos and targetPos
        // currentPos = new Vector2Int(...); (movement logic)

        // Validate new position
        if (IsValidPosition(currentPos))
        {
            targetPos = new Vector3(currentPos.x * gridManager.UnityGridSize, 0, currentPos.y * gridManager.UnityGridSize);
            isMoving = true;
            UpdateNodeColor();
        }
    }

    void UpdateNodeColor()
    {
        currentNode = gridManager.GetNodeAtPosition(currentPos);
        if (currentNode != null)
        {
            currentNode.SetOwner(this);  // This will automatically update the tile's color
        }
    }

    void SmoothMove()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                transform.position = targetPos;
                isMoving = false;
            }
        }
    }

    void SmoothRotate()
    {
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    bool IsValidPosition(Vector2Int position)
    {
        bool isValid = gridManager.GetNodeAtPosition(position) != null;
        if (!isValid)
        {
            Debug.LogError("Position is not valid in the grid: " + position);
        }
        return isValid;
    }
}
