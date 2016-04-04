using UnityEngine;
using System.Collections;

public class PathNodeLibrary : MonoBehaviour {

    // on start 
    public void Awake()
    {
        // find all path nodes
        AllPathNodes = GameObject.FindObjectsOfType<PathNode>();
    }

    // static utility function to find the PathNode with the 
    public static PathNode GetNearestPathNodeFromLocation(Vector3 fromPosition)
    {
        float minDistance = 100000000;
        PathNode closestNode = PathNodeLibrary.AllPathNodes[0];

        foreach(PathNode pathNode in PathNodeLibrary.AllPathNodes)
        {
            // simple min distance check
            if(Vector3.Distance(fromPosition, pathNode.gameObject.transform.position) < minDistance)
            {
                minDistance = Vector3.Distance(fromPosition, pathNode.gameObject.transform.position);
                closestNode = pathNode;
            }
        }

        // return the closest node found
        return closestNode;
    }

    // array of all path nodes in the game
    private static PathNode[] AllPathNodes;

}
