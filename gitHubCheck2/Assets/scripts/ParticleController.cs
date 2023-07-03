using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] PlayerMovement PlayerMovement;

    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] ParticleSystem fallParticle;
    [SerializeField] ParticleSystem wallParticle;
    [SerializeField] ParticleSystem dashBurstParticle;
    [SerializeField] ParticleSystem waveDashBurstParticle;

    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;

    [Range(0, 0.2f)]
    [SerializeField] float dustFormationPeroid;

    [SerializeField] Rigidbody2D rb;

   

    float counter;

    private void Update()
    {
        counter += Time.deltaTime;

        if (Mathf.Abs(rb.velocity.x) > occurAfterVelocity && PlayerMovement.IsGrounded()) 
        {
            if (counter > dustFormationPeroid) 
            {
                movementParticle.Play();
                counter = 0;
            }
            
        }

        if (PlayerMovement.isWallSliding) 
        {
            if (counter > dustFormationPeroid)
            {
                wallParticle.Play();
                counter = 0;
            }
          
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) 
        {
         
           fallParticle.Play();
        }
    }
    
}
