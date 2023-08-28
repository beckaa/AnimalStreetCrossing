using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public Player player;
    public AudioSource collect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //increase the life of the player since a turkey was collected
            int life = player.getLife();
            if (life < 3)
            {
                collect.Play();
                player.increaseLife(1);
                //Destroy(this.gameObject);
                this.gameObject.SetActive(false);
            }

        }
    }

}
