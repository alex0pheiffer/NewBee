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

    private float dirX = 0;
    [SerializeField] private float horizontalSpeed = 4f;
    private enum MovementState { idle, stun }

    private bool isStun = false;
    private bool homeFlower = false;
    private bool homeHive = false;
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

        setTargetFlower();
    }

    void Update()
    {
        // if stun, do nothing
        if (isStun)
        {
            // TODO check the stun CD
            isStun = true;

            // if we're still stunned, do nothing
            if (isStun) return;
        }

        // TODO keep track of amt of honey + add timer for this
        // TODO if bee has collected enough honey, set homeHive = true
        

        // if needed, set new state of homing
        if (homeFlower)
        {
           setTargetFlower();
        }
        else if (homeHive)
        {
            setTargetHive();
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
        if (isStun) state = MovementState.stun;

        anim.SetInteger("state", (int)state);
    }


    private void setTargetFlower() 
    {
        GameObject[] homeList = GameObject.FindGameObjectsWithTag("Flower");
        //must have the above line in this function and not start function... idk why
        int randIndex = Random.Range(0, homeList.Length);
        target = homeList[randIndex].transform;
    }

    private void setTargetHive()
    {
        target = GameObject.FindWithTag("Hive").transform;
    }

}