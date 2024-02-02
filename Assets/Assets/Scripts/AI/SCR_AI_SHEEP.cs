using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_SHEEP : SCR_AI_CLASS
{
    [SerializeField] private AISettings aiSettings;
    // Start is called before the first frame update
    void Start()
    {
        InitializeAISettings(aiSettings);
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        base.shouldMoveRandomly = true;
        base.Update();
    }

    protected override void PlayerSpotted()
    {
       
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
        //sheep does not collide with anything
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        //sheep does not collide with anything
    }
}
