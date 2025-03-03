﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage;
    public GameObject drop1, drop2, drop3;

    private Rigidbody2D rb;
    private Transform player;

    //public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if(this.gameObject.name == "SpiderProjectile(Clone)")
        {
            int direction = 0;
            if(player.transform.position.x < transform.position.x)direction = -1;
            else direction = 1;
            rb.AddForce(new Vector2(direction, 2) * 28, ForceMode2D.Impulse);
        }
        else if(this.gameObject.name == "SpiderDrop1(Clone)")
        {
            rb.AddForce(new Vector2(-20, 0), ForceMode2D.Impulse);
        }
        else if(this.gameObject.name == "SpiderDrop3(Clone)")
        {
            rb.AddForce(new Vector2(20, 0), ForceMode2D.Impulse);
        }
        
        Invoke("DestroyProjectileNaturally", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //Für gradlinige Projektile ohne Gravity
    }

    void DestroyProjectile()
    {
    	Destroy(gameObject);
    	//Instantiate(explosion, transform.position, Quaternion.identity);
    }
    void DestroyProjectileNaturally()
    {
    	Destroy(gameObject);
        if(this.gameObject.name == "SpiderProjectile(Clone)")
        {
            Instantiate(drop1, transform.position, Quaternion.identity);
            Instantiate(drop2, transform.position, Quaternion.identity);
            Instantiate(drop3, transform.position, Quaternion.identity);
        }
    	//Instantiate(explosion, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    	if(collision.tag == "Ground")DestroyProjectile();
        if(this.gameObject.name == "PlayerProjectile(Clone)" && collision.tag == "Enemy")
        {
            //Player Projectile macht Schaden an Gegnern
        }
        else if(this.gameObject.name == "SpiderProjectile(Clone)" && collision.tag == "Player" ||
            this.gameObject.name == "SpiderDrop1(Clone)" && collision.tag == "Player" ||
            this.gameObject.name == "SpiderDrop2(Clone)" && collision.tag == "Player" ||
            this.gameObject.name == "SpiderDrop3(Clone)" && collision.tag == "Player")
    	{
    		collision.GetComponent<Player>().TakeDamage(damage);
    		DestroyProjectile();
    	}
        
    }
}
