using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame() 
    {
        SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadScene("OverWorld", LoadSceneMode.Additive);
        SceneManager.LoadScene("Player", LoadSceneMode.Additive);
       
        
        
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
