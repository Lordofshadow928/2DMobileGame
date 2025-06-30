
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMoveMent : MonoBehaviour
{
    [Header("Running")]
    [SerializeField] private float speed;
    [Header("Player Layer Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;
    [Header("Jumping")]
    [SerializeField] private float jumpPower;
    private float jumpTimeCounter;
    private float jumpTimeMax = 0.15f;
    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float startDashTime = 0.1f;
    [SerializeField] private float dashCooldown = 0.2f;
    [SerializeField] private GameObject dashEffect;
    private bool canDash = true;
    private bool isDashing = false;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCoolDown;
    private float horizontalInput;
    public ParticleSystem ParticleEF;
    private void Awake()
    {
        body = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        boxCollider = this.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        Flip();
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("OnWall", onWall());

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
            if (isGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
        }
        else
            wallJumpCoolDown += Time.deltaTime;
        anim.SetFloat("OnAirVelocity", body.velocity.y);

        if (Input.GetKeyDown(KeyCode.Z) && canDash)
        {
            StartCoroutine(Dash());
        }
        anim.SetBool("isDashing", isDashing);
            
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

    }
    private void Jump()
    {
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            jumpTimeCounter = jumpTimeMax;
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            JumpFX();
        }
        else if (jumpTimeCounter > 0 && Input.GetButton("Jump") && !isGrounded())
        {
            jumpTimeCounter -= Time.deltaTime;
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            JumpFX();
        }
        else if (jumpTimeCounter <= 0)
        {
            jumpTimeCounter = 0;
            body.velocity = new Vector2(body.velocity.x, body.velocity.y);
            JumpFX();
        }
        if (coyoteTimeCounter <= 0 && !onWall())
        {
            return;
        }
        else if (coyoteTimeCounter > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower + 8);
        }
        coyoteTimeCounter = 0;

        if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 6, 1);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 5, 9);
            wallJumpCoolDown = 0;
        }
    }

    private void Flip()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;

        }

        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }

        if (body.velocity.y == 0)
        {
            ParticleEF.Play();
        }
    }
    
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(startDashTime);
        body.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void JumpFX()
    {
        anim.SetTrigger("Jump");
        ParticleEF.Play();
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