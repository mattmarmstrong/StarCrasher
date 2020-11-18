using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIHelperFunctions : MonoBehaviour
{
    public void LoadScene(string scene_name)
    {

        if (scene_name == "Main Scene")
        {
            // Restart game timer when game scene is loaded
            Time.timeScale = 1;
        }
        else if(scene_name == "MainMenu" || scene_name == "GameOver")
        {
            // Destroy Level Object so new game starts fresh
            Destroy(GameObject.Find("Level"));
        }


        SceneManager.LoadScene(scene_name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
