using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("��������")]
    public float speed;
    public float currentSpeed;
    public float attackColdTime = 1.5f;
    public float currentTime = 0f;
    //public int maxHealth;
    //public int currentHealth;

    [Header("���")]
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Character character;

    [Header("��������")]
    protected Vector3 faceDir;

    protected BaseState idleState;
    protected BaseState currentState;
    protected BaseState chaseState;

    [Header("���")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;
    protected RaycastHit2D hit;
    protected Vector2 direction;
    public float distanceToPlayer;

    //public Transform playerTrans;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        character = GetComponent<Character>();
        currentState = idleState;
        //print(currentState);
        currentState.OnEnter(this);
        //print(this);
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    private void Update()
    {
        if(character.currentHealth <= 0f)
        {
            animator.SetBool("Die", true);
            Invoke("DieAnimationFunc" , 1f);
        }

        faceDir = new Vector3(transform.localScale.x, 0, 0);
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0f, currentTime);

        //FoundPlayer();
        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
        //Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(speed * faceDir.x, rb.velocity.y);
    }

    //���л�״̬
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Idle => idleState,
            NPCState.Chase => chaseState,
            _ => null
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    //������
    public bool FoundPlayer()
    {
       hit = Physics2D.BoxCast(transform.position + (Vector3)centerOffset,
            checkSize, 0, faceDir, checkDistance, attackLayer);
        if(hit.collider != null && hit.collider.CompareTag("Player"))
        {
            distanceToPlayer = Mathf.Abs(Vector2.Distance(transform.position, hit.collider.transform.position));
            direction = (hit.collider.transform.position - transform.position).normalized;
        }


        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset,
            checkSize, 0, faceDir, checkDistance, attackLayer);
    }

    //������Χ
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + 
            new Vector3(faceDir.x * checkDistance, 0, 0) , 0.2f);
        Gizmos.DrawWireCube(transform.position + (Vector3)centerOffset, checkSize);
        
    }

    public virtual void Idle()
    {
        //throw new NotImplementedException();
    }

    public virtual void Attack()
    {

    }
    public void DieAnimationFunc()
    {
        Destroy(gameObject);
    }




    //public void Filp()
    //{
    //    Vector2 direction = (playerTrans.position - transform.position).normalized;
    //    float xSignDirection = Mathf.Sign(direction.x);

    //    if (xSignDirection < 0)
    //    {
    //        //face = -1;
    //        //transform.localRotation = Quaternion.Euler(0, 180, 0);
    //        transform.localScale = new Vector3(-1, 1, 1);
    //    }
    //    else
    //    {
    //        //face = 1;
    //        transform.localScale = new Vector3(1, 1, 1);
    //    }
    //}
}
