using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public Player player;
    public AStar aStar;
    public float enemySpeed;

    public Tilemap groundTilemap = null;
    public TileBase pathTile = null;
    public TileBase groundTile = null;

    private Stack<Vector3Int> pathToPlayer = null;
    private Vector3Int playerPosition;

    // Update is called once per frame
    void Update()
    {
        Vector3Int myPosition = Vector3Int.RoundToInt(transform.position);

        // Find a new path if the player has moved
        if(playerPosition != player.transform.position)
        {
            playerPosition = Vector3Int.RoundToInt(player.transform.position);
            findNewPath(playerPosition, myPosition);
        }

        // Move the enemy along the found path
        moveAlongPath();

    }

    private void findNewPath(Vector3Int playerPosition, Vector3Int myPosition)
    {
        // Compute shortest path to the player using AStar
        Stack<Vector3Int> path = aStar.aStarGetPath(myPosition, playerPosition);

        // Clear the old path from the screen
        aStar.clearPath(pathToPlayer);

        // Draw the new path
        aStar.colourPath(path);

        // Set the new path to move along
        pathToPlayer = path;

    }

    private void moveAlongPath()
    /* Used parts of the example code in the docs at the following link.
     * https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html 
     */
    {
        // Don't do anything if the path is empty
        if ((pathToPlayer is null) || (pathToPlayer.Count == 0))
            return;
        float step = enemySpeed * Time.deltaTime;
        Vector3Int point = pathToPlayer.Peek();
        transform.position = Vector3.MoveTowards(transform.position, point, step);

        // Remove the point from the stack when the enemy gets to it
        // Remove path colouring from the tile after the enemy reaches it
        if (Vector3.Distance(transform.position, point) < 0.001f)
        {
            groundTilemap.SetTile(pathToPlayer.Pop(), groundTile);
        }
            
    }
}
