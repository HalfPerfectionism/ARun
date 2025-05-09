using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private Animator animator;
    public GameObject signSprite;
    private bool canPress = false;
    public Transform playerTrans;
    private IInteractable IItem; //׼����ÿɽ�������Ľӿ�

    private void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
        animator = signSprite.GetComponent<Animator>();

    }

    private void OnDisable()
    {
        canPress = false;
    }

    private void Update()
    {
        signSprite.SetActive(canPress);
        signSprite.transform.localScale = playerTrans.localScale;
        if (canPress)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                IItem?.TriggerAction();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = true;
            IItem = collision.GetComponent<IInteractable>(); //��ÿɽ�������Ľӿ�

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
