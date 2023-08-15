using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float xInput;
    float zInput;
    public Animator animator;
    public float movementSpeed;
    public int life = 3;
    GameObject lastCollided;
    float waitTime;
    bool resetPosition;
    float time;
    Rigidbody rb;
    bool walkingOnObject;
    public int getLife()
    {
        return life;
    }
    public void increaseLife(int number)
    {
        life += number;
    }
    void Start()
    {
        if (movementSpeed == 0)
        {
            movementSpeed = 5f;
        }
        rb = this.GetComponent<Rigidbody>();
        walkingOnObject = false;  
    }

    void FixedUpdate()
    {
        //get PlayerInput
       xInput = Input.GetAxis("Horizontal");
       zInput = Input.GetAxis("Vertical");
       movePlayer();
        
    }
    private void Update()
    {
        if (resetPosition)
        {
            resetPlayer();
        }
        time += Time.deltaTime;
        if (!walkingOnObject)
        {
            rotatePlayerWithTerrain();
            alignPlayerToTerrainHeight();
        }
    }

    /*aligns the players height to the terrain so it will stay grounded*/
    void alignPlayerToTerrainHeight()
    {
        //get height of terrain at the current player position
        float height = Terrain.activeTerrain.SampleHeight(transform.position);
        //set player to terrain height + player sprite height
        transform.position = new Vector3(transform.position.x, height + 5.62f, transform.position.z);
    }

    /*rotates the player with the Terrain so it will not look like the player floats with half the body in the air while walking hills*/
    void rotatePlayerWithTerrain()
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
    void movePlayer()
    {
        if (xInput == 0 && zInput == 0)
        {
            //freeze position to prevent the player to slide down hills if not moving
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        }
        animator.SetFloat("forwards", zInput);
        if (xInput >= 1)
        {
            //rotate player in moving direction
            transform.Rotate(0, Time.deltaTime * 40 * 2, 0);
        }
        if (xInput < 0)
        {
            //rotate player in moving direction
            transform.Rotate(0, Time.deltaTime * -40 * 2, 0);
        }
        if (zInput > 0)
        {
            //changes the players position through user input
            transform.Translate(0, 0, zInput * Time.deltaTime * movementSpeed);
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
    }
    private void OnCollisionStay(Collision collision)
    {
        //makes sure that the tag will remain true [maybe can be deleted]
        if (collision.gameObject.tag == "bridge")
        {
            walkingOnObject = true;
        }
    }

    /*will play the damage animation and also reduce the players life count*/
    void detectDamage(Collision other)
    {
        lastCollided = other.gameObject;
        if (life == 0)
        {
            animator.SetBool("Death", true);
            Time.timeScale = 0;
            //game ends -> make end screen
        }
        else
        {
            animator.SetBool("damage", true);
            life -= 1;
            Debug.Log(life);
        }
    }
    /*ovrload method as above but used for triggers*/
    void detectDamage(Collider other)
    {
        lastCollided = other.gameObject;
        if (life == 0)
        {
            animator.SetBool("Death", true);
            Time.timeScale = 0;
            //game ends -> make end screen
        }
        else
        {
            animator.SetBool("damage", true);
            life -= 1;
            Debug.Log(life);
        }
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
    void stopDamage(Collision other)
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
            detectDamage(other);
            waitTime = time + 2;
            lastCollided = other.gameObject;
            resetPosition = true;
            resetPlayer();
        }
    }
    /*resets the players position to the start of the game if the player fell into a river*/
    void resetPlayer()
    {
        if (time> waitTime)
        {
            transform.position = new Vector3(0, 16.25f, -22.9f);
            animator.SetBool("damage", false);
            lastCollided = null;
            resetPosition = false;
            //detect Death
            if (life == 0)
            {
                animator.SetBool("Death", true);
                Time.timeScale = 0;
                //game ends -> make end screen
            }
        }
    }
}
