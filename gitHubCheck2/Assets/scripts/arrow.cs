using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{



    public GameObject smallSlime;
    public GameObject player;

    private playerManager playerManager; // Fixed variable declaration

    // Start is called before the first frame update
    void Start()
    {
        playerManager = player.GetComponent<playerManager>(); // Fixed variable assignment
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            Destroy(gameObject);

    }
}
