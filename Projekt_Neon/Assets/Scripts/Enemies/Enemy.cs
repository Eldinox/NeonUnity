using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int damage;
    public float speed;
    public float attackCooldown;
    
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public float distance;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.position);
        Debug.Log("Rusher distance: " + distance);
    }

    public void TakeDamage(int damageAmount)
    {
    	health -= damageAmount;

    	if(health <= 0)
    	{
    		Destroy(this.gameObject);
    	}
    }
}
