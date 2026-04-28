using UnityEngine;

public class PlatformCollision : MonoBehaviour
{

    // references to the players
    public Transform player;
    public Transform player2;
    // reference to the platform's collider
    private Collider platformCollider;
    // references to the player rigidbodies and colliders
    private Rigidbody playerRb;
    private Rigidbody player2Rb;
    // references to the player colliders
    private Collider playerCol;
    private Collider player2Col;

   
    void Start()
    {
        // get the platform's collider
        platformCollider = GetComponent<BoxCollider>();
        // get the player rigidbodies and colliders
        playerRb = player.GetComponent<Rigidbody>();
        player2Rb = player2.GetComponent<Rigidbody>();
        // get the enemy colliders
        playerCol = player.GetComponent<Collider>();
        player2Col = player2.GetComponent<Collider>();
    }

    void Update()
    {
        // check each player against the platform
        HandlePlayer(player, playerRb, playerCol);
        HandlePlayer(player2, player2Rb, player2Col);
    }

    // handles collision logic for a player
    void HandlePlayer(Transform p, Rigidbody rb, Collider col)
    {
        // get the y positions of player and platform
        float playerY = p.position.y;
        float platformY = transform.position.y;

        // If below platform AND moving up ignore collision
        if (playerY < platformY && rb.linearVelocity.y > 0.1f)
        {
            Physics.IgnoreCollision(platformCollider, col, true);
        }
        else
        {
            Physics.IgnoreCollision(platformCollider, col, false);
        }
    }
}