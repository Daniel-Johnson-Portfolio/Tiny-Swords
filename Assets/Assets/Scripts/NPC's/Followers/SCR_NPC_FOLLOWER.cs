using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SCR_NPC_FOLLOWER : SCR_NPC_CLASS
{
    [Header("Agent Settings")]
    [SerializeField] private float AcceptanceDistance = 1.5f;
    [SerializeField] float distanceToPlayer;
    [SerializeField] private NavMeshAgent agent;

    [Header("Objects")]
    [SerializeField] protected SCR_FOLLOW_ABILITY_GIVER FA_Giver;
    [SerializeField] GameObject player;
    [SerializeField] private Animator animator;

    [Header("Conditions")]
    [SerializeField] public bool CurrentlyActive = false;
    [SerializeField] private bool Cooldown = true;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!CurrentlyActive)
        {
            base.Update();
        }
        if (CurrentlyActive)
        {
            SetTarget();
            if (Cooldown) 
            {
                Cooldown = false;
                StartCoroutine(Action());
            }
        }
        animator.SetFloat("Speed", Mathf.Max(agent.velocity.magnitude, 0f));
    }
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (!CurrentlyActive)
        {
            base.OnTriggerEnter2D(collider);
        }
    }
    protected override void OnTriggerExit2D(Collider2D collider)
    {
        if (!CurrentlyActive)
        {
            base.OnTriggerExit2D(collider);
            if (FA_Giver.FAError != null)
            {
                FA_Giver.FAError.text = "";
            }
            
        }
    }


    protected override void SetButtons()
    {
        FA_Giver = FindObjectOfType<SCR_FOLLOW_ABILITY_GIVER>();
        base.denyButton.onClick.AddListener(delegate { FA_Giver.HandleDenyReRoll(denyButton); });
        base.acceptButton.onClick.AddListener(delegate { FA_Giver.HandleAccept(acceptButton); });
    }

    private void SetTarget()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > AcceptanceDistance)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.ResetPath();
        }

    }
    private IEnumerator Action() 
    {
        Debug.Log("ACTION");
        while (distanceToPlayer <= AcceptanceDistance && !Cooldown) 
        {
            Debug.Log("ACTION2");
            switch (FA_Giver.SelectedFA.Ability)
            {
                case Ability.Medic:
                    {
                        player.GetComponent<SCR_Player_MasterController>().CurrentHealth += 1;
                    }
                    break;
                    //Other Follower Ability functions
            }
            yield return new WaitForSeconds(0.1f);

        }
        Cooldown = true;
    }
}
