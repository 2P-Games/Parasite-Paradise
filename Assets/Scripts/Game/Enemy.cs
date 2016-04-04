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
        this.arrayOfAttacks = this.gameObject.GetComponents<Attack>();
        this.destinationPathNode = PathNodeLibrary.GetNearestPathNodeFromLocation(this.gameObject.transform.position);
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
        if(this.CanMove())
        {
            this.Move();
        } else
        {
            this.moveDelay -= Time.deltaTime;
        }
    }

    // virtual baseline move method 
    public virtual void Move()
    {

        float step = Time.deltaTime * this.walkingSpeed;
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.destinationPathNode.transform.position, step);

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

    protected virtual void Die()
    {
        /** Play death animation **/

        /** Play death sound **/

        // etc.
    }

    // calculates and returns if this enemy can move
    protected bool CanMove()
    {
        return movementEnabled && moveDelay <= 0.0f;
    }

    // called when the enemy reaches its destination path node from moving
    public void ReachedPathNode()
    {
        this.moveDelay += Random.Range(this.MAXIMUM_MOVEMENT_DELAY / 2.0f, this.MAXIMUM_MOVEMENT_DELAY);
    }

    // fired when something remains within a collider (designated as a trigger)
    protected virtual void OnTriggerStay(Collider collider)
    {
        // all enemies look at the player when the player enters the trigger AOE
        if(collider.tag.Equals("Player"))
        {
            this.LookAtPlayer();
        }
    }

    // internal timer which will count down;
    // if == 0, an attack can be performed.
    protected float internalAttackTimer;

    // health of this enemy
    [SerializeField]
    protected int health;

    // a random value that gets set after moving to delay random movement 
    [SerializeField]
    protected float moveDelay;

    [SerializeField]
    protected float MAXIMUM_MOVEMENT_DELAY;

    // can this enemy move at all?
    [SerializeField]
    protected bool movementEnabled;

    // array of possible attacks that this enemy can make
    protected Attack[] arrayOfAttacks;

    // path node to walk to next when deciding to move
    public PathNode destinationPathNode;

    // walking speed of this enemy
    [SerializeField]
    protected float walkingSpeed;
}
