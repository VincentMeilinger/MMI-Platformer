using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Collector : MonoBehaviour
{

    [SerializeField] private AudioSource collectsound;
    [SerializeField] private Text counter;
    private int melons = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Melon"))
        {
            collectsound.Play();
            Destroy(collision.gameObject);
            melons++;
            counter.text = "Melons: " + melons;
        }
    }
}
