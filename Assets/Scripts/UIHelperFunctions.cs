using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIHelperFunctions : MonoBehaviour
{
    public void LoadScene(string scene_name)
    {
        // Restart game timer when game scene is loaded
        if (scene_name == "Main Scene")
            Time.timeScale = 1;
        SceneManager.LoadScene(scene_name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
