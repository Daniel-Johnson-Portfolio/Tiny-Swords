using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configurations/PlayerConfig", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public float movementSpeed = 5f;
    public int maxHealth = 100;
    // Add other configurable fields here
}

