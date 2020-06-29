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

    void Start()
    {
        DontDestroyOnLoad(this);
        _canvas = GetComponent<Canvas>();
        foreach (var item in docImagesByPosition)
            item.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void HideDocUI()
    {
        _canvas.enabled = false;
        //volver al tamaño original

    }

    public void ShowDocUI(DocUIMood mood, string txt, DocUIPosition docPosition, float docScale)
    {
        foreach (var item in docImagesByPosition)
            item.gameObject.SetActive(false);

        _textToShow = txt;
        SetDoc(docPosition, txt, mood);
        _canvas.enabled = true;
    }

    void SetDoc(DocUIPosition pos, string txt, DocUIMood mood)
    {
        _selected = docImagesByPosition.FirstOrDefault(i => i.docPosition == pos);

        _selected.gameObject.SetActive(true);
        _selected.text.text = txt;
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
