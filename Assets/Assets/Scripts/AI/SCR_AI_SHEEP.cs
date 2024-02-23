using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_SHEEP : SCR_AI_CLASS
{
    [SerializeField] private AISettings aiSettings;
    [SerializeField] private Vector3 runToPosition;
    private bool isRunning = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        InitializeAISettings(aiSettings);
        base.Start();

    }

    // Update is called once per frame
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
        Vector3 directionAwayFromPlayer = (transform.position - Player.transform.position).normalized;
        Vector3 runToPosition;
        bool positionFound = false;

        // Attempt to find a valid run to position within the NavMesh
        for (int i = 0; i < 360; i += 45) // Adjust the step as needed for finer or coarser adjustments
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

        // Optionally, disable random movement for a while
        shouldMoveRandomly = false;
        // Implement logic to re-enable random movement later if desired
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
        //sheep does not collide with anything
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

        //Auidio has finished playing, disable GameObject
        Destroy(gameObject);
    }


}

