using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyshoot : MonoBehaviour
{

    public Transform player;
    public float shootingDis;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject bullet;

    void Start()
    {
        timeBtwShots = startTimeBtwShots;
    }

    
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position)<shootingDis) 
        {
        if (timeBtwShots <= 0)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else 
        {
            timeBtwShots -= Time.deltaTime;
        }
        }

    }
}
