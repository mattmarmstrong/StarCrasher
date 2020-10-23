using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playersimple : MonoBehaviour
{
    Rigidbody2D rbody;
    Vector2 speed;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
    }

    void FixedUpdate() {
        rbody.velocity = speed;
    }
}
