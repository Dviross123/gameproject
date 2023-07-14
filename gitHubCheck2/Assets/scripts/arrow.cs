using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    private float destroyArrow;
    public float destroyArrowReset;

    public GameObject smallSlime;
    public GameObject player;

    private playerManager playerManager; // Fixed variable declaration

    // Start is called before the first frame update
    void Start()
    {
        destroyArrow = destroyArrowReset;
        playerManager = player.GetComponent<playerManager>(); // Fixed variable assignment
    }

    // Update is called once per frame
    void Update()
    {
        destroyArrow -= Time.deltaTime;

        if (destroyArrow <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            Destroy(gameObject);
        
        if (collision.gameObject.CompareTag("smallSlime"))
        {
            playerManager.killSlimeArrow(collision);
        }
    }
}
