using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDFPS : MonoBehaviour
{

    // Attach this to a GUIText to make a frames/second indicator.
    //
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // correct overall FPS even if the interval renders something like
    // 5.5 frames.

    public float updateInterval = 0.5F;
    public Text txt;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    GameManager _gm;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        DontDestroyOnLoad(this);

        timeleft = updateInterval;
        if(!_gm.showDevTools)
        {
            txt.enabled = false;
        }
    }

    public void StopFPS()
    {

    }

    void Update()
    {
        if (!txt.enabled && _gm.showDevTools)
            txt.enabled = true;
        else if(txt.enabled && !_gm.showDevTools)
            txt.enabled = false;


        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = Mathf.RoundToInt( accum / frames);

            if (fps < 30)
                txt.color = Color.yellow;
            else
                if (fps < 10)
                txt.color = Color.red;
            else
                txt.color = Color.green;
            
            //Debug.Log(fps);
            txt.text = "fps " + fps;
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}