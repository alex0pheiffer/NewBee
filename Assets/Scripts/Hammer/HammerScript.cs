using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerScript : MonoBehaviour
{
    private Animator anim;
    //private CircleCollider2D coll;
    //private bool flipY = false;
    private enum MovementState { idle, wham };

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //coll = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementState state = MovementState.wham;
        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("on trigger enter2d in hammer script");
        if (collision.gameObject.CompareTag("Cat"))
        {
            collision.gameObject.GetComponent<CatMovement>().Stun(true);
        }
    }

    /*public void FlipY(bool flip)
    {
        if (flip != flipY)
        {
            flipY = flip;
            gameObject.transform.Rotate(0f, 180f, 0f, Space.Self);
        }
    }*/
}
