using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anime;
    private Rigidbody2D body;

    [SerializeField] private AudioSource deathsound;
    private void Start()
    {
        anime = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    public void Death()
    {
        deathsound.Play();
        anime.SetTrigger("death");
        body.bodyType = RigidbodyType2D.Static;
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
}
