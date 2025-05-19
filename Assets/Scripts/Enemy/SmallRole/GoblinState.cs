using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinIdleState : BaseState
{
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
    }

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
    }

    public override void OnExit()
    {
        
    }

    public override void PhysicsUpdate()
    {
        currentEnemy.Idle();
    }
}

public class GoblinChaseState : BaseState
{
    public override void LogicUpdate()
    {
        if (!currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
    }

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void PhysicsUpdate()
    {
        currentEnemy.Move();
        if(currentEnemy.distanceToPlayer <= 4f && currentEnemy.currentTime <= 0f)
        {
            currentEnemy.Attack();
        }
    }
}
