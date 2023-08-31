using UnityEngine;

public class Exit : MonoBehaviour
{
    public void exitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
