using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mapdraw : MonoBehaviour
{
    public Tilemap tilemap_open;
    public Tilemap tilemap_closed;
    public Tile floorTile;
    public Tile wallTile;

    public mapgen mapGrid;

    // Start is called before the first frame update
    void Start()
    {
        foreach (List<Chunk> chunkList in mapGrid.chunkMap) {
            foreach(Chunk ch in chunkList) {
                UpdateChunk(ch);
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
    }

    void UpdateChunk(Chunk ch)
    {
        int[,] chunkMap = ch.GetMapArray();
        for (int x = 0; x < chunkMap.GetUpperBound(0); x++){
            for (int y = 0; y < chunkMap.GetUpperBound(1); y++) {
                int trueX = x + chunkMap.GetUpperBound(0) * ch.mapPosX;
                int trueY = y + chunkMap.GetUpperBound(1) * ch.mapPosY;
                if(chunkMap[x,y] == 0) {
                    tilemap_closed.SetTile(new Vector3Int(trueX, trueY, 0), null);
                    tilemap_open.SetTile(new Vector3Int(trueX, trueY, 0), floorTile);
                } else {
                    tilemap_open.SetTile(new Vector3Int(trueX, trueY, 0), null);
                    tilemap_closed.SetTile(new Vector3Int(trueX, trueY, 0), wallTile);
                }
            }
        }
    }
    private void ClearMap()
    {
        tilemap_closed.ClearAllTiles();
        tilemap_open.ClearAllTiles();
    }
}
