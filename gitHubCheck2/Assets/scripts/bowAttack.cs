using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowAttack : MonoBehaviour
{
    public GameObject arrow;
    public Transform shotPoint;

    public PlayerMovement playerMovement;

    private float shootingTimer;
    public float resetShootingTimer;
  
    public float launchForce;
    public float maxLauncForce;

    public bool isShooting;

    // Update is called once per frame
    private void Start()
    {
        launchForce = 10f;
        shootingTimer = 0f;
        isShooting = false;
    }
    void Update()
    {
        shootingTimer -= Time.deltaTime;

        if ((Input.GetButtonDown("bow") && shootingTimer <= 0f) || (Input.GetButton("bow") && shootingTimer <= 0f))
        {
            isShooting = true;
            if (launchForce < maxLauncForce)
            {
                launchForce += Time.deltaTime * 14f;
            }
            else
            {
                shootingTimer = resetShootingTimer;
                Shoot();
                launchForce = 10f;

            }
        }
        else if (Input.GetButtonUp("bow") && shootingTimer <= 0f)
        {
            shootingTimer = resetShootingTimer;
            Shoot();
            isShooting = false;
            launchForce = 10f;
        }
        else 
        {
            isShooting = false;
        }

        

    }

    void Shoot()
    {
        
        GameObject newArrow = Instantiate(arrow, shotPoint.position, Quaternion.identity);
        Rigidbody2D arrowRigidbody = newArrow.GetComponent<Rigidbody2D>();

        if (playerMovement.isFacingRight)
        {
            arrowRigidbody.velocity = transform.right * launchForce;
            Vector3 scale = newArrow.transform.localScale;
            scale.x = 1;
            newArrow.transform.localScale = scale;
        }
        else
        {
            arrowRigidbody.velocity = transform.right * -launchForce;
            Vector3 scale = newArrow.transform.localScale;
            scale.x = -1;
            newArrow.transform.localScale = scale;
        }
    }


}
