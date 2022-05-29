using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;

    public Node(bool isWalkable, Vector2 _worldPos)
    {
        walkable = isWalkable;
        worldPosition = _worldPos;
    }
}
