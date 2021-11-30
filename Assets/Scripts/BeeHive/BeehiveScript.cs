using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehiveScript : MonoBehaviour
{
    public GameManager gameManager;

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
            && collision.gameObject.GetComponent<BeeMovement>().IsCurrentTarget(gameObject))
        {
            gameManager.addHoneyJars();
            collision.gameObject.GetComponent<BeeMovement>().SetHoneyEmpty();
        }
    }

}
