using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("���")]
    private PlayerController controller;
    private Character character;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        character = GetComponent<Character>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        animator.SetBool("isDead", character.isDead);
    }

    //����
    public void PlayerHurt()
    {
        animator.SetTrigger("Hurt");
    }
}
