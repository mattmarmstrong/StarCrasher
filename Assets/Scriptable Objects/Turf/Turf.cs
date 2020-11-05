using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*\
Turf - Custom Map tiles with additional properties
Any turfs used by the map generator must go to Resources/Turfs to be collected at runtime.
\*/
public class Turf : Tile {
    [SerializeField]
    public int id; // Every tile type must have a unique ID for file loading
}

[CreateAssetMenuAttribute(fileName="ClosedTurf", menuName="Turf/ClosedTurf")]
public class ClosedTurf : Turf
{
    public int integrity; //Damage taken before breaking
    public OpenTurf turfUnder; //Open turf created when destroyed
}
[CreateAssetMenuAttribute(fileName="OpenTurf", menuName="Turf/OpenTurf")]
public class OpenTurf : Turf {
    
}