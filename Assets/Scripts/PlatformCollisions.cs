using UnityEngine;

public class PlatformCollision : MonoBehaviour
{

   
    public Transform player;
    public Transform player2;

    private Collider platformCollider;

    private Rigidbody playerRb;
    private Rigidbody player2Rb;

    private Collider playerCol;
    private Collider player2Col;

   
    void Start()
    {
        platformCollider = GetComponent<BoxCollider>();

        playerRb = player.GetComponent<Rigidbody>();
        player2Rb = player2.GetComponent<Rigidbody>();

        playerCol = player.GetComponent<Collider>();
        player2Col = player2.GetComponent<Collider>();
    }

    void Update()
    {
        HandlePlayer(player, playerRb, playerCol);
        HandlePlayer(player2, player2Rb, player2Col);
    }

    void HandlePlayer(Transform p, Rigidbody rb, Collider col)
    {
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