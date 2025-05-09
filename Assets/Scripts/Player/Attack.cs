using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;

public class Attack : MonoBehaviour
{

    [Header("数值")]
    public int damage;
    public float attackRange;
    public float attackRate; // 攻击频率


    private void OnTriggerStay2D(Collider2D collision)
    {
        //加个？如果没有就返回null
        collision.GetComponent<Character>()?.TakeDamage(this);
        //print(collision);
    }

}
