using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Quest
{
    
        public string Title;
        public string Description;
        public int XPReward;
        public int LevelRequirement;
        public bool IsActive;
        public bool IsComplete = false;
        public string TypeToGather;

    public QuestGoal goal;

    public void Complete()
    {
        IsActive = false;
        IsComplete = true;

        // Call the Selection method in the QuestGiver
       
    }

}
