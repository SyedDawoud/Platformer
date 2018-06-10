using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {

    public static HealthBarController instance;
    public Image healthBarToAnimate,healthBarToFill,lastBarOfHealthBar,firstPoint,lastPoint;
    public List<Sprite> allSprites;
    public float percent;
    public int index;
    Coroutine cc;
    public float newValue;
    private void Awake()
    {
        instance = this;
    }
    public void LerpToNewHealthValue(float newHealth)
    {
        Debug.Log("LerpToNewHealthValue "+name+"new value= "+ newHealth);
        newValue = newHealth;
        if (cc == null)
        {   
            cc = StartCoroutine(LerpToNewHealthValueRoutine());
        }
    }
    public float time, delay = 0.4f;
    Vector2 newPosition;
    IEnumerator LerpToNewHealthValueRoutine()
    {
        time = 0;
        lastBarOfHealthBar.gameObject.SetActive(healthBarToFill.fillAmount > 0);
        newPosition.y = lastBarOfHealthBar.transform.position.y;
        while (time<delay)
        {
            //newValue= Mathf.Clamp(newValue, 0, 1);
            percent = Mathf.Lerp(percent, newValue, time / delay);
            //index = (int)(percent * (allSprites.Count - 1));

            //healthBarToAnimate.sprite = allSprites[index];
            newPosition.x = Mathf.Lerp(firstPoint.transform.position.x, lastPoint.transform.position.x, healthBarToFill.fillAmount*1.1f);
            lastBarOfHealthBar.transform.position = newPosition;
            lastBarOfHealthBar.gameObject.SetActive(healthBarToFill.fillAmount> 0.08f);
            yield return null;
            time += Time.deltaTime;
        }
        healthBarToFill.fillAmount = newValue;
        lastBarOfHealthBar.gameObject.SetActive(healthBarToFill.fillAmount > 0);
        cc = null;


    }
}
