using System.Collections.Generic;
using UnityEngine;

public class TurfManager : MonoBehaviour
{
    private Dictionary<int, Turf> TurfList;

    private void Start()
    {
        TurfList = new Dictionary<int, Turf>();
        GetTurfsFromResources();
    }

    public Turf GetTurf(int index) {
        return TurfList[index];
    }
    private void GetTurfsFromResources()
    {
        Turf[] loadedTurfs = Resources.LoadAll<Turf>("Turfs");
        foreach(Turf t in loadedTurfs) {
            Debug.Log(t.name);
            TurfList[t.id] = t;
        }
    }
}
