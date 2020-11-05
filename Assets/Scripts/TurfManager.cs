using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

enum TurfRecord {
    StoneFloor,
    StoneWall,
    SolidWall
}
public class TurfManager : MonoBehaviour
{
    private List<Turf> TurfList;

    private void Start()
    {
        TurfList = new List<Turf>();
        GetTurfsFromResources();
    }

    public Turf GetTurf(int index) {
        return TurfList[index];
    }
    private void GetTurfsFromResources()
    {
        foreach(string name in Enum.GetNames(typeof(TurfRecord))) {
            TurfList.Add(Resources.Load<Turf>("Turfs/" + name));
        }
    }
}
