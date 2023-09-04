using UnityEngine;
using UnityEditor;

public class Exit : MonoBehaviour
{
    public void exitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit(); 
        #endif
    }
}
