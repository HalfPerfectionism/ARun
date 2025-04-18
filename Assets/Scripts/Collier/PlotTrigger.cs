using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlotTrigger : MonoBehaviour
{

    public PlayerController controller;
    private bool isTrigger = false;
    public PlayableDirector director;
    public GameObject bossHealthBar;
    public EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger) return;
        isTrigger = true;
        controller.enabled = false;
        controller.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        controller.GetComponent<Animator>().speed = 0; // ÔÝÍ£¶¯»­
        director.Play();
        Invoke("Restore", (float)director.duration);

    }

    private void Restore()
    {
        controller.enabled = true;
        controller.GetComponent<Animator>().speed = 1;
        bossHealthBar.SetActive(true);
        enemyAI.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
