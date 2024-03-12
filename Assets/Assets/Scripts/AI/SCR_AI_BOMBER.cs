using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_BOMBER : SCR_AI_CLASS
{
    private bool Exploded = false;
    [SerializeField] private float timeSinceTrigger = 0f;
    [SerializeField] bool Trigger = false;
    [SerializeField] private AISettings aiSettings;
    [SerializeField] private float ExplosionRadius;
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    private const float explosionRadiusMulitplier = 2;

    protected void Start()
    {
        InitializeAISettings(aiSettings);
        ExplosionRadius = aiSettings.attackRadiusSize * explosionRadiusMulitplier;
        base.Start(); 
    }

    protected override void Update()
    {
        base.Update();
        if (Trigger) 
        {
            timeSinceTrigger += Time.deltaTime;
            Attack();
        }
        HealthCheck();
    }

    //In this case AnimChecker is used to check if the AI has exploded as it occurs in the animation
    protected override void AnimChecker()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Explosion"))
        {
            //The explosion it self is too small so we scale it up when it occurs
            gameObject.transform.localScale = new Vector3(2, 2, 2);
            Exploded = true;
            DamagePlayer();
        }

    }
    protected override void PlayerSpotted()
    {
        if (distanceToPlayer <= aiSettings.attackRadiusSize)
        {
            Attack();
            
        }
        CoolDown();

    }

    private void CoolDown()
    {
        if (timeSinceTrigger >= aiSettings.attackCooldown && Trigger)
        {
            animator.SetBool("Trigger", false);
        }
    }

    protected override void Attack()
    {
        if (timeSinceTrigger > aiSettings.attackCooldown)
        {
            animator.SetBool("Trigger", false);
            Trigger = false;

        }
        else 
        {
            animator.SetBool("Trigger", true);
            Trigger = true;
            base.shouldMoveRandomly = false;
            agent.speed = 0;
            agent.ResetPath();

        }
    }

    protected override void HealthCheck()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (Exploded && !currentState.IsName("Explosion")) 
        {
            Vector3 aiPosition = transform.position;
            GameObject goldInstance = Instantiate(Resources.Load<GameObject>("Gold"), aiPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!enemies.Contains(collision.gameObject)) 
            {
                enemies.Add(collision.gameObject);
            }
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemies.Remove(collision.gameObject);

        }
    }


    void DamagePlayer()
    {
        if (base.distanceToPlayer <= ExplosionRadius) 
        {
            if (base.playerController != null) 
            {
                base.playerController.CurrentHealth -= 10;
                base.playerStats.IncrementDamageTaken(10);
            }
        }

        foreach (GameObject enemy in enemies)
        {
            float DistanceToEnemy = Vector2.Distance(transform.position, enemy.gameObject.transform.position);
            if (enemy.GetComponent<SCR_AI_CLASS>() != null && DistanceToEnemy <= ExplosionRadius)
            {
                enemy.GetComponent<SCR_AI_CLASS>().DamageAI(1);
                Debug.Log("Damaged Enemy" + enemy);
            }
        }
        HealthCheck();
        
    }

}
