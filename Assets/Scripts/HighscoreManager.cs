using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using UnityEngine.SceneManagement;
[Serializable]
public class highScoreentry
{
    public string rank;
    public string playername;
    public string score;

    public highScoreentry(string rank,string playername, string score)
    {
        this.rank = rank;
        this.playername = playername;
        this.score = score;
    }
    public string getplayername()
    {
        return this.playername;
    }
    public string getScore()
    {
        return this.score;
    }
    public highScoreentry Clone()
    {
        return new highScoreentry(this.rank, this.playername, this.score);
    }
}
[Serializable]
public class highScoreList
{
    public int id;
    public highScoreentry[] list;
    public highScoreList(int id, highScoreentry[] list)
    {
        this.id = id;
        this.list = list;
    }
}

public class HighscoreManager : MonoBehaviour
{
    public GameObject listItem;
    public GameObject parentPlane;
    public highScoreentry[] ranking = new highScoreentry[10];
    private int levelindex;
    highScoreList level;
    bool fileLoaded;
    public TMP_InputField inputName;
    string playername;
    public GameObject popup;
    public GameObject panelHighscore;
    public ScoreCalculator calculator;
    public GameObject[] items = new GameObject[10];
    int index = -1;

    private void Awake()
    {
        listItem.SetActive(false);
        fileLoaded = false;
    }

    private void Start()
    {
        levelindex = SceneManager.GetActiveScene().buildIndex-2;
        loadLevelHighscore();
    }
    private void Update()
    {
        saveNewScore();
    }
    private void loadFile()
    {
       if (PlayerPrefs.HasKey("level" + levelindex.ToString()))
        {
            string json = PlayerPrefs.GetString("level" + levelindex.ToString());
            highScoreList score = JsonUtility.FromJson<highScoreList>(json);
            ranking = score.list;
            fileLoaded = true;
        }
        else
        {
            fileLoaded = false;
        }
    }
    public void loadLevelHighscore()
    {
        loadFile();
        displayScore();
        saveLevelHighScore(ranking);
    }
    public void displayScore()
    {
        if (fileLoaded)
        {
            for (int i = 0; i < ranking.Length; i++)
            {
                items[i].transform.GetChild(0).GetComponent<TMP_Text>().text = ranking[i].rank;
                items[i].transform.GetChild(1).GetComponent<TMP_Text>().text = ranking[i].getplayername();
                items[i].transform.GetChild(2).GetComponent<TMP_Text>().text = ranking[i].getScore();
                items[i].SetActive(true);
            }
        }
        else
        {
            //create new ranking for level if it does not exist
            for (int i = 0; i < 10; i++)
            {
                string rank = (i + 1).ToString();
                highScoreentry newEntry = new highScoreentry(rank,"Your Name","0");
                items[i].transform.GetChild(0).GetComponent<TMP_Text>().text = rank;
                items[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "Your Name";
                items[i].transform.GetChild(2).GetComponent<TMP_Text>().text = "0";
                items[i].SetActive(true);
                ranking[i] = newEntry;
            }
        }
        

    }
    public int newScoreRank()
    {

        int currentScore = calculator.getPoints();
        if (index == -1) { 
            for (int i = 0; i < ranking.Length; i++)
            {
                if (currentScore > int.Parse(ranking[i].score))
                {
                    panelHighscore.SetActive(false);
                    popup.SetActive(true);
                    index = i;
                    break;
                }
            }
        }
        return index;
   
    }
    string askPlayerName()
    {
        return inputName.text;
    }

    public void savePlayerName()
    {
        playername = askPlayerName();
        if (String.IsNullOrEmpty(playername)) 
        {
            panelHighscore.SetActive(true);
            popup.SetActive(false);
        }
    }

    void saveNewScore()
    {
        highScoreentry[] newList = new highScoreentry[10];
        int rankIndex = newScoreRank();
        if (rankIndex >= 0 && !String.IsNullOrEmpty(playername))
        {
            highScoreentry someEntry = new highScoreentry((rankIndex + 1).ToString(), playername, calculator.getPoints().ToString());
            newList[rankIndex] = someEntry;
            for (int i = 0; i < rankIndex; i++)
            {
                newList[i] = ranking[i].Clone();
            }
            for (int i =rankIndex+1; i< ranking.Length; i++)
            {
                highScoreentry clone = ranking[i - 1].Clone();
                clone.rank = (int.Parse(clone.rank)+1).ToString();
                newList[i] = clone;
           }
            saveLevelHighScore(newList);
            updateUI(newList);
            panelHighscore.SetActive(true);
        }
    }
    void updateUI(highScoreentry[] list)
    {
        for(int i =0; i < ranking.Length; i++)
        {
            items[i].transform.GetChild(1).GetComponent<TMP_Text>().text = list[i].playername;
            items[i].transform.GetChild(2).GetComponent<TMP_Text>().text = list[i].score;

        }
    }
    public void saveLevelHighScore(highScoreentry[] list)
    {
        level = new highScoreList(levelindex, list);
        string json = JsonUtility.ToJson(level);
        PlayerPrefs.SetString("level" + levelindex.ToString(), json);
    }

}
