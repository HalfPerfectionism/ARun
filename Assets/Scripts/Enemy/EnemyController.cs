using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float runSpeed = 5;
    //[SerializeField]
    //private float jumpSpeed = 5;

    private Animator anim;
    private float eps = Mathf.Epsilon;
    private bool isGround = true; //判定接触地面否


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //feetCol = GetComponent<BoxCollider2D>();
    }

    //走
    public void Run()
    {
        float movedir = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(movedir * runSpeed, rb.velocity.y);
    }
    //行走动画
    public void WalkAnim()
    {
        anim.SetFloat("hSpeed", Mathf.Abs(rb.velocity.x));
        //print(anim.GetFloat("hSpeed"));
    }

    //攻击动画
    public void AttackAnim(int x)
    {
        switch (x)
        {
            case 0:
                anim.SetTrigger("刺击");
                break;
            case 1:
                anim.SetTrigger("前进喷泉");
                break;
            case 2:
                anim.SetTrigger("三角剑");
                break;
            case 3:
                anim.SetTrigger("七星光芒阵");
                break;
        }
    }

    //翻转
    public void Filp()
    {
        if (rb.velocity.x > eps)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rb.velocity.x < -eps)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            AttackAnim(0);
        }
        if(Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.J))
        {
            AttackAnim(1);
        }
        if (Input.GetKeyDown(KeyCode.U) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            AttackAnim(2);
        }
        if(Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.U))
        {
            //print(3);S
            AttackAnim(3);
        }
    }

    void Update()
    {
        Run();
        Filp();
        WalkAnim();
        Attack();
    }
}
