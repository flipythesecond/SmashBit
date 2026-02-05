using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{

    // Speed of the player movement
    public float speed;

    // Reference to the Rigidbody component
    private Rigidbody rb;

    // Movement input values
    private float movementX;
    private float movementY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void OnMove(InputValue movementValue)
    {

        // Get movement input as a Vector2
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Update movement input values
        movementX = movementVector.x;
        movementY = movementVector.y;


    }

   


    // FixedUpdate is called once per fixed frame-rate
    void FixedUpdate()
    {
        // Create a movement vector based on input
        Vector3 movement = new Vector3(movementX, movementY, 0.0f);

        // Apply movement to the Rigidbody2D
        rb.AddForce(movement * speed);



    }
}
