using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;

public class Attack : MonoBehaviour
{

    [Header("ÊýÖµ")]
    public int damage;
    public float attackRange;
    public float attackRate; // ¹¥»÷ÆµÂÊ


    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>().TakeDamage(this);
    }

}
