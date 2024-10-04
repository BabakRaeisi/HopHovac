using UnityEngine;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSize;
    public int UnityGridSize { get { return unityGridSize; } }

    [SerializeField] List<GameObject> tileGameObjects;  // Manually assign these in the editor
    [SerializeField] Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();  // Grid dictionary to hold nodes
    [SerializeField] private List<PlayerData> players;  // Players assigned in the Unity editor
    [SerializeField] private Dictionary<PlayerData, Node> playerCurrentNodes = new Dictionary<PlayerData, Node>();  // Dictionary to hold players and their current nodes



#region InitializeGrid
    void Awake()
    {
        InitializeGrid();
    }

    void Start()
    {
      
    }

    // Initializes the grid by adding all nodes (tiles)
    void InitializeGrid()
    {
        foreach (GameObject tileObject in tileGameObjects)
        {
            AddNodeToGrid(tileObject);
        }
    }

    // Adds a tile to the grid and sets its coordinates
    void AddNodeToGrid(GameObject tileObject)
    {
        Tile tile = tileObject.GetComponent<Tile>();
        if (tile != null)
        {
            Vector2Int coordinates = GetTileCoordinates(tileObject.transform.position);
            Node node = CreateNode(coordinates, tile);
            node.IsOccupied = false;  // Initialize node as unoccupied
            grid.Add(coordinates, node);
        }
        else
        {
            Debug.LogError("Tile component missing on tile object: " + tileObject.name);
        }
    }
    // Creates a new Node for the grid
    Node CreateNode(Vector2Int coordinates, Tile tile)
    {
        return new Node(coordinates, tile);
    }
    public void AddPlayer(PlayerData player, Vector2Int startPosition)
    {
        // Ensure the player is not already in the system
        if (!playerCurrentNodes.ContainsKey(player))
        {
            players.Add(player);  // Add the player to the players list
        }

        // Retrieve the node at the player's initial position
        if (grid.TryGetValue(startPosition, out Node startNode))
        {
            // Set the node as occupied by the player
            startNode.IsOccupied = true;
            startNode.Owner = player;

            // Update the player's current position in the grid system
            playerCurrentNodes[player] = startNode;

            // Set the player's initial position in PlayerData
            player.CurrentGridPosition = startPosition;
        }
        else
        {
            Debug.LogError($"Invalid start position: {startPosition}");
        }
    }
    #endregion

    // Converts world position to grid coordinates
    public Vector2Int GetTileCoordinates(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / unityGridSize);
        int y = Mathf.RoundToInt(worldPosition.z / unityGridSize);
        return new Vector2Int(x, y);
    }
    public Node GetNodeAtPosition(Vector2Int position)
    {
        if (grid.TryGetValue(position, out Node node))
        {
            return node;  // Return the node if it exists at the given position
        }
        else
        {
            Debug.LogWarning($"No node found at position: {position}");
            return null;  // Return null if no node is found at the given position
        }
    }



    // Check if the target position is within grid boundaries and the node is not occupied
    public bool IsValidPosition(Vector2Int newCoords)
    {
        return grid.ContainsKey(newCoords) && !grid[newCoords].IsOccupied;
    }

    // Try to move player to the specified coordinates
    public bool TryMovePlayer(PlayerData player, Vector2Int newCoords)
    {
        if (IsValidPosition(newCoords))
        {
            // Get the current node of the player
            if (playerCurrentNodes.TryGetValue(player, out Node currentNode))
            {
                currentNode.IsOccupied = false;  // Unoccupy the current node
            }

            // Move player to the new node
            Node targetNode = grid[newCoords];
            targetNode.IsOccupied = true;
            targetNode.Owner = player;  // Update the node's owner to the current player
            playerCurrentNodes[player] = targetNode;  // Update the player's current node

            // Update player's current grid position
            player.CurrentGridPosition = newCoords;

            return true;  // Move successful
        }
        return false;  // Move failed (target node is occupied or out of bounds)
    }
   
    

    // Example method to return player's starting position (can be hardcoded or dynamic)
    private Vector2Int GetPlayerStartingPosition(Transform player)
    {
         
        return  GetTileCoordinates(player.transform.position);  // Use the actual position from the scene
    }
}
