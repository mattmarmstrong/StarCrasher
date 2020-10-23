using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    public float movementSpeed = 0.5f;
    public Tilemap groundTilemap = null;
    public Tilemap wallTilemap = null;
    public TileBase groundTile = null;
    public TileBase pathTile = null;
    public Rigidbody2D playerBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from arrow keys or WASD
        float horizontalInput = Input.GetAxisRaw("Horizontal") * movementSpeed;
        float verticalInput = Input.GetAxisRaw("Vertical") * movementSpeed;

        // move freely according to keyboard input
        transform.Translate(new Vector3(horizontalInput, verticalInput));
    }

    
}
