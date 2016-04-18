using UnityEngine;
using System.Collections;

public class OpenDoors : MonoBehaviour {

	public bool hasInfluence;

	// Use this for initialization
	void Awake() {
		

	}
	
	// Update is called once per frame
	void Update () {
		if (hasInfluence)
			GetComponent<Animation> ().Play ("doorOpen01");
	}
}
