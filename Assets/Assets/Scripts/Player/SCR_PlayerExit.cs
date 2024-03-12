using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_PlayerExit : MonoBehaviour
{

    public void ClickToExit() 
    {
        SceneManager.LoadSceneAsync("GameManager", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);


    }

}
