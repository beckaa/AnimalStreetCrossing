using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player2 : MonoBehaviour
{
    
    [Header("Gameplay variables")]
    public ScoreCalculator scoreCalculator;
    public Animator animator;
    private GameObject lastCollided;
    public bool resetPosition;
    Quaternion startRotation;
    Vector3 move, startPosition;
    bool playerMoves = false;

    [Header("Player attributes")]
    public float speed = 2f;
    public float rotationSpeed = 2f;
    public int life;
    public CharacterController controller;
    private float jumpHeight = 4f;
    private float jumping;
    private float jumpSpeed = 2f;
    private float gravity = 6.81f;

    [Header("UI Screens")]
    public GameObject endscreen;
    public GameObject highScoreScreen;
    public TMP_Text scoreText;

    [Header("Audio Sources")]
    public AudioSource whimper;
    public AudioSource walkingSound;
    public AudioSource collect;
    public AudioSource coinSound;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        life = 3;
        //save start position and rotation for resetting
        startRotation = transform.rotation;
        startPosition = transform.position;
    }

    void Update()
    {
        if (resetPosition)
        {
            Invoke("resetPlayer", 1.5f);
        }
        else
        {
            forward();
            rotatePlayerWithTerrain();
        }      
    }

    /*moves the player forward*/
    void forward()
    {
        animator.SetFloat("forwards", Input.GetAxis("Vertical"));
        float moveForward=0;
        //prevent walking backwards
        if (Input.GetAxis("Vertical") <= 0)
        {
            // do not move if s or downwards arrow is pressed
            moveForward = 0;
        }
        else if(Input.GetAxis("Vertical")>0)
        {
            // move if w or upwards arrow is pressed
            moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            if (!walkingSound.isPlaying && Time.timeScale >0)
            {
                walkingSound.Play();
            }
        }
        if(moveForward > 0)
        {
            playerMoves = true;
        }
        else
        {
            playerMoves = false;
        }
        jump();
        move = this.transform.forward * moveForward;
        move.y = jumping* (Time.deltaTime*jumpSpeed);
        Vector3 rotate = new Vector3(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);
        if (Time.timeScale > 0) //prevent character from moving if game was paused
        {
            this.transform.Rotate(rotate);
            controller.Move(move);
        }
        
    }
    void jump()
    {
        //jump if player is grounded
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumping =jumpHeight;
            }
        }
        else
        {
            if (!controller.isGrounded)
            {
                jumping -= gravity * Time.deltaTime;
            }
            else
            {
                jumping = 0f;
            }
            
        }

    }
    public int getLife()
    {
        return life;
    }
    public void increaseLife(int number)
    {
        life += number;
    }
    public void decreaseLife(int number)
    {
        life -= number;
    }

    /*rotates the player with the Terrain so it will not look like the player floats with half the body in the air while walking hills*/
    private void rotatePlayerWithTerrain()
    {
        //rotate the player with the terrain surface
        if (Time.timeScale > 0)
        {
            RaycastHit output;
            if (Physics.Raycast(transform.position, Vector3.down, out output) && playerMoves)
            {
                var newRotation = Quaternion.LookRotation(Vector3.Cross(transform.right, output.normal));
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 2 * Time.deltaTime);
            }
        }
        
    }
    /*resets the players position to the start of the game if the player fell into a river or bumped into other obstacles*/
    public void resetPlayer()
    {
            transform.position = startPosition;
           transform.rotation = startRotation;
        animator.SetBool("damage", false);
            lastCollided = null;
            resetPosition = false;
            detectDeath();
            controller.enabled = true; 
    }
    //detects if the player died
    private void detectDeath()
    {
        if (life == 0)
        {
            animator.SetBool("Death", true);
            stopGameSounds();
            endscreen.SetActive(true);
            Time.timeScale = 0;
        }

    }

    /*will play the damage animation and also reduce the players life count*/
    private void detectDamage(ControllerColliderHit other)
    {
        whimper.Play();
        lastCollided = other.gameObject;
        animator.SetBool("damage", true);
        life -= 1;
    }
    public void detectDamage(Collision other)
    {
        whimper.Play();
        lastCollided = other.gameObject;
        animator.SetBool("damage", true);
        life -= 1;
    }

    /*detects the collision with the character controller capsule collider*/
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "river" && !resetPosition)
        {
            resetPosition = true;
            detectDamage(hit);
            controller.enabled = false;
            lastCollided = hit.gameObject;

        }

        if (hit.gameObject.tag == "finishline")
        {
            scoreText.text = "Your Score: " + scoreCalculator.getPoints().ToString();
            highScoreScreen.SetActive(true);
            stopGameSounds();
            Time.timeScale = 0;
        }
        if (hit.gameObject.tag == "coin")
        {
            scoreCalculator.increasePoints(20);
            Destroy(hit.gameObject);
            scoreCalculator.numberOfCoins++;
            coinSound.Play();
        }
        if (hit.gameObject.tag == "turkey")
        {
            if (life < 3)
            {
                collect.Play();
                increaseLife(1);
                Destroy(hit.gameObject);
            }
        }
    }
    

    //pauses all sounds with the tag inGameSounds
    public void stopGameSounds()
    {
        GameObject[] sounds = GameObject.FindGameObjectsWithTag("inGameSounds");
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].GetComponent<AudioSource>().Pause();
        }
    }
    //unpauses all sounds with the tag inGameSounds
    public void startGameSounds()
    {
        GameObject[] sounds = GameObject.FindGameObjectsWithTag("inGameSounds");
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].GetComponent<AudioSource>().UnPause();
        }
    }
}
