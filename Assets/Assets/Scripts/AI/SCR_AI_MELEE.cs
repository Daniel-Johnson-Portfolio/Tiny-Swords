using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_Melee : SCR_AI_CLASS
{
    [SerializeField] private AISettings aiSettings;
    protected override void Start()
    {
        InitializeAISettings(aiSettings);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
