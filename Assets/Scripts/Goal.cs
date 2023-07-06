using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private int nextLevel = 1;

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
            NextLevel();
        }
    }

    public void NextLevel() {
        SceneManager.LoadScene("Level_" + nextLevel);
    }
}

