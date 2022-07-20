using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalEnum;
public class MainUI : MonoBehaviour
{
    public static MainUI Instance;

    [SerializeField] private Text startTimeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text popupText;

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text scoreResult;

    [SerializeField] private GameObject pausePanel;

    [SerializeField] private Slider timer;

    [SerializeField] private List<Sprite> resultImages = new List<Sprite>();

    [SerializeField] private Image result;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            DontDestroyOnLoad(this);
        }
    }

    public void Init()
    {
        resultPanel.SetActive(false);
        timer.maxValue = SinglePlayerData.time;
        timer.value = SinglePlayerData.time;
        pausePanel.SetActive(false);
        scoreText.text = string.Format("Score : {0}", SinglePlayerData.score);
    }

    public void ShowStartTime(float timer)
    {
        startTimeText.gameObject.SetActive(true);
        startTimeText.text = Mathf.Round(timer).ToString();
        if (timer <= 0 || (timer >= 0 && timer < 1))
        {
            startTimeText.text = "0";
            startTimeText.gameObject.SetActive(false);
        }
    }

    public void ShowPausePanel(bool active)
    {
        pausePanel.SetActive(active);
    }

    public void ShowResult(string _result)
    {
        switch(_result)
        {
            case "Rock":
                result.sprite = resultImages[0];
                break;
            case "Paper":
                result.sprite = resultImages[1];
                break;
            case "Scissor":
                result.sprite = resultImages[2];
                break;
        }

        LeanTween.delayedCall(.5f, () =>
        {
            result.sprite = resultImages[3];
        });
    }

    public void ShowPopUpText(ConditionState state)
    {
        popupText.gameObject.SetActive(true);
        switch (state)
        {
            case ConditionState.CORRECT:
                popupText.text = "Great!";
                break;
            case ConditionState.TIE:
                popupText.text = "Tie!";
                break;
            case ConditionState.WRONG:
                popupText.text = "Oh No!";
                break;
        }

        LeanTween.moveLocalY(popupText.gameObject, popupText.gameObject.transform.position.y + 15f, 0.2f).setOnComplete(() =>
        {
            LeanTween.moveLocalY(popupText.gameObject, popupText.gameObject.transform.position.y, 0.2f).setOnComplete(() =>
            {
                popupText.gameObject.SetActive(false);
            });
        });
    }
    
    public void ShowScore()
    {
        if (SinglePlayerData.score <= 0)
        {
            scoreText.text = "Score : 0";
        } else
        {
            scoreText.text = string.Format("Score : {0}", SinglePlayerData.score);
        }
    }

    public void ShowTimer()
    {
        timer.value = SinglePlayerData.time;

        if (SinglePlayerData.time <= 0 || (SinglePlayerData.time >= 0 && SinglePlayerData.time < 1))
        {
            timer.value = 0;
        }
    }

    public void ShowResultPanel()
    {
        resultPanel.SetActive(true);
        scoreResult.text = SinglePlayerData.score.ToString();
    }

}
