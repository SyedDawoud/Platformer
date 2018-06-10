using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineScript : MonoBehaviour {
	
	bool onTop;
	GameObject bouncer;
	Animator anim;
	public Vector2 velocity;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionStay2D(Collision2D other)
	{
		if (onTop) {
			anim.SetBool ("isStepped", true);
			bouncer = other.gameObject;
		}
	}
	
	void OnTriggerEnter2D()
	{
		onTop = true;
	}
	
	void OnTriggerExit2D()
	{
		onTop = false;
		anim.SetBool ("isStepped", false);
	}
	
	void Jump()
	{
		
	}
}
