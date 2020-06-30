using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DocUICanvas : MonoBehaviour
{
    public enum DocUIMood {normal, happy, dead}
    public enum DocUIPosition {bottomLeft, bottomMiddle, bottomRight, middleLeft, middleMiddle, middleRight, topLeft, topMiddle, topRight}

    public DocImageText[] docImagesByPosition;
    public Sprite doc_happy_sprite;
    public Sprite doc_normal_sprite;
    public Sprite doc_dead_sprite;

    DocImageText _selected;
    Canvas _canvas;
    string _textToShow;
    string _currentShowinText;
    int _charIndex;
    float _txtTime = .05f;
    float _txtTimeAux;
    bool _showTextAnimation;
    

    void Start()
    {
        DontDestroyOnLoad(this);
        _canvas = GetComponent<Canvas>();
        foreach (var item in docImagesByPosition)
            item.gameObject.SetActive(false);

        _txtTimeAux = _txtTime;
    }

    void Update()
    {
        if(_selected != null && _showTextAnimation)
        {
            _txtTimeAux -= Time.deltaTime;
            if(_txtTimeAux < 0)
            {
                _txtTimeAux = _txtTime;

                _charIndex++;
                _currentShowinText = _textToShow.Substring(0, _charIndex);
                _selected.text.text = _currentShowinText;
                if (_charIndex == _textToShow.Length)
                    _showTextAnimation = false;
            }
        }
    }

    public void HideDocUI()
    {
        _canvas.enabled = false;
        _selected.transform.localScale = new Vector3(1,1,1);
        _selected = null;
        _showTextAnimation = false;
    }

    public void ShowDocUI(DocUIMood mood, string txt, DocUIPosition docPosition, float docScale)
    {
        foreach (var item in docImagesByPosition)
            item.gameObject.SetActive(false);

        _charIndex = 1;
        //_currentShowinText = txt.Substring(0, _charIndex);
        //_showTextAnimation = true;
        _textToShow = txt;
        SetDoc(docPosition, txt, mood, docScale);
        
        _canvas.enabled = true;
    }

    void SetDoc(DocUIPosition pos, string txt, DocUIMood mood, float scale)
    {
        _selected = docImagesByPosition.FirstOrDefault(i => i.docPosition == pos);

        _selected.gameObject.SetActive(true);
        //_selected.text.text = _currentShowinText;
        _selected.text.text = txt;
        _selected.transform.localScale *= scale;
        SetMoodSprite(_selected.image, mood);
    }

    void SetMoodSprite(Image img, DocUIMood mood)
    {
        switch (mood)
        {
            case DocUIMood.normal:
                img.sprite = doc_normal_sprite;
                break;
            case DocUIMood.happy:
                img.sprite = doc_happy_sprite;
                break;
            case DocUIMood.dead:
                img.sprite = doc_dead_sprite;
                break;
            default:
                break;
        }
    }

















}
