using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AISettings", menuName = "AI Settings", order = 0)]
public class AISettings : ScriptableObject
{
    public float moveRadius = 15f;
    public float individualMoveRadius = 5f;
    public float randomMoveCooldown = 3f;
    public int maxHealth = 1;
    public float attackCooldown = 1f;
    public float attackRadiusSize = 1f;
    // Add other configurable fields here
}
