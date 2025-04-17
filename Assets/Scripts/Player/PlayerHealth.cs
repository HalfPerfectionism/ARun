using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //get�����ⲿ�����ȡ�����Ե�ֵ, private setֻ����ǰ���ڲ��޸ĸ����Ե�ֵ���ⲿ�����޷��޸�
    public static PlayerHealth instance;
    //{ get; private set; }

    public static int currentHealth;
    public static int maxHealth = 3000;
    public ParticleSystem hitPs;

    //����
    public GameObject floatPoint;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentHealth = maxHealth;
    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        float yVal = Random.Range(-.5f, .5f), xVal = Random.Range(-0.5f, 0.6f);
        Vector3 floatPos = new Vector3(transform.position.x + xVal, transform.position.y + yVal, transform.position.z);
        GameObject gb = Instantiate(floatPoint, floatPos, Quaternion.identity);
        gb.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Destroy(gameObject);
        }
    }


    void Update()
    {
    }
}
