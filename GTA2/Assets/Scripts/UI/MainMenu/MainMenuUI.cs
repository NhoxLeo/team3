﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Canvas")]
    [SerializeField]
    Canvas startCanvas;
    [SerializeField]
    Canvas selectCanvas;

    [Header("UI Component")]
    [SerializeField]
    Image activeStageImage;
    [SerializeField]
    Text activeStageText;
    [SerializeField]
    Sprite[] stageSprites;
    [SerializeField]
    Text highscoreText;
    [SerializeField]
    GameObject exitUI;


    [Header("UI Component")]
    [SerializeField]
    AudioClip okClip;
    //

    int stageIndex = 1;
    int maxStageIndex = 3;
    int minStageIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        CloseExitWindow();
        GotoStart();
        SetHighscore();
    }


    void SetHighscore()
    {
        JsonStreamer js = new JsonStreamer();
        HighScoreData highData = js.Load<HighScoreData>("HighScoreData.json");
        highscoreText.text = "HIGH SCORE: ";

        if (highData != null)
        {
            highscoreText.text += highData.highScore.ToString();
        }
        else
        {
            highscoreText.text += "0";
        }
    }

    public void GotoStart()
    {
        startCanvas.gameObject.SetActive(true);
        selectCanvas.gameObject.SetActive(false);
        SoundManager.Instance.PlayClip(okClip, SoundPlayMode.UISFX);
    }

    public void GotoSelect()
    {
        startCanvas.gameObject.SetActive(false);
        selectCanvas.gameObject.SetActive(true);
        SoundManager.Instance.PlayClip(okClip, SoundPlayMode.UISFX);
    }


    public void NextStageBtn()
    {
        stageIndex++;
        if (stageIndex >= maxStageIndex)
        {
            stageIndex = maxStageIndex;
        }

        SoundManager.Instance.PlayClip(okClip, SoundPlayMode.UISFX);
    }
    public void PrevStageBtn()
    {
        stageIndex--;
        if (stageIndex <= minStageIndex)
        {
            stageIndex = minStageIndex;
        }

        SoundManager.Instance.PlayClip(okClip, SoundPlayMode.UISFX);
    }

    public void StartGameBtn()
    {
		PlayerPrefs.SetInt("TargetSceneIdx", stageIndex);
		SceneManager.LoadScene("LoadingScene");
        //SceneManager.LoadScene("Stage" + stageIndex.ToString());
    }


    void Update()
    {
        UpdateStageInfo();
        UpdateMenu();
        UpdateExit();
    }

    void UpdateStageInfo()
    {
        activeStageImage.sprite = stageSprites[stageIndex - 1];
        activeStageText.text = stageIndex.ToString();
    }

    void UpdateMenu()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (startCanvas.gameObject.activeInHierarchy)
            {
                return;
            }
            else if (selectCanvas.gameObject.activeInHierarchy)
            {
                SoundManager.Instance.PlayClip(okClip, SoundPlayMode.UISFX);
                GotoStart();
            }
        }
    }
    void UpdateExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitUI.activeInHierarchy)
            {
                CloseExitWindow();
            }
            else if (!exitUI.activeInHierarchy)
            {
                SoundManager.Instance.PlayClip(okClip, SoundPlayMode.UISFX);
                exitUI.SetActive(true);
            }
        }
    }


    public void CloseExitWindow()
    {
        SoundManager.Instance.PlayClip(okClip, SoundPlayMode.UISFX);
        exitUI.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
