using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    //TODO: also increase by passed time ??
    int points;
    public int getPoints()
    {
        return points;
    }
    public void increasePoints(int number)
    {
        points += number;
    }

}
