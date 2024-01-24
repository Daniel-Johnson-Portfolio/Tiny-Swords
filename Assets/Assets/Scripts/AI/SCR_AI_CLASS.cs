using UnityEngine;
using UnityEngine.AI;

public class SCR_AI_CLASS : MonoBehaviour
{
    protected virtual float AttackRadiusSize
    {
        get { return 1f; }
    }
    protected virtual float attackCooldown
    {
        get { return 1f; }
    }
    protected virtual int MaxHealth
    {
        get { return 1; }
    }

    [Header("Target Settings")]
    [SerializeField] protected Transform Target;
    [SerializeField] protected SCR_Player_MasterController playerController;
    [SerializeField] protected float distanceToPlayer;
    [SerializeField] protected SCR_Player_Stats playerStats;

    [Header("AI Settings")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected const float MoveRadius = 15f;
    [SerializeField] protected Animator animator;
    [SerializeField] protected bool shouldMoveRandomly;
    [SerializeField] protected const float IndividualMoveRadius = 5f;
    public int AICurrentHealth;

    [Header("AI Attack Setings")]
    [SerializeField] protected float timeSinceLastAttack = 0f;
    [SerializeField] protected float timeSinceLastMove = 0f;
    [SerializeField] protected const float randomMoveCooldown = 3f;

    protected virtual void Start()
    {
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        Target = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        AICurrentHealth = MaxHealth;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        SetRandomDestination();
    }

    protected virtual void Update()
    {
        AnimChecker();

        float maxSpeed = Mathf.Max(agent.velocity.magnitude, 0f);
        animator.SetFloat("Speed", maxSpeed);

        UpdateRotation();

        Collider2D[] overlappingColliders = new Collider2D[50];
        int numOverlapping = Physics2D.OverlapCollider(agent.GetComponent<Collider2D>(), new ContactFilter2D(), overlappingColliders);

        shouldMoveRandomly = true;

        for (int i = 0; i < numOverlapping; i++)
        {
            Collider2D touchingCollider = overlappingColliders[i];
            GameObject touchingObject = touchingCollider.gameObject;

            if (touchingObject.CompareTag("Player"))
            {
                playerController = touchingObject.GetComponent<SCR_Player_MasterController>();

                if (playerController != null)
                {
                    shouldMoveRandomly = false;
                    distanceToPlayer = Vector2.Distance(transform.position, touchingObject.transform.position);

                    if (distanceToPlayer <= MoveRadius)
                    {
                        agent.SetDestination(Target.position);
                    }

                    PlayerSpotted();
                    
                }
            }
        }

        if (shouldMoveRandomly && timeSinceLastMove >= randomMoveCooldown)
        {
            SetRandomDestination();
            timeSinceLastMove = 0f;
        }

        timeSinceLastMove += Time.deltaTime;

        HealthCheck();
        timeSinceLastAttack += Time.deltaTime;
    }

    public virtual void DamageAI(int damage)
    {
        AICurrentHealth -= damage;
    }

    protected virtual void PlayerSpotted()
    {
        if (distanceToPlayer <= AttackRadiusSize && timeSinceLastAttack >= attackCooldown)
        {
            Attack();
            timeSinceLastAttack = 0f;
        }

        if (distanceToPlayer <= AttackRadiusSize && timeSinceLastAttack >= 0.5f)
        {
            playerController.CurrentHealth -= 2;
            playerStats.IncrementDamageTaken(2);
        }
    }


    protected virtual void AnimChecker()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (!currentState.IsName("Attack_Down"))
        {
            animator.SetBool("AttackDown", false);
        }
        if (!currentState.IsName("Attack_Up"))
        {
            animator.SetBool("AttackUp", false);
        }
        if (!currentState.IsName("Attack_Right"))
        {
            animator.SetBool("AttackFront", false);
        }
    }

    protected virtual void Attack()
    {
        float horizontalInput = agent.velocity.x;
        float verticalInput = agent.velocity.y;

        if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
        {
            animator.SetBool("AttackFront", true);
        }
        else
        {
            animator.SetBool("AttackUp", verticalInput > 0);
            animator.SetBool("AttackDown", verticalInput < 0);
        }
    }

    protected virtual void SetRandomDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * IndividualMoveRadius;
        Vector3 randomDestination = new Vector3(transform.position.x + randomPoint.x, transform.position.y + randomPoint.y, transform.position.z);
        agent.SetDestination(randomDestination);
    }

    protected virtual void HealthCheck()
    {
        if (AICurrentHealth <= 0)
        {
            playerStats.IncrementAmountKilled();
            Vector3 aiPosition = transform.position;
            GameObject goldInstance = Instantiate(Resources.Load<GameObject>("Gold"), aiPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    protected virtual void UpdateRotation()
    {
        if (agent.velocity.x < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (agent.velocity.x > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}