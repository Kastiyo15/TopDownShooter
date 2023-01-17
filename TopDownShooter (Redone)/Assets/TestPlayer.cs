using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TestPlayer : MonoBehaviour
{
 
    Rigidbody2D rb;
   float speed = 10f;
    Vector2 input;
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
 
    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        GetInput();
        UpdateMovement();
    }
 
    private void RotatePlayer()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (mousePos - (Vector2)transform.position).normalized;
        if (input.x == 0)
        {
            transform.up = lookDir;
        }
    }
 
    private void UpdateMovement()
    {
      if (input.x == 0)
        {
            rb.velocity = transform.up * input.y * speed;
        }
 
        else if(input.y == 0)
        {
            rb.velocity = transform.right * input.x * speed;
 
        }
    }
 
    private void GetInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        input = new Vector2(horizontal, vertical);       
    }
}
