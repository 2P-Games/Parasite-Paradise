using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        /** Note: Using GetKey() because force needs to be added every frame while key is down, not only once. */

        // upward movement
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, maxSpeed, 0) * this.acceleration);
        }

        // left movement
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-maxSpeed, 0, 0) * this.acceleration);
        }

        // down movement
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -maxSpeed, 0) * this.acceleration);
        }

        // right movement
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(maxSpeed, 0, 0) * this.acceleration);
        }
    }

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private float maxSpeed;
}
