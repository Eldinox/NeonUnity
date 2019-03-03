using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rusher : Enemy
{
    public float stopDistance;
    public float attackSpeed;
    public float dashSpeed;
    public float knockbackForce;

    private float attackTime;
    private Rigidbody2D rb;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
        	if(Vector2.Distance(transform.position, player.position) > stopDistance)
        	{
        		transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        	}
            else
            {
                if(Time.time >= attackTime)
                {
                    StartCoroutine(Attack());
                    attackTime = Time.time + attackCooldown;
                }
            }
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1);
        int direction = 0;
        if(player.transform.position.x < transform.position.x)direction = -1;
        else direction = 1;
        rb.velocity = new Vector2(direction, 0) * dashSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Vector2 difference = new Vector2(transform.position.x, transform.position.y) - new Vector2(collision.transform.position.x, collision.transform.position.y);
            difference = difference.normalized * knockbackForce;
            rb.AddForce(difference, ForceMode2D.Impulse);
            
            player.GetComponent<Player>().TakeDamage(damage);
            //player.GetComponent<Player>().Knockback(transform.position);
        }
    }
}
