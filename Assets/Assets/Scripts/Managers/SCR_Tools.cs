using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCR_Tools : MonoBehaviour
{
    [SerializeField] public Queue<IEnumerator> codeQueue = new Queue<IEnumerator>();


    public IEnumerator Open(GameObject toOpen)
    {
        while (toOpen.transform.localScale != new Vector3(1, 1, 1))
        {
            if (toOpen.transform.localScale.x == 1)
            {
                toOpen.transform.localScale += new Vector3(0f, 0.1f, 0f);
            }
            if (toOpen.transform.localScale.y == 1)
            {
                toOpen.transform.localScale += new Vector3(0.1f, 0f, 0f);
            }

            yield return new WaitForSeconds(0.001f);
        }
    }
    public IEnumerator Close(GameObject ToClose)
    {
        while (ToClose.transform.localScale.x > 0)
        {
            ToClose.transform.localScale += new Vector3(-Mathf.Min(0.1f, ToClose.transform.localScale.x), 0f, 0f);
            yield return new WaitForSeconds(0.01f);

        }
    }
    public IEnumerator FadeIn(Image ToFade)
    {
        ToFade.gameObject.SetActive(true);
        while (ToFade.color != new Color(0, 0, 0, 1))
        {
            ToFade.color = ToFade.color + new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        ToFade.gameObject.SetActive(true);
    }
    public IEnumerator FadeOut(Image ToFade)
    {
        ToFade.gameObject.SetActive(true);
        while (ToFade.color.a > 0)
        {
            ToFade.color -= new Color(0, 0, 0, Mathf.Min(0.01f, ToFade.color.a));
            yield return new WaitForSeconds(0.01f);
        }
        ToFade.gameObject.SetActive(false);
        ToFade.color = new Color(0, 0, 0, 0);
    }

    public void AddToQueue(IEnumerator code)
    {
        codeQueue.Enqueue(code);
    }
    public IEnumerator ProcessCodeQueue()
    {
        while (codeQueue.Count > 0)
        {
            yield return StartCoroutine(codeQueue.Dequeue());
        }

        Debug.Log("Code queue is empty");
    }

    public void ResetCamera()
    {
        Camera.main.GetComponent<CameraScript>().PlayerLocked = true;
        Camera.main.orthographicSize = 5;
    }
    public void SetCamera(Vector3 Pos) 
    {
        Camera.main.orthographicSize = 2;
        Camera.main.GetComponent<CameraScript>().PlayerLocked = false;
        Camera.main.transform.position = Pos;

    }

    public void ReturnToMain() 
    {
        if (!SceneManager.GetSceneByName("MainMenu").IsValid()) 
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("SampleScene");
            SceneManager.UnloadSceneAsync("Player");

        }
        
    }


}


