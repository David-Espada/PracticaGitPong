using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class MainMenu : MonoBehaviour

{

    [Header("Fade (opcional)")]

    public FadeController fadeController;  
    public string gameSceneName = "GameScene";
    public void StartGame()

    {
        if (fadeController != null)

            fadeController.FadeOutAndLoad(gameSceneName);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }
    public void QuitGame()

    {
        Debug.Log("Quit requested");

        #if UNITY_EDITOR
            EditorApplication.isPlaying = false; 
        #else
            Application.Quit(); 
        #endif
    }
}
 