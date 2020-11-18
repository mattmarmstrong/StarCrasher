using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mapdraw : MonoBehaviour
{
    [SerializeField]
    public Tilemap tilemap_open;
    [SerializeField]
    public Tilemap tilemap_closed;
    [SerializeField]
    public GameObject enemy;
    [SerializeField]
    public int numEnemies;

    private int mapScanned = 0;

    // Start is called before the first frame update
    void Start()
    {
        LevelControl.Instance.OnMapLoad += DrawFromMap;
    }

    public void DrawFromMap(Chunk mapData) {
        int createdEnemies = 0;


        int[,] map = mapData.map;
        TurfManager turfs = LevelControl.Instance.turfs;
        for (int x = 0; x < map.GetUpperBound(0); x++){
            for (int y = 0; y < map.GetUpperBound(1); y++) {
                Turf t = turfs[map[x,y]];

                /* Test if this cell is near a wall to avoid placing enemies
                    inside the walls with a rounding error */
                bool nearWall = false;
                for(int i = x - 1; i <= x + 1; i++)
                {
                    for(int j = y - 1; j <= y + 1; j++)
                    {
                        if (i < map.GetUpperBound(0) && i > map.GetLowerBound(0) &&
                            j < map.GetUpperBound(1) && j > map.GetLowerBound(1) &&
                            !turfs[map[i, j]].open)
                            nearWall = true;
                    }
                }

                if (t.open) {
                    // Place enemies across the map
                    if (x % 10 == 0 && y % 10 == 0 && createdEnemies < numEnemies && !nearWall)
                    {
                        Vector3 worldCoord = GetComponent<Grid>().CellToWorld(new Vector3Int(x, y, 0));
                        Instantiate(enemy, worldCoord, Quaternion.identity);
                        createdEnemies++;
                    }
                    tilemap_open.SetTile(new Vector3Int(x, y, 0), t);
                    
                } else {
                    tilemap_closed.SetTile(new Vector3Int(x, y, 0), t);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0)) {
        //     Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     Chunk clickedChunk = mapGrid.LocateChunk(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //     int[] tilepos = clickedChunk.GetTilePoint(pos);
        //     Debug.Log(string.Format("X:{0}, Y:{1}", tilepos[0], tilepos[1]));
        //     clickedChunk.TogglePoint(tilepos[0], tilepos[1]);
        //     UpdateChunk(clickedChunk);
        //     AstarPath.active.UpdateGraphs(new Bounds(pos, new Vector3(1, 1, 0)));
        // }

        // Scan for two frames to make sure map is loaded when scanning
        if (mapScanned < 2)
        {
            Debug.Log("Scanning...");
            AstarPath.active.Scan();
            mapScanned++;
            Debug.Log("After Scan.");
        }

    }
    private void ClearMap()
    {
        tilemap_closed.ClearAllTiles();
        tilemap_open.ClearAllTiles();
    }
}
