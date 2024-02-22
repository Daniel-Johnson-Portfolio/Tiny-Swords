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
        public int Meat;
    }
    [SerializeField] public Inventory PlayerInventory;

    private GameObject player;
    private GameObject inventory;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null) 
        {
            player = GameObject.FindGameObjectWithTag("Player");
            inventory = player.transform.GetChild(0).Find("Inventory").gameObject;
            LoadPlayerStats();
        } 
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
    public void IncrementMeat()
    {
        PlayerInventory.Meat++;
    }
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            inventory.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = PlayerInventory.Wood.ToString();
            inventory.transform.GetChild(2).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = PlayerInventory.Gold.ToString();
            inventory.transform.GetChild(3).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = PlayerInventory.Meat.ToString();
        }
    }

    void OnApplicationQuit()
    {
       SavePlayerStats();
    }

    

}
