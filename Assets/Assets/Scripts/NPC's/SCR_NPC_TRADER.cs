using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_NPC_TRADER : SCR_NPC_CLASS
{
    public bool TransactionComplete = false;
    [SerializeField] protected SCR_TRADE_GIVER TradeGiver;
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
            timer += Time.deltaTime;
            if (timer > 120) //2 minute cooldown
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            base.OnTriggerExit2D(collision);
            if (TradeGiver.TradeError != null)
            {
                TradeGiver.TradeError.text = "";
            }
                
        }
    }

    protected override void SetButtons() 
    {
        TradeGiver = FindObjectOfType<SCR_TRADE_GIVER>();
        base.denyButton.onClick.AddListener(delegate { TradeGiver.HandleDenyReRoll(denyButton); });
        base.acceptButton.onClick.AddListener(delegate { TradeGiver.HandleAccept(acceptButton); });
    }
}
