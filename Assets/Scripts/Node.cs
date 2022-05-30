using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    //the node we visited before this node, on the shortest path to this node.
    public Node parent;

    public Node(bool isWalkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = isWalkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
