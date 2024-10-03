using UnityEngine;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSize;
    public int UnityGridSize { get { return unityGridSize; } }

    [SerializeField] List<GameObject> tileGameObjects;  // Manually assign these in the editor
    [SerializeField] Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    void Awake()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        foreach (GameObject tileObject in tileGameObjects)
        {
            AddNodeToGrid(tileObject);
        }
    }

    void AddNodeToGrid(GameObject tileObject)
    {
        Tile tile = tileObject.GetComponent<Tile>();
        if (tile != null)
        {
            Vector2Int coordinates = GetTileCoordinates(tileObject.transform.position);
            Node node = CreateNode(coordinates, tile);
            grid.Add(coordinates, node);

            Debug.Log($"Added Node at {coordinates} for tile: {tileObject.name}");
        }
        else
        {
            Debug.LogError("Tile component missing on tile object: " + tileObject.name);
        }
    }

    public Vector2Int GetTileCoordinates(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int(
            Mathf.RoundToInt(position.x / unityGridSize),
            Mathf.RoundToInt(position.z / unityGridSize)
        );
        Debug.Log($"World position {position} -> Grid coordinates {coordinates}");
        return coordinates;
    }

    Node CreateNode(Vector2Int coordinates, Tile tile)
    {
        return new Node(coordinates, tile);
    }
    public virtual void SetNodeOccupied(Vector2Int position, bool isOccupied)
    {
        if (grid.ContainsKey(position))  // Check if the node exists in the dictionary
        {
            grid[position].IsOccupied=isOccupied ;  // Set the occupancy status based on the boolean
            Debug.Log($"{(isOccupied ? "Occupied" : "Unoccupied")} node at position {position}");
        }
        else
        {
            Debug.LogError($"No node found at position {position} to set occupancy");
        }
    }
     
    public bool isValidPosition(Vector3 position)
    {
        Vector2Int targetPosition = GetTileCoordinates(position);  // Convert to grid coordinates
        Node targetNode = GetNodeAtPosition(targetPosition);       // Retrieve the node at the target position

        if (targetNode == null)
        {
            Debug.Log($"Invalid position: {targetPosition} (no node found)");
            return false; // Out of bounds or invalid position
        }

        if (targetNode.IsOccupied)  // Check if the node is already occupied
        {
            Debug.Log($"Position: {targetPosition} is occupied.");
            return false; // Tile is occupied, so the position is not valid
        }

        return true; // Position is valid and tile is free
    }

    public virtual Node GetNodeAtPosition(Vector2Int position)
    {
        if (grid.ContainsKey(position))
        {
            return grid[position];
        }
        else
        {
            Debug.LogError($"No node found at position {position}");
            return null;
        }
    }

    public virtual Node GetNodeAtPosition(Vector3 worldPosition)
    {
        Vector2Int gridPosition = new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / unityGridSize),
            Mathf.RoundToInt(worldPosition.z / unityGridSize)
        );

        return GetNodeAtPosition(gridPosition);
    }
}
