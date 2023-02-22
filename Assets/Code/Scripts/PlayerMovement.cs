using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private string currentPlayerColor;
    private Color playerColor, colliderObjectColor;
    public JoystickMovement joystickMovement;
    private Rigidbody rb;
    [SerializeField] private Transform backRestriction, frontRestriction;
    private CameraFollowPlayer playerCamera;
    private bool forwardForce = true;
    private GameManager gameManager;
    [SerializeField] private AudioSource obstacleHitSound;

    void Start()
    {
        playerCamera = FindObjectOfType<CameraFollowPlayer>();
        gameManager = FindObjectOfType<GameManager>();
        playerColor = gameObject.GetComponent<Renderer>().material.GetColor("_Color");
        rb = gameObject.GetComponent<Rigidbody>();
        Debug.Log("Current Player Color: " + currentPlayerColor);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            obstacleHitSound.Play();
            colliderObjectColor = other.gameObject.GetComponent<Renderer>().material.GetColor("_Color");
            if (playerColor != colliderObjectColor)
            {
                Debug.Log("GAME LOST!!!");
                gameManager.GameOver();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FinishLine")
        {
            PlayerInfo.isGamePaused = true;
            gameManager.LevelComplete();
            Debug.Log("LEVEL COMPLETED!");
        }
    }

    void Update()
    {
        if (PlayerInfo.isGamePaused) return;
        if (forwardForce) transform.position += transform.forward * moveSpeed * Time.deltaTime;
        backRestriction.position += backRestriction.forward * moveSpeed * Time.deltaTime;
        frontRestriction.position += frontRestriction.forward * moveSpeed * Time.deltaTime;

        // if (movementScrollbar.value < 0.5)
        // {
        //     if (transform.position.x > -7) MoveHorizontally("Left");
        // }
        // if (movementScrollbar.value > 0.5)
        // {
        //     if (transform.position.x < 7) MoveHorizontally("Right");
        // }
        // 0 - 1 (0.1, 0.2, 0.3)
        // -6.9 - 6.9

    }

    private void FixedUpdate()
    {
        if (PlayerInfo.isGamePaused) return;
        // Debug.Log("Joystick X: " + joystickMovement.joystickVector.x + " Joystick Y: " + joystickMovement.joystickVector.y);
        if (joystickMovement.joystickVector.y != 0)
        {
            forwardForce = false;
            rb.velocity = new Vector3(joystickMovement.joystickVector.x * 10f, 0f, joystickMovement.joystickVector.y * 10f);

        }
        else
        {
            rb.velocity = Vector3.zero;
            forwardForce = true;
        }
    }

}
