using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SCR_QUEST_GIVER : SCR_M_GIVER_A_CLASS
{
    [Header("Objects")]
    [SerializeField] public QuestManager questManager;

    [Header("Quest")]
    [SerializeField] private Quest selectedQuest = null;
    [SerializeField] public bool IsQuestSelected { get; private set; } = false;

    private void Start()
    {
        base.TAG = "NPC_QUEST";
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        base.StartInteraction();
    }

    protected override void Selection()
    {
        if (IsQuestSelected == false || selectedQuest.IsComplete == true)
        {
            selectedQuest.IsComplete = false;

            do
            {
                int randomIndex = Random.Range(0, questManager.quests.Length);
                selectedQuest = questManager.quests[randomIndex];
            } while (selectedQuest.LevelRequirement > playerStats.playerStats.Level);

            IsQuestSelected = true;
        }
    }

    protected override void SetText(TMP_Text Title, TMP_Text Description) 
    {
        Title.text = selectedQuest.Title;
        Description.text = selectedQuest.Description;

    }
    protected override void Update()
    {
        base.Update();
    }

    public override void HandleDenyReRoll(Button button)
    {
        IsQuestSelected = false;
        base.HandleDenyReRoll(button);
    }

    public void OnClick()
    {
        title.text = selectedQuest.Title;
        description.text = selectedQuest.Description;
    }

    public override void HandleAccept(Button button)
    {
        tools.ResetCamera();
        selectedQuest.IsActive = true;
        player.GetComponent<SCR_Player_MasterController>().quest = selectedQuest;
        CurrentNPC = button.gameObject.transform.parent.parent.parent.parent.gameObject;

        StartCoroutine(tools.Open(player.transform.GetChild(0).GetChild(0).gameObject));
        StartCoroutine(tools.Open(player.transform.GetChild(0).GetChild(1).gameObject));
        StartCoroutine(tools.Close(CurrentNPC.transform.GetChild(0).gameObject));
    }
}
