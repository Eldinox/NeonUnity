using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Werte die sich im Spiel ändern
    public int health;
    
    //Feste Werte
    public float speed;
    public float jumpForce;
    public int jumpAmount;
    public float jumpTime;
    public float knockbackForce;
    
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public Transform spawnPoint;
    
    private float moveInput;
    private bool facingRight = true;
    private bool isGrounded;
    private bool isJumping;
    private int extraJumps;
    private float jumpTimeCounter;

    private Rigidbody2D rb;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = jumpAmount;
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        spawnPoint = GameObject.Find("PlayerSpawnStart").transform;

        if(transform.position.x < spawnPoint.position.x)
        {
            spawnPoint = GameObject.Find("PlayerSpawnEnd").transform;
            transform.position = spawnPoint.position;
        }
        else if(transform.position.x > spawnPoint.position.x)
        {
            transform.position = spawnPoint.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded == true)
        {
            extraJumps = jumpAmount;
        }

        if(Input.GetKeyDown(KeyCode.W) && extraJumps > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        if(Input.GetKey(KeyCode.W) && isJumping == true)
        {
            if(jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
    	isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        
        //Input.GetAxisRaw("Horizontal"); <- damit Player sofort anhält (kein sliden)
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if(facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            int direction = 0;
            if(transform.position.x > collision.transform.position.x)direction = -1;
            else direction = 1;
            Debug.Log(new Vector2(direction, 1) * knockbackForce);
            rb.velocity = new Vector2(direction, 1) * knockbackForce;
        }
    }
    public void Knockback(Vector2 direction)
    {
        Vector2 difference = direction - new Vector2(transform.position.x, transform.position.y);
        difference = difference.normalized * knockbackForce;
        rb.AddForce(difference, ForceMode2D.Impulse);
        
    }
}
