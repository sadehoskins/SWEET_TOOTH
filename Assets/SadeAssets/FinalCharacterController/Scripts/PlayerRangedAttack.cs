using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    public Transform enemy;

    public GameObject bullet;
    public GhostMover ghostMover;
    public PlayerHealth playerHealth;

    private float shotCooldown;

    public float startShotCooldown;

    void Update()
    {
        Vector2 direction = new Vector2(enemy.position.x, enemy.position.y - transform.position.y);
        transform.up = direction;

        if (shotCooldown <= 0)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            shotCooldown = startShotCooldown;
        }
        else{
            shotCooldown -= Time.deltaTime;
        }
    }





}
