using UnityEngine;

public class Node
{
    public Vector2Int Coordinates { get; private set; }
    private Color nodeColor;
    private Player owner;
    private GameObject tileGameObject;

    public Node(Vector2Int coordinates, GameObject tileGameObject)
    {
        this.Coordinates = coordinates;
        this.tileGameObject = tileGameObject;
        this.nodeColor = Color.white;  // Default color
    }

    public Player Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    public Color NodeColor
    {
        get { return nodeColor; }
        set
        {
            nodeColor = value;
            UpdateTileColor();
        }
    }

    public void ResetNode()
    {
        this.Owner = null;
        this.NodeColor = Color.white;
    }

    private void UpdateTileColor()
    {
        Tile tileScript = tileGameObject.GetComponent<Tile>();
        if (tileScript != null)
        {
            tileScript.SetColor(nodeColor);
        }
    }
}
