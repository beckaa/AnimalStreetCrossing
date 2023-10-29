using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject lifeContainer;
    public ScoreCalculator score;
    public Player player;
    public Image heart;
    public TMP_Text coinText;
    public GameObject pauseMenu;
    public GameObject looseScreen;
    public GameObject winScreen;
    // Start is called before the first frame update
    void Start()
    {
        //fill the heart display
        heartController();
        //if the menu or level scene was loaded start the game
        //start the game
        Time.timeScale = 1;
        //start audio
        //AudioListener.volume = 1;
    }

    // Update is called once per frame
    void Update()
    {
        heartController();
        coinCounter();
        pause();
    }

    void heartController()
    {
        int current = player.getLife();
        int uiHearts = lifeContainer.transform.childCount;
        if (current < uiHearts)
        {
            Destroy(lifeContainer.transform.GetChild(uiHearts - 1).gameObject);
        }
        if (current > uiHearts)
        {
            Instantiate(heart, new Vector3(lifeContainer.transform.position.x + 50 + uiHearts * 100, lifeContainer.transform.position.y, lifeContainer.transform.position.z), lifeContainer.transform.rotation, lifeContainer.transform);
        }
    }
    void coinCounter()
    {
        coinText.text = score.numberOfCoins.ToString() + " x ";
    }
    void pause()
    {
        if (Input.GetButtonDown("Cancel") && !looseScreen.activeSelf && !winScreen.activeSelf)
        {
            pauseMenu.SetActive(true);
            //stop the game
            Time.timeScale = 0;
            //stop audio
            //AudioListener.volume = 0;
            player.stopGameSounds();
        }
    }
    public void closeMenu()
    {
        pauseMenu.SetActive(false);
        //start the game
        Time.timeScale = 1;
        //start audio
        player.startGameSounds();
        //AudioListener.volume = 1;
    }
    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
