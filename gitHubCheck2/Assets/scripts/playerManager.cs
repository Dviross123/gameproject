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
    private float life;
    public float resetLife;
    public PlayerMovement PlayerMovement;
    public float jumpTime;
    public float jumpTimeReset;
    public bool startTimer=false;




    [SerializeField] public Rigidbody2D rb;

    public int respawn;


    public Animator animator;

    private void Start()
    {
        tpTimer = tpTimerReset;
        life = resetLife;
        jumpTime = jumpTimeReset;

    }

    void Update()
    {
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

        if (life < 0) 
        {
            Debug.Log("player Die");
            SceneManager.LoadScene(respawn);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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

   

   
}
