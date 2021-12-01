using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class BeeMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private Seeker seeker;
    private Path path;
    [SerializeField] float nextWaypointDistance = 0.3f;
    private GameObject targetObj = null;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    private float dirX = 0;
    [SerializeField] private float horizontalSpeed = 4f;
    private enum MovementState { idle, stun }

    private enum StateState { idle, stun, flower, hive }
    private StateState currentState = StateState.flower;
    private StateState previousState = StateState.idle;

    private bool onFlower = false;
    private bool onHive = false;

    private float counter = 0f;
    private float honeyWait = 3f;
    private float hiveWait = 5f;
    private const float stunCooldown = 4f;

    private int amtHoney = 0;
    private static int maxHoney = 1;        //for testing--put real number later
    private int health = 10;
    private GameObject[] homeList;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        UpdateAnimationState();

        // if stun, do nothing
        if (currentState == StateState.stun)
        {
            // subtract from the stun cooldown
            counter -= Time.deltaTime;
            Debug.Log("stunned state");
            if (counter <= 0)
            {
                counter = 0;
                currentState = previousState;
                previousState = StateState.idle;
                // stop our stun path
                path = null;
            }

            // if we're still stunned, do nothing
            if (currentState == StateState.stun) return;
        }

        // TODO:
        // if at flower and amtHoney >= maxHoney: homeHive = true
        // else if at flower: increment amtHoney based on last time checked [done in Flower.cs]
        // if at hive and amtHoney >= maxHoney: amtHoney = 0; honeycount++ [done in BeehiveScript.cs]
        // else if at hive: homeFlower = true [done in SetHoneyEmpty, called by BeehiveScript.cs]
        
        
        if (onFlower)
        {
            counter += Time.deltaTime;
            if (counter >= honeyWait)
            {
                counter = 0;
                amtHoney += 1;
                Debug.Log("Added 1 honey, currentState = " + currentState);
            }

            if (amtHoney >= maxHoney)
            {
                Debug.Log("amtHoney = " + amtHoney + ", maxHoney = " + maxHoney);
                currentState = StateState.hive;
                onFlower = false;
            }
        }
        else if (onHive)
        {
            counter += Time.deltaTime;
            if (counter >= hiveWait)
            {
                counter = 0;
                SetHoneyEmpty();
                Debug.Log("Waited enough at hive, currentState = " + currentState);
                onHive = false;
                currentState = StateState.flower;
            }
        }

        // if needed, set new state of homing
        if (currentState == StateState.flower && path == null)
        {
            Debug.Log("set target flower");
            SetTargetFlower();
            return;
        }
        else if (currentState == StateState.hive && path == null)
        {
            Debug.Log("set target hive");
            SetTargetHive();
            return;
        }

        /*

        // if we have a path, follow it
        if (path != null && path.vectorPath != null && currentWaypoint < path.vectorPath.Count)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * horizontalSpeed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
        */

    }

    private void UpdateAnimationState()
    {
        MovementState state;


        // presently the run and idle sprite for bee is the same (b/c it is flying)
        if (dirX > 0f)
        {
            state = MovementState.idle;
            sprite.flipX = true;
        }
        else if (dirX < 0f)
        {
            state = MovementState.idle;
            sprite.flipX = false;
        }
        else
        {
            state = MovementState.idle;
        }

        // if we're stunned
        if (currentState == StateState.stun) state = MovementState.stun;

        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Cat"))
        {
            health -= 1;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // show death animation
        anim.SetTrigger("death");          //TODO: make sure that this renders the sprite correctly
        // stop the player from moving
        // rb.bodyType = RigidbodyType2D.Static;
        // start sfx
        // deathSound.Play();
    }

    private void SetTargetFlower() 
    {
        Debug.Log("Target is set to flower, currentState = " + currentState);
        GameObject[] homeList = GameObject.FindGameObjectsWithTag("Flower");
        //must have the above line in this function and not start function... idk why
        int randIndex = Random.Range(0, homeList.Length);
        targetObj = homeList[randIndex];
        // set the target
        seeker.StartPath(rb.position, targetObj.transform.position, OnPathComplete);
    }

    private void SetTargetHive()
    {
        Debug.Log("Target is set to hive, currentState = " + currentState);
        targetObj = GameObject.FindWithTag("Hive");
        // set the target
        seeker.StartPath(rb.position, targetObj.transform.position, OnPathComplete);
    }

    private void SetHoneyEmpty()
    {
        amtHoney = 0;
        //currentState = StateState.flower;         //TODO: IS THIS NEEDED HERE?
    }

    private bool IsCurrentTargetType(string s) //Compares object tag
    {
        return targetObj.CompareTag(s);
    }

    public bool IsCurrentTarget(GameObject ob) //Compares references
    {
        return ob.transform == targetObj.transform;
    }

    public bool Stun(bool stun)
    {
        if (stun)
        {
            // set the state
            previousState = currentState;
            currentState = StateState.stun;
            // start the stun cooldown
            counter = stunCooldown;
            // stop movement
            seeker.StartPath(rb.position, rb.position, OnPathComplete);

            Debug.Log("stunning bee...");
            return true;
        }

        return false;
    }

    public void CollidedWithFlower()
    {
        onFlower = true;
        Debug.Log("In CollidedWithFlower, , currentState = " + currentState);

        // set the path and target to null
        targetObj = null;
        path = null;
        // set state to idle
        currentState = StateState.idle;
    }

    public void CollidedWithHive()
    {
        onHive = true;
        Debug.Log("In CollidedWithHive, , currentState = " + currentState);

        // set the path and target to null
        targetObj = null;
        path = null;
        // set state to idle
        currentState = StateState.idle;
    }

    public bool IsOnFlower()
    {
        return onFlower;
    }

    public bool IsOnHive()
    {
        return onHive;
    }

    // this is called when a path has been created to the target, and then sets the path
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}