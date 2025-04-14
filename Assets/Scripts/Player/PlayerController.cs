using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float runSpeed = 5;
    [SerializeField]
    private float jumpSpeed = 5;

    private Animator anim;
    private float eps = Mathf.Epsilon;
    private bool isGround = true; //判定接触地面否
    private BoxCollider2D feetCol;
    //private PlayerHealth playerHealth;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        feetCol = GetComponent<BoxCollider2D>();
    }

    //走
    void Run()
    {
        float movedir = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(movedir * runSpeed, rb.velocity.y);
    }
    //行走动画
    void WalkAnim()
    {
        anim.SetFloat("hSpeed", Mathf.Abs(rb.velocity.x));
    }
    //跳跃动画
    void JumpAnim()
    {
        if (!isGround)
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
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if(rb.velocity.x < -eps)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }  

    }
    void CheckIsGround()
    {
        isGround = feetCol.IsTouchingLayers(LayerMask.GetMask("Ground"));

    }

    //跳
    void Jump()
    {
        if(isGround && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
    //攻击
    void Attack()
    {
        if (Input.GetButtonDown("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            AttackAnim();
            //print(anim.GetCurrentAnimatorStateInfo(0).IsName("attack"));
        }
    }
    //自伤
    void SelfInjury()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerHealth.instance.DamagePlayer(10);
        }
    }

    void Update()
    {
        Run();
        Filp();
        WalkAnim();
        CheckIsGround();
        Jump();
        JumpAnim();
        Attack();
        SelfInjury();
    }
}
