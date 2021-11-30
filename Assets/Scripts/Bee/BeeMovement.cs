﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    //private GameObject target;
    private Transform target;
    private GameObject targetObj;

    private float dirX = 0;
    [SerializeField] private float horizontalSpeed = 4f;
    private enum MovementState { idle, stun }

    private enum StateState { stun, flower, hive }
    private StateState currentState = StateState.flower;

    //private bool isStun = false;
    //private bool homeFlower = false;
    //private bool homeHive = false;
    private bool onFlower = false;
    private bool onHive = false;

    private float counter = 0f;
    private float honeyWait = 5f;

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

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

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

        if (onFlower)
        {
            counter += Time.deltaTime;
            if (counter >= honeyWait)
            {
                onFlower = false;
                counter = 0;
                amtHoney += 1;
            }
        }

        // TODO:
        // if at flower and amtHoney >= maxHoney: homeHive = true
        // else if at flower: increment amtHoney based on last time checked [done in Flower.cs]
        // if at hive and amtHoney >= maxHoney: amtHoney = 0; honeycount++ [done in BeehiveScript.cs]
        // else if at hive: homeFlower = true [done in SetHoneyEmpty, called by BeehiveScript.cs]
        if (amtHoney >= maxHoney)
        {
            Debug.Log("amtHoney = " + amtHoney + ", maxHoney = " + maxHoney);
            currentState = StateState.hive;
        }


        // if needed, set new state of homing
        if (currentState == StateState.flower && !IsCurrentTargetType("Flower"))
        {
            SetTargetFlower();
        }
        else if (currentState == StateState.hive)
        {
            SetTargetHive();
        }

        // keep moving to target
        agent.SetDestination(target.position);

        // TODO add in its collision with respective home

        // homing movement will hoepfully be controlled by unity navigation

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
        Debug.Log("Target is set to flower");
        GameObject[] homeList = GameObject.FindGameObjectsWithTag("Flower");
        //must have the above line in this function and not start function... idk why
        int randIndex = Random.Range(0, homeList.Length);
        targetObj = homeList[randIndex];
        target = targetObj.transform;
        currentState = StateState.flower; //TODO: CHECK IF WE NEED THIS HERE OR ELSEWHERE?????
    }

    private void SetTargetHive()
    {
        Debug.Log("Target is set to hive");
        targetObj = GameObject.FindWithTag("Hive");
        target = targetObj.transform;
        //used to set HomeHive to false here which was glitching
    }

    public void SetHoneyEmpty()
    {
        amtHoney = 0;
        currentState = StateState.flower;
    }

    /*public void CollectHoney()
    {
        amtHoney += 1;          //increment based on time, time is in [Flower.cs]
    }*/

    private bool IsCurrentTargetType(string s)
    {
        return targetObj.CompareTag(s);
    }

    public bool IsCurrentTarget(GameObject ob)
    {
        return ob.transform == target;
    }

    public void CollidedWithFlower()
    {
        onFlower = true;
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