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
        if (!posInitialized)
        {
            Vector3 startPos;
            if (this.name == "Enemy1")
                startPos = player.transform.position + new Vector3(0.5f, 0.1f, 0f);
            else
                startPos = player.transform.position + new Vector3(-0.5f, 0.1f, 0f);
            transform.position = startPos;
            posInitialized = true;
        }

    }
}
