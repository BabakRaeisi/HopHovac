using UnityEngine;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSize;
    public int UnityGridSize { get { return unityGridSize; } }

    [SerializeField] List<GameObject> tileGameObjects;  // Manually assign these in the editor
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
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

    public bool isValidPosition(Vector3 position)
    {
        Vector2Int targetPosition = GetTileCoordinates(position);  // Convert to grid coordinates
        Node targetNode = GetNodeAtPosition(targetPosition);       // Retrieve the node at the target position

        Debug.Log($"Checking validity of position: {targetPosition}. Is occupied: {targetNode?.isOccupied ?? true}");

        if (targetNode == null)
        {
            Debug.Log($"Invalid position: {targetPosition} (no node found)");
            return false; // Out of bounds or invalid position
        }

        if (targetNode.isOccupied)
        {
            Debug.Log($"Position: {targetPosition} is occupied.");
            return false; // Tile is occupied, so the position is not valid
        }

        return true; // Position is valid and tile is free
    }

    public Node GetNodeAtPosition(Vector2Int position)
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

    public Node GetNodeFromWorldPosition(Vector3 worldPosition) // I dont remember why I had made this 
    {
        Vector2Int gridPosition = new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / unityGridSize),
            Mathf.RoundToInt(worldPosition.z / unityGridSize)
        );

        return GetNodeAtPosition(gridPosition);
    }
}
