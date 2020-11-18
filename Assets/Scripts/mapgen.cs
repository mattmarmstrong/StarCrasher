using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

/*\ Enum reference to turfs
ensure they match the ID numbers given in Resources/Turfs \*/
enum tileIndex {
    ground,
    wall,
    unbreakable
}

public class mapgen : MonoBehaviour {
    public mapdraw mapRenderer;
    public int mapSize;
    [Range(0, 100)]
    public float fillProb;
    public int smoothCount;
    public int roomSizeThreshold;
    private System.Random mapRng;
    private Chunk currentMap;

    void Start()
    {
        SeedRng(LevelControl.Instance.mapSeed);
        currentMap = new Chunk(mapSize, smoothCount, mapRng);
        LoadMap();
    }

    private async void LoadMap()
    {
        await Task.Run(() => currentMap.Build(fillProb, roomSizeThreshold));
        LevelControl.Instance.MapLoad(currentMap);
    }

    void SeedRng(string seed = "random")
    {
        if(seed == "random") {
            seed = Time.time.ToString();
        }
        mapRng = new System.Random(seed.GetHashCode());
    }
}
public class Chunk
{
    int[,] mapArray;
    List<Room> areas;

    private int smoothCount;
    private System.Random random;
    public int[,] map
    {
        get {return mapArray;}
    }
    public int size
    {
        get {return mapArray.GetUpperBound(0);}
    }
    public Chunk(int size, int smCount, System.Random rng)
    {
        mapArray = new int[size, size];
        smoothCount = smCount;
        random = rng;
    }
    public async void Build(float fillProb, int roomSizeThreshold)
    {
        FillMap(fillProb);
        for (int i = 0; i < smoothCount; i++) {
            SmoothMap();
        }

        var roomTask = FillPocketAreas(roomSizeThreshold);
        List<Room> chunkRooms = await roomTask;
    }

    struct Coord
    {
        public int posX;
        public int posY;
        public Coord(int x, int y) {
            posX = x;
            posY = y;
        }
    }

    //Fills mapArray with random noise
    void FillMap(float fillProb)
    {
        for (int x = 0; x < mapArray.GetUpperBound(0); x++){
            for (int y = 0; y < mapArray.GetUpperBound(1); y++) {
                GenerateTile(x, y, fillProb);
            }
        }
    }
    //Determines if a tile should be filled semi-randomly, used in FillMap()
    private void GenerateTile(int xPos, int yPos, float fillProb)
    {
        if(IsInMapBorder(xPos, yPos)) {
            mapArray[xPos, yPos] = (int)tileIndex.unbreakable;
        } else {
            mapArray[xPos, yPos] = (random.Next(0, 100) < fillProb)? 1 : 0;
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < mapArray.GetUpperBound(0); x++){
            for (int y = 0; y < mapArray.GetUpperBound(1); y++) {
                if(IsInMapBorder(x, y)) {
                    continue; //Ignores border walls, which should be indestructable
                }
                int neighbouringWalls = CountAdjacentWalls(x, y);
                if (neighbouringWalls > 5) {
                    mapArray[x,y] = 1;
                } else if (neighbouringWalls < 4) {
                    mapArray[x,y] = 0;
                }
            }
        }
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < mapArray.GetUpperBound(0) && y >= 0 && y < mapArray.GetUpperBound(1);
    }

    bool IsInMapBorder(int x, int y)
    {
        return (x == 0 || x == mapArray.GetUpperBound(0)-1 || y == 0 || y == mapArray.GetUpperBound(1)-1);
    }

    public int CountAdjacentWalls(int xPos, int yPos)
    {
        int wallCount = 0;
        for (int nearX = xPos-1; nearX <= xPos+1; nearX++) {
            for (int nearY = yPos-1; nearY <= yPos+1; nearY++) {
                if(nearX == xPos && nearY == yPos) {continue;} // Ignore the tile we're looking around
                if(IsInMapRange(nearX, nearY)) { //Count edge-cases as walls
                    wallCount += (mapArray[nearX, nearY] > 0)? 1 : 0; //Incase we introduce more complex tiles greater than 1
                } else {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }
    
    List<Coord> GetArea(int startX, int startY) {
        List<Coord> area = new List<Coord>();
        int[,] mapFlags = new int[mapArray.GetUpperBound(0), mapArray.GetUpperBound(1)];
        int posType = mapArray[startX, startY];
        
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0) {
            Coord point = queue.Dequeue();
            area.Add(point);
            for (int x = point.posX-1; x <= point.posX+1; x++) {
                for (int y = point.posY-1; y <= point.posY+1; y++) {
                    if(IsInMapRange(x, y) && (y == point.posY || x == point.posX) && (mapFlags[x, y] == 0 && mapArray[x,y] == posType)) {
                        mapFlags[x, y] = 1;
                        queue.Enqueue(new Coord(x, y));
                    }
                }
            }
        }
        return area;
    }

    List<List<Coord>> GetAllAreas(int posType) {
        List<List<Coord>> rooms = new List<List<Coord>>();
        int[,] mapFlags = new int[mapArray.GetUpperBound(0), mapArray.GetUpperBound(1)];
        for (int x = 0; x < mapArray.GetUpperBound(0); x++){
            for (int y = 0; y < mapArray.GetUpperBound(1); y++) {
                if (mapFlags[x,y] == 0 && mapArray[x,y] == posType) {
                    List<Coord> newRoom = GetArea(x, y);
                    rooms.Add(newRoom);

                    foreach (Coord pos in newRoom) {
                        mapFlags[pos.posX, pos.posY] = 1;
                    }
                }
            }
        }
        return rooms;
    }
    //Tracks and fills small "pocket" rooms, returns the remaining rooms
    async Task<List<Room>> FillPocketAreas(int threshold) {
        List<List<Coord>> roomRegions = GetAllAreas(0);
        List<Task> roomsToFill = new List<Task>();
        List<Room> keptRooms = new List<Room>();
        foreach (List<Coord> region in roomRegions) {
            if (region.Count < threshold) {
                roomsToFill.Add(FillPocket(region));
            } else {
                keptRooms.Add(new Room(region, mapArray));
            }
        }
        await Task.WhenAll(roomsToFill);
        return keptRooms;
    }
    Task FillPocket(List<Coord> region)
    {
        return Task.Run(() => {
            foreach(Coord c in region) {
                mapArray[c.posX, c.posY] = 1;
            }
        });
    }

    class Room
    {
        public List<Coord> area;
        public List<Coord> border;
        public List<Room> linkedRooms;
        public int size;

        public Room(List<Coord> roomArea, int[,] mapArray)
        {
            area = roomArea;
            size = area.Count;
            linkedRooms = new List<Room>();

            border = new List<Coord>();
            foreach (Coord pos in area) {
                for (int x = pos.posX-1; x<=pos.posX+1; x++) {
                    for(int y = pos.posY-1; y<=pos.posY+1; y++) {
                        if(x == pos.posX || y == pos.posY) {
                            if(mapArray[x,y]==1) {
                                border.Add(pos);
                            }
                        }
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // if (mapArray == null) {return;}
        // for (int x = 0; x < mapWidth; x++) {
        //     for (int y = 0; y < mapHeight; y++) {
        //         Gizmos.color = (mapArray[x, y] == 1)? Color.black : Color.white;
        //         Vector3 pos = new Vector3(-mapWidth/2 + x + .5f, -mapHeight/2 + y + .5f, 0);
        //         Gizmos.DrawCube(pos, Vector3.one);
        //     }
        // }
    }
}
