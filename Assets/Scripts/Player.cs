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
    void Start()
    {
        if (movementSpeed == 0)
        {
            movementSpeed = 5f;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get PlayerInput
       xInput = Input.GetAxis("Horizontal");
       zInput = Input.GetAxis("Vertical");

        animator.SetFloat("forwards", zInput);
        if (xInput >=1)
        {
            transform.Rotate(0, Time.deltaTime * 40*2, 0);
        }
        if (xInput < 0)
        {
            transform.Rotate(0, Time.deltaTime * -40*2, 0);
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
    }
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "car")
        {
            animator.SetBool("damage", false);
        }
        lastCollided = null;
    }
}
