using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float xInput;
    float zInput;
    public Animator animator;
    private Vector3 currentPosition;
    public float movementSpeed;
    void Start()
    {
        movementSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
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
       currentPosition = transform.position;
        if (zInput > 0)
        {
            transform.Translate(0, 0, zInput * Time.deltaTime * movementSpeed);
        }
        

        
    }
}
