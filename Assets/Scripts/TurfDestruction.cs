using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurfDestruction : MonoBehaviour
{
    Tilemap compTilemap;
    // Start is called before the first frame update
    private void Start()
    {
        compTilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(false) { //Currently unused
            Vector3 hitPos = Vector3.zero;
            foreach(ContactPoint2D hit in collision.contacts) {
                hitPos.x = hit.point.x - 0.01f * hit.normal.x;
                hitPos.y = hit.point.x - 0.01f * hit.normal.y;
                Turf hitTurf = (Turf)compTilemap.GetTile(compTilemap.WorldToCell(hitPos));
                Destroy(hitTurf);
            }
        }
    }
}
