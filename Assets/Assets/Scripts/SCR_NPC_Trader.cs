using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_NPC_TRADER : SCR_NPC_CLASS
{
    public bool TransactionComplete = false;
    private float timer = 0;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (TransactionComplete)
        {
            base.CloseAll();
            base.tools.ResetCamera();
            timer += Time.deltaTime;
            if (timer > 1) //2 minute cooldown
            {
                timer = 0;
                TransactionComplete = false;
            }
        }
        if (!TransactionComplete) 
        {
            base.Update();
        }
        
    }

    protected override void SetButtons() 
    {
        base.TradeGiver = FindObjectOfType<SCR_TradeGiver>();
        base.denyButton.onClick.AddListener(delegate { TradeGiver.DenyReRoll(denyButton); });
        base.acceptButton.onClick.AddListener(delegate { TradeGiver.AcceptTrade(acceptButton); });
    }
}
