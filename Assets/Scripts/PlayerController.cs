using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;
	public float moveSpeed;
	private Rigidbody2D myRigidbody;
	
	public float jumpSpeed;
	
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	
	public bool isGrounded;
	public bool isFalling;
	public bool isJumping;
	
	public  Animator myAnim;
    public Animator shield;
	public Vector3 respawnPosition;
	
	public LevelManager theLevelManager;
	
	public GameObject stompBox;
	
	public AudioSource jumpSound;
    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnim = GetComponent<Animator>();
		
		respawnPosition = transform.position;
		
		theLevelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
		
		if(Input.GetAxisRaw ("Horizontal") > 0f)
		{
			myRigidbody.velocity = new Vector3(moveSpeed, myRigidbody.velocity.y, 0f);
			transform.localScale = new Vector3(1f,1f,1f);
		}	else if(Input.GetAxisRaw ("Horizontal") < 0f)
		{
			myRigidbody.velocity = new Vector3(-moveSpeed, myRigidbody.velocity.y, 0f);
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}	else {
				myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
		}
			
		if(Input.GetButtonDown ("Jump") && isGrounded)
		{
			myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpSpeed, 0f);
			jumpSound.Play();
		}

		
		
			isFalling = myRigidbody.velocity.y < -3f ;
			isJumping = myRigidbody.velocity.y > 3f ;
        if(isGrounded)
        {
            isFalling = false;
            isJumping = false;
        }
		         
		myAnim.SetFloat("Speed", Mathf.Abs( myRigidbody.velocity.x));
		myAnim.SetBool("Ground", isGrounded);
		myAnim.SetBool("Falling", isFalling);
		myAnim.SetBool("Jumping", isJumping);
		
		if(myRigidbody.velocity.y < 0)
		{
			stompBox.SetActive(true);
		}else {
			stompBox.SetActive(false);
		}

	}	
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "KillPlane")
		{
			//gameObject.SetActive(false);
			
			//transform.position = respawnPosition;
			
			theLevelManager.Respawn();
		}
		
		if(other.tag == "Checkpoint")
		{
			other.tag = "CheckpointPassed";
			respawnPosition = other.transform.position;
		}
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "MovingPlatform")
		{
			transform.parent = other.transform;
		}
	}
	
	void OnCollisionExit2D(Collision2D other)
	{
		if(other.gameObject.tag == "MovingPlatform")
		{
			transform.parent = null;
		}
	}
}
