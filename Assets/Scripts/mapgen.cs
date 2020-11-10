using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

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
        currentMap = new Chunk(mapSize, smoothCount, roomSizeThreshold, fillProb, mapRng);
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
    int[,] map;


    public Chunk(int size, int smoothCount, int roomSizeThreshold, float fillProb, System.Random rng)
    {
        map = new int[size, size];
        BuildChunk(smoothCount, roomSizeThreshold, fillProb, rng);
    }

    private async void BuildChunk(int smoothCount, int roomSizeThreshold, float fillProb, System.Random rng)
    {
        //Debug.Log(String.Format("Starting Chunk {0},{1}", mapPosX, mapPosY));
        FillMap(fillProb, rng);
        //Debug.Log("Filled...");
        for (int i = 0; i < smoothCount; i++) {
            SmoothMap();
        }
        //Debug.Log("Smoothed...");

        var roomTask = FillPocketAreas(roomSizeThreshold);
        List<Room> chunkRooms = await roomTask;
        //Debug.Log("Pockets Filled...");
        // ConnectNearbyRooms(chunkRooms, tunnelSize);
        // Debug.Log("Tunnels Dug... Next");
    }

    public int[,] GetMapArray()
    {
        return map;
    }

    public void TogglePoint(int x, int y)
    {
        Debug.Log(String.Format("X:{0} Y:{1}", x, y));
        if(map[x,y] == 0) {
            map[x,y] = 1;
        } else {
            map[x,y] = 0;
        }
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

    //Fills map with random noise
    void FillMap(float fillProb, System.Random rng)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++){
            for (int y = 0; y < map.GetUpperBound(1); y++) {
                map[x,y] = GenerateTile(x, y, fillProb, rng);
            }
        }
    }
    //Determines if a tile should be filled semi-randomly, used in FillMap()
    private int GenerateTile(int xPos, int yPos, float fillProb, System.Random rng)
    {
        if(xPos == 0 || xPos == map.GetUpperBound(0)-1 || yPos == 0 || yPos == map.GetUpperBound(1)-1) {
            return 1;
        } else {
            return (rng.Next(0, 100) < fillProb)? 1 : 0;
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < map.GetUpperBound(0); x++){
            for (int y = 0; y < map.GetUpperBound(1); y++) {
                int neighbouringWalls = CountAdjacentWalls(x, y);
                if (neighbouringWalls > 5) {
                    map[x,y] = 1;
                } else if (neighbouringWalls < 4) {
                    map[x,y] = 0;
                }
            }
        }
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < map.GetUpperBound(0) && y >= 0 && y < map.GetUpperBound(1);
    }

    public int CountAdjacentWalls(int xPos, int yPos)
    {
        int wallCount = 0;
        for (int nearX = xPos-1; nearX <= xPos+1; nearX++) {
            for (int nearY = yPos-1; nearY <= yPos+1; nearY++) {
                if(nearX == xPos && nearY == yPos) {continue;} // Ignore the tile we're looking around
                if(IsInMapRange(nearX, nearY)) { //Count edge-cases as walls
                    wallCount += (map[nearX, nearY] > 0)? 1 : 0; //Incase we introduce more complex tiles greater than 1
                } else {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }
    
    List<Coord> GetArea(int startX, int startY) {
        List<Coord> area = new List<Coord>();
        int[,] mapFlags = new int[map.GetUpperBound(0), map.GetUpperBound(1)];
        int posType = map[startX, startY];
        
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0) {
            Coord point = queue.Dequeue();
            area.Add(point);
            for (int x = point.posX-1; x <= point.posX+1; x++) {
                for (int y = point.posY-1; y <= point.posY+1; y++) {
                    if(IsInMapRange(x, y) && (y == point.posY || x == point.posX) && (mapFlags[x, y] == 0 && map[x,y] == posType)) {
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
        int[,] mapFlags = new int[map.GetUpperBound(0), map.GetUpperBound(1)];
        for (int x = 0; x < map.GetUpperBound(0); x++){
            for (int y = 0; y < map.GetUpperBound(1); y++) {
                if (mapFlags[x,y] == 0 && map[x,y] == posType) {
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
                keptRooms.Add(new Room(region, map));
            }
        }
        await Task.WhenAll(roomsToFill);
        return keptRooms;
    }
    Task FillPocket(List<Coord> region)
    {
        return Task.Run(() => {
            foreach(Coord c in region) {
                map[c.posX, c.posY] = 1;
            }
        });
    }

    void ConnectNearbyRooms(List<Room> allRooms, int tunnelSize)
    {
        int closestDist = 0;
        Coord bestACoord = new Coord();
        Coord bestBCoord = new Coord();
        Room bestARoom = new Room();
        Room bestBRoom = new Room();
        bool foundLink = false;
        foreach (Room roomA in allRooms) {
            foundLink = false;
            foreach (Room roomB in allRooms) {
                if (roomA == roomB) {continue;}
                if (roomA.IsJoined(roomB)) {
                    foundLink = true;
                    break;
                }
                for (int indexA=0; indexA<roomA.border.Count; indexA++) {
                    for (int indexB=0; indexB<roomB.border.Count; indexB++) {
                        Coord posA = roomA.border[indexA];
                        Coord posB = roomB.border[indexB];
                        int distanceOfRooms = (int)(Mathf.Pow(posA.posX-posB.posX, 2) + Mathf.Pow(posA.posY-posB.posY, 2));

                        if(distanceOfRooms < closestDist || !foundLink) {
                            closestDist = distanceOfRooms;
                            foundLink = true;
                            bestACoord = posA;
                            bestBCoord = posB;
                            bestARoom = roomA;
                            bestBRoom = roomB;
                        }
                    }
                }
            }
            if (foundLink) {
                Task.Run(() => BuildTunnel(bestARoom, bestBRoom, bestACoord, bestBCoord, tunnelSize));
            }
        }
    }

    void BuildTunnel(Room roomA, Room roomB, Coord pointA, Coord pointB, int size = 1)
    {
        Room.JoinRooms(roomA, roomB);

        List<Coord> line = GetLine(pointA, pointB);
        foreach (Coord c in line) {
            DigCircle(c, size);
        }
    }

    void DigCircle(Coord c, int r)
    {
        for(int x = -r; x <= r; x++) {
            for(int y = -r; y <= r; y++) {
                if (x*x + y*y <= r*r) {
                    int dugX = c.posX + x;
                    int dugY = c.posY + y;
                    if (IsInMapRange(dugX, dugY)) {
                        map[dugX, dugY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();
        int x = from.posX;
        int y = from.posY;
        int dx = to.posX - from.posX;
        int dy = to.posY - from.posY;

        bool invert = false;
        int step = Math.Sign(dx);
        int gStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest) {
            invert = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);
            step = Math.Sign(dy);
            gStep = Math.Sign(dx);
        }

        int gAccumulation = longest / 2;
        for (int i=0; i < longest; i++) {
            line.Add(new Coord(x,y));
            if(invert) {
                y += step;
            } else {
                x += step;
            }
            gAccumulation += shortest;
            if(gAccumulation >= longest) {
                if(invert) {
                    x += gStep;
                } else {
                    y += gStep;
                }
                gAccumulation -= longest;
            }
        }
        return line;
    }

    class Room
    {
        public List<Coord> area;
        public List<Coord> border;
        public List<Room> linkedRooms;
        public int size;

        public Room()
        {}

        public Room(List<Coord> roomArea, int[,] map)
        {
            area = roomArea;
            size = area.Count;
            linkedRooms = new List<Room>();

            border = new List<Coord>();
            foreach (Coord pos in area) {
                for (int x = pos.posX-1; x<=pos.posX+1; x++) {
                    for(int y = pos.posY-1; y<=pos.posY+1; y++) {
                        if(x == pos.posX || y == pos.posY) {
                            if(map[x,y]==1) {
                                border.Add(pos);
                            }
                        }
                    }
                }
            }
        }

        public static void JoinRooms(Room roomA, Room roomB)
        {
            roomA.linkedRooms.Add(roomB);       
        }

        public bool IsJoined(Room otherRoom)
        {
            return linkedRooms.Contains(otherRoom);
        }
    }

    void OnDrawGizmos()
    {
        // if (map == null) {return;}
        // for (int x = 0; x < mapWidth; x++) {
        //     for (int y = 0; y < mapHeight; y++) {
        //         Gizmos.color = (map[x, y] == 1)? Color.black : Color.white;
        //         Vector3 pos = new Vector3(-mapWidth/2 + x + .5f, -mapHeight/2 + y + .5f, 0);
        //         Gizmos.DrawCube(pos, Vector3.one);
        //     }
        // }
    }
}
