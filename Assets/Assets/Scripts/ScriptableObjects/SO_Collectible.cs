using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public enum ItemType
{
    Gold,
    Log,
    Meat,
}
[CreateAssetMenu(fileName = "Collectible", menuName = "ScriptableObjects/Collectible", order = 1)]
public class SO_Collectible : ScriptableObject
{
    public AnimatorController animator;
    public ItemType itemType;
    public GameObject gameObject;


}
