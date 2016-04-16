using UnityEngine;
using System.Collections;
using MORPH3D;

public class PropsScript : MonoBehaviour {

	private M3DCharacterManager charman;

	// Use this for initialization
	void Start () {
		charman = GetComponent<M3DCharacterManager>();	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("n")) {
			charman.DetachPropFromAttachmentPoint ("TyroleanGreatSword", "rHandAttachmentPointR");
			charman.AttachPropToAttachmentPoint ("TyroleanGreatSword", "hipAttachmentPoint");
		}
	}
}
