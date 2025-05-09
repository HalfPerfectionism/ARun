using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public Sprite openSprite;
    public Sprite closeSprite;
    private SpriteRenderer spriteRenderer; //µ±Ç°µÄÕÕÆ¬
    private bool isDone;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closeSprite;
    }

    public void TriggerAction()
    {
        print("Open Chest");
        OnOpenChest();
    }



    private void OnOpenChest()
    {
        spriteRenderer.sprite = openSprite;
        isDone = true;
        this.gameObject.tag = "Untagged";
    }
}
