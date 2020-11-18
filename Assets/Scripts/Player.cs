using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : ObjectWithStatsOrInventory
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public GameObject EquipmentScreen;
    public GameObject InventoryScreen;

    public float speed;
    private Rigidbody2D rb2d;
    private Vector2 moveVelocity;

    public int Stamina = 10;
    public int Strength = 10;
    public int Intellect = 10;
    public int Agility = 10;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        EquipmentScreen.transform.Find("Stamina").GetChild(0).GetComponent<TextMeshProUGUI>().text = Stamina.ToString();
        EquipmentScreen.transform.Find("Strength").GetChild(0).GetComponent<TextMeshProUGUI>().text = Strength.ToString();
        EquipmentScreen.transform.Find("Intellect").GetChild(0).GetComponent<TextMeshProUGUI>().text = Intellect.ToString();
        EquipmentScreen.transform.Find("Agility").GetChild(0).GetComponent<TextMeshProUGUI>().text = Agility.ToString();
        LevelControl.Instance.OnMapLoad += SpawnInMap;
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (EquipmentScreen.activeSelf)
            {
                EquipmentScreen.SetActive(false);
            }
            else
            {
                EquipmentScreen.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (InventoryScreen.activeSelf)
            {
                InventoryScreen.SetActive(false);
            }
            else
            {
                InventoryScreen.SetActive(true);
            }
        }

    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + moveVelocity * Time.fixedDeltaTime);
    }

    void SpawnInMap(Chunk level)
    {
        Grid mapGrid = GameObject.Find("Grid").GetComponent<Grid>();
        int[,] mapArray = level.map;
        for (int x = 0; x < mapArray.GetUpperBound(0); x++){
            for (int y = 0; y < mapArray.GetUpperBound(1); y++) {
                if(mapArray[x,y] == 0 && level.CountAdjacentWalls(x,y) == 0) {
                    transform.position = mapGrid.CellToWorld(new Vector3Int(x, y, 0));
                    return;
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            if (inventory.AddItem(new Item(item.itemObject), 1))
            {
                Destroy(other.gameObject);
            }
        }

    }
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
        equipment.Container.Clear();
    }
}
