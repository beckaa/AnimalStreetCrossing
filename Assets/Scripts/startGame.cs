using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    AsyncOperation async;
    private void Start()
    {
        async= SceneManager.LoadSceneAsync("levelZero");
        async.allowSceneActivation = false;
    }
    public void start() => async.allowSceneActivation=true;
}
