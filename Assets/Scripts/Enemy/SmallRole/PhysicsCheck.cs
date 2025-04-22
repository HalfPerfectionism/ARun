using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{

    public float checkRadius; //·¶Î§
    public LayerMask groundLayer;
    public Vector2 bottomOffset; // Æ«ÒÆÁ¿
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    [Header("×´Ì¬")]
    public bool isGround;
    public bool leftTouchWall;
    public bool rightTouchWall;


    private void Update()
    {
        Check();
    }

    public void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius, groundLayer);
        leftTouchWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
        rightTouchWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);
    }

    //»æÖÆGizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
    }
}
