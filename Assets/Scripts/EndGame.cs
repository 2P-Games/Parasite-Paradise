using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {
	private SceneFadeInOut sceneFadeInOut;  
	private PlayerInventory playerInventory;
	private Transform player; 
	// Use this for initialization
	void Awake() {
		sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFadeInOut>();
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		playerInventory = player.gameObject.GetComponent<PlayerInventory>();
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other) {
		if(playerInventory.numberOfParasites >= 3)
			{
					sceneFadeInOut.EndScene();
			}
	}
}
