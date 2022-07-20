using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalEnum;
public class SinglePlayerManager : MonoBehaviour
{
    private GameState gameState;
    private bool isActive;

    [SerializeField] private float startTime;
    [SerializeField] private float time;
    [SerializeField] private int score;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        gameState = GameState.START;
        SinglePlayerData.time = time;
        SinglePlayerData.score = score;
        MainUI.Instance.Init();
    }

    private void Update()
    {
        switch(gameState)
        {
            case GameState.START: StartGame(); break;
            case GameState.PAUSE: break;
            case GameState.RPS: TimeDeplete(); break;
            case GameState.END: EndGame(); break;
        }
    }

    private void StartGame()
    {
        startTime -= Time.deltaTime;
        if (startTime <= 0)
        {
            startTime = 0;
            isActive = true;
            gameState = GameState.RPS;
        }
        MainUI.Instance.ShowStartTime(startTime);
    }

    public void Pause()
    {
        if (gameState != GameState.RPS) return;
        MainUI.Instance.ShowPausePanel(true);
        gameState = GameState.PAUSE;
    }

    public void Resume()
    {
        if (gameState != GameState.PAUSE) return;
        MainUI.Instance.ShowPausePanel(false);
        gameState = GameState.RPS;
    }

    public void Restart()
    {
        Init();
        gameState = GameState.START;
    }

    public void Exit()
    {
        SceneManager.LoadScene("Game");
    }

    public void Play(string choice)
    {
        if (!isActive) return;

        if (gameState != GameState.RPS) return;

        int random = Random.Range(0, 3); // 0 = rock, 1 = paper, 2 = scissor
        switch(random)
        {
            case 0:
                switch(choice)
                {
                    case "Paper":
                        CheckCondition(ConditionState.CORRECT);
                        break;
                    case "Rock":
                        CheckCondition(ConditionState.TIE);
                        break;
                    default:
                        CheckCondition(ConditionState.WRONG);
                        break;
                }
                MainUI.Instance.ShowResult("Rock");
                break;
            case 1:
                switch (choice)
                {
                    case "Scissor":
                        CheckCondition(ConditionState.CORRECT);
                        break;
                    case "Paper":
                        CheckCondition(ConditionState.TIE);
                        break;
                    default:
                        CheckCondition(ConditionState.WRONG);
                        break;
                }
                MainUI.Instance.ShowResult("Paper");
                break;
            case 2:
                switch (choice)
                {
                    case "Rock":
                        CheckCondition(ConditionState.CORRECT);
                        break;
                    case "Scissor":
                        CheckCondition(ConditionState.TIE);
                        break;
                    default:
                        CheckCondition(ConditionState.WRONG);
                        break;
                }
                MainUI.Instance.ShowResult("Scissor");
                break;
        }
    }

    private void CheckCondition(ConditionState state)
    {
        isActive = false;
        MainUI.Instance.ShowPopUpText(state);
        AddScore(state);
        LeanTween.delayedCall(.5f, () =>
        {
            isActive = true;
        });
    }

    private void AddScore(ConditionState state)
    {
        switch (state)
        {
            case ConditionState.CORRECT:
                SinglePlayerData.score += 2;
                break;
            case ConditionState.WRONG:
                SinglePlayerData.score -= 1;
                break;
        }

        if (SinglePlayerData.score <= 0)
        {
            SinglePlayerData.score = 0;
        }

        MainUI.Instance.ShowScore();
    }

    private void TimeDeplete()
    {
        SinglePlayerData.time -= Time.unscaledDeltaTime;

        if (SinglePlayerData.time <= 0)
        {
            SinglePlayerData.time = 0;
            isActive = false;
            gameState = GameState.END;
        }

        MainUI.Instance.ShowTimer();
    }

    private void EndGame()
    {
        MainUI.Instance.ShowResultPanel();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        } 
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }
}
