using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LibraryTypeInfoCanvas : MonoBehaviour
{
    const int ELEMENTS_DISTANCE = 50;

    public RectTransform content;
    public ScrollRect scrollRect;
    public VideoPlayer videoPlayer_1;
    public VideoPlayer videoPlayer_2;
    public Text partOneText;
    public Text partTwoText;
    public RawImage imageToRenderVideo_1;
    public RawImage imageToRenderVideo_2;
    public bool isShowingInfo;//is showing texts, videos, etc? 

    Canvas _canvas;
    LibraryCategoryTypeInfoDef _info;
    GameManager _gameManager;
    

    private void Awake()
    {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();

        _gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        
    }


    void Update()
    {

    }

    public void SetCanvas(bool enable)
    {
        if(_canvas == null)
            _canvas = GetComponent<Canvas>();

        if (enable)
            scrollRect.verticalNormalizedPosition = 1;

        _canvas.enabled = enable;
        if(!enable)
        {
            if(videoPlayer_1.isPlaying)
                videoPlayer_1.Stop();

            if (videoPlayer_2.isPlaying)
                videoPlayer_2.Stop();

            isShowingInfo = false;
        }
    }
    

    public void SetInfo(LibraryCategoryTypeInfoDef info)
    {
        _info = info;
        partOneText.text = "";
        partTwoText.text = "";
        var i = 0;

        if (_info.partOneTexts != null)
        {
            foreach (var item in _info.partOneTexts)
            {
                if(i == _info.partOneTexts.Length-1)
                    partOneText.text += item;
                else
                    partOneText.text += item + "\n";
                i++;
            }    
        }

        videoPlayer_1.clip = _gameManager.LoadedAssets.GetVideoByName(_info.type);
        if (videoPlayer_1.clip != null)
        {
            videoPlayer_1.Play();
            imageToRenderVideo_1.enabled = true;
        }
        else
            imageToRenderVideo_1.enabled = false;
            

        if (_info.partTwoTexts != null)
        {
            i = 0;
            foreach (var item in _info.partTwoTexts)
            {
                if (i == _info.partTwoTexts.Length - 1)
                    partTwoText.text += item;
                else
                    partTwoText.text += item + "\n";
                i++;
            }

            partTwoText.enabled = true;
        }
        else
            partTwoText.enabled = false;

        videoPlayer_2.clip = _gameManager.LoadedAssets.GetVideoByName(_info.type);
        if (videoPlayer_2.clip != null)
        {
            videoPlayer_2.Play();
            imageToRenderVideo_2.enabled = true;
        }
        else
            imageToRenderVideo_2.enabled = false;

        StartCoroutine(WaitToRefreshPosition());
    }

    IEnumerator WaitToRefreshPosition()
    {
        yield return new WaitForEndOfFrame();

        SetElementsPositions();
        isShowingInfo = true;
        SetCanvas(true);
    }

    void SetElementsPositions()
    {
        float posX = 0f;
        float posY = -partOneText.rectTransform.sizeDelta.y; //get the height, negative because is going down Y axis

        if (imageToRenderVideo_1.isActiveAndEnabled)
        {
            posX = imageToRenderVideo_1.rectTransform.localPosition.x;
            posY -= ELEMENTS_DISTANCE;
            imageToRenderVideo_1.rectTransform.localPosition = new Vector3(posX, posY, 0);
            
            posY -= imageToRenderVideo_1.rectTransform.sizeDelta.y;
        }

        if(partTwoText.isActiveAndEnabled)
        {
            posX = partTwoText.rectTransform.localPosition.x;
            posY -= ELEMENTS_DISTANCE;
            partTwoText.rectTransform.localPosition = new Vector3(posX, posY, 0);
            
            posY -= partTwoText.rectTransform.sizeDelta.y + ELEMENTS_DISTANCE;
        }

        if(imageToRenderVideo_2.isActiveAndEnabled)
        {
            posX = imageToRenderVideo_2.rectTransform.localPosition.x;
            posY -= ELEMENTS_DISTANCE;
            imageToRenderVideo_2.rectTransform.localPosition = new Vector3(posX, posY, 0);

            posY -= imageToRenderVideo_2.rectTransform.sizeDelta.y;
        }
        
        content.sizeDelta = new Vector2(content.sizeDelta.x, -posY);
    }
}





