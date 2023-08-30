using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Vector3 offset = new Vector3(0,4,-7);
    public Rigidbody player;
    Rigidbody cameraRb;
    private Vector3 position;

    // Update is called once per frame
    void FixedUpdate()
    {
        position = player.position;
        //keep y static to let the camera not follow beneath the river
        position.y = 16;
        //move the camera with the player
        cameraRb = gameObject.GetComponent<Rigidbody>();
        cameraRb.MovePosition(position + offset);
    }
}
