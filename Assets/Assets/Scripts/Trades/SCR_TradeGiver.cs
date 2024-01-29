using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SCR_TradeGiver : MonoBehaviour
{
    public SCR_TradeManager tradeManager;
    [SerializeField] private Trades selectedTrade = null;

    private TMP_Text title;
    private TMP_Text description;
    private SCR_Player_Stats playerStats;
    private SCR_PlayerInventory playerInventory;
    [SerializeField] private GameObject player;
    [SerializeField] public GameObject CurrentNPC;
    [SerializeField] public SCR_Tools tools;

    public bool IsTradeSelected = false;

    private void Start()
    {
        tradeManager = FindObjectOfType<SCR_TradeManager>();
        tools = FindObjectOfType<SCR_Tools>();
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        playerInventory = FindObjectOfType<SCR_PlayerInventory>();
        player = GameObject.FindGameObjectWithTag("Player");
        //title = transform.parent.GetChild(0).GetComponent<TMP_Text>();
        //description = transform.parent.GetChild(1).GetComponent<TMP_Text>();
        // Call the Selection method once in Start to choose a quest
        Selection();
    }

    private void Selection()
    {
        if (IsTradeSelected == false)
        {
            do
            {
                int randomIndex = Random.Range(0, tradeManager.tradesList.Length);
                selectedTrade = tradeManager.tradesList[randomIndex];
            } while (selectedTrade.LevelRequirement > playerStats.playerStats.Level);

            IsTradeSelected = true;
        }
    }

    public void DenyReRoll(Button button)
    {
        button.transform.parent.transform.GetChild(3).GetComponent<TMP_Text>().text = "";
        button.transform.parent.parent.parent.parent.gameObject.GetComponent<SCR_NPC_TRADER>().TransactionComplete = true;
        IsTradeSelected = false;
        Selection();
    }

    private void Update()
    {
        GameObject[] Traders = GameObject.FindGameObjectsWithTag("Trader");
        Selection();
        foreach (GameObject Trader in Traders)
        {
            TMP_Text npcTitle = Trader.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            TMP_Text npcDescription = Trader.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<TMP_Text>();
            TMP_Text npcReward = Trader.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(2).GetComponent<TMP_Text>();

            npcTitle.text = "Trade";
            npcDescription.text = selectedTrade.Amount.ToString() + " " + selectedTrade.Description;
            npcReward.text = "For: " + selectedTrade.Reward.ToString() + " XP";

        }
    }

    public void OnClick()
    {
        // Display quest details when the player clicks
        description.text = selectedTrade.Description;
    }

    public void AcceptTrade(Button button)
    {
        TMP_Text TradeError = button.transform.parent.transform.GetChild(3).GetComponent<TMP_Text>();

        switch (selectedTrade.TradeItem) 
        {
                case TradeItem.Wood:

                        if (playerInventory.PlayerInventory.Wood >= selectedTrade.Amount)
                        {
                            playerInventory.PlayerInventory.Wood -= selectedTrade.Amount;
                            playerStats.IncrementXP(selectedTrade.Reward);
                            button.transform.parent.parent.parent.parent.gameObject.GetComponent<SCR_NPC_TRADER>().TransactionComplete = true;
                            IsTradeSelected = false;
                            Selection();
                        }
                        else
                        {
                            TradeError.text = "You do not have enough resources!";
                        }
                        break;

                case TradeItem.Gold:

                        if (playerInventory.PlayerInventory.Gold >= selectedTrade.Amount)
                        {
                            playerInventory.PlayerInventory.Gold -= selectedTrade.Amount;
                            playerStats.IncrementXP(selectedTrade.Reward);
                            button.transform.parent.parent.parent.parent.gameObject.GetComponent<SCR_NPC_TRADER>().TransactionComplete = true;
                            IsTradeSelected = false;
                            Selection();
                        }
                        else
                        {
                            TradeError.text = "You do not have enough resources!";
                        }
                        break;

                default:

                        TradeError.text = "You do not have enough resources!";
                        break;
        }

        
    }
}

