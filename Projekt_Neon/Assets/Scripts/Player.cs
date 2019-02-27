using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Werte die sich im Spiel ändern
    public int health;
    
    //Feste Werte
    public float speed;
    public float jumpForce;
    public int jumpAmount;
    public float jumpTime;
    
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float moveInput;
    private bool facingRight = true;
    private bool isGrounded;
    private bool isJumping;
    private int extraJumps;
    private float jumpTimeCounter;
    
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = jumpAmount;
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
}
