using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoadingScreen : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(1);

        while (!op.isDone)
        {
            var progress = Mathf.Clamp01(op.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }
}
