using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mapdraw : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap_open;
    [SerializeField]
    private Tilemap tilemap_closed;
    public TurfManager Turfs;
    public mapgen mapGrid;
    private int mapScanned = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (List<Chunk> chunkList in mapGrid.chunkMap) {
            foreach(Chunk ch in chunkList) {
                UpdateChunk(ch);
            }
        }
    }

    public void DrawFromMap(int[,] map,int chunkX = 0, int chunkY = 0) {

        for (int x = 0; x < map.GetUpperBound(0); x++){
            for (int y = 0; y < map.GetUpperBound(1); y++) {
                int trueX = x + map.GetUpperBound(0) * chunkX;
                int trueY = y + map.GetUpperBound(1) * chunkY;
                Turf t = Turfs.GetTurf(map[x,y]);
                if(t is OpenTurf) {
                    tilemap_open.SetTile(new Vector3Int(trueX, trueY, 0), t);
                } else {
                    tilemap_closed.SetTile(new Vector3Int(trueX, trueY, 0), t);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Chunk clickedChunk = mapGrid.LocateChunk(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            int[] tilepos = clickedChunk.GetTilePoint(pos);
            Debug.Log(string.Format("X:{0}, Y:{1}", tilepos[0], tilepos[1]));
            clickedChunk.TogglePoint(tilepos[0], tilepos[1]);
            UpdateChunk(clickedChunk);
        }
        // if (mapScanned < 2)
        // {
        //     Debug.Log("Scanning...");
        //     AstarPath.active.Scan();
        //     mapScanned++;
        //     Debug.Log("After Scan.");
        // }

    }

    void UpdateChunk(Chunk ch)
    {
        int[,] chunkMap = ch.GetMapArray();
        DrawFromMap(chunkMap, ch.mapPosX, ch.mapPosY);
    }
    private void ClearMap()
    {
        tilemap_closed.ClearAllTiles();
        tilemap_open.ClearAllTiles();
    }
}
