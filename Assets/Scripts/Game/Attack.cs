using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // returns true if the target can be hit by the source of the attack, false otherwise.
    public bool IsAttackInRange(GameObject source, GameObject target)
    {
        return Vector3.Distance(target.transform.position, source.transform.position) <= this.range;
    }

    // the delay that occurs after this attack is made
    public float delay;

    // the range restriction that must be met for the attack to occur
    public float range;

    // the damage taken from being hit by this attack
    public int damage;

    /****** CONVERT THESE INTO A 1:1 MAP ******/

    // the Vector3 to add to the forward motion when firing the projectile, if applicable.
    public Vector3 adjustmentVector;

    // speed of projectile attack, if applicable
    public float speed;

    /**************** END ****************/

    // the prefab object associated with this attack, if applicable.
    public GameObject AttackPrefab;

    // the range type of this attack
    public RangeType attackType;

    // audio of the attack
    public AudioClip attackSound;

    // enum selection of range type
    public enum RangeType
    {
        Melee, Ranged
    }
}
