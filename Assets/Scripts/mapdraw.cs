﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mapdraw : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap_open;
    [SerializeField]
    private Tilemap tilemap_closed;
    private TurfRecord turfs;
    public mapgen mapGrid;
    private int mapScanned = 0;

    // Start is called before the first frame update
    void Start()
    {
        turfs = LevelControl.Instance.loadedTurfs.Turfs;
    }

    public void DrawFromMap(int[,] map,int chunkX = 0, int chunkY = 0) {

        for (int x = 0; x < map.GetUpperBound(0); x++){
            for (int y = 0; y < map.GetUpperBound(1); y++) {
                Turf t = turfs[map[x,y]];
                if(t is OpenTurf) {
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
        // if (mapScanned < 2)
        // {
        //     Debug.Log("Scanning...");
        //     AstarPath.active.Scan();
        //     mapScanned++;
        //     Debug.Log("After Scan.");
        // }

    }
    private void ClearMap()
    {
        tilemap_closed.ClearAllTiles();
        tilemap_open.ClearAllTiles();
    }
}
