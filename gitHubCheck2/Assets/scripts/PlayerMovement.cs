using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontal;
    public float speed = 12f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    public bool isJumping;
    private int Jumps = 0;
    public int maxJumps = 1;

    public bool IsBouncing = false;
    public float BouncingSpeed = 0f;
    public float BouncingDirection = 1f;

    public float originalGravity;
    public bool canDash = true;
    public bool isDashing;
    public float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.2f;
    private float extraMomentum;
    private float extraMomentumDirection;
    private bool gravityReturned = true;
    public float dashCounter = 1f;

    public bool isWallSliding;
    private float wallSlidingSpeed = 1f;

    private bool canSlamStorage;

    private bool isFastFalling = false;
    private bool canFastFall = true;
    public float fastFallpower = 5f;

    private bool isWallJumping;
    private float wallJumpingAmount = 0f;
    public float wallJumpingAllowed = 3f;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(6f, 16f);


    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    private float preVel = 0f;
    public float slidingSpeed = 18f;
    public bool IsSliding = false;
    private bool isJumpSliding = false;
    private float slidingDirection = 0f;
    private bool JumpSliding = false;



    // Start is called before the first frame update
    void Start()
    {
        tr.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //kills the player if he is too low
        
        //resets extra momentum direction when the is no extra momentum
        if (extraMomentum == 0)
            extraMomentumDirection = transform.localScale.x;
        //sets the max momentum to 40
        if (extraMomentum > 40)
            extraMomentum = 40;
        //limits the slam storage to 1 time after momentum is higher than 20
        if (extraMomentum < 20)
            canSlamStorage = true;
        //makes sure the momentum isnt negitive to negate possible errors (just to be safe)
        if (extraMomentum < 0)
            extraMomentum = 0;
        //lower momentum if moving to other direction
        if (extraMomentumDirection == horizontal * -1 && extraMomentumDirection != 0 && extraMomentum > 0.1f)
        {
            if (extraMomentum > 24f)
                extraMomentum = 24f;
            extraMomentum -= 0.1f;
        }
        horizontal = Input.GetAxisRaw("Horizontal");

        //checks if you can jump
        if (Input.GetButtonDown("Jump") && Jumps < maxJumps && !isFastFalling)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower + rb.velocity.y / 4);
            Jumps++;
        }
        else 
        {
            isJumping = false;
        }
        //if you let go of jump while jumping stop the jump early
        //if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f && !isDashing)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, -1f);
        //}
        //checks when you can dash
        if (Input.GetButtonDown("Fire3") && canDash && gravityReturned && !isFastFalling&& dashCounter>0f)
        {
            StartCoroutine(Dash());
        }

        //gives back jump and dash when grounded
        if (IsGrounded())
        {
            Jumps = 0;
            canDash = true;
            if (dashCounter < 1f) 
            {
                dashCounter++;
            }
            wallJumpingAmount = 0f;
            BouncingSpeed = 0f;
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
        if (Input.GetButton("Fire2") && !isWallJumping && !isWallSliding && !IsGrounded() && canFastFall)
        {
            isFastFalling = true;
            if (IsGrounded())
                extraMomentum = 0;
        }
        //cancel fast fall
        if ((Input.GetButtonUp("Fire2") || IsGrounded()) && isFastFalling)
        {
            isFastFalling = false;
        }
        //sliding
        if (Input.GetButtonDown("Fire2") && IsGrounded())
        {
            preVel = rb.velocity.x;
            preVel = Mathf.Abs(preVel);
            IsSliding = true;
            slidingDirection = transform.localScale.x;
            StartCoroutine(StopSliding());
        }
        //cancel slide
        if ((Input.GetButtonUp("Fire2") || !IsGrounded() || isJumpSliding) && IsSliding)
        {
            IsSliding = false;
        }
        if (IsSliding && Input.GetButtonDown("Jump"))
        {
            IsSliding = false;
            isJumpSliding = true;
            canFastFall = false;
            JumpSliding = true;
            StartCoroutine(JumpSlide());
        }

    }

    private void FixedUpdate()
    {
        //if is dashing do nothing
        if (isDashing)
        {
            return;
        }
        //if fast falling change momentum accordingly
        else if (isFastFalling)
        {
            rb.velocity = new Vector2(horizontal * speed / 3, rb.velocity.y+fastFallpower);
        }
        else if (IsSliding)
        {
            rb.velocity = new Vector2((slidingSpeed + preVel / 3) * slidingDirection, 0);
        }
        else if (isJumpSliding)
        {
            rb.velocity = new Vector2((slidingSpeed + preVel / 3) * slidingDirection, jumpingPower);
            isJumpSliding = false;
        }
        //other than that and when starting a bounce normal
        else if (!isWallJumping && !JumpSliding)
        {
            rb.velocity = new Vector2(horizontal * speed + extraMomentum * extraMomentumDirection, rb.velocity.y);
        }
    }


    public IEnumerator StopSliding()
    {
        yield return new WaitForSeconds(1f);
        IsSliding = false;
    }


    public IEnumerator JumpSlide()
    {
        
        yield return new WaitForSeconds(0.5f);
        canFastFall = true;
        JumpSliding = false;
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

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f && !isDashing)
        {

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
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            if (canSlamStorage) extraMomentum = 0f;

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



    private IEnumerator Dash()
    {
        dashCounter--;
        canDash = false;
        isDashing = true;
        originalGravity = rb.gravityScale;
        gravityReturned = false;
        rb.gravityScale = 0f;
        //if using controller
        float hori = 0f, vert = 0f;
        if (Input.GetAxisRaw("Horizontal") < 0)
            hori = -1f;
        else if (Input.GetAxisRaw("Horizontal") > 0)
            hori = 1f;
        else hori = 0f;
        if (Input.GetAxisRaw("Vertical") < 0)
            vert = -1f;
        else if (Input.GetAxisRaw("Vertical") > 0)
            vert = 1f;
        else vert = 0f;
        if (hori == 0 && vert == 0)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, hori * dashingPower);
        }
        //when not wave dashing but dashing delete momentum
        else if (hori == 0 || vert >= 0)
        {
            rb.velocity = new Vector2(hori * dashingPower, vert * dashingPower);
            extraMomentum = 0;
        }
        else
        {
            rb.velocity = new Vector2(hori * dashingPower + hori * extraMomentum, vert * dashingPower);
            if (extraMomentum <= 6f)
                extraMomentum = 22f;
            else
                extraMomentum += 16f;
            extraMomentumDirection = transform.localScale.x;

        }

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = 10f;
        yield return new WaitForSeconds(0.1f);
        rb.gravityScale = originalGravity;
        gravityReturned = true;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
    }

    private void Momentum()
    {
        if (BouncingSpeed > 4f && !IsBouncing)
        {
            BouncingSpeed -= 1f;
            if (BouncingDirection == -transform.localScale.x && BouncingSpeed > 0)
            {
                BouncingSpeed = 0f;
            }
        }

        if (extraMomentum > 0)
        {
            extraMomentum -= 0.005f;
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
        if (collision.gameObject.layer == 3)
        {
            Jumps = 0;
            canDash = true;
            wallJumpingAmount = 0f;
        }
    }

}