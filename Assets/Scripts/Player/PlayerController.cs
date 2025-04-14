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
    private bool isGround = true; //�ж��Ӵ������
    private BoxCollider2D feetCol;
    //private PlayerHealth playerHealth;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        feetCol = GetComponent<BoxCollider2D>();
    }

    //��
    void Run()
    {
        float movedir = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(movedir * runSpeed, rb.velocity.y);
    }
    //���߶���
    void WalkAnim()
    {
        anim.SetFloat("hSpeed", Mathf.Abs(rb.velocity.x));
    }
    //��Ծ����
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
    //��������
    void AttackAnim()
    {
        anim.SetTrigger("Attack");
    }

    //��ת
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

    //��
    void Jump()
    {
        if(isGround && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
    //����
    void Attack()
    {
        if (Input.GetButtonDown("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            AttackAnim();
            //print(anim.GetCurrentAnimatorStateInfo(0).IsName("attack"));
        }
    }
    //����
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
