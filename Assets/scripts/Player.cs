using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player  
{
    private string name;
    private int point;
    private float speed;
    private Color color;
    
    public string Name { set { } get { return name; } }
    public int Point { set { } get { return point; } }
    public float Speed { set { } get { return speed; } }
    public Color Color { set { } get { return color; } }

    public Player(string name, float speed, int point = 0)
    {
        this.name = name;
        this.speed = speed;
        this.point = point;
    
    }

    public void Movement() { }
    public void TriggerCollectable() { }
    public void ChangeCellColor() { }
    public void GetPoints() { }

  
}
