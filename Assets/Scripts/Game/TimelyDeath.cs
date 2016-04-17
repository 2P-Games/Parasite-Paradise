using UnityEngine;
using System.Collections;

public class TimelyDeath : MonoBehaviour {
	
	// Called at script start
	void Start () {
        Object.Destroy(this.gameObject, timeToDeath);
    }

    [SerializeField]
    private float timeToDeath = 0.0f;
}
