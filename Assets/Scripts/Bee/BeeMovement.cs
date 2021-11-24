using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private float dirX = 0;
    [SerializeField] private float horizontalSpeed = 4f;
    private enum MovementState { idle, stun }

    private bool isStun = false;

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

        // TODO add in the state of homing (plant, hive)

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
}
