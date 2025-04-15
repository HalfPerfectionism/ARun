using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
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

    private int comboStep;
    public float interval = 0.1f;
    private float timer;
    private bool isAttack = false;
    public float attackSpeed; //�����Ĳ����ٶ� 




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        feetCol = GetComponent<BoxCollider2D>();
    }

    //��
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
            //transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(rb.velocity.x < -eps)
        {
            //transform.localRotation = Quaternion.Euler(0, 180, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }  

    }
    void CheckIsGround()
    {
        isGround = feetCol.IsTouchingLayers(LayerMask.GetMask("Ground"));

    }

    //��
    void Jump()
    {
        if (isAttack) return;
        if(isGround && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
    //����
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
