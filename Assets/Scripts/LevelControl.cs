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
    public string mapSeed = "random"; //The seed given to the map generator, "random" will generate a unique seed based on the system clock.
    public Turf[] turfs; //Turf objects loaded, used in map generation.
    //### Events
    public delegate void MapHandler(Chunk map);
    public event MapHandler OnMapLoad; //Called when the map data has been loaded by mapgen.cs
    void Start()
    {

    }
    public void MapLoad(Chunk map)
    {
        if(OnMapLoad != null) {
            OnMapLoad(map);
        }
        Debug.Log("Map loaded");
    }
    public void NextLevel()
    {
        //Reloads the scene and lets the map generator take over.
        progress++;
        OnMapLoad = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    //Called on controller initialization
    private void Awake()
    {
        //Preserves instance
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
