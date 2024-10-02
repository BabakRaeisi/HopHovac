using UnityEngine;

public class Node
{
    public Vector2Int Coordinates { get; private set; }
    public Tile Tile { get; private set; }
    public PlayerData Owner { get; private set; }



    // Color property updates the tile when changed
    private Color nodeColor;
    public Color NodeColor
    {
        get { return nodeColor; }
        set
        {
            nodeColor = value;
            Tile.SetColor(nodeColor); // Automatically update tile appearance
        }
    }

    public Node(Vector2Int coordinates, Tile tile)
    {
        Coordinates = coordinates;
        Tile = tile;
        NodeColor = Color.white; // Default tile color
    }

    public void SetOwner(PlayerData player)
    {
        Owner = player;
        SetColor(Owner.PlayerColor);
    }

    public void SetColor(Color color) { NodeColor =  color; }


}
