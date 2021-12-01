using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    
    private AudioSource finishSound;
    private bool ended = false;

    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameManager.instance.GetHoneyJars() >= 12 && !ended)    // TODO: will need to change if honey jars needed changes
        {
            Debug.Log("HONEYJARS >= 2 IN FINLEVEL");
            //GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GameManager.instance.BGM.GetComponent<AudioSource>().Pause();
            finishSound.Play();
            Invoke("CompleteLevel", 5.812f);
            ended = true;
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // stop the player from moving
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            // stop BGM
            GameManager.instance.BGM.GetComponent<AudioSource>().Pause();
            finishSound.Play();
            Invoke("CompleteLevel", 5.812f);
        }
    }*/

    private void CompleteLevel()
    {
        SceneManager.LoadScene(5);
    }
}
