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
    }

    protected virtual void Update()
    {

        /** Looking at player */
        // TODO: Add conditions to this later, such as line of sight.
        this.LookAtPlayer();

        /** Attacking */
        if (this.CanAttack())
        {
            this.Attack();
        }
        else if (this.IsAttackOnCooldown())
        {
            this.internalAttackTimer -= Time.deltaTime;
        }
    }

    // virtual baseline move method 
    public virtual void Move()
    {

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

    protected void LookAtPlayer()
    {
        this.transform.LookAt(this.playerReference.transform);
    }

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

    // internal timer which will count down;
    // if == 0, an attack can be performed.
    protected float internalAttackTimer;

    // array of possible attacks that this enemy can make
    protected Attack[] arrayOfAttacks;

}
