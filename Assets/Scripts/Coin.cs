using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public ScoreCalculator score;
    public AudioSource coinSound;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            score.increasePoints(20);
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            Debug.Log(score.getPoints());
            score.numberOfCoins++;
            coinSound.Play();
        }
    }
}
