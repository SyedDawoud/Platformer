using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	
	public Transform leftPoint;
	public Transform rightPoint;
	
	public float moveSpeed;
	
	private Rigidbody2D myRigidbody;
	
	public bool movingRight;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(movingRight && transform.position.x > rightPoint.position.x)
		{
			movingRight = false;
		}	
		if(!movingRight && transform.position.x < leftPoint.position.x)
		{
			movingRight = true;
		}
		
		if(movingRight)
		{
			myRigidbody.velocity = new Vector3(moveSpeed, myRigidbody.velocity.y, 0f);
			transform.localScale = new Vector3(-1f,1f,1f);
		} else {
			myRigidbody.velocity = new Vector3(-moveSpeed, myRigidbody.velocity.y, 0f);
			transform.localScale = new Vector3(1f,1f,1f);
		}		
	}
}
