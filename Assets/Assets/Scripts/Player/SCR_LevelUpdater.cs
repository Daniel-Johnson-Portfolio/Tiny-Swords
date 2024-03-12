using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_LevelUpdater : MonoBehaviour
{
    private SCR_Player_Stats playerStats;
    private TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        Text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = "Level: " + playerStats.playerStats.Level.ToString();
    }
}
