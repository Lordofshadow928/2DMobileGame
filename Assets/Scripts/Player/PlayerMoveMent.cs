using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMoveMent : MonoBehaviour
{
    
    public float speed;
    public float jumpPower;
    [SerializeField]private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCoolDown;
    private float horizontalInput;
    private float onAirVelocity;
    private float jumpTimeCounter;
    private float jumpTimeMax = 0.15f;
    public ParticleSystem ParticleEF_Run;
    private void Awake()
    {
        body = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        boxCollider = this.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("OnWall", onWall());
        RunEffect();

        if (wallJumpCoolDown > 0.2f)
        {
            

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 4;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;
            
            if (Input.GetKey(KeyCode.Space)) 
                Jump();
        }
        else
            wallJumpCoolDown += Time.deltaTime;
        anim.SetFloat("OnAirVelocity", body.velocity.y);
    }

    private void Jump()
    {
        if(isGrounded() && Input.GetButtonDown("Jump"))
        {
            jumpTimeCounter = jumpTimeMax;
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
        }
        else if (jumpTimeCounter > 0 && Input.GetButton("Jump") && !isGrounded())
        {
            jumpTimeCounter -= Time.deltaTime;
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
        }
        else if (jumpTimeCounter <= 0)
        {
            jumpTimeCounter = 0;
            body.velocity = new Vector2(body.velocity.x, body.velocity.y);
            anim.SetTrigger("Jump");
        }
        //else if(onWall() && !isGrounded())
        //{
        //    if(horizontalInput == 0)
        //    {
        //        body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 1);
        //        transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        //    }
        //    else
        //        body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 5, 9);
        //    wallJumpCoolDown = 0;
            
        //}
        
    }
    private void RunEffect()
    {
        ParticleEF_Run.Play();
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    
    
        
    

}
