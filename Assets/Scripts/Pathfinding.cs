using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    AstarGrid grid;

    void Awake() {
        grid = gameObject.GetComponent<AstarGrid>();
        requestManager = gameObject.GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    //The A* Algorithm
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos) {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        if(!startNode.walkable || targetNode.walkable) {
            requestManager.FinishedProcessingPath(waypoints, pathSuccess);
        }

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
                pathSuccess = true;
                break;
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
        yield return null;
        if(pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld) {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
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
