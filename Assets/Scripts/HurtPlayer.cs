using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour {
	
	
	
	public int damageToGive;

    // Use this for initialization


    // Update is called once per frame
    private void Start()
    {
       lastCollideTime=System.DateTime.Now;
    }
    void Update () {
		
	}
	public System.DateTime lastCollideTime;
	void OnTriggerEnter2D(Collider2D other)
	{

        if ((System.DateTime.Now - lastCollideTime).TotalSeconds > 1)
        {
            if (other.tag=="Player")
                {
                    Debug.Log("other.attachedRigidbody.GetComponent<PlayerController>()");
                    //theLevelManager.Respawn();
                    lastCollideTime = System.DateTime.Now;
                    Invoke("HurtPlayerAfterTime", 0.1f);
                }
        }
        else
        {
            Debug.Log("cant touch in the same time twice \n (System.DateTime.Now- lastCollideTime).TotalSeconds < 1");
        }
	}
    void HurtPlayerAfterTime()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        LevelManager.instance.HurtPlayer(this, damageToGive);
        if (damageToGive < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
