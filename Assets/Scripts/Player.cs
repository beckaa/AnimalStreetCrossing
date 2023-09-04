using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Animator animator;
    public float movementSpeed;
    public int life;
    public GameObject endscreen;
    public GameObject winscreen;
    public TMP_Text scoreText;
    public ScoreCalculator ScoreCalculator;
    //private use only
    private float xInput;
    private float zInput;
    private GameObject lastCollided;
    private float waitTime;
    private bool resetPosition;
    private float time;
    private Rigidbody rb;
    private bool walkingOnObject;
    private bool doJump;
    private bool isGrounded;
    private Quaternion startRotation;
    public AudioSource whimper;
    public AudioSource walkingSound;
    int terrainIndex;

    void Start()
    {
        initialize();
    }
    void FixedUpdate()
    {
        movePlayer();
        setRigidbodyConstraints();
        jump();
    }
    private void Update()
    {
        if (resetPosition)
        {
            resetPlayer();
        }
        time += Time.deltaTime;
        if (!walkingOnObject && !doJump)
        {
            rotatePlayerWithTerrain();
            alignPlayerToTerrainHeight();
        }
        if (Input.GetButtonDown("Jump"))
        {
            doJump = true;
        }
        //movePlayer();
        detectDeath();
    }
    private void initialize()
    {
        //get the rigidbody of the player
        rb = this.GetComponent<Rigidbody>();
        //init variables
        life = 3;
        walkingOnObject = false;
        isGrounded = true;
        doJump = false;
        startRotation = transform.rotation;
    }
    public int getLife()
    {
        return life;
    }
    public void increaseLife(int number)
    {
        life += number;
    }

    /*aligns the players height to the terrain so it will stay grounded*/
    private void alignPlayerToTerrainHeight()
    {
        //get height of terrain at the current player position
        //terrain if there are terrain neighbors -> needs debugging
        /*if (getTerrain() > 0)
        {
            terrainIndex = getTerrain()-1;
        }*/
         float groundHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        //set player to terrain height + player sprite height
        transform.position = new Vector3(transform.position.x, groundHeight + 5.62f, transform.position.z);
    }

    //returns the current terrain index the player is on
    /*int getTerrain()
    {
        RaycastHit output;
        int index = 0;
        if(Physics.Raycast(transform.position,Vector3.down,out output))
        {
            foreach (Terrain t in Terrain.activeTerrains)
            {
                index++;
                if(t.gameObject == output.transform.gameObject)
                {
                    break;
                }
            }
        }
        return index;
    }*/
    /*rotates the player with the Terrain so it will not look like the player floats with half the body in the air while walking hills*/
    private void rotatePlayerWithTerrain()
    {
        //rotate the player with the terrain surface
        
        RaycastHit output;
        if (Physics.Raycast(transform.position, Vector3.down, out output))
        {
            var newRotation = Quaternion.LookRotation(Vector3.Cross(transform.right, output.normal));
            transform.rotation = Quaternion.Slerp(transform.rotation,newRotation, 2*Time.deltaTime);
        }
    }
    /*moves the player according to horizontal and vertical input
     * movement with w,a,s,d or arrow keys possible
     */
    private void movePlayer()
    {
        //get PlayerInput
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        animator.SetFloat("forwards", zInput);
        if (xInput >= 1)
        {
            //rotate player in moving direction
            transform.Rotate(0, Time.deltaTime * 40 * 3, 0);
        }
        if (xInput < 0)
        {
            //rotate player in moving direction
            transform.Rotate(0, Time.deltaTime * -40 * 3, 0);
        }
        if (zInput > 0)
        {
            //changes the players position through user input
            transform.Translate(0, 0, zInput * Time.deltaTime * movementSpeed);
            if (!walkingSound.isPlaying)
            {
                walkingSound.PlayOneShot(walkingSound.clip);
            }

        }
    }
    private void setRigidbodyConstraints()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        if (xInput == 0 && zInput == 0 && !doJump)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if (xInput == 0 && zInput == 0 && doJump)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
        else if(xInput < 0 &&zInput==0 && doJump|| xInput > 0 &&zInput==0&&!doJump)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    //lets the player jump once if grounded
    private void jump()
    {
        if (doJump && isGrounded)
        {
            rb.AddForce(new Vector3(0,2,0),ForceMode.Impulse);
            isGrounded = false;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        //if the player hits a car it will receive damage
        if(other.gameObject.tag == "car" && other.gameObject!=lastCollided)
        {
            detectDamage(other); 
        }
        //if player enters a bridge the flag will be set to true to not use terrain height
        //makes it possible to walk on objects or cross a bridge of a river
        if (other.gameObject.tag == "bridge")
        {
            walkingOnObject = true;
        }
        //if the player hits the ground or the bridge ground the jump ended
        if(other.gameObject.name == "Ground" || other.gameObject.tag=="bridge" || other.gameObject.tag=="ground")
        {
            doJump = false;
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        //makes sure that the tag will remain true
        if (collision.gameObject.tag == "bridge")
        {
            walkingOnObject = true;
        }
        if (collision.gameObject.tag=="barrier" || collision.gameObject.tag == "car")
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
        
        
        
    }

    /*will play the damage animation and also reduce the players life count*/
    private void detectDamage(Collision other)
    {
        whimper.Play();
        lastCollided = other.gameObject;
        animator.SetBool("damage", true);
        life -= 1;
    }
    /*ovrload method as above but used for triggers [especially for the water collision]*/
    private void detectDamage(Collider other)
    {
        whimper.Play();
        lastCollided = other.gameObject;
        animator.SetBool("damage", true);
        life -= 1;
    }
    /* if the player leaves a collider....*/
    private void OnCollisionExit(Collision other)
    {
        stopDamage(other);
        //if player does not walk on an object set the flag to false
        if (other.gameObject.tag == "bridge")
        {
            walkingOnObject = false;
        }
    }

    /*stops the damage taking and animation*/
    private void stopDamage(Collision other)
    {
        if (other.gameObject.tag == "car")
        {
            animator.SetBool("damage", false);
        }
        lastCollided = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "river" && !walkingOnObject)
        {
            resetPosition = true;
            detectDamage(other);
            waitTime = time + 2;
            lastCollided = other.gameObject;
            resetPlayer();
        }
        if(other.gameObject.tag == "finishline")
        {
            scoreText.text = "Your Score: " +ScoreCalculator.getPoints().ToString();
            winscreen.SetActive(true);
            stopGameSounds();
            Time.timeScale = 0;
        }
    }
    /*resets the players position to the start of the game if the player fell into a river*/
    private void resetPlayer()
    {
        if (time> waitTime)
        {
            transform.position = new Vector3(0, 16.25f, -22.9f);
            transform.rotation = startRotation;
            animator.SetBool("damage", false);
            lastCollided = null;
            resetPosition = false;
            //detect Death
            if (life == 0)
            {
                detectDeath();
            }
        }
    }
    private void detectDeath()
    {
        if(life == 0)
        {
            if (resetPosition)
            {
                resetPlayer();
            }
            animator.SetBool("Death", true);
            stopGameSounds();
            endscreen.SetActive(true);
            Time.timeScale = 0;
        }

    }
    
    //stops all sounds with the tag inGameSounds
    void stopGameSounds()
    {
        GameObject[] sounds = GameObject.FindGameObjectsWithTag("inGameSounds");
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].GetComponent<AudioSource>().Stop();
        }
    }
}
