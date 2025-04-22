using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    public override void Move()
    {
        base.Move();
        animator.SetBool("Run", true);
    }

}
