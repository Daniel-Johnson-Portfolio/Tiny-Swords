using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    public struct Inventory
    {
        public int Gold;
        public int Wood;
        public int Meat; //Unlikly to be used
    }
    [SerializeField] public Inventory PlayerInventory;

    private GameObject player;
    private GameObject inventory;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.transform.GetChild(0).GetChild(2).gameObject;
        LoadPlayerStats();
    }

    void SavePlayerStats()
    {
        PlayerPrefs.SetString("PlayerInventory", JsonUtility.ToJson(PlayerInventory));
        PlayerPrefs.Save();
    }

    void LoadPlayerStats()
    {
        PlayerInventory = JsonUtility.FromJson<Inventory>(PlayerPrefs.GetString("PlayerInventory"));
    }

    public void IncrementGold()
    {
        PlayerInventory.Gold++;
    }
    public void IncrementWood() 
    {
        PlayerInventory.Wood++;
    }
    void Update()
    {
        inventory.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = PlayerInventory.Wood.ToString();
        inventory.transform.GetChild(2).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = PlayerInventory.Gold.ToString();
    }

    void OnApplicationQuit()
    {
       SavePlayerStats();
    }
}
