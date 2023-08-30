using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    //TODO: also increase by passed time ??
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
