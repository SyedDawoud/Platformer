using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller))]
public class Player : MonoBehaviour {

    float gravity = -10;
    Vector2 velocity;


    Controller controller;
	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {
        // Avoid Gravity Accumulation
        if( controller.col_info.above || controller.col_info.below)
        {
            velocity.y = 0;
        }

        Vector2 inp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = 5;
        }
        velocity.x = inp.x * 5f;
        // Making gravity effect the y velocity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	}
}
