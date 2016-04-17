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
    }

    protected virtual void Awake()
    {
        // Setting up the references.
        nav = this.gameObject.GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {

        /** If not passive, start counting down until becoming passive. **/
        if (stateTimer > 0.0f)
        {
            stateTimer -= Time.deltaTime;
        }
        else if(this.currentState == BehaviorState.Alerted || this.currentState == BehaviorState.Restless)
        {
            this.currentState = BehaviorState.Passive;
            stateTimer = 0.0f;
            playerHeard = false;
            AlarmManager.Get().TurnOffAlarm();
        }

        // Alerted = chasing
        if(this.currentState == BehaviorState.Alerted)
        {
            Chasing();
        }
        // Otherwise patrol.
        else {
            Patrolling();
        }

        if (AlarmManager.Get().isAlarmSounding)
        {

            // I am alert.
            this.currentState = BehaviorState.Alerted;
        }


    }

    void Patrolling()
    {
        if (patrolWayPoints.Length == 0)
        {
            return;
        }

        nav.Resume();

        // Set an appropriate speed for the NavMeshAgent.
        nav.speed = patrolSpeed;

        // If near the next waypoint or there is no destination...
        if (nav.destination == AlarmManager.Get().resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            if (patrolTimer == 0)
            {
                patrolWaitTime = Random.Range(patrolMinWaitTime, patrolMaxWaitTime);
            }

            // ... increment the timer.
            patrolTimer += Time.deltaTime;

            // If the timer exceeds the wait time...
            if (patrolTimer >= patrolWaitTime)
            {
                // ... increment the wayPointIndex.
                if (wayPointIndex == patrolWayPoints.Length - 1)
                {
                    wayPointIndex = 0;
                }
                else {
                    wayPointIndex++;
                }

                // Reset the timer.
                patrolTimer = 0;
            }
        }
        else {
            // If not near a destination, reset the timer.
            patrolTimer = 0;
        }

        // Set the destination to the patrolWayPoint.
        nav.destination = patrolWayPoints[wayPointIndex].position;
    }

    void Chasing()
    {
        nav.Resume();

        // Either use the global alarm position or our own position for the player.
        // Create a vector from the enemy to the last known position of the player.
        Vector3 sightingDeltaPos = AlarmManager.Get().lastPlayerPosition - transform.position;

        // If the the last personal sighting of the player is not close...
        if (sightingDeltaPos.sqrMagnitude > 4f)
        {
            // ... set the destination for the NavMeshAgent to the last personal sighting of the player.
            nav.destination = AlarmManager.Get().lastPlayerPosition;
        }

        // Set the appropriate speed for the NavMeshAgent.
        nav.speed = chaseSpeed;
    }

    // simple method to rotate the transform to look at the player's transform
    protected void LookAtPlayer()
    {
        this.transform.LookAt(this.playerReference.transform);
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
                    // If not Alerted become restless and increase state timer.
                    this.currentState = BehaviorState.Restless;
                    stateTimer = 45.0f;
                }
            }
        }

        // hearing detection
        if (playerReference.GetComponent<PlayerControl>().state == "run")
        {
            if (!playerHeard && this.currentState == BehaviorState.Passive)
            {
                playerHeard = true;

                // If not Alerted become restless and increase state timer.
                this.currentState = BehaviorState.Restless;
                stateTimer += 15.0f;
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

        if (collider.tag.Equals("Player"))
        {
            // set player last spotted
            AlarmManager.Get().lastPlayerPosition = collider.transform.position;
        }
    }

    // the time it takes for the player to infect this enemy
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

    // angle of the vision cone
    public float visionAngle;

    // The nav mesh agent's speed when patrolling.
    public float patrolSpeed = 2f;

    // The nav mesh agent's speed when chasing.			
    public float chaseSpeed = 5f;

    // The amount of time to wait when the patrol way point is reached.			
    public float patrolMinWaitTime = 1f;
    public float patrolMaxWaitTime = 5f;

    // An array of transforms for the patrol route.			
    public Transform[] patrolWayPoints;

    // A timer for the patrolWaitTime.				
    float patrolTimer;

    // A counter for the way point array.				
    int wayPointIndex;

    // wait time between patrol points
    float patrolWaitTime;

    [SerializeField]
    protected float PassiveMaxMovementDelay;

    [SerializeField]
    protected float RestlessMaxMovementDelay;

    // can this enemy move?
    [SerializeField]
    protected bool movementEnabled;

    // can this enemy currently attack
    protected bool attackEnabled = true;

    protected bool playerHeard;

    // Reference to the nav mesh agent.				
    protected NavMeshAgent nav;

    // walking speed of this enemy
    [SerializeField]
    protected float walkingSpeed;

    public BehaviorState currentState;

    public float stateTimer;

    public AudioClip deathSound;
}
