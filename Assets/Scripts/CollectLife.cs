using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectLife : MonoBehaviour
{
    public Player2 player;
    // Start is called before the first frame update
    void Start()
    {
        Collider collider = this.gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
     void Update()
    {
        if (player.getLife() < 3)
        {
            GetComponent<Collider>().enabled = true;
        }
        else
        {
            GetComponent<Collider>().enabled = false;
        }
    }
}
