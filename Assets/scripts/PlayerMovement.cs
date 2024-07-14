using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Current position of the object in grid coordinates
    private Vector2Int currentPos;

    // Target position for smooth movement
    private Vector3 targetPos;

    [SerializeField]
    private GridManager gridManager;

    private Node currentNode;

    // Size of each cell
    private int cellSize = 2;

    // Flag to determine if movement is ongoing
    private bool isMoving = false;

    // The target rotation direction
    private Vector3 targetDirection;

    // Speed of movement
    public float moveSpeed = 5f;

    // Speed of rotation
    public float rotationSpeed = 720f;

    private void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
    }


    void Start()
    {
        // Get the initial position from the transform
        Vector3 initialPosition = transform.position;

        // Convert the initial position to grid coordinates
        currentPos = new Vector2Int(Mathf.RoundToInt(initialPosition.x / cellSize), Mathf.RoundToInt(initialPosition.z / cellSize));
        targetPos = new Vector3(currentPos.x * cellSize, 0, currentPos.y * cellSize);

        // Ensure the transform position is correctly set
        transform.position = targetPos;
        targetDirection = transform.forward;

        currentNode = gridManager.GetNode(currentPos);
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

        // Check if the new position is within grid bounds
        if (IsValidPosition(newPosition))
        {
            currentPos = newPosition;
            targetPos = new Vector3(currentPos.x * cellSize, 0, currentPos.y * cellSize);

            // Set the target direction based on the direction of movement
            targetDirection = new Vector3(direction.x, 0, direction.y);

            isMoving = true;
        }
    }

    void SmoothMove()
    {
        // Lerp towards the target position
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);

            // Check if the object has reached the target position
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                transform.position = targetPos;
                isMoving = false;
            }
        }
    }

    void SmoothRotate()
    {
        // Smoothly rotate towards the target direction
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    bool IsValidPosition(Vector2Int position)
    {
        // Check if the position is within the 8x8 grid bounds
        return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
    }
}
