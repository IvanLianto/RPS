using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using GlobalEnum;
using UnityEngine.UI;
public class MultiPlayerManager : MonoBehaviourPunCallbacks
{
    public static MultiPlayerManager Instance;

    private GameState gameState;
    private bool isActive;

    [SerializeField] private GameObject playerObj;

    [Header("Init")]
    [SerializeField] private float startTime;
    [SerializeField] private float time;
    
    [SerializeField] private int score;

    [Header("Player")]
    private string player1Input;
    private string player2Input;

    private int player1Score;
    private int player2Score;

    [Header("Choise Button")]
    [SerializeField] private ChoiseButton rockButton;
    [SerializeField] private ChoiseButton scissorButton;
    [SerializeField] private ChoiseButton paperButton;

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

    private void Start()
    {
        SetPlayers();
        photonView.RPC("Init", RpcTarget.AllBuffered);
    }

    private void SetPlayers()
    {
        var obj = PhotonNetwork.Instantiate(playerObj.name, Vector3.zero, Quaternion.identity);
        obj.GetComponent<MultiPlayerInput>().Init(rockButton, paperButton, scissorButton);
    }

    [PunRPC]
    private void Init()
    {
        gameState = GameState.START;
        MultiPlayerData.time = time;
        MultiPlayerData.Player1Score = score;
        MultiPlayerData.Player2Score = score;
        player1Score = score;
        player2Score = score;
        player1Input = "";
        player2Input = "";
        MultiPlayerUI.Instance.photonView.RPC("Init", RpcTarget.All);

    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.START: 
                photonView.RPC(nameof(StartGame), RpcTarget.AllBuffered); 
                break;
            case GameState.PAUSE: break;
            case GameState.RPS: 
                photonView.RPC(nameof(TimeDeplete), RpcTarget.AllBuffered);
                photonView.RPC(nameof(ShowScore), RpcTarget.AllBuffered); 
                break;
            case GameState.END: 
                photonView.RPC(nameof(EndGame), RpcTarget.AllBuffered); 
                break;
        }
    }

    #region START
    [PunRPC]
    private void StartGame()
    {
        startTime -= Time.deltaTime;
        if (startTime <= 0)
        {
            startTime = 0;
            isActive = true;
            gameState = GameState.RPS;
        }
        MultiPlayerUI.Instance.ShowStartTime(startTime);
    }
    #endregion

    [PunRPC]
    public void PickChoise(string choise, int playerId)
    {
        if (!isActive) return;

        if (gameState != GameState.RPS) return;

        if (playerId == 1)
        {
            if (player1Input != "") return;
            player1Input = choise;
        }

        if (playerId == 2)
        {
            if (player2Input != "") return;
            player2Input = choise;
        }

        if (player1Input != "" && player2Input != "")
        {
            photonView.RPC(nameof(Play), RpcTarget.AllBuffered, playerId);
        }
    }

    [PunRPC]
    public void Play(int playerId)
    {
        if (!isActive) return;

        if (gameState != GameState.RPS) return;

        if (playerId == 1)
        {
            Player1Condition(playerId);
            MultiPlayerUI.Instance.ShowResultImage(player2Input);
        }

        if (playerId == 2)
        {
            Player2Condition(playerId);
            MultiPlayerUI.Instance.ShowResultImage(player1Input);
        }
    }

    private void Player1Condition(int playerID)
    {
        switch (player1Input)
        {
            case "Rock":
                switch (player2Input)
                {
                    case "Paper":
                        CheckCondition(ConditionState.WRONG, playerID);
                        break;
                    case "Scissor":
                        CheckCondition(ConditionState.CORRECT, playerID);
                        break;
                    case "Rock":
                        CheckCondition(ConditionState.TIE, playerID);
                        break;
                }
                break;
            case "Paper":
                switch (player2Input)
                {
                    case "Rock":
                        CheckCondition(ConditionState.CORRECT, playerID);
                        break;
                    case "Scissor":
                        CheckCondition(ConditionState.WRONG, playerID);
                        break;
                    case "Paper":
                        CheckCondition(ConditionState.TIE, playerID);
                        break;
                }
                break;
            case "Scissor":
                switch (player2Input)
                {
                    case "Rock":
                        CheckCondition(ConditionState.WRONG, playerID);
                        break;
                    case "Paper":
                        CheckCondition(ConditionState.CORRECT, playerID);
                        break;
                    case "Scissor":
                        CheckCondition(ConditionState.TIE, playerID);
                        break;
                }
                break;
        }
        
    }

    private void Player2Condition(int playerID)
    {
        switch (player2Input)
        {
            case "Rock":
                switch (player1Input)
                {
                    case "Paper":
                        CheckCondition(ConditionState.WRONG, playerID);
                        break;
                    case "Scissor":
                        CheckCondition(ConditionState.CORRECT, playerID);
                        break;
                    case "Rock":
                        CheckCondition(ConditionState.TIE, playerID);
                        break;
                }
                break;
            case "Paper":
                switch (player1Input)
                {
                    case "Rock":
                        CheckCondition(ConditionState.CORRECT, playerID);
                        break;
                    case "Scissor":
                        CheckCondition(ConditionState.WRONG, playerID);
                        break;
                    case "Paper":
                        CheckCondition(ConditionState.TIE, playerID);
                        break;
                }
                break;
            case "Scissor":
                switch (player1Input)
                {
                    case "Rock":
                        CheckCondition(ConditionState.WRONG, playerID);
                        break;
                    case "Paper":
                        CheckCondition(ConditionState.CORRECT, playerID);
                        break;
                    case "Scissor":
                        CheckCondition(ConditionState.TIE, playerID);
                        break;
                }
                break;
        }
    }

    private void CheckCondition(ConditionState state, int playerID)
    {
        isActive = false;
        ConditionState _state = state;
        Debug.Log(_state);

        if (playerID == 1)
        {
            _state = state;
            AddScore(_state, playerID);
            MultiPlayerUI.Instance.ShowPopUpText(_state, PlayerState.PLAYER1);
        }

        if (playerID == 2)
        {
            _state = state;
            AddScore(_state, playerID);
            MultiPlayerUI.Instance.ShowPopUpText(_state, PlayerState.PLAYER2);
        }
    }

    private void AddScore(ConditionState state, int playerID)
    {
        if (playerID == 1)
        {
            switch (state)
            {
                case ConditionState.CORRECT:
                    player1Score += 2;
                    break;
                case ConditionState.WRONG:
                    player1Score -= 1;
                    break;
                case ConditionState.TIE:
                    break;
            }
        }

        if (playerID == 2)
        {
            switch (state)
            {
                case ConditionState.CORRECT:
                    player2Score += 2;
                    break;
                case ConditionState.WRONG:
                    player2Score -= 1;
                    break;
                case ConditionState.TIE:
                    break;
            }
        }

        photonView.RPC(nameof(AddScoreToData), RpcTarget.AllBuffered, playerID);

        LeanTween.delayedCall(.5f, () =>
        {
            player1Input = "";
            player2Input = "";
            isActive = true;
        });
    }

    [PunRPC]
    private void TimeDeplete()
    {
        MultiPlayerData.time -= Time.deltaTime;

        if (MultiPlayerData.time <= 0)
        {
            MultiPlayerData.time = 0;
            isActive = false;
            gameState = GameState.END;
        }

        MultiPlayerUI.Instance.photonView.RPC("ShowTimer", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void ShowScore()
    {
        MultiPlayerUI.Instance.photonView.RPC("ShowScore", RpcTarget.AllBuffered, player1Score, player2Score);
    }

    [PunRPC]
    private void AddScoreToData(int playerId)
    {
        if (player1Score <= 0) player1Score = 0;
        if (player2Score <= 0) player2Score = 0;

        if (playerId == 1) MultiPlayerData.Player1Score = player1Score;
        if (playerId == 2) MultiPlayerData.Player2Score = player2Score;

        Debug.Log(string.Format("Player1Score : {0}, Player2Score : {1}", MultiPlayerData.Player1Score, MultiPlayerData.Player2Score));
    }

    [PunRPC]
    public void Pause()
    {
        if (gameState != GameState.RPS) return;
        MultiPlayerUI.Instance.photonView.RPC("ShowPausePanel", RpcTarget.AllBuffered, true);
        gameState = GameState.PAUSE;
    }

    [PunRPC]
    public void Resume()
    {
        if (gameState != GameState.PAUSE) return;
        MultiPlayerUI.Instance.photonView.RPC("ShowPausePanel", RpcTarget.AllBuffered, false);
        gameState = GameState.RPS;
    }

    [PunRPC]
    public void Restart()
    {
        photonView.RPC("Init", RpcTarget.AllBuffered);
        gameState = GameState.START;
    }

    [PunRPC]
    public void Exit()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Game");
    }

    [PunRPC]
    private void EndGame()
    {
        MultiPlayerUI.Instance.photonView.RPC("ShowResultPanel", RpcTarget.AllBuffered);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("Restart", RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            photonView.RPC("Exit", RpcTarget.AllBuffered);
        }
    }
}
