using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	private Animator m_Animator;

    protected virtual void Start()
    {
        this.controlsEnabled = true;
        UIUpdater.UpdateHealthBar(this.health);
    }

    protected virtual void Awake()
	{
		m_Animator = this.gameObject.GetComponent<Animator>();
	}

    // Update is called once per frame
    protected virtual void Update () {

        // attempt to acquire an enemy target once
        if(Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {

            // if no enemies are close enough or is already infecting an enemy, cannot possess new target.
            if(EnemiesInRange.Count == 0 || isPossessingEnemy)
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
        if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.Joystick1Button7))
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

        if(Input.GetKeyUp(KeyCode.I) || Input.GetKeyUp(KeyCode.Joystick1Button7))
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

        // disable enemy components
        targetEnemy.GetComponent<Enemy>().enabled = false;
        targetEnemy.GetComponent<EnemyAnimation>().enabled = false;

        // copy this player script to enemy
        enemyObject.AddComponent<PlayerControl>();
        UnityEditorInternal.ComponentUtility.CopyComponent(this);
        UnityEditorInternal.ComponentUtility.PasteComponentValues(enemyObject.GetComponent<PlayerControl>());

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

        // if player is not already dead and damage kills player, player dies.
        if (this.health != 0)
        {
            if ((this.health -= damageAmount) <= 0)
            {
                this.health = 0;
                this.Die();
            }
        }

        // update the player's on-screen health UI
        UIUpdater.UpdateHealthBar(this.health);

        return this.health;
    }

    private void Die()
    {

        // dead players can't move or attack.
        this.controlsEnabled = false;

        // play death sound
        this.GetComponent<AudioSource>().PlayOneShot(this.deathSound);

        // not ideal, but temporary to give sense of death.
       // Object.Destroy(this.gameObject);
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
                //this.TakeDamage(50);
                break;
            default:
                // walls, etc.
                break;
        }
    }

    [SerializeField]
    protected int health = 100;

    private float infectTimer = 0.0f;

    private Enemy infectionTarget;

    // are we currently possessing an enemy
    public bool isPossessingEnemy = false;

    // if the controls need to be disabled for any reason, this can be turned off.
    protected bool controlsEnabled;

    private List<GameObject> EnemiesInRange = new List<GameObject>();

    // used to reset the mesh after infection
    public SkinnedMeshRenderer DefaultSkinnedMesh;

    // sound played on death
    public AudioClip deathSound;
}
