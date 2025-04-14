using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyMode { careful, serious, rage };
    public enum EnemyState { idle, walk, run, withdraw, �̻�, ǰ����Ȫ, ���ǽ�, ���ǹ�â�� };
    public EnemyState currentState = EnemyState.idle;
    private Dictionary<EnemyState, int> stateWeights = new Dictionary<EnemyState, int>()
    {
        { EnemyState.idle, 10 },
        { EnemyState.walk, 5 },
        { EnemyState.run, 3 },
        { EnemyState.withdraw, 2 },
        { EnemyState.�̻�, 20 },
        { EnemyState.ǰ����Ȫ, 15 },
        { EnemyState.���ǽ�, 10 },
        { EnemyState.���ǹ�â��, 5 },
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
    private float distanceToPlayer; //������Ҿ���
    private Vector2 direction;
    private float xSignDirection; //���Ż�����
    private int face = 1; // ������1����-1��
    private bool isPlayerMoving;
    private float eps = 1.984e-07f;

    //Ԥ��������
    private  int[] nums1 = new int[8] { 0, 0, 1000, 0, 0, 0, 0, 0 };
    private  int[] nums2 = new int[8] { 10000, 300, 500, 0, 0, 100, 300, 0 };
    private int[] nums3 = new int[8] { 10000, 300, 400, 0, 10, 300, 400, 0 };
    private int[] nums4 = new int[8] { 10000, 100, 200, 0, 0, 400, 200, 400 };

    //��ǰ���е�Э�̺���
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

        // ��������ҵľ���
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

        // ���ݾ����л�״̬
        if (!isAttacking)
        {
            
            Filp();
            EnemyState state = AI();
            ChangeState(state);
        }

        // ִ�е�ǰ״̬����Ϊ
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

    //�����ǰ�ĸ���
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
                ModifyWeight(EnemyState.�̻�, 1000, 1);
            }
        }
    }
    //�޸�Ȩ��
    void ModifyWeight(EnemyState _enemyState, int num, int pn)
    {
        stateWeights[_enemyState] += pn * num;
        if (stateWeights[_enemyState] <= 0) stateWeights[_enemyState] = 0;
    }

    //����Ȩ��
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

    //�л�״̬
    void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    //���ܵľ����߼�
    IEnumerator PreformAttack(int x)
    {
        isAttacking = true;
        //canMove = false;
        if (x == 0)//�̻�
        {
            a0LastTime = Time.time;
            animator.SetTrigger("�̻�");
            yield return new WaitForSeconds(backShakeTime[x]);
        }
        else if (x == 1)//ǰ����Ȫ
        {
            a1LastTime = Time.time;
            canFilp = false;
            animator.SetTrigger("ǰ����Ȫ");

            // ��ʼ�����ٶ�
            float initialSpeed = 20f * face;
            float timer = 0f;
            float duration = 1f; // 1���ڼ��ٵ�0

            while (timer < duration)
            {
                // ���Լ��٣���ʼ�ٶȰ�ʣ��ʱ�����˥��
                float currentSpeed = Mathf.Lerp(initialSpeed, 0, timer / duration);
                rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

                timer += Time.deltaTime;
                yield return null; // ÿ֡����
            }

            rb.velocity = new Vector2(0, rb.velocity.y); // ȷ�������ٶ�Ϊ0
            canFilp = true;
        }
        else if(x == 2)
        {
            a2LastTime = Time.time;
            canFilp = false;
            canMove = false;
            animator.SetTrigger("���ǽ�");

            //�����﷭ת
            Quaternion rotation = Quaternion.Euler(0, face == 1 ? 0 : 180, 0);

            yield return new WaitForSeconds(forwardShakeTime[x]); //ǰҡ
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
            animator.SetTrigger("���ǹ�â��");
            yield return new WaitForSeconds(backShakeTime[x]);
            canFilp = true;
            canMove = true;
        }
        isAttacking = false;
        canMove = true;
        canFilp = true;
        yield return null;
    }

    //��ת
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

    //�ƶ���Эͬ����
   IEnumerator PreformMove(int state, float time)
    {
        float startTime = Time.time;

        while (Time.time - startTime < time)
        {
            
            rb.velocity = new Vector2(face * runSpeed, 0);
            yield return null; //������Ը���Time.time

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

            case EnemyState.�̻�:
                if (Time.time < a0LastTime + a0ColdTime) return;
                if (moveCoroutine != null) StopAllCoroutines();
                StartCoroutine(PreformAttack(0));
                break;

            case EnemyState.ǰ����Ȫ:
                if (Time.time < a1LastTime + a1ColdTime) return;
                if (moveCoroutine != null) StopAllCoroutines();
                StartCoroutine(PreformAttack(1));
                break;
            case EnemyState.���ǽ�:
                if (Time.time < a2LastTime + a2ColdTime) return;
                if (moveCoroutine != null) StopAllCoroutines();
                StartCoroutine(PreformAttack(2));
                break;
            case EnemyState.���ǹ�â��:
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
