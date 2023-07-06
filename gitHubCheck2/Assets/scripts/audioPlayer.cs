using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip dashSfx, runSfx;
    public PlayerMovement playerMovement;
    private bool playDashSound=true;
    public bool playRunSound = true;


    private void Dash() 
    {
        src.clip = dashSfx;
        src.Play();
    }

    private void Run()
    {
        src.clip = runSfx;
        src.Play();
    }

    private void stopPlay()
    {

        src.Stop();
    }

    private void Update()
    {
        //if (playerMovement.isDashing && playDashSound)
        //{
        //    Debug.Log("Dash");
        //    Dash();
        //    playDashSound = false;
        //}
        //else if (!playerMovement.isDashing && !playDashSound) 
        //{
        //    playDashSound = true;
        //}

        //if (playerMovement.horizontal != 0f && playerMovement.IsGrounded() && playRunSound)
        //{
        //    Run();
        //    Debug.Log("run");
        //    playRunSound = false;
        //}
        //else if (!playerMovement.IsGrounded()) 
        //{
        //    stopPlay();
        //    playRunSound = true;
        //}
    }
}
