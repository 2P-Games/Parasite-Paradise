using UnityEngine;
using System.Collections;

public class LaserPlayerDetection : BasicObject {					

    void OnTriggerStay(Collider other) {
		// If the beam is on...
        if (GetComponent<Renderer>().enabled) {
			// ... and if the colliding gameobject is the player...
            if (other.gameObject.tag.Equals("Player")) {
                // ... set the last global sighting of the player to the colliding object's position.
                AlarmManager.Get().TurnOnAlarm(other.gameObject.transform.position);
			}
		}
    }
}