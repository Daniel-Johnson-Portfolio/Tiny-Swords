using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Player_Stats : MonoBehaviour
{
    [System.Serializable]
    public struct Goblins 
    {
        [SerializeField] public int AmountKilled;
        [SerializeField] public float DamageDealt;
        [SerializeField] public float DamageTaken;
    }
    [System.Serializable]
    public struct Resources
    {
        [SerializeField] public int WoodCollected;
        [SerializeField] public int GoldCollected;
        [SerializeField] public int MeatCollected;
    }
    [System.Serializable]
    public struct Quests
    {
        [SerializeField] public int AmountCompleted;
        [SerializeField] public float TimeSpent;
    }
    [System.Serializable]
    public struct Game 
    {
        [SerializeField] public float TimeInGame;
        [SerializeField] public int CavesExplored;
    }
    [System.Serializable]
    public struct Player 
    {
        [SerializeField] public int Level;
        [SerializeField] public float XP;
        [SerializeField] public float TotalXP;
        [SerializeField] public int TimesDied;
    }

    [SerializeField] public Goblins goblinStats;
    public Resources resourceStats;
    public Quests questStats;
    public Game gameStats;
    public Player playerStats;


    void SavePlayerStats()
    {
        // Serialize each struct to JSON and save to PlayerPrefs
        PlayerPrefs.SetString("GoblinStats", JsonUtility.ToJson(goblinStats));
        PlayerPrefs.SetString("ResourceStats", JsonUtility.ToJson(resourceStats));
        PlayerPrefs.SetString("QuestStats", JsonUtility.ToJson(questStats));
        PlayerPrefs.SetString("GameStats", JsonUtility.ToJson(gameStats));
        PlayerPrefs.SetString("PlayerStats", JsonUtility.ToJson(playerStats));
        
        // Save PlayerPrefs
        PlayerPrefs.Save();
    }

    void LoadPlayerStats()
    {
        goblinStats = JsonUtility.FromJson<Goblins>(PlayerPrefs.GetString("GoblinStats", JsonUtility.ToJson(new Goblins())));
        resourceStats = JsonUtility.FromJson<Resources>(PlayerPrefs.GetString("ResourceStats", JsonUtility.ToJson(new Resources())));
        questStats = JsonUtility.FromJson<Quests>(PlayerPrefs.GetString("QuestStats", JsonUtility.ToJson(new Quests())));
        gameStats = JsonUtility.FromJson<Game>(PlayerPrefs.GetString("GameStats", JsonUtility.ToJson(new Game())));
        playerStats = JsonUtility.FromJson<Player>(PlayerPrefs.GetString("PlayerStats", JsonUtility.ToJson(new Player())));
    }



    // Start is called before the first frame update
    void Start()
    {
        LoadPlayerStats();
        if (playerStats.Level == 0) 
        {
            playerStats.Level = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameStats.TimeInGame += Time.deltaTime;
        SavePlayerStats();
    }

    void OnApplicationQuit()
    {
        SavePlayerStats();
    }

    public void IncrementAmountKilled()
    {
        goblinStats.AmountKilled++;
    }
    public void IncrementDamageDealt(int Damage)
    {
        goblinStats.DamageDealt += Damage;
    }
    public void IncrementDamageTaken(float Damage)
    {
        goblinStats.DamageTaken += (Damage / 10);
    }
    public void IncrementWoodCollected()
    {
        resourceStats.WoodCollected++;
    }
    public void IncrementGoldCollected()
    {
        resourceStats.GoldCollected++;
    }
    public void IncrementMeatCollected()
    {
        resourceStats.MeatCollected++;
    }
  
    protected void IncrementLevel()
    {
       playerStats.Level++;
    }
    public void IncrementXP(float XP)
    {
        playerStats.XP += XP;
        playerStats.TotalXP += XP;
        while (playerStats.XP > (playerStats.Level * 100)) 
        {
            IncrementLevel();
            playerStats.XP = playerStats.XP - (playerStats.Level * 100);
        }
    }
    public void IncrementTimesDied()
    {
        print("TiemesDied");
        playerStats.TimesDied++;
    }
    public void IncrementQuestsCompleted()
    {
        questStats.AmountCompleted++;
    }
    public void IncrementTimeSpent(float Time)
    {
        questStats.TimeSpent += Time;
    }
    public void IncrementCavesExplored()
    {
        gameStats.CavesExplored++;
    }




}
