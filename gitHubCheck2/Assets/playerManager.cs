using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    private GameObject currentTeleporter;
    public GameObject player;
    private float tpTimer;
    public float tpTimerReset;
    private bool canTp = true;
    private float life;
    public float resetLife;
    public PlayerMovement PlayerMovement;

    Vector2 startPos;

    [SerializeField] public Rigidbody2D rb;

    private void Start()
    {
        startPos = transform.position;

        tpTimer = tpTimerReset;
        life = resetLife;
    }

    void Update()
    {

        tpTimer-=Time.deltaTime;
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
            //kill player
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("positiveTp"))
        {
            if (canTp)
            {
            currentTeleporter = collision.gameObject;
                transform.position = currentTeleporter.GetComponent<teleport>().GetDestinatrion().position;

                Vector3 newRotation = player.transform.eulerAngles;
                newRotation.z += 180f;
                player.transform.eulerAngles = newRotation;

                if (PlayerMovement.isDashing)
                {
                    PlayerMovement.originalGravity = 3f;
                }
                else
                {

                rb.gravityScale = 3f;
                }
                PlayerMovement.fastFallpower *= -1;
                PlayerMovement.jumpingPower *= -1;
                PlayerMovement.wallJumpingPower *= -1;
                tpTimer = tpTimerReset;
            }
        }
        if (collision.CompareTag("negativeTp"))
        {
            if (canTp)
            {
                currentTeleporter = collision.gameObject;
                transform.position = currentTeleporter.GetComponent<teleport>().GetDestinatrion().position;

                Vector3 newRotation = player.transform.eulerAngles;
                newRotation.z += 180f;
                player.transform.eulerAngles = newRotation;


                if (PlayerMovement.isDashing)
                {
                    PlayerMovement.originalGravity = -3f;
                }
                else
                {

                    rb.gravityScale = -3f;
                }
                PlayerMovement.fastFallpower *= -1;
                PlayerMovement.jumpingPower *= -1;
                PlayerMovement.wallJumpingPower *= -1;
                tpTimer = tpTimerReset;
            }
        }
        if (collision.CompareTag("obs"))
        {
            transform.position = startPos;
                if (negGravity(PlayerMovement.jumpingPower)) 
                {
                    rb.gravityScale = 3f;
                    PlayerMovement.fastFallpower *= -1;
                    PlayerMovement.jumpingPower *= -1;
                    PlayerMovement.wallJumpingPower *= -1;
                    Vector3 newRotation = player.transform.eulerAngles;
                    newRotation.z += 180f;
                    player.transform.eulerAngles = newRotation;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("tp"))
        {
            if (collision.gameObject == currentTeleporter) 
            {
                currentTeleporter = null;
            }
        }
    }

    private bool negGravity(float jumpingPower)
    {
        if (jumpingPower < 0)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
