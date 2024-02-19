using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Quest[] quests;

    void Start()
    {
        // Initialize the array and instantiate objects of Quest
        quests = new Quest[]
        {
            //CreateQuest("Wet Feet", "Go swimming", 100, 50, Goaltype.Location, 1),
            CreateQuest("Getting Wood", "Cut down 3 trees", 200, 1, Goaltype.Gathering, 3, "Wood"),
            CreateQuest("Killing Goblins", "Kill 3 Goblins", 200, 1, Goaltype.kill, 3, ""),
            CreateQuest("Killing Goblins 2", "Kill 6 Goblins", 400, 3, Goaltype.kill, 6, ""),
            CreateQuest("Kill 3 sheep", "Collect 3 Meat", 400, 3, Goaltype.Gathering, 3, "Meat"),
            // Add more quests as needed
        };
    }

    // Method to create a quest with a specified goal type
    private Quest CreateQuest(string title, string description, int xpReward, int LevelRequirement, Goaltype goalType, int requiredAmount, string ToGather = "N/A")
    {
        Quest newQuest = new Quest
        {
            Title = title,
            Description = description,
            XPReward = xpReward,
            LevelRequirement = LevelRequirement,
            IsActive = false,
            IsComplete = false,
            TypeToGather = ToGather,
            goal = new QuestGoal
            {
                Goaltype = goalType,
                requiredAmount = requiredAmount,
                CurrentAmount = 0
            }
        };

        return newQuest;
    }
}

