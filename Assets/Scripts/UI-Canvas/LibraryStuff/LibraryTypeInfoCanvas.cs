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
    public Button button_ground;
    public Button button_air;
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

        if (_info.partOneTexts != null)
            SetText(partOneText, _info.partOneTexts);

        if (_info.partTwoTexts != null)
        {
            SetText(partTwoText, _info.partTwoTexts);
            partTwoText.enabled = true;
        }
        else
            partTwoText.enabled = false;

        if(!_info.hasTwoButtons)
        {
            button_air.enabled = false;
            button_ground.enabled = false;
        }

        SetVideos(imageToRenderVideo_1, videoPlayer_1, _info.type);
        SetVideos(imageToRenderVideo_2, videoPlayer_2, _info.type);

        StartCoroutine(WaitToRefreshPosition());
    }

    IEnumerator WaitToRefreshPosition()
    {
        yield return new WaitForEndOfFrame();

        SetElementsPositions();
        isShowingInfo = true;
        SetCanvas(true);
    }

    void SetVideos(RawImage renderTarget, VideoPlayer player, string type)
    {
        player.clip = _gameManager.LoadedAssets.GetVideoByName(type);
        if (player.clip != null)
        {
            player.Play();
            renderTarget.enabled = true;
        }
        else
            renderTarget.enabled = false;
    }

    void SetText(Text toWrite, string[] info)
    {
        var i = 0;
        foreach (var item in info)
        {
            if (i == info.Length - 1)
                toWrite.text += item;
            else
                toWrite.text += item + "\n";
            i++;
        }
    }

    void SetElementsPositions()
    {
        float posX = 0f;
        float posY = -partOneText.rectTransform.sizeDelta.y; //get the height, negative because is going down Y axis

        if (_info.hasTwoButtons)
        {
            posY -= ELEMENTS_DISTANCE;

            var halfScreen = _canvas.pixelRect.size.x / 2;
            var quarterScreen = halfScreen / 2;

            posX = halfScreen - quarterScreen;
            button_ground.transform.localPosition = new Vector3(posX, posY, 0);

            posX = halfScreen + quarterScreen;
            button_air.transform.localPosition = new Vector3(posX, posY, 0);
        }

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





