using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private enum MovementState { idle, run, jump, stun, wham }

    private bool isStun = false;
    private bool isWham = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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

        // TODO add in the state of homing (a bee, if in sight (we will need to make a radius of sight); otherwise random movement)

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
            // you cannot be in a whamming state if stunned
            if (isWham) isWham = false;
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
}
