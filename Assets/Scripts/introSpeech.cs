using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introSpeech : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject child;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            child = gameObject.transform.GetChild(0).gameObject;
            child.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        child.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            child.SetActive(true);
        }
    }
}
