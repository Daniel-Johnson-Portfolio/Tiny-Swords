using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SCR_TRADE_GIVER : SCR_M_GIVER_A_CLASS
{
    public SCR_TradeManager tradeManager;
    [SerializeField] private Trades selectedTrade = null;
    public bool IsTradeSelected = false;
    private SCR_PlayerInventory playerInventory;

    private void Start()
    {
        base.TAG = "Trader";
        playerInventory = FindObjectOfType<SCR_PlayerInventory>();
        tradeManager = GameObject.Find("TradeManager").GetComponent<SCR_TradeManager>();
        base.StartInteraction();

    }

    protected override void Selection()
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

    protected override void SetText(TMP_Text Title, TMP_Text Description)
    {
        Title.text = "Trade";
        Description.text = selectedTrade.Description;

    }
    protected override void Update()
    {
        GameObject[] Traders = GameObject.FindGameObjectsWithTag("NPC_TRADER");
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

    public override void HandleDenyReRoll(Button button)
    {
        IsTradeSelected = false;
        base.HandleDenyReRoll(button);
    }

    public void OnClick()
    {
        title.text = "Trade";
        description.text = selectedTrade.Description;
    }

    public override void HandleAccept(Button button)
    {
        TMP_Text TradeError = button.transform.parent.transform.Find("TradeError").GetComponent<TMP_Text>();

        switch (selectedTrade.TradeItem)
        {
            case TradeItem.Wood:

                if (playerInventory.PlayerInventory.Wood >= selectedTrade.Amount)
                {
                    playerInventory.PlayerInventory.Wood -= selectedTrade.Amount;
                    playerStats.IncrementXP(selectedTrade.Reward);
                    button.transform.parent.parent.parent.parent.gameObject.GetComponent<SCR_NPC_TRADER>().TransactionComplete = true;
                    IsTradeSelected = false;
                    tools.ResetCamera();
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
                    tools.ResetCamera();
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

