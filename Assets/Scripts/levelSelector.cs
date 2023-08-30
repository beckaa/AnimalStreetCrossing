using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSelector : MonoBehaviour
{

    bool isNearLevel=false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && isNearLevel)
        {
            SceneManager.LoadScene(gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject child = gameObject.transform.GetChild(0).gameObject;
            child.SetActive(true);
            isNearLevel = true;
            if(gameObject.name == "YourLevel")
            {
                isNearLevel = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isNearLevel = false;
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        child.SetActive(false);
    }
}
