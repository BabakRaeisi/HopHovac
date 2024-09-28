using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player
{
    
    public string Name { get; protected set; }
    public int Points { get; protected set; }
    public float Speed { get; protected set; }
    public Color PlayerColor { get; protected set; }

    
    public Player(string name, float speed, int points = 0)
    {
        Name = name;
        Speed = speed;
        Points = points;
    }

    // Abstract methods for subclasses to implement
    protected abstract void Movement();
    protected abstract void TriggerCollectable();
    protected abstract void ChangeCellColor();
    protected abstract int GetPoints();
}