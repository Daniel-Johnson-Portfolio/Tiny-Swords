using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SCR_StatsMenu : MonoBehaviour
{
    protected SCR_Player_Stats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<SCR_Player_Stats>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Find("GoblinsKilled_Text").GetComponent<TextMeshProUGUI>().text = "Total Goblins Killed:  " + playerStats.goblinStats.AmountKilled;
        transform.Find("DamageDealt_Text").GetComponent<TextMeshProUGUI>().text = "Total Damage Dealt: " + playerStats.goblinStats.DamageDealt;
        transform.Find("DamageTaken_Text").GetComponent<TextMeshProUGUI>().text = "Total Damage Taken: " + Mathf.Round(playerStats.goblinStats.DamageTaken * 10.0f) * 0.1f;
        transform.Find("TimesDied_Text").GetComponent<TextMeshProUGUI>().text = "Total Times Died: " + playerStats.playerStats.TimesDied;
        transform.Find("WoodCollected_Text").GetComponent<TextMeshProUGUI>().text = "Total Wood Collected: " + playerStats.resourceStats.WoodCollected;
        transform.Find("GoldCollected_Text").GetComponent<TextMeshProUGUI>().text = "Total Gold Collected: " + playerStats.resourceStats.GoldCollected;
        transform.Find("AmountOfQuests_Text").GetComponent<TextMeshProUGUI>().text = "Total Quests Completed: " + playerStats.questStats.AmountCompleted;
        transform.Find("TimeInQuests_Text").GetComponent<TextMeshProUGUI>().text = "Total Time In Quests: " + playerStats.questStats.TimeSpent;
        transform.Find("CavesExplored_Text").GetComponent<TextMeshProUGUI>().text = "Total Caves Explored: " + playerStats.gameStats.CavesExplored;
        transform.Find("TimeInGame_Text").GetComponent<TextMeshProUGUI>().text = "Total Time In Game: " + Mathf.Round(playerStats.gameStats.TimeInGame * 10.0f) * 0.1f;
        transform.Find("Level_Text").GetComponent<TextMeshProUGUI>().text = "Player Level: " + playerStats.playerStats.Level;
        transform.Find("XP_Text").GetComponent<TextMeshProUGUI>().text = "Total XP Earned: " + playerStats.playerStats.TotalXP;
    }
}
