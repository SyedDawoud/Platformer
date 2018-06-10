using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTriggerScript : MonoBehaviour {
    // Player Box Collider
	public BoxCollider2D playerCollider;
    // Trigger Collider
	public BoxCollider2D triggerCollider;
    // Bats Object Reference
    public GameObject bats;
    // To make sure bats only appear once
    private bool firstBatAppearance;

    void Setup()
    {
        firstBatAppearance = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   // when player enters the trigger and bats haven't been activated
        if (other.gameObject.name == "Player" && !firstBatAppearance)
        {
            // Ignore collision b/w player and trigger
            Physics2D.IgnoreCollision(playerCollider, triggerCollider, true);
            // Active bats and set bool to true
            bats.SetActive(true);
            firstBatAppearance = true;
            // Destroy bats after 3 seconds
            Destroy(bats, 3);
            
        }
    }



}
