using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killStrrongRobot : MonoBehaviour
{
    //floats
    public float robotHealth;
    public float maxRobotHealth;
    public bool canDamage = true;

    //scripts
    public swordAttack swordAttack;
    public bowAttack bowAttack;

    // Start is called before the first frame update
    void Start()
    {
        robotHealth = maxRobotHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (robotHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("arrow"))
        {
            robotHealth--;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("arrow"))
        {
            if (bowAttack.isMaxForce)
            {
                Debug.Log("big pew");
                robotHealth -= 2f;
            }
            else
            {
                Debug.Log("little pew");
                robotHealth--;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("sword") && swordAttack.isKilling && swordAttack.attackNum == 3 && canDamage)
        {
            robotHealth -= 2;
            canDamage = false;
        }

        else if (collision.gameObject.CompareTag("sword") && swordAttack.isKilling && canDamage)
        {
            robotHealth--;
            canDamage = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canDamage = true;
    }


    }