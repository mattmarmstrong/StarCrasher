using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurfManager
{
    private TurfRecord record;

    public TurfManager(string path = "Turfs")
    {
        record = GetTurfsFromResources(path);
    }
    private TurfRecord GetTurfsFromResources(string path)
    {
        Turf[] loadedTurfs = Resources.LoadAll<Turf>(path);
        Dictionary<int, Turf> turfDict = new Dictionary<int, Turf>();
        foreach(Turf t in loadedTurfs) {
            Debug.Log(t.name);
            turfDict[t.id] = t;
        }
        return new TurfRecord(turfDict);
    }
    public void ReloadTurfs(string path = "Turfs") {
        record = GetTurfsFromResources(path);
    }
    public TurfRecord Turfs {
        get {return record;}
    }
}

public class TurfRecord {
    private Turf[] Turfs;
    public TurfRecord(Dictionary<int, Turf> turfsFromFile)
    {
        int arraySize = turfsFromFile.Keys.Max();
        Turfs = new Turf[arraySize+1]; //+1 to account for 0
        for (int i=0; i < arraySize; i++) {
            try {
                Turfs[i] = turfsFromFile[i];
            } catch (KeyNotFoundException) {
                Turfs[i] = null;
            }
        }
    }
    public Turf this[int index]
    {
        get => Turfs[index];
    }
    public int Length => Turfs.Length;
}