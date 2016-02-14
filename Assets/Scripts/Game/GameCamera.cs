using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        // GameObject.Find() is typically slow and not recommended, but for a project of our scale I think it's fine.
        playerReference = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        // gameObject is how you reference the object attached to.

        /*
        * This basically continuously updates the camera's 'x' and 'y' position to always be above the player. 
        * Note: The X and Y data fields are private and cannot be directly modified in C#, so this a workaround.
        */
        gameObject.transform.position = new Vector3(playerReference.transform.position.x, playerReference.transform.position.y, -10);

    }

    private GameObject playerReference;
}
