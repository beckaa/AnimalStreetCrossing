using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Vector3 offset = new Vector3(0,4,-7);
    public Transform player;
    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position = player.position;
        //keep y static
        position.y = 16;
        //move the camera with the player
        transform.position = position+offset; 
    }
}
