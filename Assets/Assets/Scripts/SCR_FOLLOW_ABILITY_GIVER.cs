using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_FOLLOW_ABILITY_GIVER : SCR_M_GIVER_A_CLASS
{
    public SCR_FA_MANAGER FA_Manager;
    public FollowerAbilities SelectedFA = null;
    private bool IsFaSelected = false;
    public TMP_Text FAError;
    private SCR_PlayerInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = FindObjectOfType<SCR_PlayerInventory>();
        base.TAG = "NPC_FOLLOWER";
        FA_Manager = GameObject.Find("FA_Manager").GetComponent<SCR_FA_MANAGER>();
        base.StartInteraction();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GameObject[] Followers = GameObject.FindGameObjectsWithTag(TAG);
        Selection();
        foreach (GameObject Follower in Followers)
        {
            TMP_Text npcTitle = Follower.transform.GetChild(0).GetChild(0).GetChild(1).Find("FATitle").GetComponent<TMP_Text>();
            TMP_Text npcDescription = Follower.transform.GetChild(0).GetChild(0).GetChild(1).Find("FADescription").GetComponent<TMP_Text>();
            TMP_Text npcCost = Follower.transform.GetChild(0).GetChild(0).GetChild(1).Find("FACost").GetComponent<TMP_Text>();
            npcTitle.text = SelectedFA.Title;
            npcDescription.text = SelectedFA.Description;
            npcCost.text = "For: " + SelectedFA.Cost.ToString() + " Gold";
        }
    }
    protected override void Selection()
    {
        if (IsFaSelected == false)
        {
            do
            {
                int randomIndex = Random.Range(0, FA_Manager.FA.Length);
                SelectedFA = FA_Manager.FA[randomIndex];
            } while (SelectedFA.LevelRequirement > playerStats.playerStats.Level);

            IsFaSelected = true;
        }
    }


    protected override void SetText(TMP_Text Title, TMP_Text Description)
    {
        Title.text = SelectedFA.Title;
        Description.text = SelectedFA.Description;

    }
    public override void HandleDenyReRoll(Button button)
    {
        FAError = button.transform.parent.transform.Find("FAError").GetComponent<TMP_Text>();
        IsFaSelected = false;
        base.HandleDenyReRoll(button);
        FAError.text = "";
    }

    public override void HandleAccept(Button button)
    {
        CurrentNPC = button.gameObject.transform.parent.parent.parent.parent.gameObject;
        FAError = button.transform.parent.transform.Find("FAError").GetComponent<TMP_Text>();

        switch (SelectedFA.Ability)
        {
            case Ability.Medic:

                if (playerInventory.PlayerInventory.Gold >= SelectedFA.Cost)
                {
                    playerInventory.PlayerInventory.Gold -= SelectedFA.Cost;
                    IsFaSelected = false;
                    tools.ResetCamera();
                    CurrentNPC.GetComponent<SCR_NPC_FOLLOWER>().CurrentlyActive = true;
                    Selection();
                    StartCoroutine(tools.Close(CurrentNPC.transform.GetChild(0).gameObject));
                }
                else
                {
                    FAError.text = "You do not have enough resources!";
                }
                break;




        }


    }
}
