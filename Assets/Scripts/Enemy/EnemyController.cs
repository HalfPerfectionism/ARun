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
    private bool isGround = true; //�ж��Ӵ������


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //feetCol = GetComponent<BoxCollider2D>();
    }

    //��
    public void Run()
    {
        float movedir = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(movedir * runSpeed, rb.velocity.y);
    }
    //���߶���
    public void WalkAnim()
    {
        anim.SetFloat("hSpeed", Mathf.Abs(rb.velocity.x));
        //print(anim.GetFloat("hSpeed"));
    }

    //��������
    public void AttackAnim(int x)
    {
        switch (x)
        {
            case 0:
                anim.SetTrigger("�̻�");
                break;
            case 1:
                anim.SetTrigger("ǰ����Ȫ");
                break;
            case 2:
                anim.SetTrigger("���ǽ�");
                break;
            case 3:
                anim.SetTrigger("���ǹ�â��");
                break;
        }
    }

    //��ת
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
