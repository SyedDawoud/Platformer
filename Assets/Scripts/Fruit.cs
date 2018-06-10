using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {
	
	private LevelManager theLevelManager;
	
	public int coinValue;

	// Use this for initialization
	void Start () {
		theLevelManager = FindObjectOfType<LevelManager>();
		
		Animator anim = GetComponent<Animator> ();
		AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo (0);
		anim.Play (state.fullPathHash, -1, Random.Range(0f,1f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			theLevelManager.AddCoins(coinValue);
			
			//Destroy(gameObject);
			
			gameObject.SetActive(false);
		}
	}
}
