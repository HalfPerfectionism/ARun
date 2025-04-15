using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private int damage = 1000;
    [SerializeField]
    private PolygonCollider2D[] cols;
    //private GameObject[] gos;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().DamageEnemy(damage);
            if(transform.localScale.x > 0)
            {
                other.GetComponent<EnemyHealth>().GetHit(Vector2.right);
            }
            else
            {
                other.GetComponent<EnemyHealth>().GetHit(Vector2.left);
            }
        }
    }

    void StartCol(int index)
    {
        cols[index].gameObject.SetActive(true);
    }

    void CloseCol(int index)
    {
        cols[index].gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
