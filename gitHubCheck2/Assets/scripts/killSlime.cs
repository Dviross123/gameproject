using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killSlime : MonoBehaviour
{
    public bool slimeDeath;
    public bool smallSlimeExplode;
    private GameObject sword;
    private GameObject camera;
    private Animator animator;
    private BoxCollider2D box;
    private Animator slimeAnimator;

    private void Start()
    {
        sword = GameObject.Find("sword");
        camera = GameObject.Find("MainCamera");
        animator = camera.GetComponent<Animator>();
        slimeAnimator = gameObject.GetComponent<Animator>();
        box = gameObject.GetComponent<BoxCollider2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("arrow"))
        {
            StartCoroutine(Die());
            animator.SetBool("slimeDie", true);
            StartCoroutine(waitForShakeEnd());
            slimeAnimator.SetBool("isDead", true);
        }

        

     
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("sword") && sword != null && sword.GetComponent<swordAttack>().isKilling)
        {
            StartCoroutine(Die());
            animator.SetBool("slimeDie", true);
             StartCoroutine(waitForShakeEnd());
            slimeAnimator.SetBool("isDead", true);
        }

        
        

    


    }

    private IEnumerator Die()
    {
        smallSlimeExplode = true;
        slimeDeath = true;
        box.enabled = false;
        yield return new WaitForSeconds(0.8f);
        slimeDeath = false;
        slimeAnimator.SetBool("isDead", true);
        Destroy(gameObject);
    }


    private IEnumerator waitForShakeEnd() 
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("slimeDie", false);
    }
    

}
