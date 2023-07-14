using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordAttack : MonoBehaviour
{
    private bool canAttack = true;
    private bool isReseting = false;
    public bool isAttacking = false;
    private bool wasAttacking = false;
    private bool isKilling = false;
    public int attackNum = 0;
    private float resetAttackTime = 1.25f;
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        attackNum = 0;
    }
    //dvir start attack animation where isKilling = true
    //dvir start attack animation where isKilling = true
    //dvir start attack animation where isKilling = true
    //dvir start attack animation where isKilling = true

    // Update is called once per frame
    void Update()
    {
        if (attackNum > 3)
        {
            StartCoroutine(attackR());
        }
        Debug.Log(attackNum);
        if (Input.GetButtonDown("sword") && !isAttacking && canAttack)
        {
            StartCoroutine(attack());
        }
    }
    //coroutine not stopping when attacking in combo
    private IEnumerator attack()
    {
        
        isAttacking = true;
        attackNum++;
        if (attackNum == 1)
        {
            yield return new WaitForSeconds(0.15f);
            isKilling = true;
            yield return new WaitForSeconds(0.25f);

        }

        else if (attackNum == 2) 
        {
            yield return new WaitForSeconds(0.05f);
            isKilling = true;
            yield return new WaitForSeconds(0.25f/2f);
        }

        else if (attackNum == 3)
        {
            yield return new WaitForSeconds(.75f);
            isKilling = true;
            yield return new WaitForSeconds(.25f);
        }

        isAttacking = false;
        isKilling = false;
        if (isReseting)
        {
            wasAttacking = true;
        }
        StartCoroutine(attackReset());
        yield return new WaitForSeconds(0.1f);
    }
    private IEnumerator attackReset()
    {
        isReseting = true;
        yield return new WaitForSeconds(resetAttackTime);
        if (!isAttacking && !wasAttacking)
        {

            StartCoroutine(attackR());
        }
        else
        {
            wasAttacking = false;
            //Debug.Log("aaaaaaaaaaaaaaaaaaaa");
            isReseting = false;
            yield return null;
        }
        isReseting = false;
    }
    private IEnumerator attackR()
    {
        Debug.Log("0");
        attackNum = 0;
        canAttack = false;
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("a");
        if (isKilling)
        {
            if (collision.CompareTag("smallSlime"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
    

}
