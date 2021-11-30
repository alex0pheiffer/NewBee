using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerScript : MonoBehaviour
{
    private Animator anim;

    private bool flipY = false;
    private enum MovementState { idle, wham };

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementState state = MovementState.wham;
        anim.SetInteger("state", (int)state);
    }

    public void FlipY(bool flip)
    {
        if (flip != flipY)
        {
            flipY = flip;
            gameObject.transform.Rotate(0f, 180f, 0f, Space.Self);
        }
    }
}
