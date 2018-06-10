using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	
	public GameObject deathSplosion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Explosion")
		{
			//Destroy(other.gameObject);
			
			other.gameObject.SetActive(false);
			
			Instantiate(deathSplosion, other.transform.position, other.transform.rotation);
		}
	}
}
