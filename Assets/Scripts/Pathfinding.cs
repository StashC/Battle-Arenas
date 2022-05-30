using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    AstarGrid grid;

    public Transform seeker, target;
    void Awake() {
        grid = gameObject.GetComponent<AstarGrid>();
    }

    void Update() {
        FindPath(seeker.position, target.position);
    }
    void FindPath(Vector2 startPos, Vector2 targetPos) {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>(); //unevaluated nodes
        HashSet<Node> closedSet = new HashSet<Node>(); //evaluated nodes

        openSet.Add(startNode);

        while(openSet.Count > 0) {
            Node curNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++) {
                if(openSet[i].fCost < curNode.fCost || openSet[i].fCost == curNode.fCost && openSet[i].hCost < curNode.hCost) {
                    curNode = openSet[i];
                }
            }
            //remove curNode from open set, to be evaluated
            openSet.Remove(curNode);
            closedSet.Add(curNode);

            if(curNode == targetNode) {
                //we have found our target
                RetrachPath(startNode, targetNode);
                return;
            }
            //loop through neighbouring nodes
            foreach (Node neighbour in grid.GetNeighbours(curNode)) {
                if(!neighbour.walkable || closedSet.Contains(neighbour)) 
                    continue;

                int newCostToNeighbour = curNode.gCost + getDistance(curNode, neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = getDistance(neighbour, targetNode);
                    neighbour.parent = curNode;
                }
                if(!openSet.Contains(neighbour)) openSet.Add(neighbour);
            }
        }
    }

    void RetrachPath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int getDistance(Node nodeA, Node nodeB) {
        //# of diagonals = the shorter of either horizontal or vertical moves. 
        //# of lateral moves = longer distance - # of diagonals or longer - shorter
        //Diagonal = 14, lateral = 10

        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
