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


    [SerializeField] public Rigidbody2D rb;

    public int respawn;

    private void Start()
    {
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
