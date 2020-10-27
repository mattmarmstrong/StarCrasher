using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*\
Main level controller, handles progression and generation of new maps, and any global data needed between scenes
\*/
public class LevelControl : MonoBehaviour {
    public static LevelControl Instance { get; private set; }

    public int progress = 0; //Number of levels succeeded
    void Start()
    {
        Debug.Log("Starting Game...");
    }
    public void NextLevel()
    {
        //Reloads the scene and lets the map generator take over.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //Preserve Instance
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
