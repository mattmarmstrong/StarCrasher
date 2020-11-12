using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject optionsMenuPrefab;
    private GameObject createdMenuObject;
    private bool showingOptions;

    // Start is called before the first frame update
    void Start()
    {
        showingOptions = false;
        //optionsMenu.SetActive(showingOptions);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!showingOptions)
            {
                createdMenuObject = Instantiate(optionsMenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                Destroy(createdMenuObject);
            }
            showingOptions = !showingOptions;
            //showingOptions = !showingOptions;
            //optionsMenu.SetActive(showingOptions);
            //showingOptions = true;
        }
            
    }
}
