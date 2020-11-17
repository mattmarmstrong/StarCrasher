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
    public int integrity; //Turf "health"
    public Turf under; //Turf to be spawned once the current one is destroyed
}