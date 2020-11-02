using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public Player player;
    //public AStar aStar;
    //public float enemySpeed;

    //public Tilemap groundTilemap = null;
    //public TileBase pathTile = null;
    //public TileBase groundTile = null;

    //private Stack<Vector3Int> pathToPlayer = null;
    //private Vector3Int playerPosition;
    //private Vector3Int oldPlayerPosition = new Vector3Int(0, 0, 0);

    //bool caughtPlayer = false;

    bool posInitialized = false;
    //public Grid worldGrid;
    //public mapgen mapGrid;
    //private Chunk curChunk;

    //public CapsuleCollider2D enemyCollider;
    //public CompositeCollider2D wallCollider;

    //private bool mapScanned = false;

    private void Start()
    {

        //Physics2D.IgnoreCollision(enemyCollider, wallCollider);
    }

    private void Update()
    {
        if (!posInitialized)
        {
            transform.position = player.transform.position + new Vector3(0.5f, 0.1f, 0f);
            posInitialized = true;
        }

    }
}

    /*
        // Update is called once per frame
        void Update()
        {
            if (!posInitialized)
            {
                Debug.Log("Enemy name = " + this.name);
                Vector3 startPos;
                if (this.name == "Enemy1")
                    startPos = player.transform.position + new Vector3(0.5f, 0.1f, 0f);
                else
                    startPos = player.transform.position + new Vector3(-0.5f, 0.1f, 0f);
                //transform.position = player.transform.position + new Vector3(0.5f, 0.1f, 0f);
                transform.position = startPos;
                posInitialized = true;
                //Debug.Log("Enemy Grid Position = " + getTilePos(transform.position));
                //Debug.Log("Enemy World Position = " + getWorldPos(getTilePos(transform.position)));
                //Debug.Log("Player Position = " + getTilePos(player.transform.position));
            }

            //Vector3Int myPosition = Vector3Int.RoundToInt(transform.position);
            //playerPosition = Vector3Int.RoundToInt(player.transform.position);

            // Set positions to be tile positions
            Vector3Int myPosition = getTilePos(transform.position);
            playerPosition = getTilePos(player.transform.position);


            // Find a new path if the player has moved
            if ((playerPosition != oldPlayerPosition) && (!caughtPlayer) )
            {
                //Debug.Log("playerPosition does not equal oldPlayerPosition");
                oldPlayerPosition = playerPosition;
                //playerPosition = Vector3Int.RoundToInt(player.transform.position);
                findNewPath(playerPosition, myPosition);
            }
            if(!caughtPlayer)
                // Move the enemy along the found path
                moveAlongPath();

        }

        private Vector3Int getTilePos(Vector3 pos)
        {
            curChunk = mapGrid.LocateChunk(pos);
            int[] tilepos = curChunk.GetTilePoint(pos);
            return new Vector3Int(tilepos[0], tilepos[1], 0);

        }

        private Vector3 getWorldPos(Vector3Int pos)
        {
            return curChunk.GetWorldPos(pos);
        }

        private void findNewPath(Vector3Int playerPosition, Vector3Int myPosition)
        {
            // Compute shortest path to the player using AStar
            Stack<Vector3Int> path = aStar.aStarGetPath(myPosition, playerPosition, curChunk);

            //TODO maybe check if myPos and playerPos are on walls before sending to pathfinding
                // Could just find a close walkable tile to use instead?


            // Clear the old path from the screen
            aStar.clearPath(pathToPlayer);

            // Draw the new path
            aStar.colourPath(path);

            // Set the new path to move along
            pathToPlayer = path;

        }
    */

    //private void moveAlongPath()
    /* Used parts of the example code in the docs at the following link.
     * https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html 
     */
/*    {
        // Don't do anything if the path is empty
        if ((pathToPlayer is null) || (pathToPlayer.Count == 0))
            return;
        float speed = enemySpeed * Time.deltaTime;
        Vector3 targetPos = getWorldPos(pathToPlayer.Peek());
        //Debug.Log("Path point (world) = " + targetPos);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);

        // Remove the point from the stack when the enemy gets to it
        // Remove path colouring from the tile after the enemy reaches it
        if (Vector3.Distance(transform.position, targetPos) < 0.001f)
        {
            pathToPlayer.Pop();
            //groundTilemap.SetTile(pathToPlayer.Pop(), groundTile);
        } 
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            caughtPlayer = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            caughtPlayer = false;
    }

}
*/