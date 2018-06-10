using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompBird : MonoBehaviour {

	private Rigidbody2D playerRigidbody;
	
	public float bounceForce;
	
	public GameObject deathSplosion;

	// Use this for initialization
	void Start () {
		playerRigidbody = transform.parent.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "EnemyBird")
		{
			//Destroy(other.gameObject);
			
			other.gameObject.SetActive(false);
			
			Instantiate(deathSplosion, other.transform.position, other.transform.rotation);
		    if(other.GetComponent<HurtPlayer>())
            {
                other.GetComponent<HurtPlayer>().lastCollideTime = System.DateTime.Now;
            }
			playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, bounceForce, 0f);
		}
	}
}
