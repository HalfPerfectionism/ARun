using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Goblin : Enemy
{


    public override void Move()
    {
        
        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);

        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        animator.SetBool("Run", true);
    }

    public override void Idle()
    {
        base.Idle();
        //print(234);
        rb.velocity = Vector2.zero;
        animator.SetBool("Run", false);
        animator.SetBool("Idle", true);
    }

    public override void Attack()
    {
        rb.velocity = Vector2.zero;
        currentTime = attackColdTime;
        animator.SetTrigger("Attack");
    }

    protected override void Awake()
    {
        base.Awake();
        idleState = new GoblinIdleState();
        chaseState = new GoblinChaseState();
        //patrolState = new GoblinPatrolState();
    }

}
