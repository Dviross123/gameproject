using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 12f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    private int Jumps = 0;
    public int maxJumps = 1;
    public float fastFallpower = -0.5f;

    public float dashCounter = 1f;
    public float originalGravity= 3f;
    public bool canDash = true;
    public bool isDashing ;
    public float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.2f;
    private float extraMomentum;
    private float extraMomentumDirection;
    private bool gravityReturned = true;
    public bool isWaveDashing;

    public bool isWallSliding;
    private float wallSlidingSpeed = 1f;

    private bool canSlamStorage;

    private bool isFastFalling = false;

    private bool isWallJumping;
    private float wallJumpingAmount = 0f;
    public float wallJumpingAllowed = 3f;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(6f, 16f);


    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    // Start is called before the first frame update
    void Start()
    {
        tr.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //kills the player if he is too low
        if (transform.localPosition.y <= -50)
        {
            PlayerDied();
        }
        //resets extra momentum direction when the is no extra momentum
        if (extraMomentum == 0)
            extraMomentumDirection = transform.localScale.x;
        //sets the max momentum to 2
        if (extraMomentum > 20)
            extraMomentum = 20;
        //limits the slam storage to 1 time after momentum is higher than 20
        if (extraMomentum < 20)
            canSlamStorage = true;
        //makes sure the momentum isnt negitive
        if (extraMomentum < 0)
            extraMomentum = 0;
        //lower momentum if moving to other direction
        if (extraMomentumDirection == horizontal * -1 && extraMomentumDirection != 0 && extraMomentum > 0.1f)
            extraMomentum -= 0.05f;
        horizontal = Input.GetAxisRaw("Horizontal");
        //checks if you can jump
        if (Input.GetButtonDown("Jump") && Jumps < maxJumps && !isFastFalling)
        {
            rb.velocity = new Vector2(rb.velocity.x * Time.deltaTime, jumpingPower);
            Jumps++;
        }


        ////if you let go of jump while jumping stop the jump early
        //if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f && !isDashing)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, -1f);
        //}


        //checks when you can dash
        if (Input.GetButtonDown("Fire3") && canDash && gravityReturned && !isFastFalling && dashCounter>0)
        {
            StartCoroutine(Dash());
        }
        //checks when the player does a wave dash
        if (isDashing && Input.GetButtonDown("Jump") && (Input.GetAxisRaw("Vertical") < 0  || (IsGrounded() && Input.GetAxisRaw("Vertical") < 0)))
        {

            if (extraMomentum <= 15f)
            {
                extraMomentum = 24f;
            }
            else
            {
                extraMomentum += 16;
            }

        
            rb.velocity = new Vector2(rb.velocity.x + extraMomentum * transform.localScale.x * Time.deltaTime, rb.velocity.y);
            extraMomentumDirection = transform.localScale.x;
            isWaveDashing = true;
        }
       
        //gives back jump and dash when grounded
        if (IsGrounded())
        {
            Jumps = 0;
            wallJumpingAmount = 0f;
        }
        if (IsGrounded() || dashingCooldown <= 0f ) 
        {
            if (dashCounter == 0f)
            {
                dashCounter++;
            }
            else if (dashCounter < 0f) 
            {
                dashCounter++;
            }
            canDash = true;
            dashingCooldown = 0.2f;
        }

        //always checks wall slide, jump and momentum
        WallSlide();
        WallJump();
        Momentum();
        //flips the player when nessecery
        if (!isWallJumping)
        {
            Flip();
        }
        //fast fall
        if (Input.GetButton("Fire2") && !isWallJumping && !isWallSliding)
        {
            isFastFalling = true;
            rb.velocity = new Vector2(0f, rb.velocity.y + fastFallpower);
            if (IsGrounded())
                extraMomentum = 0;
        }
        //cancel fast fall
        if (Input.GetButtonUp("Fire2") && isFastFalling)
        {
            isFastFalling = false;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

    }

    private void FixedUpdate()
    {
        Debug.Log(extraMomentum);
        //if is dashing or fast falling on ground do nothing
        if (isDashing || (isFastFalling && IsGrounded()))
        {
            return;
        }
        //if fast falling change momentum accordingly
        if (isFastFalling)
        {
            rb.velocity = new Vector2(horizontal * speed / 3 * Time.deltaTime, rb.velocity.y);
        }
        //other than that normal
        else if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed + extraMomentum * extraMomentumDirection * Time.deltaTime, rb.velocity.y);
        }
    }
    //checks when grounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }
    //checks when walled
    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer); ;
    }

    public void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f && !isDashing)
        {
            Debug.Log("walled");
            isWallSliding = true;
            if (extraMomentum > 30)
                rb.velocity = new Vector2(rb.velocity.x, wallSlidingSpeed * 24);
            else
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed * 3);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        //slam storage thing
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0 && Input.GetButton("Fire2") && canSlamStorage)
        {
            extraMomentum += 36f;
            canSlamStorage = false;
            
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0 && wallJumpingAmount < wallJumpingAllowed)
        {
            isWallJumping = true;
            Jumps = 0;
            wallJumpingAmount++;
            extraMomentumDirection *= -1;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x * Time.deltaTime, wallJumpingPower.y );
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void PlayerDied()
    {
        //LevelManager.instance.GameOver();
        gameObject.SetActive(false);
    }


    private IEnumerator Dash()
    {
        if (dashCounter<= 0f)
        {
            canDash = false;
        }
        dashCounter--;
        isDashing = true;
        originalGravity = rb.gravityScale;
        gravityReturned = false;
        rb.gravityScale = 0f;

        Vector2 dashDirection = Vector2.zero;

        // Check input for dash direction
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            dashDirection.x = Input.GetAxisRaw("Horizontal");
            dashDirection.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            dashDirection.x = transform.localScale.x;
        }

        rb.velocity = dashDirection.normalized * dashingPower;

        // Reset momentum if not wave dashing
        if (dashDirection.x == 0 || dashDirection.y >= 0)
        {
            extraMomentum = 0;
        }

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;

        // Restore gravity gradually
        float elapsedTime = 0f;
        float transitionDuration = 0.1f;
        while (elapsedTime < transitionDuration)
        {
            rb.gravityScale = Mathf.Lerp(0f, originalGravity, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.gravityScale = originalGravity;
        gravityReturned = true;

        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
    }


    private void Momentum()
    {
        if (extraMomentum > 0)
        {
            extraMomentum -= 0.005f;
            new WaitForSeconds(0.2f);
            if (IsGrounded())
            {
                extraMomentum -= 0.1f;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy" && isDashing)
        {
            Destroy(collision.gameObject);
        }
    }

}