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
    void alignPlayerToTerrainHeight()
    {
        //TODO if player walks on an object you can not use terrain height !!!
        //get height of terrain at the current player position
        float height = Terrain.activeTerrain.SampleHeight(transform.position);
        //set player to terrain height + player sprite height
        transform.position = new Vector3(transform.position.x, height + 5.62f, transform.position.z);
    }
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
    void movePlayer()
    {
        if (xInput == 0 && zInput == 0)
        {
            //freeze position
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        }
        animator.SetFloat("forwards", zInput);
        if (xInput >= 1)
        {
            transform.Rotate(0, Time.deltaTime * 40 * 2, 0);
        }
        if (xInput < 0)
        {
            transform.Rotate(0, Time.deltaTime * -40 * 2, 0);
        }
        if (zInput > 0)
        {
            transform.Translate(0, 0, zInput * Time.deltaTime * movementSpeed);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "car" && other.gameObject!=lastCollided)
        {
            detectDamage(other); 
        }
        if (other.gameObject.tag == "bridge")
        {
            walkingOnObject = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "bridge")
        {
            walkingOnObject = true;
        }
    }
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
    private void OnCollisionExit(Collision other)
    {
        stopDamage(other);
        if (other.gameObject.tag == "bridge")
        {
            walkingOnObject = false;
        }
    }
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
    void resetPlayer()
    {
        if (time> waitTime)
        {
            transform.position = new Vector3(0, 16.25f, -22.9f);
            animator.SetBool("damage", false);
            lastCollided = null;
            resetPosition = false;
        }
    }
}
