using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSound : MonoBehaviour
{
    public string[] collisionTag;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        foreach (string s in collisionTag){
            if (other.gameObject.tag == s)
            {
                audioSource.Play();
            }
        }

    }
}
