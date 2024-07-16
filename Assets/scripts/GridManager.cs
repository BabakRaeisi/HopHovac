using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSize;
    public int UnityGridSize { get { return unityGridSize; } }

    [SerializeField] List<GameObject> tileGameObjects;  // Manually assign these in the editor
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    private void Awake()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        foreach (GameObject tileObject in tileGameObjects)
        {
            Tile tile = tileObject.GetComponent<Tile>();
            if (tile != null)
            {
                Vector3 position = tileObject.transform.position;
                Vector2Int coordinates = new Vector2Int(
                    Mathf.RoundToInt(position.x / unityGridSize),
                    Mathf.RoundToInt(position.z / unityGridSize)
                );

                Node node = new Node(coordinates, tileObject);
                grid.Add(coordinates, node);

                // Set the initial color of the tile based on the node's color
                tile.SetColor(node.NodeColor);

                Debug.Log($"Added Node at {coordinates} for Tile at {position}");
            }
            else
            {
                Debug.LogError($"Tile object {tileObject.name} does not have a Tile component.");
            }
        }
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

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        Vector2Int gridPosition = new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / unityGridSize),
            Mathf.RoundToInt(worldPosition.z / unityGridSize)
        );

        return GetNodeAtPosition(gridPosition);
    }
}
