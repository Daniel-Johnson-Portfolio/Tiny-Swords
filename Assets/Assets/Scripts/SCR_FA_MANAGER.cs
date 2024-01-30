using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SCR_Player_Stats;

public class SCR_FA_MANAGER : MonoBehaviour
{
    public FollowerAbilities[] FA;


    // Start is called before the first frame update
    void Start()
    {
        FA = new FollowerAbilities[]
       {
            CreateFA("Medic", "Will Heal You", 1, Ability.Medic, 5),
            CreateFA("Medic", "Will Heal You", 1, Ability.Medic, 6),
            CreateFA("Medic", "Will Heal You", 1, Ability.Medic, 7),

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



    // Update is called once per frame
    void Update()
    {
        
    }
}
