using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Bee")
            && collision.gameObject.GetComponent<BeeMovement>().IsCurrentTarget(gameObject)
            && !collision.gameObject.GetComponent<BeeMovement>().IsOnFlower())
        {
            //WaitAtFlower();
            collision.gameObject.GetComponent<BeeMovement>().CollidedWithFlower();
        }
    }

    /*private IEnumerator WaitAtFlower()
    {
        Debug.Log("Started Coroutine at timestamp: " + Time.time);
        yield return new WaitForSeconds(5);
        Debug.Log("Ended Coroutine at timestamp: " + Time.time);
    }*/
}
