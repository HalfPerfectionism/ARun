using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyMode { careful, serious, rage };
    public enum EnemyState { idle, walk, run, withdraw, 刺击, 前进喷泉, 三角剑, 七星光芒阵 };
    public EnemyState currentState = EnemyState.idle;
    private Dictionary<EnemyState, int> stateWeights = new Dictionary<EnemyState, int>()
    {
        { EnemyState.idle, 10 },
        { EnemyState.walk, 5 },
        { EnemyState.run, 3 },
        { EnemyState.withdraw, 2 },
        { EnemyState.刺击, 20 },
        { EnemyState.前进喷泉, 15 },
        { EnemyState.三角剑, 10 },
        { EnemyState.七星光芒阵, 5 },
        //{ EnemyState.filp, 10},
    };

    [Header("Settings")]
    public float a0Range = 4.0f;
    public float a1Range = 4.0f;
    public float a2Range = 8.0f;
    public float a3Range = 5.0f;
    public float a0ColdTime = 3f;
    public float a1ColdTime = 5f;
    public float a2ColdTime = 5f;
    public float a3ColdTIme = 10f;

    private float a0LastTime = 0f;
    private float a1LastTime = 0f;
    private float a2LastTime = 0f;
    private float a3LastTime = 0f;

    public float walkSpeed = 0.41f;
    public float runSpeed = 5.0f;
    public float projectileSpeed = 35f;
    public float[] forwardShakeTime;
    public float[] backShakeTime;


    [Header("Reference")]

    public GameObject projectilePrefab;
    public Transform attackPoint;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;
    private EnemyAttack ea;

    private float lastTime;
    private bool isAttacking = false;
    private bool canMove = true;
    private bool canFilp = true;
    private float distanceToPlayer; //距离玩家距离
    private Vector2 direction;
    private float xSignDirection; //符号化方向
    private int face = 1; // 朝向，正1，反-1；
    private bool isPlayerMoving;
    private float eps = 1.984e-07f;

    //预处理数组
    private  int[] nums1 = new int[8] { 0, 0, 1000, 0, 0, 0, 0, 0 };
    private  int[] nums2 = new int[8] { 10000, 300, 500, 0, 0, 100, 300, 0 };
    private int[] nums3 = new int[8] { 10000, 300, 400, 0, 10, 300, 400, 0 };
    private int[] nums4 = new int[8] { 10000, 100, 200, 0, 0, 400, 200, 400 };

    //当前进行的协程函数
    private Coroutine moveCoroutine;


    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ea = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        if (player == null) return;

        if (player.GetComponent<Rigidbody2D>().velocity == Vector2.zero) isPlayerMoving = false;
        else isPlayerMoving = true;

        // 计算与玩家的距离
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        direction = (player.transform.position - transform.position).normalized;
        xSignDirection = Mathf.Sign(direction.x);
        //set "hSpeed"
        if (rb.velocity.x > eps || rb.velocity.x < -eps)
            animator.SetFloat("hSpeed", math.abs(rb.velocity.x));
        else animator.SetFloat("hSpeed", 0f);

        //print(Mathf.Sign(direction.x) * distanceToPlayer);
        //print( "face : " + face + " xSignDirection:" + xSignDirection);

        if (!canMove) rb.velocity = new Vector2(0f, rb.velocity.y);

        // 根据距离切换状态
        if (!isAttacking)
        {
            
            Filp();
            EnemyState state = AI();
            ChangeState(state);
        }

        // 执行当前状态的行为
        ExecuteState();
    }

    EnemyState AI()
    {

        AIperform();

        int totalWeight = 0;
        foreach (var pair in stateWeights)
        {
            totalWeight += pair.Value;
        }
        int godDice = UnityEngine.Random.Range(1, totalWeight + 1);
        int cumulativeWeight = 0;
        foreach (var pair in stateWeights)
        {
            cumulativeWeight += pair.Value;
            if (godDice <= cumulativeWeight)
                return pair.Key;
        }

        return EnemyState.idle;
    }

    //算出当前的概率
    void AIperform()
    {
         if (distanceToPlayer > a2Range)
        {
            
            SetWeights(nums1);
        } else if (distanceToPlayer < a2Range && distanceToPlayer > a1Range)
        {
            
            SetWeights(nums2);
        } else if (distanceToPlayer < a1Range && distanceToPlayer > a0Range) {
            
            SetWeights(nums3);
        } else if (distanceToPlayer < a0Range)
        {
            
            SetWeights(nums4);
            if (Input.GetButton("Jump"))
            {
                ModifyWeight(EnemyState.刺击, 1000, 1);
            }
        }
    }
    //修改权重
    void ModifyWeight(EnemyState _enemyState, int num, int pn)
    {
        stateWeights[_enemyState] += pn * num;
        if (stateWeights[_enemyState] <= 0) stateWeights[_enemyState] = 0;
    }

    //设置权重
    bool SetWeights(int[] nums)
    {
        if(nums.Length < stateWeights.Count || nums == null) return false;
        int index = 0;
        foreach(EnemyState state in Enum.GetValues(typeof(EnemyState)))
        {
            stateWeights[state] = nums[index++];
        }
        return true;
    }

    //切换状态
    void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    //技能的具体逻辑
    IEnumerator PreformAttack(int x)
    {
        isAttacking = true;
        //canMove = false;
        if (x == 0)//刺击
        {
            a0LastTime = Time.time;
            animator.SetTrigger("刺击");
            yield return new WaitForSeconds(backShakeTime[x]);
        }
        else if (x == 1)//前进喷泉
        {
            a1LastTime = Time.time;
            canFilp = false;
            animator.SetTrigger("前进喷泉");

            // 初始爆发速度
            float initialSpeed = 20f * face;
            float timer = 0f;
            float duration = 1f; // 1秒内减速到0

            while (timer < duration)
            {
                // 线性减速：初始速度按剩余时间比例衰减
                float currentSpeed = Mathf.Lerp(initialSpeed, 0, timer / duration);
                rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

                timer += Time.deltaTime;
                yield return null; // 每帧更新
            }

            rb.velocity = new Vector2(0, rb.velocity.y); // 确保最终速度为0
            canFilp = true;
        }
        else if(x == 2)
        {
            a2LastTime = Time.time;
            canFilp = false;
            canMove = false;
            animator.SetTrigger("三角剑");

            //飞行物翻转
            Quaternion rotation = Quaternion.Euler(0, face == 1 ? 0 : 180, 0);

            yield return new WaitForSeconds(forwardShakeTime[x]); //前摇
            //print(forwardShakeTime[x]);

            GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(face * projectileSpeed, 0f);

            yield return new WaitForSeconds(backShakeTime[x]);
            canFilp = true;
            canMove = true;
        }
        else if(x == 3)
        {
            a3LastTime = Time.time;
            canFilp = false;
            canMove = false;
            animator.SetTrigger("七星光芒阵");
            yield return new WaitForSeconds(backShakeTime[x]);
            canFilp = true;
            canMove = true;
        }
        isAttacking = false;
        canMove = true;
        canFilp = true;
        yield return null;
    }

    //翻转
    void Filp()
    {
        if (canFilp)
        {
            if (xSignDirection < 0)
            {
                face = -1;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                face = 1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    //移动的协同函数
   IEnumerator PreformMove(int state, float time)
    {
        float startTime = Time.time;

        while (Time.time - startTime < time)
        {
            
            rb.velocity = new Vector2(face * runSpeed, 0);
            yield return null; //好像可以更新Time.time

        }
        
    }



    void ExecuteState()
    {
        switch (currentState)
        {
            case EnemyState.idle:
                rb.velocity = Vector2.zero;
                canFilp = true;
                break;

            case EnemyState.walk:
                if (!canMove) return;              
                canFilp = true;
                break;

            case EnemyState.run:
                if (!canMove) return;
                canFilp = true;

                moveCoroutine = StartCoroutine(PreformMove(0, 0.5f));
                break;

            case EnemyState.刺击:
                if (Time.time < a0LastTime + a0ColdTime) return;
                if (moveCoroutine != null) StopAllCoroutines();
                StartCoroutine(PreformAttack(0));
                break;

            case EnemyState.前进喷泉:
                if (Time.time < a1LastTime + a1ColdTime) return;
                if (moveCoroutine != null) StopAllCoroutines();
                StartCoroutine(PreformAttack(1));
                break;
            case EnemyState.三角剑:
                if (Time.time < a2LastTime + a2ColdTime) return;
                if (moveCoroutine != null) StopAllCoroutines();
                StartCoroutine(PreformAttack(2));
                break;
            case EnemyState.七星光芒阵:
                if (Time.time < a3LastTime + a3ColdTIme) return;
                if (moveCoroutine != null) StopAllCoroutines();
                StartCoroutine(PreformAttack(3));
                break;
            //case EnemyState.filp:
            //    Filp();
            //    break;
        }
    }
}
