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
    // Start is called before the first frame update
    void Start()
    {
        initialSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate( speed*Time.deltaTime, 0,0);
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
    void setCarBack()
    {
        if(speed <0 && transform.position.x <= position.x - 1)
        {
            //stop car
            speed = 0;
            waitTime = time+4;
            
        }else if(speed>0 && transform.position.x>= position.x +1)
        {
            //stop car
            speed = 0;
            waitTime = time+4;
            
        }
        //after car stopped make it move again - maybe wait for a second
        if (speed == 0 && time > waitTime)
        {
            hitSomething = false;
        }
    }
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
        //if the car collides with anything the driver gets shocked and drives backwards for a bit
        speed = (speed - (speed / 2)) * -1;
        position = transform.position;
        hitSomething = true;
    }
}
