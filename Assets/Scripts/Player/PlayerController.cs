using System.Collections;
using System.Collections.Generic;
//using UnityEditor.XR;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    

    [Header("基础数值")]
    public float runSpeed = 5;
    public float jumpSpeed = 5;
    public float interval = 0.1f; // 普攻的连续时间
    public float attackSpeed; //攻击的补偿速度 

    [Header("参数")]
    private int comboStep;
    private float eps = Mathf.Epsilon;
    private float timer;

    [Header("组件")]
    private Animator anim;
    private PhysicsCheck physicsCheck;
    private Rigidbody2D rb;
    private Character character;

    [Header("状态")]
    private bool isAttack = false;
 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        character = GetComponent<Character>();
    }

    //走
    void Run()
    {
        if (!isAttack)
        {
            float movedir = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(movedir * runSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(transform.localScale.x * attackSpeed, rb.velocity.y);
        }

    }
    //行走动画
    void WalkAnim()
    {
        anim.SetFloat("hSpeed", Mathf.Abs(rb.velocity.x));
    }
    //跳跃动画
    void JumpAnim()
    {
        if (!physicsCheck.isGround)
        {
            anim.SetBool("isAir", true);
            if(rb.velocity.y > -eps)
            {
                anim.SetBool("isJump", true);
                anim.SetBool("isFall", false);

            }
            else
            {
                anim.SetBool("isJump", false);
                anim.SetBool("isFall", true);

            }
        }
        else
        {
            anim.SetBool("isAir", false);
            anim.SetBool("isFall", false);
            anim.SetBool("isJump", false);
        }

    }
    //攻击动画
    void AttackAnim()
    {
        anim.SetTrigger("Attack");
    }

    //翻转
    void Filp()
    {
        if(rb.velocity.x > eps)
        {
            //transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(rb.velocity.x < -eps)
        {
            //transform.localRotation = Quaternion.Euler(0, 180, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }  

    }

    //跳
    void Jump()
    {
        if (isAttack) return;
        if(physicsCheck.isGround && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
    //攻击
    void Attack()
    {
        if (Input.GetButtonDown("Attack") && !isAttack)
        {
            isAttack = true;
            comboStep++;
            if (comboStep > 4) comboStep = 1;
            timer = interval;
            anim.SetTrigger("Attack");
            anim.SetInteger("ComboStep", comboStep);
            //print(44);
        }

        if(timer != 0)
        {
            timer -= Time.deltaTime;
            if(timer < 0f)
            {
                timer = 0f;
                comboStep = 0;
            }
        }
    }
    public void AttackOver()
    {
        isAttack = false;
    } //动画调用

    //角色死亡
    public void PlayerDead()
    {
        if (character.isDead)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            transform.GetComponent<CapsuleCollider2D>().enabled = false;
            Destroy(transform.gameObject, 1f);
            return;
        }
    }


    void Update()
    {
        PlayerDead();
        Run();
        Filp();
        WalkAnim();
        Jump();
        JumpAnim();
        Attack();
    }
}
