using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public GameObject player;

    bool posInitialized = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        // Set target for pathfinding
        var aiDestSetter = GetComponent<AIDestinationSetter>();
        aiDestSetter.target = player.transform;

        
    }

    private void Update()
    {

    }
}
