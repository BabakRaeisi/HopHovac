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
    [SerializeField] private GridManager gridManager; // Make this field visible in the Inspector

    void Start()
    {
        if (gridManager == null)
        {
            Debug.LogError("GridManager is not assigned!");
            return;
        }

        if (colorManager == null)
        {
            Debug.LogError("ColorManager is not assigned!");
            return;
        }

        Vector3 initialPosition = transform.position;
        currentPos = new Vector2Int(
            Mathf.RoundToInt(initialPosition.x / gridManager.UnityGridSize),
            Mathf.RoundToInt(initialPosition.z / gridManager.UnityGridSize)
        );

        targetPos = new Vector3(currentPos.x * gridManager.UnityGridSize, 0, currentPos.y * gridManager.UnityGridSize);
        transform.position = targetPos;
        targetDirection = transform.forward;

        // Get the initial node and change its color
        currentNode = gridManager.GetNodeAtPosition(currentPos);
        if (currentNode != null)
        {
            Debug.Log($"Setting color for currentNode at position: {currentPos}");
            IColorManager colorManagerInterface = colorManager; // Cast to interface
            Color playerColor = colorManagerInterface.GetPlayerColor(playerIndex);
            Debug.Log($"Player color obtained: {playerColor}");
            currentNode.NodeColor = playerColor;
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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveObject(Vector2Int.left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveObject(Vector2Int.right);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveObject(Vector2Int.up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveObject(Vector2Int.down);
        }
    }

    void MoveObject(Vector2Int direction)
    {
        Vector2Int newPosition = currentPos + direction;
        Debug.Log($"Moving to new position: {newPosition}");
        if (IsValidPosition(newPosition))
        {
            currentPos = newPosition;
            targetPos = new Vector3(currentPos.x * gridManager.UnityGridSize, 0, currentPos.y * gridManager.UnityGridSize);
            targetDirection = new Vector3(direction.x, 0, direction.y);
            isMoving = true;

            // Get the new node and change its color if different
            Node newNode = gridManager.GetNodeAtPosition(currentPos);
            if (newNode != null)
            {
                if (newNode != currentNode)
                {
                    IColorManager colorManagerInterface = colorManager; // Cast to interface
                    newNode.NodeColor = colorManagerInterface.GetPlayerColor(playerIndex);
                    currentNode = newNode;
                }
            }
            else
            {
                Debug.LogError("New node is null. Position: " + newPosition);
            }
        }
        else
        {
            Debug.LogError("Invalid position: " + newPosition);
        }
    }

    void SmoothMove()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
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
