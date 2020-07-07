using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelPopup : AcceptPopup
{
    public Image[] starsImg; 
    public GameObject LoseButtonContainer;
    public Button retryButton;
    public Sprite starWin;

    bool _winPopup;
    string _levelID;
    string _minionsPassed;
    string _coinsWon;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        
    }

    public override void InitPopup(string parameters)
    {
        var split = parameters.Split(',');
        bool isWinPopup = bool.Parse(split[0]);
        _levelID = split[2];
        int stars = 0;

        if (!isWinPopup)
        {
            foreach (var item in starsImg)
                item.enabled = false;

            LoseButtonContainer.SetActive(true);
            okButton.gameObject.SetActive(false);
        }
        else
        {
            stars = int.Parse(split[1]);
            _minionsPassed = split[3];
            _coinsWon = split[4];

            LoseButtonContainer.SetActive(false);
            okButton.gameObject.SetActive(true);

            for (int i = 0; i < stars; i++)
                starsImg[i].sprite = starWin;

            _winPopup = true;
        }

        SetTexts();
    }

    public void OnRetry()
    {
        SceneManager.LoadScene("Level" + _levelID);
    }

    void SetTexts()
    {
        title.text = "Level " + _levelID;
        if (_winPopup)
        {
            title.text += " Completed!";
            description.text = "You've just crushed this level!\n Minions passed: " + _minionsPassed;
            description.text += "\n Chips gained: " + _coinsWon;
        }
        else
        {
            description.text = "The Virus cought you!\n";
            description.text = "\nTry to upgrade your minions or replay this level.";
        }
    }
}
