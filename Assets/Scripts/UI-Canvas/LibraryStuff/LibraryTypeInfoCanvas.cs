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
    public Image staticImage;
    public bool isShowingInfo;//is showing texts, videos, etc? 

    Canvas _canvas;
    LibraryCategoryTypeInfoDef _info;
    GameManager _gameManager;
    RectTransform _button_ground_rect;
    RectTransform _button_air_rect;

    private void Awake()
    {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();

        _gameManager = FindObjectOfType<GameManager>();

        _button_air_rect = button_air.GetComponent<RectTransform>();
        _button_ground_rect = button_ground.GetComponent<RectTransform>();
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
            if(videoPlayer_1 != null && videoPlayer_1.isPlaying)
                videoPlayer_1.Stop();

            if (videoPlayer_2 != null && videoPlayer_2.isPlaying)
                videoPlayer_2.Stop();

            isShowingInfo = false;
        }
    }

    public void OnButtonPressed(bool isGroundButton)
    {
        SetVideo(imageToRenderVideo_1, videoPlayer_1, "runner");
        StartCoroutine(WaitToRefreshPosition());
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
            button_air.gameObject.SetActive(false);
            button_ground.gameObject.SetActive(false);
        }
        else
        {
            button_air.gameObject.SetActive(true);
            button_ground.gameObject.SetActive(true);
        }

        if (!_info.hasStaticImage)
            staticImage.gameObject.SetActive(false);
        else
            staticImage.gameObject.SetActive(true);



        SetVideo(imageToRenderVideo_1, videoPlayer_1, _info.type);
        SetVideo(imageToRenderVideo_2, videoPlayer_2, _info.type);

        SetStaticImage(_info.type);

        StartCoroutine(WaitToRefreshPosition());
    }

    IEnumerator WaitToRefreshPosition()
    {
        yield return new WaitForEndOfFrame();

        SetElementsPositions();
        isShowingInfo = true;
        SetCanvas(true);
    }

    void SetVideo(RawImage renderTarget, VideoPlayer player, string type)
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

    void SetStaticImage(string type)
    {
        var s = _gameManager.LoadedAssets.GetSpriteByName(type);
        if (s != null)
            staticImage.sprite = s;
        else
            staticImage.gameObject.SetActive(false);
    }

    void SetElementsPositions()
    {
        float posX = 0f;
        float posY = -partOneText.rectTransform.sizeDelta.y; //get the height, negative because is going down Y axis

        if (_info.hasTwoButtons)
        {
            posY -= ELEMENTS_DISTANCE;

            var halfScreen = content.sizeDelta.x / 2;
            var quarterScreen = halfScreen / 2;

            posX = halfScreen - halfScreen/2;
            _button_ground_rect.localPosition = new Vector3(posX, posY, 0);

            posX = halfScreen + halfScreen/2;
            _button_air_rect.localPosition = new Vector3(posX, posY, 0);

            posY -= _button_ground_rect.sizeDelta.y;
        }

        if (imageToRenderVideo_1.isActiveAndEnabled)
        {
            posY -= ELEMENTS_DISTANCE;
            posX = imageToRenderVideo_1.rectTransform.localPosition.x;
            
            imageToRenderVideo_1.rectTransform.localPosition = new Vector3(posX, posY, 0);
            
            posY -= imageToRenderVideo_1.rectTransform.sizeDelta.y;
        }

        if(_info.hasStaticImage)
        {
            posY -= ELEMENTS_DISTANCE;
            posX = staticImage.rectTransform.localPosition.x;

            staticImage.rectTransform.localPosition = new Vector3(posX, posY, 0);
            posY -= staticImage.rectTransform.sizeDelta.y;
        }

        if(partTwoText.isActiveAndEnabled)
        {
            posY -= ELEMENTS_DISTANCE;
            posX = partTwoText.rectTransform.localPosition.x;
            
            partTwoText.rectTransform.localPosition = new Vector3(posX, posY, 0);
            
            posY -= partTwoText.rectTransform.sizeDelta.y + ELEMENTS_DISTANCE;
        }

        if(imageToRenderVideo_2.isActiveAndEnabled)
        {
            posY -= ELEMENTS_DISTANCE;
            posX = imageToRenderVideo_2.rectTransform.localPosition.x;
            
            imageToRenderVideo_2.rectTransform.localPosition = new Vector3(posX, posY, 0);

            posY -= imageToRenderVideo_2.rectTransform.sizeDelta.y;
        }

        
        
        content.sizeDelta = new Vector2(content.sizeDelta.x, -posY);
    }
}





