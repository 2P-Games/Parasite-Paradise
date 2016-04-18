using UnityEngine;
using System.Collections;

public class Rescue : MonoBehaviour {

	// Audioclip to play when rescued.
	//public AudioClip rescue;							

	// Reference to the player.
	private GameObject player;	
	public bool deadleader;
	private Animator anim;

	// Reference to the player's inventory.
	private PlayerInventory playerInventory;		

	void Awake() {
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
		playerInventory = player.GetComponent<PlayerInventory>();
		anim = GetComponent<Animator> ();
	}

	void Update(){
		
	}

	void OnTriggerEnter(Collider other) {
		// If the colliding gameobject is the player...
		if(other.gameObject == player) {
			// ... play the clip at the position of the key...
			//AudioManager.instance.PlaySoundAtPosition(audioclip, transform.position);

			// ... the player has a key ...
			playerInventory.numberOfParasites++;

			// ... and destroy this gameobject.
			Destroy(gameObject);
		}
	}
}