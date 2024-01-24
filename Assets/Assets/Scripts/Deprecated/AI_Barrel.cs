using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Reflection;
public class AI_barrel : MonoBehaviour
{
    [SerializeField] Transform Target;
    public float MoveRadius;
    public float AttackRadiusSize = 2f;

    NavMeshAgent agent;
    Animator animator;
    float attackCooldown = 1f;
    float timeSinceLastAttack = 0f;
    public float timeSinceLastMove = 0f;
    public float randomMoveCooldown = 3f;
    private bool Trigger = false;
    private bool Exploded = false;
    private bool shouldMoveRandomly = true;
    public int MaxHealth = 1;
    public int AICurrentHealth;
    private float distanceToPlayer;
    public float IndividualMoveRadius = 5f;
    private SCR_Player_MasterController playerController;
    void Start()
    {
        MoveRadius = 10f;
        // Assign a random starting position within the overall MoveRadius
        Vector2 randomOffset = Random.insideUnitCircle * MoveRadius;
        Vector3 randomPosition = new Vector3(transform.position.x + randomOffset.x, transform.position.y + randomOffset.y, transform.position.z);


        Target = GameObject.FindWithTag("Player").transform;
        AICurrentHealth = MaxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();

        // Set initial random destination within the individual move radius
        SetRandomDestination();
    }

    void Update()
    {
        AnimChecker();
        HealthCheck();
        // Update animation speed based on NavMeshAgent speed
        float maxSpeed = Mathf.Max(agent.velocity.magnitude, 0f);
        animator.SetFloat("Speed", maxSpeed);

        if (agent.velocity.x < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (agent.velocity.x > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        Collider2D[] overlappingColliders = new Collider2D[50];
        int numOverlapping = Physics2D.OverlapCollider(agent.GetComponent<Collider2D>(), new ContactFilter2D(), overlappingColliders);

         // Check if the AI should move randomly

        for (int i = 0; i < numOverlapping; i++)
        {
            Collider2D touchingCollider = overlappingColliders[i];
            GameObject touchingObject = touchingCollider.gameObject;

            if (touchingObject.CompareTag("Player"))
            {
                playerController = touchingObject.GetComponent<SCR_Player_MasterController>();

                if (playerController != null)
                {
                    shouldMoveRandomly = false; // Player detected, don't move randomly

                    distanceToPlayer = Vector2.Distance(transform.position, touchingObject.transform.position);

                    // Move towards the player if within the MoveRadius
                    if (distanceToPlayer <= MoveRadius && Trigger == false)
                    {
                        agent.SetDestination(Target.position);
                    }

                    // Attack if within the AttackRadius
                    if (distanceToPlayer <= 1 && timeSinceLastAttack >= attackCooldown && Trigger == false)
                    {
                        Attack(playerController);
                        timeSinceLastAttack = 0f;
                    }

                    if (timeSinceLastAttack >= attackCooldown && Trigger == true)
                    {
                        animator.SetBool("Trigger", false);
                    }

                }
            }
        }

        if (shouldMoveRandomly && timeSinceLastMove >= randomMoveCooldown)
        {
            SetRandomDestination();
            timeSinceLastMove = 0f;
        }

        timeSinceLastMove += Time.deltaTime;

        
        timeSinceLastAttack += Time.deltaTime;
    }

    void AnimChecker()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (currentState.IsName("Explosion"))
        {
            gameObject.transform.localScale = new Vector3(2, 2, 2);
            Exploded = true;
            DamagePlayer();
        }
       
    }
    void DamagePlayer()
    {
        Collider2D[] overlappingColliders = new Collider2D[50];
        ContactFilter2D contactFilter = new ContactFilter2D();
        int numOverlapping = Physics2D.OverlapCollider(agent.GetComponent<Collider2D>(), contactFilter, overlappingColliders);

        for (int i = 0; i < numOverlapping; i++)
        {
            Collider2D touchingCollider = overlappingColliders[i];
            GameObject touchingObject = touchingCollider.gameObject;

            if (touchingObject.CompareTag("Player"))
            {
                SCR_Player_MasterController playerController = touchingObject.GetComponent<SCR_Player_MasterController>();
                if (playerController != null)
                {
                    float distanceToPlayer = Vector2.Distance(transform.position, touchingObject.transform.position);
                    if (distanceToPlayer <= AttackRadiusSize)
                    {
                        playerController.CurrentHealth -= 10;
                    }
                }
            }
            else if (touchingObject.CompareTag("Enemy"))
            {
                try 
                {
                    AI script = touchingObject.GetComponent<AI>();
                    float distanceToAI = Vector2.Distance(transform.position, touchingObject.transform.position);
                    if (distanceToAI <= AttackRadiusSize)
                    {
                        script.AICurrentHealth = 0;
                    }
                }
                catch { }
                try
                {
                    AI_TNT script = touchingObject.GetComponent<AI_TNT>();
                    float distanceToAI = Vector2.Distance(transform.position, touchingObject.transform.position);
                    if (distanceToAI <= AttackRadiusSize)
                    {
                        script.AICurrentHealth = 0;
                    }
                }
                catch { }
              
            }

        }
    }


    void Attack(SCR_Player_MasterController playerController)
    {
            animator.SetBool("Trigger", true);
            Trigger = true;
            shouldMoveRandomly = false;
            agent.ResetPath();
    }

    void SetRandomDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * IndividualMoveRadius;
        Vector3 randomDestination = new Vector3(transform.position.x + randomPoint.x, transform.position.y + randomPoint.y, transform.position.z);
        agent.SetDestination(randomDestination);
    }

    void HealthCheck()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (Exploded == true && !currentState.IsName("Explosion")) 
        {
            AICurrentHealth--;
        }
        if (AICurrentHealth <= 0)
        {
            Vector3 aiPosition = transform.position;
            GameObject goldInstance = Instantiate(Resources.Load<GameObject>("Gold"), aiPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}