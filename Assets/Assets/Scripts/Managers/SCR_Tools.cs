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
        Vector3 targetScale = new Vector3(1f, 1f, 1f);
        float duration = 0.5f; // Duration of the animation
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            toOpen.transform.localScale = Vector3.Lerp(toOpen.transform.localScale, targetScale, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toOpen.transform.localScale = targetScale; // Ensure target scale is set
    }


    public IEnumerator Close(GameObject ToClose)
    {
        while (ToClose.transform.localScale.x > 0)
        {
            ToClose.transform.localScale += new Vector3(-Mathf.Min(0.1f, ToClose.transform.localScale.x), 0f, 0f);
            yield return new WaitForSeconds(0.01f);

        }
    }
    public IEnumerator FadeIn(Image toFade)
    {
        Color startColor = toFade.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        float duration = 1f; // Duration of the fade
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            toFade.color = Color.Lerp(startColor, endColor, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toFade.color = endColor; // Ensure final color is set
        toFade.gameObject.SetActive(false);
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


