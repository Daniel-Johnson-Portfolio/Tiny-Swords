using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_SHEEP : SCR_AI_CLASS
{
    [SerializeField] private AISettings aiSettings;
    [SerializeField] private Vector3 runToPosition;
    private bool isRunning = false;
    protected override void Start()
    {
        InitializeAISettings(aiSettings);
        base.Start();

    }

    protected override void Update()
    {
        if (isRunning)
        {
            PlayerSpotted();
        }
        else 
        {
            base.shouldMoveRandomly = true;
        }
        
        base.Update();
    }

    protected override void PlayerSpotted()
    {
        Flee();
    }
    private void Flee() 
    {
        Vector3 directionAwayFromPlayer = (transform.position - Player.transform.position).normalized;
        Vector3 runToPosition;
        bool positionFound = false;

        // Attempt to find a valid run to position within the NavMesh
        for (int i = 0; i < 360; i += 45)
        {
            // Calculate a new direction by rotating the initial direction
            Vector3 potentialDirection = Quaternion.Euler(0, i, 0) * directionAwayFromPlayer;
            runToPosition = transform.position + potentialDirection * aiSettings.individualMoveRadius;

            // Check if the new position is within the NavMesh
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(runToPosition, out hit, aiSettings.individualMoveRadius, UnityEngine.AI.NavMesh.AllAreas))
            {
                // If a position within the NavMesh is found, use it and break the loop
                agent.SetDestination(hit.position);
                positionFound = true;
                break;
            }
        }

        if (!positionFound)
        {
            // If no valid position was found, optionally log a warning or handle the case as needed
            Debug.Log("Could not find a valid position within the NavMesh for the sheep to run to .");
        }

        shouldMoveRandomly = false;

    }

    protected override void Attack()
    {
        //sheep does not attack
    }
    protected override void AnimChecker()
    {
        
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            Player = other.gameObject;
            isRunning = true;
        }
       
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        
    }
    protected override void HealthCheck()
    {
        if (AICurrentHealth <= 0)
        {
            transform.GetComponent<AudioSource>().Play();
            playerStats.IncrementAmountKilled();
            Vector3 aiPosition = transform.position;
            GameObject goldInstance = Instantiate(Resources.Load<GameObject>("Meat"), aiPosition, Quaternion.identity);
            transform.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(waitForSound());
        }
    }
    IEnumerator waitForSound()
    {
        //Wait Until Sound has finished playing
        while (transform.GetComponent<AudioSource>().isPlaying)
        {
            yield return null;
        }

        //Auidio has finished playing, destroy GameObject
        Destroy(gameObject);
    }


}

