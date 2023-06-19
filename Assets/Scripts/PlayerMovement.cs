using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator anime;
    private SpriteRenderer sprite;
    private BoxCollider2D collider;

    [SerializeField] private LayerMask Ground;

    [SerializeField] private float jumpheight = 15f;
    [SerializeField] private float movespeed = 18f;
    private float dirX = 0f;

    private enum MoveState { idle,running,jumping,falling}

    [SerializeField] private AudioSource jumpsound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();    
        collider = GetComponent<BoxCollider2D>();   
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movespeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && Groundcheck())
        {
            jumpsound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpheight);
        }
        Updateanime();
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
