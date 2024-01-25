using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TradeManager : MonoBehaviour
{
    public Trades[] tradesList;
    void Start()
    {
        tradesList = new Trades[]
        {
            // Example trades creation
            CreateTrade("Wood Bundle", 10, TradeItem.Wood),
            CreateTrade("Meat Pack", 15, TradeItem.Meat),
            CreateTrade("Gold Ingot", 25, TradeItem.Gold),
         
         };
    }

    // Example method to create a trade and add it to the tradesList
    private Trades CreateTrade(string description, int cost, TradeItem itemType)
    {
        Trades newTrade = new Trades
        {
            Description = description,
            Reward = cost,
            TradeItem = itemType
            // Add more properties as needed
        };

        return newTrade;
    }

}
