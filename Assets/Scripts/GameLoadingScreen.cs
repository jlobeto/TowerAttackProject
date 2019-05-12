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

    public IEnumerator Loading(string name = "MenuScreen")
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(name);

        while (!op.isDone)
        {
            var progress = Mathf.Clamp01(op.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }
}
