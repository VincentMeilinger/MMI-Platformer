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
    private bool isJumping = false;

    float smoothTime = .1f;
    float velocitySmoothing;
    

    [SerializeField] 
    private LayerMask Ground;

    [SerializeField] 
    private float jumpheight = 14f;

    [SerializeField] 
    private float movespeed = 14f;

    [SerializeField]
    float jumpBoost = 1.2f;

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

        m_Recognizer = new KeywordRecognizer(m_Keywords, ConfidenceLevel.Low);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();  
    }

    private void OnDestroy() {
        m_Recognizer.Stop();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = new Vector2(
            Mathf.SmoothDamp(rb.velocity.x , dirX * movespeed, ref velocitySmoothing, smoothTime), 
            rb.velocity.y);
        rb.velocity = velocity;
        if (Groundcheck()) {
            isJumping = false;
        }
        if ((Input.GetButtonDown("Jump") || // UP
            Input.GetAxisRaw("Vertical") > 0) && 
            Groundcheck())
        {
            jumpsound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpheight);
            isJumping = true;
        }
        if (command == VoiceCommand.up && // VOICE UP
            isJumping) {
            jumpsound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpBoost * jumpheight);
        }
        if (Input.GetAxisRaw("Vertical") < 0 ||
            command == VoiceCommand.down) { // DOWN + VOICE DOWN
            rb.velocity = new Vector2(rb.velocity.x, -jumpheight);
            
        }
        command = VoiceCommand.none;
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
        else if (args.text == "slam" || args.text == "down") 
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
