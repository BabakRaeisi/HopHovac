using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UIElements;

public class Node
{
    public Vector2Int cords;
    private Color defaultColor = Color.white;
    private Color Color;
    private Player owner;


    public Node(Vector2Int cords)
    {
        this.cords = cords;
    }


    void SetOwner(Player owner)
    {
        this.owner = owner;
    }
    void SetColor(Color color)
    {
        this.Color = color;
    }

    public void ResetNode() 
    {
    this.owner = null;

    }

    
     
}

