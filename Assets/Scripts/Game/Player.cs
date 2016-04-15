using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
    void Start()
    {
        this.controlsEnabled = true;
        UIUpdater.UpdateHealthText(this.health);
    }

    // Update is called once per frame
    void Update () {

        if (this.controlsEnabled)
        {
            // player looking at mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePosition = new Vector3(ray.origin.x, ray.origin.y, transform.position.z);
            Quaternion rotationQuaternion = Quaternion.LookRotation(mousePosition - transform.position);
            rotationQuaternion.y = 0;
            rotationQuaternion.x = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationQuaternion, rotationSpeed * Time.deltaTime);

            /** Note: Using GetKey() because force needs to be added every frame while key is down, not only once. **/

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

        // attempt to acquire an enemy target once
        if(Input.GetKeyDown(KeyCode.Space))
        {

            // if no enemies are close enough or is already infecting an enemy, cannot possess new target.
            if(EnemiesInRange.Capacity == 0 || isPossessingEnemy)
            {
                return;
            }

            // perform a min distance check; pick the closest enemy to the player.
            GameObject closestEnemy = EnemiesInRange[0];
      
            foreach(GameObject obj in this.EnemiesInRange) {
                if(Vector3.Distance(obj.transform.position, gameObject.transform.position) < Vector3.Distance(closestEnemy.transform.position, gameObject.transform.position))
                {
                    closestEnemy = obj;
                }
            }

            // rotate to look at the closest enemy so it looks more natural.
            gameObject.transform.LookAt(closestEnemy.transform);

            // extract the enemy script
            infectionTarget = closestEnemy.GetComponent<Enemy>();

            // turn off movement while infecting
            this.controlsEnabled = false;
            infectionTarget.EnterInfectionStasis();


        }

        // continue to hold down spacebar to fill infection bar
        if (Input.GetKey(KeyCode.Space))
        {

            // if we currently are not possessing an enemy, and a target has been successfully selected, begin infection.
            if (infectionTarget != null)
            {
                if (infectTimer < infectionTarget.timeToInfect)
                {
                    // increase "infection" meter
                    infectTimer += Time.deltaTime * 2.0f;
                } else
                {
                    // infection complete!
                    this.Infect(infectionTarget);
                }

            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            if (this.infectTimer > 0.0f)
            {
                // space bar released, reset timer and enable controls
                this.controlsEnabled = true;
                this.infectionTarget.ReleaseFromInfectionStasis();
                this.infectTimer = 0.0f;
                this.infectionTarget = null;
            }
        }
    }

    public void Infect(Enemy targetEnemy)
    {
        this.isPossessingEnemy = true;

        GameObject enemyObject = targetEnemy.gameObject;

        // steal the mesh to make it look like the enemy
        UnityEditorInternal.ComponentUtility.CopyComponent(enemyObject.GetComponent<SkinnedMeshRenderer>());
        UnityEditorInternal.ComponentUtility.PasteComponentValues(gameObject.GetComponent<SkinnedMeshRenderer>());

        // for cubes/enemies without a skinned mesh renderer
        UnityEditorInternal.ComponentUtility.CopyComponent(enemyObject.GetComponent<MeshRenderer>());
        UnityEditorInternal.ComponentUtility.PasteComponentValues(gameObject.GetComponent<MeshRenderer>());

        // "kill" the enemy
        targetEnemy.TakeDamage(9999);

        // reset timers
        this.infectTimer = 0.0f;

        // re-enable controls, just in case.
        this.controlsEnabled = true;
    }


    public void ReleaseFromEnemy()
    {
        // you somehow got here without possessing an enemy
        if(!isPossessingEnemy)
        {
            return;
        }

        // reset infection values
        this.isPossessingEnemy = false;
        infectionTarget = null;

        // re-acquire parasite mesh
        UnityEditorInternal.ComponentUtility.CopyComponent(this.DefaultSkinnedMesh);
        UnityEditorInternal.ComponentUtility.PasteComponentValues(gameObject.GetComponent<SkinnedMeshRenderer>());

    }

    public int TakeDamage(int damageAmount)
    {

        if((this.health -= damageAmount) <= 0)
        {
            this.health = 0;
            this.Die();
        }

        // update the player's on-screen health UI
        UIUpdater.UpdateHealthText(this.health);

        return this.health;
    }

    private void Die()
    {

        // dead players can't move or attack.
        this.controlsEnabled = false;

        // play death sound
        this.GetComponent<AudioSource>().PlayOneShot(this.deathSound);

        // not ideal, but temporary to give sense of death.
        Object.Destroy(this.gameObject);
    }

    // add enemies that are close enough to an arraylist
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag.Equals("Enemy"))
        {
            this.EnemiesInRange.Add(collider.gameObject);
        }
    }

    // remove them once they leave the radius
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Enemy"))
        {
            this.EnemiesInRange.Remove(collider.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case "Enemy":
                // collision with the collider of the enemy gameObject
                Debug.Log("Player collision with " + collision.collider.gameObject.name);
                break;
            default:
                // walls, etc.
                break;
        }
    }

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private int health = 100;

    [SerializeField]
    private float rotationSpeed = 200.0f;

    [SerializeField]
    private float infectTimer = 0.0f;

    private Enemy infectionTarget;

    // are we currently possessing an enemy
    public bool isPossessingEnemy = false;

    // if the controls need to be disabled for any reason, this can be turned off.
    private bool controlsEnabled;

    private List<GameObject> EnemiesInRange = new List<GameObject>();

    // used to reset the mesh after infection
    public SkinnedMeshRenderer DefaultSkinnedMesh;

    // sound played on death
    public AudioClip deathSound;
}
