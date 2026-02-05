
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{

    // Reference to the platform's collider
    private Collider platformCollider;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformCollider = GetComponent<BoxCollider>();

    }

    // Function for platform collision handling
    private void OnCollisionEnter(Collision collision)
    {

        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {

            // Get the player's Rigidbody component
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            // Ignore collision if the player is moving upwards
            if (playerRb.linearVelocity.y > 0)
            {
                Physics.IgnoreCollision(collision.collider, platformCollider, true);
            }
        }
    }

    // Function to reenable collision when the player exits the platform
    private void OnCollisionExit(Collision collision)
    { 
        if( collision.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collision.collider, platformCollider, false);
        }

    }


}
