using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurfManager : MonoBehaviour
{
    private Turf[] turfs;

    [SerializeField]
    private Turf floorTurf;
    [SerializeField]
    private Turf wallTurf;
    [SerializeField]
    private Turf unbreakableTurf;

    private void Awake()
    {
        turfs = new Turf[] {
            floorTurf,
            wallTurf,
            unbreakableTurf
        };
    }
    public Turf this[int i]
    {
        get { return turfs[i]; }
    }
}