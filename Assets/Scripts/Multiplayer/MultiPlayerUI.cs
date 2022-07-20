using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalEnum;
using Photon.Pun;
public class MultiPlayerUI : MonoBehaviourPunCallbacks
{
    public static MultiPlayerUI Instance;

    [SerializeField] private Text startTimeText;
    [SerializeField] private Text player1ScoreText;
    [SerializeField] private Text player2ScoreText;
    [SerializeField] private Text popupText;
    [SerializeField] private Text penaltyTimer;

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
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    [PunRPC]
    public void Init()
    {
        resultPanel.SetActive(false);
        timer.maxValue = MultiPlayerData.time;
        timer.value = MultiPlayerData.time;
        pausePanel.SetActive(false);
        player1ScoreText.text = string.Format("{0} : {1}", PhotonNetwork.CurrentRoom.GetPlayer(1).NickName, MultiPlayerData.Player1Score);
        player2ScoreText.text = string.Format("{0} : {1}", PhotonNetwork.CurrentRoom.GetPlayer(2).NickName, MultiPlayerData.Player2Score);
    }

    [PunRPC]
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

    [PunRPC]
    public void ShowPausePanel(bool active)
    {
        pausePanel.SetActive(active);
    }

    public void ShowResultImage(string _result)
    {
        switch (_result)
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

    public void ShowPopUpText(ConditionState state, PlayerState playerState)
    {
        popupText.gameObject.SetActive(true);

        switch (playerState)
        {
            case PlayerState.PLAYER1:
                switch (state)
                {
                    case ConditionState.CORRECT:
                        popupText.text = "Great!";
                        break;
                    case ConditionState.WRONG:
                        popupText.text = "Oh No!";
                        break;
                    case ConditionState.TIE:
                        popupText.text = "Tie!";
                        break;
                }
                break;
            case PlayerState.PLAYER2:
                switch (state)
                {
                    case ConditionState.CORRECT:
                        popupText.text = "Great!";
                        break;
                    case ConditionState.WRONG:
                        popupText.text = "Oh No!";
                        break;
                    case ConditionState.TIE:
                        popupText.text = "Tie!";
                        break;
                }
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

    [PunRPC]
    public void ShowScore(int player1Score, int player2Score)
    {
        if (player1Score <= 0)
        {
            player1Score = 0;
        } else
        {
            player1ScoreText.text = string.Format("{0} : {1}", PhotonNetwork.CurrentRoom.GetPlayer(1).NickName, player1Score);
        }

        if (player2Score <= 0)
        {
            player2Score = 0;
        } else
        {
            player2ScoreText.text = string.Format("{0} : {1}", PhotonNetwork.CurrentRoom.GetPlayer(2).NickName, player2Score);
        }
    }

    [PunRPC]
    public void ShowTimer()
    {
        timer.value = MultiPlayerData.time;

        if (MultiPlayerData.time <= 0 || (MultiPlayerData.time >= 0 && MultiPlayerData.time < 1))
        {
            timer.value = 0;
        }
    }

    [PunRPC]
    public void ShowPenaltyTimer(float timer, bool flag)
    {
        if (flag)
        {
            penaltyTimer.gameObject.SetActive(flag);
            if (timer <= 0 || (timer > 0 && timer < 1))
            {
                timer = 0;
            }

            penaltyTimer.text = Mathf.Round(timer).ToString();
        } else
        {
            penaltyTimer.gameObject.SetActive(flag);
        }
        
    }


    [PunRPC]
    public void ShowResultPanel()
    {
        resultPanel.SetActive(true);
        scoreResult.text = SinglePlayerData.score.ToString();
    }
}
