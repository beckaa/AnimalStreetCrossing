using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate( speed*Time.deltaTime, 0,0);
        cycleCar();
    }

    void cycleCar()
    {
        if(transform.position.x > 27 && speed >0)
        {
            Vector3 position = transform.position;
            position.x = -30;
            transform.position = position;
        }
        if(transform.position.x < -30 &&speed < 0)
        {
            Vector3 position = transform.position;
            position.x = 50;
            transform.position = position;
        }
    }
}
