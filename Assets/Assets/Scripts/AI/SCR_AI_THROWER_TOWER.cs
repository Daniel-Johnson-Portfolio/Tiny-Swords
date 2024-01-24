using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_THROWER_TOWER : SCR_AI_THROWER
{
    protected override float AttackRadiusSize
    {
        get { return 15f; }
    }
    protected override float attackCooldown
    {
        get { return 5f; }
    }

    protected override void Start()
    {
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
