using UnityEngine;

public class Movement : MonoBehaviour
{
    private GridSystem gridSystem; // Reference to the GridSystem
    public PlayerData playerData; // Reference to PlayerData

    private void Awake()
    {
        gridSystem = FindObjectOfType<GridSystem>(); // Get the GridSystem instance
    }

    public void MoveTo(Vector3 targetPos, Transform transform)
    {
        // Move towards the target position based on the speed from PlayerData
        if (playerData != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * playerData.Speed);
        }
    }

    public bool IsAtTarget(Vector3 targetPos, Transform transform)
    {
        // Check if the current position is close enough to the target position
        return Vector3.Distance(transform.position, targetPos) < 0.1f;
    }

    public void RotateTowards(Vector3 targetDirection, Transform transform)
    {
        // Smoothly rotate towards the specified direction using rotationSpeed from PlayerData
        if (targetDirection != Vector3.zero && playerData != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * playerData.RotationSpeed);
        }
    }
    
}
