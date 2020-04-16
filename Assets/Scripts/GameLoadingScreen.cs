using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoadingScreen : MonoBehaviour
{
    public bool isLevelLoading;
    public Slider slider;

    void Start()
    {
        if(!isLevelLoading)
            StartCoroutine(Loading());
    }

    public void ActivateLoadingAsyncProcess(string name = "MenuScreen")
    {
        transform.parent.gameObject.SetActive(true);
        StartCoroutine(Loading(name));
    }

    IEnumerator Loading(string name = "MenuScreen")
    {
        isLevelLoading = true;
        AsyncOperation op = SceneManager.LoadSceneAsync(name);

        while (!op.isDone)
        {
            var progress = Mathf.Clamp01(op.progress / .9f);

            slider.value = progress;

            yield return null;
        }

        isLevelLoading = false;
    }
}
