using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 target;
 
    public float bulletLifeTime;
    public GameObject Player;
    private playerManager player1;


    void Start()
    {
        player1 = Player.GetComponent<playerManager>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            Destroy(gameObject);
        }



        bulletLifeTime -= Time.deltaTime;
        if (bulletLifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

   
}
