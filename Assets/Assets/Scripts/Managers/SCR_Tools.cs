using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCR_Tools : MonoBehaviour
{
    [SerializeField] public Queue<IEnumerator> codeQueue = new Queue<IEnumerator>();

    private Camera mainCamera; // Cache for camera reference

    private void Awake()
    {
        mainCamera = Camera.main; // Cache the main camera at start
    }

    public IEnumerator Open(GameObject toOpen)
    {
        Vector3 targetScale = Vector3.one;
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            toOpen.transform.localScale = Vector3.Lerp(toOpen.transform.localScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toOpen.transform.localScale = targetScale;
    }

    public IEnumerator Close(GameObject toClose)
    {
        Vector3 targetScale = new Vector3(0,1,1); // Target scale to shrink to new Vector3(0, 1, 1)
        float duration = 0.5f; // Duration of the animation, matching the Open function for symmetry
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // Smoothly interpolate from the current scale to the target scale over time
            toClose.transform.localScale = Vector3.Lerp(toClose.transform.localScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toClose.transform.localScale = targetScale; // Ensure the target scale is exactly set at the end
    }

    public IEnumerator Close(GameObject toClose, bool Instant) //Overload for instant close
    {
        Vector3 targetScale = new Vector3(0, 1, 1);
        while (toClose.transform.localScale != targetScale)
        {
            toClose.transform.localScale = targetScale;
            yield return null;
        }
    }

    public IEnumerator FadeIn(Image toFade)
    {
        float duration = 1f;
        float elapsedTime = 0;
        Color startColor = toFade.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1);

        while (elapsedTime < duration)
        {
            toFade.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toFade.color = endColor;
    }

    public IEnumerator FadeOut(Image toFade)
    {
        toFade.gameObject.SetActive(true);
        float duration = 1f;
        float elapsedTime = 0;
        Color startColor = toFade.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        while (elapsedTime < duration)
        {
            toFade.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toFade.gameObject.SetActive(false);
        toFade.color = endColor;
    }

    public void AddToQueue(IEnumerator code)
    {
        if (!codeQueue.Contains(code)) 
        {
            codeQueue.Enqueue(code);
        }
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
        if (Camera.main != null)
        {
            Camera.main.GetComponent<CameraScript>().PlayerLocked = true;
            Camera.main.orthographicSize = 5;
        }
    }

    public void SetCamera(Vector3 position)
    {
        if (Camera.main != null)
        {
            Camera.main.orthographicSize = 2;
            Camera.main.GetComponent<CameraScript>().PlayerLocked = false;
            Camera.main.transform.position = position;
        }
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



