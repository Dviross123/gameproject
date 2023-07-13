using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerManager: MonoBehaviour
{

    private GameObject currentTeleporter;
    public GameObject player;
    public float tpTimer;
    public float tpTimerReset;
    public bool canTp = true;
    public float health;
    public float resetHealth;
    public PlayerMovement PlayerMovement;
    public float jumpTime;
    public float jumpTimeReset;
    public bool startTimer=false;
    public float smallBoostPower;


    private GameObject smallSlime;
    [SerializeField] public Rigidbody2D rb;

    public int respawn;


    public Animator animator;
    public Animator SlimeAnimator;

    public ParticleSystem smallSlimeExplosion;

    public bool killSmallSlime=false;

    private void Start()
    {
        tpTimer = tpTimerReset;
        health = resetHealth;
        jumpTime = jumpTimeReset;

    }

    void Update()
    {
        //kill player 
        if (health <= 0f) 
        {
            SceneManager.LoadScene(respawn);
        }

        //animations

        //run
        if (Input.GetButton("Horizontal"))
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        //wall slide
        if (PlayerMovement.isWallSliding)
        {
            animator.SetBool("isWallSliding", true);
            animator.SetBool("isRunning", false);
        }
        else 
        {
            animator.SetBool("isWallSliding", false);
        }

        //jump
        if ((Input.GetButton("Jump") && rb.velocity.y>0) || Input.GetButton("Fire3") && rb.velocity.y > 0)
        {
            animator.SetBool("isJumping", true);

        }
        if(PlayerMovement.IsGrounded()|| PlayerMovement.IsWalled()) 
        {
            animator.SetBool("isJumping", false);
        }

        //fall
        if (rb.velocity.y < 0 && !PlayerMovement.IsGrounded() && !PlayerMovement.IsWalled() && !PlayerMovement.isJumping)
        {
            animator.SetBool("isFalling", true);
        }
        else 
        {
            animator.SetBool("isFalling", false);
        }
        //slide
        if (PlayerMovement.IsSliding && PlayerMovement.IsGrounded())
        {
            animator.SetBool("isSliding", true);
        }
        else 
        {
            animator.SetBool("isSliding", false);
        }

     


 



        if (transform.localPosition.y <= -50)
        {
            SceneManager.LoadScene(respawn);
        }

        tpTimer -=Time.deltaTime;
        if (tpTimer <= 0f)
        {
            canTp = true;
        }
        else 
        {
            canTp = false;
        }

        if (health < 0) 
        {
            Debug.Log("player Die");
            SceneManager.LoadScene(respawn);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) 
        {
            health--;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("positiveTp"))
        {
            Debug.Log("touch tp");
            if (canTp)
            {
                Debug.Log("tp");
                currentTeleporter = collision.gameObject;
                transform.position = currentTeleporter.GetComponent<teleport>().GetDestinatrion().position;


                if (PlayerMovement.isDashing)
                {
                    PlayerMovement.originalGravity = 3f;
                }
                else
                {

                rb.gravityScale = 3f;
                }
                PlayerMovement.fastFallpower = -0.5f;
                PlayerMovement.jumpingPower = 12f;

                Vector3 newRotation = player.transform.eulerAngles;
                newRotation.z += 180f;
                player.transform.eulerAngles = newRotation;
                tpTimer = tpTimerReset;
            }
        }
        if (collision.CompareTag("negativeTp"))
        {
            if (canTp)
            {
                currentTeleporter = collision.gameObject;
                transform.position = currentTeleporter.GetComponent<teleport>().GetDestinatrion().position;



                if (PlayerMovement.isDashing)
                { 
                    PlayerMovement.originalGravity = -3f;
                }
                else
                {

                    rb.gravityScale = -3f;
                }
                PlayerMovement.fastFallpower = 0.5f;
                PlayerMovement.jumpingPower = -12f;
               
                Vector3 newRotation = player.transform.eulerAngles;
                newRotation.z += 180f;
                player.transform.eulerAngles = newRotation;

                tpTimer = tpTimerReset;
            }
        }
        if (collision.CompareTag("obs"))
        {
            SceneManager.LoadScene(respawn);          
        }

        if (collision.CompareTag("addDash"))
        {
            PlayerMovement.dashCounter++;
            Destroy(collision.gameObject);

        }

        


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("smallSlime"))
        {
            if (PlayerMovement.isFastFalling)
            {             
                 rb.velocity = new Vector2(rb.velocity.x, smallBoostPower);  
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                smallSlimeExplosion.Play();
                killSmallSlime = true;
                StartCoroutine(KillWait(collision));
                collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                smallSlime = collision.gameObject;
                SlimeAnimator.SetBool("isDead", true);
            }

            else if (collision.gameObject.CompareTag("smallSlime") && !PlayerMovement.isDashing && !PlayerMovement.isFastFalling)
            {
                SceneManager.LoadScene(respawn);
            }
        }
    }



    public IEnumerator KillWait(Collision2D collision)   
    {  
        yield return new WaitForSeconds(.7f);
        
        Destroy(smallSlime);
        killSmallSlime = false;
    }
        

}
