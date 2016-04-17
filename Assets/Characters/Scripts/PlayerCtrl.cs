using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {

	private Animator playerAnimator;
	// Use this for initialization
	void Start () {
		playerAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("q")) {
			playerAnimator.SetBool ("Withdraw", true);
			Invoke ("StopWithdraw", 2.0f);
		}
	}

	void StopWithdraw() {
		playerAnimator.SetBool ("Withdraw", false);
	}
}
