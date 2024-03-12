using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_THROWER_TOWER : SCR_AI_THROWER
{

   // private AISettings aiSettings;

    protected override void Start()
    {
        InitializeAISettings(aiSettings);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void HealthCheck()
    {
        //Unable to be kiiled
    }
}
