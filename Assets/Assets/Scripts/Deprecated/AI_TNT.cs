using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AI_TNT : MonoBehaviour
{
    [SerializeField] Transform Target;
    public float MoveRadius;
    public float AttackRadiusSize = 10f;

    NavMeshAgent agent;
    Animator animator;
    float attackCooldown = 5f;
    float timeSinceLastAttack = 0f;
    public float timeSinceLastMove = 0f;
    public float randomMoveCooldown = 3f;
    private float distanceToPlayer;
    public int MaxHealth = 1;
    public int AICurrentHealth;

    public float IndividualMoveRadius = 5f;

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

        bool shouldMoveRandomly = true; // Check if the AI should move randomly

        for (int i = 0; i < numOverlapping; i++)
        {
            Collider2D touchingCollider = overlappingColliders[i];
            GameObject touchingObject = touchingCollider.gameObject;

            if (touchingObject.CompareTag("Player"))
            {
                SCR_Player_MasterController playerController = touchingObject.GetComponent<SCR_Player_MasterController>();

                if (playerController != null)
                {
                    shouldMoveRandomly = false; // Player detected, don't move randomly

                    distanceToPlayer = Vector2.Distance(transform.position, touchingObject.transform.position);

                    // Move towards the player if within the MoveRadius
                    if (distanceToPlayer <= MoveRadius)
                    {
                        agent.SetDestination(Target.position);
                    }

                    // Attack if within the AttackRadius
                    if (distanceToPlayer <= (AttackRadiusSize / 2)) 
                    {
                        agent.ResetPath();
                    }
                    if (distanceToPlayer <= AttackRadiusSize && timeSinceLastAttack >= attackCooldown)
                    {
                        Attack(playerController);
                        timeSinceLastAttack = 0f;
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

        HealthCheck();
        timeSinceLastAttack += Time.deltaTime;
    }

    void AnimChecker()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (!currentState.IsName("Throw"))
        {
            animator.SetBool("Throw", false);
        }
    
    }

    void Attack(SCR_Player_MasterController playerController)
    {
        animator.SetBool("Throw", true);

        Vector3 aiPosition = transform.position + new Vector3(0, 1, 0);
        if (distanceToPlayer <= AttackRadiusSize && timeSinceLastAttack >= 0.5f)
        {
            GameObject DynamiteInstance = Instantiate(Resources.Load<GameObject>("Dynamite"), aiPosition, Quaternion.identity);
        }
    }

 
    void SetRandomDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * IndividualMoveRadius;
        Vector3 randomDestination = new Vector3(transform.position.x + randomPoint.x, transform.position.y + randomPoint.y, transform.position.z);
        agent.SetDestination(randomDestination);
    }

    void HealthCheck()
    {
        if (AICurrentHealth <= 0)
        {
            Vector3 aiPosition = transform.position;
            GameObject goldInstance = Instantiate(Resources.Load<GameObject>("Gold"), aiPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

