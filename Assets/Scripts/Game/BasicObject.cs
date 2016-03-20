using UnityEngine;
using System.Collections;

public class BasicObject : MonoBehaviour {

	// Use this for initialization
	public virtual void Start () {

        // GameObject.Find() is typically slow and not recommended, but for a project of our scale I think it's fine.
        playerReference = GameObject.Find("Player");
    }

    // reference to the player GameObject
    protected GameObject playerReference;
}
