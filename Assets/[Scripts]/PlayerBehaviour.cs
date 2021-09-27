using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;
    
    private Animator animatorController;
    private Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float x = 0.0f;
        float y = 0.0f;

        if (isGrounded)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Jump");

            // check if player is moving
            if (x != 0)
            {
                x = FlipAnimation(x);

                // shift to run animation
                animatorController.SetInteger("AnimationState", 1); // run
            }
            else
            {
                // shift back to idle
                animatorController.SetInteger("AnimationState", 0); // idle
            }
        }
        else
        {
            x = Input.GetAxisRaw("Horizontal") * 0.01f; // less input when falling or jumping

            if (x != 0)
            {
                x = FlipAnimation(x);
            }

            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x * 0.90f, rigidBody2D.velocity.y);

            animatorController.SetInteger("AnimationState", 2); // jump
        }

        Vector2 movementVector = new Vector2(x * horizontalForce, y * verticalForce);
        rigidBody2D.AddForce(movementVector);
    }

    private float FlipAnimation(float x)
    {
        // uses the Ternary operator
        x = (x > 0) ? 1 : -1;

        transform.localScale = new Vector3(x, 1.0f);
        return x;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

}
