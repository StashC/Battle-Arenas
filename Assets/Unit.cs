using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public Transform target;
    public float speed;

    Vector3[] path;
    int targetIndex;

    void Start() {
        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] path, bool pathFound) {
        if(pathFound)
            this.path = path;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];
        while(true) {
            if(transform.position == currentWaypoint) {
                targetIndex++;
                if(targetIndex >= path.Length) {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if(path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(path[targetIndex], Vector3.one);

                if(i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}