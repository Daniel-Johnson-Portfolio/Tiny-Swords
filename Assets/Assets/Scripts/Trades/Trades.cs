using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trades
{
    public TradeItem TradeItem;
    public string Description;
    public int Reward;
    
    
    public int LevelRequirement;
}
public enum TradeItem
{
    Wood,
    Meat,
    Gold,
}
