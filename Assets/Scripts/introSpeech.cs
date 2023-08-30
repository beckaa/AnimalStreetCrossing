using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class introSpeech : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panel;
    public TMP_Text text;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            panel.SetActive(true);
            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        panel.SetActive(false);
        text.gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            panel.SetActive(true);
            text.gameObject.SetActive(true);
        }
    }
}
