using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType 
{
    Melee,
    Thrower,
    Bomber,
}

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Eenmies", order = 1)]
public class SCR_SO_ENEMY : ScriptableObject
{
    public EnemyType enemyType;
    public float AttackRadius;
    public float AttackCooldown;
    public int MaxHealth;
    public GameObject Enemy;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
