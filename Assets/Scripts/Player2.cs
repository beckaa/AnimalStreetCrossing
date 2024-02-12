using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player2 : MonoBehaviour
{
    public Animator animator;
    private GameObject lastCollided;
    private float time, waitTime;
    //player attributes
    private CharacterController controller;
    public float speed = 2f;
    public float rotationSpeed = 4f;
    Vector3 move, startPosition;
    public int life;
    private float jumpHeight = 3f;
    private float jumping;

    public bool resetPosition;
    Quaternion startRotation;

    //UI screens
    public GameObject endscreen;
    public GameObject highScoreScreen;
    public TMP_Text scoreText;

    //Score Calculator
    public ScoreCalculator scoreCalculator;

    //audio
    public AudioSource whimper;
    public AudioSource walkingSound;
    public AudioSource collect;
    public AudioSource coinSound;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        life = 3;
        startRotation = transform.rotation;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (resetPosition)
        {
            Invoke("resetPlayer", 1.5f);
        }
        //jump();
        forward();
        rotatePlayerWithTerrain();
    }
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
        }
        jump();
        move = this.transform.forward * moveForward;
        move.y = jumping;
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
            jumping -= 2;
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
            if (Physics.Raycast(transform.position, Vector3.down, out output))
            {
                var newRotation = Quaternion.LookRotation(Vector3.Cross(transform.right, output.normal));
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 2 * Time.deltaTime);
            }
        }
        
    }
    /*resets the players position to the start of the game if the player fell into a river*/
    public void resetPlayer()
    {
            transform.position = startPosition;
            transform.rotation = startRotation;
            animator.SetBool("damage", false);
            lastCollided = null;
            resetPosition = false;
            //detect Death
            if (life == 0)
            {
                detectDeath();
            }
        controller.enabled = true;
    }
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "river" && !resetPosition)
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
            if(life < 3)
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
