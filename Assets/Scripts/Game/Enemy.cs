using UnityEngine;
using System.Collections;

public abstract class Enemy : BasicObject {

    public enum BehaviorState
    {
        Alerted, // player's presence is known
        Passive, // player has not been spotted
        Restless // player not necessarily spotted, but myabe sketchy things were heard, seen, etc.
    }

    protected new virtual void Start()
    {
        base.Start();
        internalAttackTimer = 0.0f;
        this.currentState = BehaviorState.Passive;
        this.arrayOfAttacks = this.gameObject.GetComponents<Attack>();
        this.destinationPathNode = PathNodeLibrary.GetNearestPathNodeFromLocation(this.gameObject.transform.position);
        this.visionRadius = this.GetComponent<SphereCollider>().radius;
    }

    protected virtual void Update()
    {

        /** Attacking */
        if (this.CanAttack())
        {
            this.Attack();
        }
        else if (this.IsAttackOnCooldown())
        {
            this.internalAttackTimer -= Time.deltaTime;
        }

        /** Moving **/
        if (this.CanMove())
        {
            this.Move();
        }
        else
        {
            this.moveDelay -= Time.deltaTime;
        }

        /** If not passive, start counting down until becoming passive. **/
        if (stateTimer > 0.0f)
        {
            stateTimer -= Time.deltaTime;
        }
        else if(this.currentState == BehaviorState.Alerted || this.currentState == BehaviorState.Restless)
        {
            this.currentState = BehaviorState.Passive;
            stateTimer = 0.0f;
        }
    }

