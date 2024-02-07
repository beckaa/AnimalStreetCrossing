using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    //TODO: increase score if the player reaches the finishline and has time left (set a timer) ??
    int points;
    public int numberOfCoins;
    public int getPoints()
    {
        return points;
    }
    public void increasePoints(int number)
    {
        points += number;
    }
    public void resetPoints()
    {
        points = 0;
        numberOfCoins = 0;
    }

}
