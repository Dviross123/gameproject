using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShakescontroller : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement ;
    public playerManager playerManager;
    public killSlime killSlime;

    // Start is called before the first frame update
    void Start()
    {
  
    }
    void Update()
    {

        if ((playerMovement.isDashing && !playerMovement.IsGrounded())|| playerMovement.IsSliding || playerMovement.isFastFalling)
        {
            animator.SetBool("playerDash", true);

        }
       
        else 
        {
            animator.SetBool("playerDash", false);
        }




    }
    
}