    // virtual baseline move method 
    public virtual void Move()
    {
        float step = Time.deltaTime * this.walkingSpeed;

        switch (this.currentState) {
            case BehaviorState.Passive:
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.destinationPathNode.transform.position, step);
                break;
            case BehaviorState.Alerted:
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.playerReference.transform.position, step);
                break;
            case BehaviorState.Restless:
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.destinationPathNode.transform.position, step);
                break;
        }
    }

    // virtual baseline attack method 
    public virtual void Attack()
    {

        // first select an attack 
        Attack attackToMake = this.SelectAttack();

        // cancel attack if selected attack is out of range
        if (!attackToMake.IsAttackInRange(this.gameObject, this.playerReference))
        {
            return; 
        }

        /** Attack is in range, let's attack */
        {

            /** Play enemy's attack animation here */

            if(attackToMake.attackType == global::Attack.RangeType.Melee)
            {
                // Melee attacks have no projectile, and thus presumably cannot be avoided if you are within range.
                // CAN BE CHANGED
                this.playerReference.GetComponent<Player>().TakeDamage(attackToMake.damage);

            } else if(attackToMake.attackType == global::Attack.RangeType.Ranged)
            {
                // creates the attack's prefab projectile and adds the attack's force vector as relative force.
                // Note: The damage for RangeType.Ranged attacks is accounted for in the player's collision handling for projectiles.
                // TODO: simple implementation for now, would like to add more variety to projectile attacks in the future.
                GameObject projectileFired = (GameObject)Instantiate(attackToMake.AttackPrefab, this.gameObject.transform.position, Quaternion.identity);
                projectileFired.GetComponent<Rigidbody>().AddForce((this.gameObject.transform.forward + attackToMake.adjustmentVector) * attackToMake.speed);
            } else
            {
                // something is horribly wrong
            }

            /** Attack completed, add attack's delay to internal timer */
            // Note: using '=' to overwrite any "over reduction" that might have occured while Update() was reducing timer.
            this.internalAttackTimer = attackToMake.delay;

            /** Play attacking sound **/
            gameObject.GetComponent<AudioSource>().PlayOneShot(attackToMake.attackSound);

        }
    }

    // simple method to rotate the transform to look at the player's transform
    protected void LookAtPlayer()
    {
        this.transform.LookAt(this.playerReference.transform);
    }

    // returns true if this enemy has attacked recently, false otherwise.
    protected bool IsAttackOnCooldown()
    {
        return this.internalAttackTimer > 0.0f;
    }

    // interfacing function call to determine if this enemy can attack
    protected bool CanAttack()
    {
        return !IsAttackOnCooldown() && // has an attack been made recently
            this.attackEnabled && // not being infected
            arrayOfAttacks.Length > 0; // Do I have any possible attacks to make
    }

    // randomly select an attack to perform from array
    protected Attack SelectAttack()
    {
        return arrayOfAttacks[Random.Range(0, arrayOfAttacks.Length)];
    }

    // simple health reduction
    public int TakeDamage(int damageAmount)
    {

        if ((this.health -= damageAmount) <= 0)
        {
            this.health = 0;
            this.Die();
        }

        return this.health;
    }

    public void EnterInfectionStasis()
    {

        // probably start flailing and doing panic animations

        // disable movement and attacks
        this.movementEnabled = false;
        this.attackEnabled = false;

    }

    public void ReleaseFromInfectionStasis()
    {

        // reenable movement and attacks
        this.movementEnabled = true;
        this.attackEnabled = true;

    }

    protected virtual void Die()
    {
        /** Play death animation **/

        /** Play death sound **/
        gameObject.GetComponent<AudioSource>().PlayOneShot(this.deathSound);

        // disable movement and attacks
        this.movementEnabled = false;
        this.attackEnabled = false;
    }

    // returns true if this enemy is alive, false otherwise.
    public bool IsDead()
    {
        return this.health <= 0;
    }

    // calculates and returns if this enemy can move
    protected bool CanMove()
    {
        return movementEnabled && moveDelay <= 0.0f;
    }

    // called when the enemy reaches its destination path node from moving
    public void ReachedPathNode()
    {
        switch (this.currentState) {
            case BehaviorState.Passive:
            case BehaviorState.Alerted:
            this.moveDelay += Random.Range(this.PassiveMaxMovementDelay / 2.0f, this.PassiveMaxMovementDelay);
                break;
            case BehaviorState.Restless:
                this.moveDelay += Random.Range(this.RestlessMaxMovementDelay / 2.0f, this.RestlessMaxMovementDelay);
                break;
        }
    }

    // check things that enter vision
    void OnTriggerEnter(Collider collider)
    {

        // Create a vector from the enemy to the player and store the angle between it and forward.
        Vector3 direction = transform.position - collider.transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        // If the angle between forward and where the object is, is less than half the angle of view...
        if (angle < this.visionAngle * 0.5f)
        {
            // Object in view is player
            if (collider.tag.Equals("Player"))
            {
                // The player is in sight.
                if (this.currentState != BehaviorState.Alerted)
                {
                    // become alerted and increase state timer.
                    this.currentState = BehaviorState.Alerted;
                    stateTimer = 20.0f;
                }
            }

            // Object in view is another enemy
            else if (collider.tag.Equals("Enemy"))
            {
                // dead body checking
                if(collider.gameObject.GetComponent<Enemy>().IsDead() && this.currentState != BehaviorState.Alerted)
                {
                    // If not Alerted become restless  and increase state timer.
                    this.currentState = BehaviorState.Restless;
                    stateTimer = 45.0f;
                }
            }
        }
 
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            /** Player in vision **/
            if (this.currentState == BehaviorState.Alerted)
            {
                this.LookAtPlayer();
                stateTimer = 20.0f;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        
    }

    public float timeToInfect;

    // internal timer which will count down;
    // if == 0, an attack can be performed.
    protected float internalAttackTimer;

    // health of this enemy
    [SerializeField]
    protected int health;

    // a random value that gets set after moving to delay random movement 
    [SerializeField]
    protected float moveDelay;

    public float visionAngle;

    private float visionRadius;

    [SerializeField]
    protected float PassiveMaxMovementDelay;

    [SerializeField]
    protected float RestlessMaxMovementDelay;

    // can this enemy move?
    [SerializeField]
    protected bool movementEnabled;

    // can this enemy currently attack
    protected bool attackEnabled = true;

    // array of possible attacks that this enemy can make
    protected Attack[] arrayOfAttacks;

    // path node to walk to next when deciding to move
    public PathNode destinationPathNode;

    // walking speed of this enemy
    [SerializeField]
    protected float walkingSpeed;

    public BehaviorState currentState;

    public float stateTimer;

    public AudioClip deathSound;
}
