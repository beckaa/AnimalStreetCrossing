using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //increase the life of the player since a turkey was collected
            int life = player.getLife();
            if (life < 3)
            {
                player.increaseLife(1);
                Destroy(this.gameObject);
            }

        }
    }

}
