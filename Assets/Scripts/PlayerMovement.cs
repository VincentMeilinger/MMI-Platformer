using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator anime;
    private SpriteRenderer sprite;
    private BoxCollider2D collider;
    private float dirX = 0f;
    private KeywordRecognizer m_Recognizer;
    private VoiceCommand command;

    [SerializeField] 
    private LayerMask Ground;

    [SerializeField] 
    private float jumpheight = 15f;

    [SerializeField] 
    private float movespeed = 18f;
    
    [SerializeField] 
    private AudioSource jumpsound;

    [SerializeField]
    private string[] m_Keywords;

    private enum MoveState 
    { 
        idle,running,jumping,falling
    }
    private enum VoiceCommand
    { 
        none, up, down
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();    
        collider = GetComponent<BoxCollider2D>(); 

        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();  
    }
    
    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movespeed, rb.velocity.y);

        if ((Input.GetButtonDown("Jump") || command == VoiceCommand.up) && Groundcheck())
        {
            jumpsound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpheight);
            command = VoiceCommand.none;
        }
        Updateanime();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        if (args.text == "jump" || args.text == "up") 
        {
            command = VoiceCommand.up;
        } 
        else if (args.text == "duck" || args.text == "down") 
        {
            command = VoiceCommand.down;
        }
    }

    private void Updateanime()
    {
        //Debug.Log("dirX value: " + dirX.ToString());

        MoveState state;

        if (dirX > 0f)
        {
            state = MoveState.running;
            sprite.flipX = false;
        } 
        else if (dirX < 0f)
        {
            state = MoveState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MoveState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MoveState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MoveState.falling;
        }

        anime.SetInteger("state",(int)state);

    }

    private bool Groundcheck()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, Ground);
    }
}
