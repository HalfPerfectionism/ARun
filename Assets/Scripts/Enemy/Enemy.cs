using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("基本参数")]
    public float speed;
    public float currentSpeed;
    public int maxHealth;
    public int currentHealth;

    [Header("组件")]
    protected Rigidbody2D rb;
    protected Animator animator;

    [Header("变量数据")]
    protected int faceDir;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        faceDir = (int)transform.localScale.x;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(speed * faceDir, rb.velocity.y);
    }

}
