using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSound : MonoBehaviour
{

    public AudioSource audioMovement;
    public AudioSource audioJump;
    public AudioSource audioAttack;
    public float fadeSpeed = 4f; // Time delay before stopping the audio

    private float movementTargetVolume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementTargetVolume = 0f;
        audioMovement.volume = 0f;
        audioMovement.loop = true; // Set the movement audio to loop

        

    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current == null)
        {
            return; // No keyboard detected, exit the method
        }

        Keyboard keyInput = Keyboard.current;
        // Check for movement input (WASD or arrow keys)
        bool moveLeft = keyInput.aKey.isPressed || keyInput.leftArrowKey.isPressed;
        bool moveRight = keyInput.dKey.isPressed || keyInput.rightArrowKey.isPressed;
        bool moveUp = keyInput.wKey.wasPressedThisFrame || keyInput.upArrowKey.wasPressedThisFrame || keyInput.spaceKey.wasPressedThisFrame;
        bool attack = keyInput.jKey.wasPressedThisFrame;

        bool isMoving = moveLeft || moveRight;

        movementTargetVolume = isMoving ? 1f : 0f; //Set target volume based on movement state

        // Play audio if player is moving left or right, stop audio if not
        if (isMoving && !audioMovement.isPlaying)
        {
            audioMovement.Play();

            audioMovement.volume = Mathf.MoveTowards(
                audioMovement.volume,
                movementTargetVolume,
                fadeSpeed * Time.deltaTime);
        }

        // Fade out the movement audio when the player stops moving
        if (!isMoving && audioMovement.isPlaying)
        {
            audioMovement.Stop();
        }

        // Play jump audio if player is moving up
        if (moveUp)
        {
            audioJump.PlayOneShot(audioJump.clip);
        }

        // Play attack audio if player is attacking
        if (attack)
        {
            audioAttack.PlayOneShot(audioAttack.clip);
        }
    }
}
