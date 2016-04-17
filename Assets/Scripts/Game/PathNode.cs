using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour {

    // grab location values from attached game object
    public void Start()
    {
        this.xLocation = this.gameObject.transform.position.x;
        this.yLocation = this.gameObject.transform.position.y;
    }

    // returns a random path node from ReachableNodes
    public PathNode PickRandomNode()
    {
        return ReachableNodes[Random.Range(0, ReachableNodes.Length)];
    }

    // set next target once enemy steps on node
    public virtual void OnTriggerEnter(Collider collider)
    {
        // only enemies interact with path nodes currently
        if(!collider.isTrigger && collider.tag.Equals("Enemy"))
        {
            // create reference for easy access
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();

            // Update their destination path node
            enemy.destinationPathNode = this.PickRandomNode();

            // inform enemy that it has reached path node
            enemy.ReachedPathNode();
        }
    }

    // array of path nodes that can be reached from this node
    [SerializeField]
    PathNode[] ReachableNodes;

    private float xLocation;
    private float yLocation;
}
