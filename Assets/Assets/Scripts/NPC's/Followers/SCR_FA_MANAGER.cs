using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SCR_Player_Stats;

public class SCR_FA_MANAGER : MonoBehaviour
{
    public FollowerAbilities[] FA;
    void Start()
    {
        FA = new FollowerAbilities[]
       {
            CreateFA("Medic", "Will Heal You", 1, Ability.Medic, 50),
            //More abilities can be added here
       };
    }

    private FollowerAbilities CreateFA(string title, string description, int LevelRequirement, Ability ability, int cost)
    {
        FollowerAbilities newFA = new FollowerAbilities
        {
            Title = title,
            Description = description,
            LevelRequirement = LevelRequirement,
            IsActive = false,
            Ability = ability,
            Cost = cost,
        };

        return newFA;
    }
}
