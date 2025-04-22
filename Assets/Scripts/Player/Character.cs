using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
   
    public static int currentHealth;
    public static int maxHealth = 3000;

    //ÆøÅÝ
    public GameObject floatPoint;


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(Attack attacker)
    {
        currentHealth -= attacker.damage;
        float yVal = Random.Range(-.5f, .5f), xVal = Random.Range(-0.5f, 0.6f);
        Vector3 floatPos = new Vector3(transform.position.x + xVal, transform.position.y + yVal, transform.position.z);
        GameObject gb = Instantiate(floatPoint, floatPos, Quaternion.identity);
        gb.transform.GetChild(0).GetComponent<TextMesh>().text = attacker.damage.ToString();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
    }


    void Update()
    {
    }
}
