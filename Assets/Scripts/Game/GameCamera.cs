using UnityEngine;
using System.Collections;

public class GameCamera : BasicObject
{

    // Update is called once per frame
    void Update()
    {

        // gameObject is how you reference the object attached to.

        /*
        * This basically continuously updates the camera's 'x' and 'y' position to always be above the player. 
        * Note: The X and Y data fields are private and cannot be directly modified in C#, so this a workaround.
        */
        gameObject.transform.position = new Vector3(playerReference.transform.position.x, playerReference.transform.position.y + 15, playerReference.transform.position.z);

    }

}
