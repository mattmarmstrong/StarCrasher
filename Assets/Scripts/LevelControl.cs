using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*\
Main level controller, handles progression and generation of new maps, and any global data needed between scenes
\*/
public class LevelControl : MonoBehaviour {
    public static LevelControl Instance { get; private set; }
    //### Persistient Variables
    public int progress = 0; //Number of levels succeeded
    public GameObject player; //The Players controlled GameObject
    public string mapSeed = "random"; //The seed given to the map generator, "random" will generate a unique seed based on the system clock.
    public TurfManager loadedTurfs; //Turf objects loaded from file, used in map generation.
    //###
    void Start()
    {
        Debug.Log("Starting Game");
        loadedTurfs = new TurfManager("Turfs");
    }
    public void NextLevel()
    {
        //Reloads the scene and lets the map generator take over.
        progress++;
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
