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

    private GameObject targetObj;

    private float dirX = 0;
    [SerializeField] private float horizontalSpeed = 4f;
    private enum MovementState { idle, stun }

    private enum StateState { idle, stun, flower, hive }
    private StateState currentState = StateState.flower;

    private bool onFlower = false;
    private bool onHive = false;

    private float counter = 0f;
    private float honeyWait = 3f;
    private float hiveWait = 5f;

    private int amtHoney = 0;
    private static int maxHoney = 1;        //for testing--put real number later
    private int health = 10;
    private GameObject[] homeList;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        SetTargetFlower();
    }

    void Update()
    {
        // if stun, do nothing
        if (currentState == StateState.stun)
        {
            // TODO check the stun CD
            //currentState = StateState.flower;

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
            counter+= Time.deltaTime;
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
        if (currentState == StateState.flower && (targetObj == null || !IsCurrentTargetType("Flower")))
        {
            SetTargetFlower();
        }
        else if (currentState == StateState.hive && (targetObj == null || !IsCurrentTargetType("Hive")))
        {
            SetTargetHive();
        }


        UpdateAnimationState();
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
        gameObject.GetComponent<AIDestinationSetter>().target = targetObj.transform;
    }

    private void SetTargetHive()
    {
        Debug.Log("Target is set to hive, currentState = " + currentState);
        targetObj = GameObject.FindWithTag("Hive");
        // set the target
        gameObject.GetComponent<AIDestinationSetter>().target = targetObj.transform;
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

    public void CollidedWithFlower()
    {
        onFlower = true;
        Debug.Log("In CollidedWithFlower, , currentState = " + currentState);

        // set the target to null
        targetObj = null;
        // set the target to null
        gameObject.GetComponent<AIDestinationSetter>().target = null;
        // set state to idle
        currentState = StateState.idle;
    }

    public void CollidedWithHive()
    {
        onHive = true;
        Debug.Log("In CollidedWithHive, , currentState = " + currentState);

        // set the target to null
        targetObj = null;
        // set the target to null
        gameObject.GetComponent<AIDestinationSetter>().target = null;
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
}