﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private float dirX = 0;
    [SerializeField] private float horizontalSpeed = 4f;
    [SerializeField] private float jumpForce = 4f;

    private enum MovementState { idle, run, jump, fall }

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource landSound;

    // [FALSE] jump off ground only (default) or [TRUE] once in a sequence (tfm)
    private const bool USE_SINGLEJUMP = true;
    private int jumpCounter = 0; // a counter in case you want to implement double-jumping
    [SerializeField] private int maxJumps = 1;
    private float timeSinceLastJump = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ///////////// MOVEMENT

        // you can use .GetAxis instead of .GetAxisRaw to slide a bit after releasing the a/d keys
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalSpeed * dirX, rb.velocity.y);

        /*
        // check if you need to clear the jump counter
        if (jumpCounter >= maxJumps && IsGrounded() && Time.time - timeSinceLastJump > 0.05f)
        {

            // todo this shouldnt be true if you hit the bottom of the ground.

            // reset the jump counter
            jumpCounter = 0;
            // play the land sound
            landSound.Play();
        }
        */
        if (Input.GetButtonDown("Jump"))
        {
            if (USE_SINGLEJUMP && jumpCounter < maxJumps)
            {
                // jump
                rb.velocity = new Vector2(0, jumpForce);
                // play the sound
                jumpSound.Play();
                // set jump counter
                jumpCounter++;
                timeSinceLastJump = Time.time;
            }
        }


        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.run;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.run;
            sprite.flipX = true;
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
            state = MovementState.fall;
        }

        anim.SetInteger("state", (int)state);
    }

    public void SetJumpable(bool canJump)
    {
        if (jumpCounter < maxJumps) return;
        if (Time.time - timeSinceLastJump < 0.05f) return;
        // reset the jump counter
        if (canJump)
            jumpCounter = 0;
        else
            jumpCounter = maxJumps;
    }
}
