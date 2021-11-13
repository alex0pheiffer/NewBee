using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private AudioSource deathSound;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        // show death animation
        anim.SetTrigger("death");
        // stop the player from moving
        rb.bodyType = RigidbodyType2D.Static;
        // stop the bgm
        GameManager.instance.BGM.GetComponent<AudioSource>().Pause();
        // start sfx
        deathSound.Play();
    }

    // this is called by the death ANIMATION attached to the player object
    private void EndGame()
    {
        // todo replace this with the high score scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
