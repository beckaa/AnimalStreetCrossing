using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed;
    private float initialSpeed;
    private bool hitSomething;
    private Vector3 position;
    private float time;
    private float waitTime;
    public AudioSource startCarSound;
    public Player2 player;
    // Start is called before the first frame update
    void Start()
    {
        //save the initial speed to set it back to originalspeed after collision
        initialSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.resetPosition)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            cycleCar();
            if (hitSomething)
            {
                setCarBack();
            }
            else
            {
                speed = initialSpeed;
            }
            time += Time.deltaTime;
        }
    }
    //if the car collides with anything the driver gets shocked and drives backwards for a bit
    void setCarBack()
    {
        if(speed <0 && transform.position.x <= position.x - 1)
        {
            //stop car
            speed = 0;
            startCarSound.Play();
            waitTime = time+4;
            
        }else if(speed>0 && transform.position.x>= position.x +1)
        {
            //stop car
            speed = 0;
            startCarSound.Play();
            waitTime = time+4;
            
        }
        //after car stopped make it move again - wait for 4 seconds until start driving again
        if (speed == 0 && time > waitTime)
        {
            hitSomething = false;
        }
    }
    /*The cars will pop up at the other side of the screen if they drive out of the gameview*/
    void cycleCar()
    {
        
        if(transform.position.x > 27 && speed >0)
        {
            Vector3 position = transform.position;
            position.x = -30;
            transform.position = position;
        }
        if(transform.position.x < -30 && speed < 0)
        {
            Vector3 position = transform.position;
            position.x = 50;
            transform.position = position;
        } 
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("car"))
        {
            speed = (speed - (speed / 2)) * -1; //half the speed
            position = transform.position;
            hitSomething = true;
        }
        if(other.gameObject.tag.Equals("Player") && !player.resetPosition)
        {
            speed = (speed - (speed / 2)) * -1; //half the speed
            position = transform.position;
            hitSomething = true;
            player.resetPosition = true;
            player.detectDamage(other);
        }
    }
}
