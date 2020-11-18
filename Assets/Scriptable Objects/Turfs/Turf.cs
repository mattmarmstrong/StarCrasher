using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*\
Turf - Custom Map tiles with additional properties
Any turfs used by the map generator must go to Resources/Turfs to be collected at runtime.
\*/
[CreateAssetMenuAttribute(fileName="Turf", menuName="Turf")]
public class Turf : Tile {
    [SerializeField]
    public int id; // Every tile type must have a unique ID for file loading
    public bool open; // Is the tile meant to be collided with
    public int integrity; //Turf "health" set to a negitive value to be indestructible
    public Turf under; //Turf to be spawned once the current one is destroyed

    public void Damage(int dmg = 1)
    {
        if(integrity < 0) return;
        else {
            integrity -= dmg;
        }
        if(integrity <= 0) {
            Demolish();
        }
    }
    public void Demolish() {
        if(!under) return;
        Tilemap underMap;
        underMap = GameObject.Find(under.open ? "OpenTiles" : "ClosedTiles").GetComponent<Tilemap>();
        Vector3Int currentPos = new Vector3Int((int)transform[0,3], (int)transform[1,3], (int)transform[2,3]);
        underMap.SetTile(currentPos, under);
        Destroy(this);
    }
}