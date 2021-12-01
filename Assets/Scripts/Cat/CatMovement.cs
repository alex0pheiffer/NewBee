using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


// Cat Movement
//
// Cats must see their target in order to start path-finding.
// When a cat is stunned, their pathfinding is interrupted and canceled.
public class CatMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask solidGround;

    private float dirX = 0;
    [SerializeField] private float horizontalSpeed = 4f;
    
    // TODO we still need a run animation sprite
    private enum MovementState { idle, run, jump, wham, stun }

    private bool isStun = false;
    private bool isWham = false;
    public bool endWham = false;

    private float cooldown = 0;
    private const float stunCooldown = 4f;
    private const float whamCooldown = 2f;

    private Seeker seeker;
    private Path path;
    [SerializeField] float nextWaypointDistance = 0.3f;
    private GameObject targetObj = null;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (targetObj != null)
            seeker.StartPath(rb.position, targetObj.transform.position, OnPathComplete);

        InvokeRepeating("FindPath", 0f, 0.5f);
    }

    private void FindPath()
    {
        if (targetObj != null && !isStun && !reachedEndOfPath)
        {
            seeker.StartPath(rb.position, targetObj.transform.position, OnPathComplete);
        }
    }

    void Update()
    {
        // if stun, do nothing
        if (isStun)
        {
            // subtract from the stun cooldown
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                cooldown = 0;
                isStun = false;
            }

            // if we're still stunned, do nothing
            if (isStun) return;
        }
        // when the wham animation is finished...
        else if (cooldown > 0)
        {
            if (endWham)
            {
                Debug.Log("endwham");
                endWham = false;
                isWham = false;
            }

            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                cooldown = 0;
                Debug.Log("wham cooldown finished");
            }
        }

        // don't move along a path if we have no path
        if (path == null)
        {
            // TODO move randomly
            return;
        }
        else if (reachedEndOfPath && cooldown == 0)
        {
            // don't wham the bee if we have a cooldown
            WhamBee();
        }

        // check if the path is complete
        if (path != null && currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }


        if (path != null)
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
        


        // TODO add in its collision with respective home

        // homing movement will hopefully be controlled by unity navigation

        // cats can only jump if IsGrounded() ((they cannot air jump))

        UpdateAnimationState();
    }

    public bool Stun(bool stun)
    {
        isStun = stun;
        if (isStun)
        {
            // start the stun cooldown
            cooldown = stunCooldown;
            // you cannot be in a whamming state if stunned
            if (isWham) isWham = false;
        }
        else
        {
            cooldown = 0;
        }

        // TODO return if the stunning/unstunning was successful
        return true;
    }

    private void UpdateAnimationState()
    {
        MovementState state;


        // presently the run and idle sprite for bee is the same (b/c it is flying)
        if (dirX > 0f)
        {
            state = MovementState.run;
            sprite.flipX = true;
        }
        else if (dirX < 0f)
        {
            state = MovementState.run;
            sprite.flipX = false;
        }
        else
        {
            state = MovementState.idle;
        }

        // if an upward force is being supplied (we're not on the ground)
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -.1f)
        {
            // cat's do not have a fall sprite at this time, so it is the same as jump
            state = MovementState.jump;
        }

        // if we're whamming
        if (isWham) state = MovementState.wham;

        // if we're stunned
        if (isStun) state = MovementState.stun;

        anim.SetInteger("state", (int)state);
    }
    private bool IsGrounded()
    {
        // return if .1f below us is overlapping with jumpable ground
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, solidGround);
    }

    // the trigger box is the vision circle
    private void OnTriggerStay2D(Collider2D collision)
    {

        // do nothing if we're stunned
        if (isStun) return;
        // do nothing if we're currently chasing a bee
        if (targetObj != null) return;

        // if you see a bee, chase
        if (collision.gameObject.CompareTag("Bee"))
        {
            Debug.Log("Cat sees bee");

            targetObj = collision.gameObject;
            seeker.StartPath(rb.position, targetObj.transform.position, OnPathComplete);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // do nothing if we're stunned
        if (isStun) return;
        
        // if the creature exiting is a bee, and specifically the bee you're chasing
        if (collision.gameObject.CompareTag("Bee") && collision.gameObject == targetObj)
        {
            Debug.Log("Cat saw bee");

            targetObj = null;
            path = null;
        }
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

    private void WhamBee()
    {
        // wham the bee
        isWham = true;
        path = null;
        targetObj.GetComponent<BeeMovement>().Stun(true);
        targetObj = null;
        cooldown = whamCooldown;
        Debug.Log("Reached bee");
    }
}
