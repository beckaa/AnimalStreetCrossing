using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public ScoreCalculator score;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            score.increasePoints(20);
            Destroy(this.gameObject);
            Debug.Log(score.getPoints());
        }
    }
}
