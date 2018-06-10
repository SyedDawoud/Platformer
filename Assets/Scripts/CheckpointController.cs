using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {
	
	public Sprite flagClosed;
	public Sprite flagOpen;
	
	private SpriteRenderer theSpriteRenderer;
	
	public bool checkpointActive;
	
	public AudioSource checkSound;
	
	private bool soundHasPlayed = false;

	// Use this for initialization
	void Start () {
		theSpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
     void OnTriggerEnter2D(Collider2D other)
     {
         if (other.tag == "Player")
         {
             checkpointActive = true;
             if (!soundHasPlayed)
             {
                PlayerController.instance.respawnPosition = transform.position;
                this.gameObject.GetComponent<Animator>().SetBool("Has Passed", true);
                checkSound.Play();
                 soundHasPlayed = true;
             }
		}
	}
}
