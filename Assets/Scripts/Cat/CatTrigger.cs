using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatTrigger : MonoBehaviour
{
    [SerializeField] private CatMovement cat;


    // the trigger box is the vision circle
    private void OnTriggerStay2D(Collider2D collision)
    {
        cat.TriggerStay(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cat.TriggerExit(collision);
    }
}
