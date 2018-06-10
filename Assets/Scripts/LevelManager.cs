using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;
	public float waitToRespawn;
	//public PlayerController thePlayer;
	
	public GameObject deathGhost;
	public GameObject deathSplosion;
    public int coinCount;
	
	public Text coinText;
	
	public AudioSource coinSound;
	
	public Image invisibility1;
	
	public Sprite invisibility11;
	public Sprite invisibility22;
	public Sprite invisibility33;
	public Sprite invisibility44;
	
	public float maxHealth=1;
	public float healthCount;
    public bool playerCanTakeDomage=true;
	private bool respawning;
	
	public ResetOnRespawn[] objectsToReset;
    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
		
		
		coinText.text = " " + coinCount;
		
		healthCount = maxHealth*(2.2f/3.3f);
        HealthBarController.instance.LerpToNewHealthValue(healthCount / maxHealth);
		objectsToReset = FindObjectsOfType<ResetOnRespawn>();
	}
	
	// Update is called once per frame

	
	public void Respawn()
	{
		StartCoroutine("RespawnCo");
	}
	
	public IEnumerator RespawnCo()
	{
        PlayerController.instance.isFalling = false;
        PlayerController.instance.isGrounded = true;
		PlayerController.instance.gameObject.SetActive(false);
        PlayerController.instance.shield.gameObject.SetActive(false);
        PlayerController.instance.transform.parent = null;
        playerCanTakeDomage = true;
		Instantiate(deathGhost, PlayerController.instance.transform.position, PlayerController.instance.transform.rotation);
		
		yield return new WaitForSeconds(waitToRespawn);
		
		healthCount = maxHealth*(3.3f/4.4f);
		respawning = false;
        HealthBarController.instance.LerpToNewHealthValue(healthCount / maxHealth);
		
		coinCount = 0;
		coinText.text = " " + coinCount;

        PlayerController.instance.transform.position = PlayerController.instance.respawnPosition;
        PlayerController.instance.gameObject.SetActive(true);
		
		for(int i = 0; i < objectsToReset.Length; i++)
		{
			objectsToReset[i].gameObject.SetActive(true);
			objectsToReset[i].ResetObject();
		}
	}
	
	public void AddCoins(int coinsToAdd)
	{
		coinCount += coinsToAdd;
		
		coinText.text = " " + coinCount;
		
		coinSound.Play();
	}
   

	public void HurtPlayer(HurtPlayer caller,float damageToTake)
	{
       
        if (!playerCanTakeDomage)
        {
            caller.gameObject.SetActive(false);
            if (damageToTake > 0)
            {
                Instantiate(deathSplosion, caller.transform.position, caller.transform.rotation);
            }
            return;
        }
        healthCount -= damageToTake;
        if (healthCount>=maxHealth)
        {
            StartCoroutine(PlayerCantBeDomagedFor10Seconds());
        }
		
        HealthBarController.instance.LerpToNewHealthValue(healthCount/maxHealth);
        if (healthCount <= 0 && !respawning)
        {
            Respawn();
            respawning = true;
        }
       // UpdateHeartMeter();
	}
	IEnumerator PlayerCantBeDomagedFor10Seconds()
    {
        playerCanTakeDomage = false;
        PlayerController.instance.shield.gameObject.SetActive(true);
        yield return new WaitForSeconds(10);
        healthCount = maxHealth * (3.3f / 4.4f);
        HealthBarController.instance.LerpToNewHealthValue(healthCount / maxHealth);
        PlayerController.instance.shield.gameObject.SetActive(false);
        playerCanTakeDomage = true;

    }
    /*public void UpdateHeartMeter()
	{
		switch(healthCount)
		{
			case 0:
			invisibility1.sprite = invisibility11;
			return;
			
			case 1:
			invisibility1.sprite = invisibility22;
			return;
			
			case 2:
			invisibility1.sprite = invisibility33;
			return;
			
			case 3:
			invisibility1.sprite = invisibility44;
			return;
			
			default:
			invisibility1.sprite = invisibility11;
			return;
		}
	}*/
}
