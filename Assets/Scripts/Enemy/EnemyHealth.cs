using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;
    public string name;

    //受击反馈
    public float attackedSpeed;
    private Vector2 direction;
    public bool isHit; //让AI调用
    private AnimatorStateInfo animState; //获取动画进度
    private Animator animator;
    private Animator hitAnimator;
    private Rigidbody2D rb;
    private EnemyAI enemyAI;

    //气泡
    public GameObject floatPoint;
    public Transform floatPos;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyAI = GetComponent<EnemyAI>();
        hitAnimator = transform.GetChild(1).GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    public void DamageEnemy(int damage)
    {
        hitAnimator.SetTrigger("Hit");
        currentHealth -= damage;
        float yVal = Random.Range(-1.5f, -2.5f), xVal = Random.Range(-1f, 0.6f);
        Vector3 floatPos = new Vector3(transform.position.x + xVal, transform.position.y + yVal, transform.position.z);
        
        
        GameObject gb = Instantiate(floatPoint, floatPos, Quaternion.identity);
        gb.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
        //print(currentHealth);
    }



    // Update is called once per frame
    void Update()
    {
        animState = animator.GetCurrentAnimatorStateInfo(0);
        if (isHit)
        {
            rb.velocity = direction * attackedSpeed;
            if (animState.normalizedTime >= .8f)
            {
                isHit = false;
                //rb.velocity = Vector2.zero;
            }
               
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            DamageEnemy(10);
        }
    }

    public void GetHit(Vector2 direction)
    {
        //print()
        if (enemyAI.isStoic == true) return;
        transform.localScale = new Vector3(-direction.x, 1, -1);
        isHit = true;
        this.direction = direction;
        StopAllCoroutines();
        animator.SetTrigger("Hit");
    }

}
