using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTp : MonoBehaviour
{
    private GameObject currentTeleporter;
    private float tpTimer;
    public float tpTimerReset;
    private bool canTp = true;
    private float life;
    public float resetLife;

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
        if (collision.CompareTag("tp"))
        {
            currentTeleporter = collision.gameObject;
            if (canTp) 
            {
                transform.position = currentTeleporter.GetComponent<teleport>().GetDestinatrion().position;
                tpTimer = tpTimerReset;
            }
            
        }
        if (collision.CompareTag("obs")) 
        {
            life -= resetLife;
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
}
