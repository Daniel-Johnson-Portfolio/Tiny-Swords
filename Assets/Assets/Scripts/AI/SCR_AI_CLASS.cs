using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCR_AI_CLASS : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] protected Transform Target;
    [SerializeField] protected SCR_Player_MasterController playerController;
    [SerializeField] protected float distanceToPlayer;
    [SerializeField] protected SCR_Player_Stats playerStats;
    [SerializeField] protected GameObject Player;

    [Header("AI Settings")]
    
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    //[SerializeField] protected Animator animator2;
    [SerializeField] protected bool shouldMoveRandomly;
    public int AICurrentHealth;

    [Header("AI Attack Setings")]
    [SerializeField] protected float timeSinceLastAttack = 0f;
    [SerializeField] protected float timeSinceLastMove = 0f;

    [SerializeField] protected AISettings aiSettings { get; private set; }
    protected List<GameObject> gameObjects = new List<GameObject>();

    protected void InitializeAISettings(AISettings settings)
    {
        aiSettings = settings;
    }


    protected virtual void Start()
    {
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        Target = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        
        agent = GetComponent<NavMeshAgent>();
        AICurrentHealth = aiSettings.maxHealth;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        SetRandomDestination();
    }

    protected virtual void Update()
    {
        AnimChecker();
        UpdateRotation();
        HandleMovement();
        UpdateTimers();
    }

    private void HandleMovement()
    {
        animator.SetFloat("Speed", Mathf.Max(agent.velocity.magnitude, 0f));
       
        if (playerController != null)
        {
            distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);

            if (distanceToPlayer <= aiSettings.moveRadius)
            {
                agent.SetDestination(Target.position);
            }
            PlayerSpotted();
        }

        if (shouldMoveRandomly && timeSinceLastMove >= aiSettings.randomMoveCooldown)
        {
            SetRandomDestination();
            timeSinceLastMove = 0f;
        }
    }


    private void UpdateTimers()
    {
        timeSinceLastMove += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;
    }


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
        // Player has entered the detection area
            playerController = other.GetComponent<SCR_Player_MasterController>();
            shouldMoveRandomly = false;
            Player = other.gameObject;
            
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Player has exited the detection area
            shouldMoveRandomly = true;
        }
    }

    public virtual void DamageAI(int damage)
    {
        AICurrentHealth -= damage;
        HealthCheck();
    }

    protected virtual void PlayerSpotted()
    {
        if (distanceToPlayer <= aiSettings.attackRadiusSize && timeSinceLastAttack >= aiSettings.attackCooldown)
        {
            Attack();
            timeSinceLastAttack = 0f;
        }

        if (distanceToPlayer <= aiSettings.attackRadiusSize && timeSinceLastAttack >= 0.5f)
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
            //animator2.SetBool("AttackRight", false);
        }
    }

    protected virtual void Attack()
    {
        float horizontalInput = agent.velocity.x;
        float verticalInput = agent.velocity.y;

        if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
        {
            if (!animator.GetBool("AttackFront"))
            {
                animator.SetBool("AttackFront", true);
                //if(animator2 != null) animator2.SetBool("AttackRight", true);
            }
        }
        else
        {
            if (!animator.GetBool("AttackUp")) 
            {
                animator.SetBool("AttackUp", verticalInput > 0);
            }
            if (!animator.GetBool("AttackDown"))
            {
                animator.SetBool("AttackDown", verticalInput < 0);
            }
        }
    }

    protected virtual void SetRandomDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * aiSettings.individualMoveRadius;
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