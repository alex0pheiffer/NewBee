using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// When the player collides with this ground, they will regain their jump ability
public class JumpGround : MonoBehaviour
{
    // When a player hits this ground, let them be able to jump
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().SetJumpable(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}