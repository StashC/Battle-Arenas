using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarGrid : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public float nodeSize;
    public LayerMask unwalkableMask;
    //2d array of nodes
    Node[,] grid;

    float nodeDiameter;
    int gridLenX, gridLenY;

    void Start() {
        gridLenX = Mathf.RoundToInt(gridWorldSize.x / nodeSize);
        gridLenY = Mathf.RoundToInt(gridWorldSize.y / nodeSize);
        CreateGrid();
    }

    void CreateGrid() {
        grid = new Node[gridLenX, gridLenY];
        Vector3 bottomLeftWorld = transform.position - gridWorldSize.x / 2 * Vector3.right - gridWorldSize.y/ 2 * Vector3.up;


        for (int x = 0; x < gridLenX; x++) {
            for(int y = 0; y < gridLenY; y++) {
                Vector3 worldPoint = bottomLeftWorld + Vector3.right * (x * nodeSize + nodeSize / 2) + 
                    Vector3.up * (y * nodeSize + nodeSize / 2);
                bool isWalkable = !Physics2D.OverlapCircle(worldPoint, nodeSize / 2, unwalkableMask);
                grid[x, y] = new Node(isWalkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector2 point) {
        //gets the closest node in the grid to the input point.
        //0% is furthest left / down, 100% is furthest right / up
        float percentX = (point.x + gridWorldSize.x / 2)/gridWorldSize.x;
        float percentY = (point.y + gridWorldSize.y / 2)/gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridLenX - 1) * percentX);
        int y = Mathf.RoundToInt((gridLenY - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if(x == 0 & y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridLenX && checkY >= 0 && checkY < gridLenY) 
                    neighbours.Add(grid[checkX, checkY]);
            }
        }

        return neighbours;
    }

    public List<Node> path;
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if(grid != null) {
            foreach(Node n in grid) {
                //if n is walkable? -> color = white. else, color = red.
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(path != null)
                    if(path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeSize - 0.1f));
            }
        }
    }

}
