using UnityEngine;
using System.Collections;

public class CCTV : BasicObject {

    public AudioClip servoSound;
    public float maxRotation = 75F;
    public float rotationSpeed = 15;
    public float trackPlayerSpeed = 4;

    // The joint the camera rotates around.
    Transform joint;
    Quaternion initialRotation;
    bool lockedOnPlayer = false;
    float angle;

	void Awake() {
		// Setting up the references.
        joint = transform.parent.parent;
        initialRotation = joint.rotation;

        // Play the gun shot clip at the position of the muzzle flare.
        AudioManager.instance.PlaySound(joint.GetComponent<AudioSource>(), servoSound, 0.1f, 50f, true);

        InvokeRepeating("CheckSoundOcclusion", 1f, 1f);
    }

    void CheckSoundOcclusion() {
        AudioManager.instance.CheckDistanceAndOcclusionToListener(joint.GetComponent<AudioSource>());
    }

    void Update() {

        if (lockedOnPlayer) {
            return;
        }

        joint.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        if (Quaternion.Angle(initialRotation, joint.rotation) >= maxRotation) {
            rotationSpeed *= -1;
        }
    }
	
	void OnTriggerStay(Collider other) {

		// If the colliding gameobject is the player.
		if (other.gameObject == playerReference) {
            // Don't do anything unless we can see the player. He could be behind something.
            Vector3 direction = playerReference.transform.position - transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit)) {
                if (hit.transform.gameObject != playerReference) {
                    return;
                }
            }

            // Sound alarm.
            AlarmManager.Get().TurnOnAlarm(playerReference.transform.position);
 
            lockedOnPlayer = true;

            Quaternion newRotation = Quaternion.LookRotation(playerReference.transform.position - joint.position, Vector3.up);
            newRotation.x = 0.0f;
            newRotation.z = 0.0f;
            if (Quaternion.Angle(initialRotation, newRotation) <= maxRotation) {
                joint.rotation = Quaternion.Slerp(joint.rotation, newRotation, Time.deltaTime * trackPlayerSpeed);
            }
		}
	}

    void OnTriggerExit(Collider other) {

		// If the colliding gameobject is the player.
        if (other.gameObject.tag.Equals("Player")) {
            lockedOnPlayer = false;
        }
    }
}
