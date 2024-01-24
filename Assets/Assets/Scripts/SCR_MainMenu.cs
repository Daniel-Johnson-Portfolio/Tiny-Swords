using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame() 
    {
        SceneManager.LoadSceneAsync("GameManager", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("Player", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
