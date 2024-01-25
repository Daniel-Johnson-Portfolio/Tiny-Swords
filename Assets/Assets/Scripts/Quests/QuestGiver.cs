using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public QuestManager questManager;
    [SerializeField] private Quest selectedQuest = null;

    private TMP_Text title;
    private TMP_Text description;
    private SCR_Player_Stats playerStats;
    private GameObject player;
    public GameObject CurrentNPC;
    [SerializeField] public SCR_Tools tools;

    public bool IsQuestSelected { get; private set; } = false;

    private void Start()
    {
        tools = FindObjectOfType<SCR_Tools>();
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        player = GameObject.FindGameObjectWithTag("Player");
        title = transform.parent.GetChild(0).GetComponent<TMP_Text>();
        description = transform.parent.GetChild(1).GetComponent<TMP_Text>();
        // Call the Selection method once in Start to choose a quest
        Selection();
    }

    private void Selection()
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

    public void DenyReRoll()
    {
        IsQuestSelected = false;
        Selection();
    }

    private void Update()
    {
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");
        Selection();
        foreach (GameObject NPC in NPCs)
        {
            TMP_Text npcTitle = NPC.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            TMP_Text npcDescription = NPC.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<TMP_Text>();

            npcTitle.text = selectedQuest.Title;
            npcDescription.text = selectedQuest.Description;
        }
    }

    public void OnClick()
    {
        // Display quest details when the player clicks
        title.text = selectedQuest.Title;
        description.text = selectedQuest.Description;
    }

    public void AcceptQuest(Button button)
    {
        Camera.main.GetComponent<CameraScript>().PlayerLocked = true;
        Camera.main.orthographicSize = 5;
        selectedQuest.IsActive = true;
        player.GetComponent<SCR_Player_MasterController>().quest = selectedQuest;
        CurrentNPC = button.gameObject.transform.parent.parent.parent.parent.gameObject;
        // Open UI elements
        StartCoroutine(tools.Open(player.transform.GetChild(0).GetChild(0).gameObject));
        StartCoroutine(tools.Open(player.transform.GetChild(0).GetChild(1).gameObject));
    }
}
