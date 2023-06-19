using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private AudioSource goalsound;
    private bool won;
    private void Start()
    {
        goalsound = GetComponent<AudioSource>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !won)
        {
            goalsound.Play();
            won = true;
        }
    }

    private void CompleteLevel()
    {

    }
    
}
