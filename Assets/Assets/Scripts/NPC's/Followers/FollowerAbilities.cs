using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FollowerAbilities
{
    public Ability Ability;
    public string Title;
    public string Description;
    public int LevelRequirement;
    public bool IsActive;
    public int Cost;
}
public enum Ability
{
    Medic,

}

