using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public int[] skillDamage;
    public float[] skillInterval;
    public float[] damageRandomRange;
    public PolygonCollider2D[] skillCol;


    private int currentSkill = -1; //当前是那个技能
    private Coroutine workingCoroutine = null;



    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && 
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            float _damageInterval = 0f;
            int damage = 0;
            //print("yes");
            if (SkillEffect(currentSkill, out _damageInterval, out damage))//返回为true就是连续攻击
            {
                workingCoroutine = StartCoroutine(
                    ApplyContinuousDamage(other, _damageInterval, damage));
            }
            else
            {
                //other.GetComponent<PlayerHealth>().DamagePlayer(damage);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {

        //print(workingCoroutine);

        if(currentSkill != -1 && workingCoroutine != null)
        {
            //StopCoroutine(workingCoroutine);
            //workingCoroutine = null;
            StopAllCoroutines();
        }
    }

    //public void OnChildTriggerEnter(Collider2D other)
    //{
    //    OnTriggerEnter2D(other); // 复用原有逻辑
    //}

    //public void OnChildTriggerExit(Collider2D other)
    //{
    //    OnTriggerExit2D(other); // 复用原有逻辑
    //}

    //true为持续伤害
    public bool SkillEffect(int x, out float _damageInterval, out int damage)
    {
        damage = 0;
        _damageInterval = 0;
        if (x == 0)
        {
            damage = skillDamage[0];
            return false;
        }else if(x == 1)
        {
            damage = skillDamage[1];
            _damageInterval = skillInterval[1];
            return true;
        }
        else if(x == 2)
        {
            damage = skillDamage[2];
            _damageInterval = skillInterval[2];
            return true;
        }else if(x == 3)
        {
            damage = skillDamage[3];
            _damageInterval = skillInterval[3];
            return true;
        }
        return false;  
    }

    //持续伤害
    IEnumerator ApplyContinuousDamage(Collider2D other, float _damageInterval, int damage)
    {
        while (true)
        {
            if (other == null) break;

            //other.GetComponent<PlayerHealth>().DamagePlayer(damage);

            yield return new WaitForSeconds(_damageInterval);
            //print(damage);
        }
    }


    //动画事件调用
    public void StartCol(int _skill)
    {
        currentSkill = _skill;
       
        skillCol[_skill].gameObject.SetActive(true);
    }

    public void CloseCol(int _skill)
    {
        
        skillCol[_skill].gameObject.SetActive(false);
        currentSkill = -1;
    }


   
}
