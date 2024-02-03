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
            CreateTrade("Wood", 10, TradeItem.Wood, 3, 1),
           // CreateTrade("Meat Pack", 15, TradeItem.Meat, 3),
            CreateTrade("Gold", 25, TradeItem.Gold, 3, 1),
            CreateTrade("Gold", 50, TradeItem.Gold, 6, 1),
            CreateTrade("Gold", 75, TradeItem.Gold, 9, 1),
            CreateTrade("Meat", 75, TradeItem.Meat, 1, 1),

         };
    }

    // Example method to create a trade and add it to the tradesList
    private Trades CreateTrade(string description, int reward, TradeItem itemType, int amount, int levelRequirement)
    {
        Trades newTrade = new Trades
        {
            Amount = amount,
            Description = description,
            Reward = reward,
            TradeItem = itemType,
            LevelRequirement = levelRequirement
            // Add more properties as needed
        };

        return newTrade;
    }

}
