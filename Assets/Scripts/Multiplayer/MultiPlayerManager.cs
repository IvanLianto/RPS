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

    [SerializeField] private float startTime;
    [SerializeField] private float time;
    [SerializeField] private float penaltyTime;
    
    [SerializeField] private int score;

    [SerializeField] private MultiPlayerInput player1;
    [SerializeField] private MultiPlayerInput player2;
    [SerializeField] private List<MultiPlayerInput> listPlayers = new List<MultiPlayerInput>();

    [SerializeField] private string player1Input;
    [SerializeField] private string player2Input;

    [SerializeField] private int player1Score;
    [SerializeField] private int player2Score;

    [SerializeField] private bool player1HasChoise;
    [SerializeField] private bool player2HasChoise;

    public ChoiseButton rockButton;
    public ChoiseButton scissorButton;
    public ChoiseButton paperButton;

    [Header("Test")]
    [SerializeField]
    private Text textInfo;
    [SerializeField]
    private Text textInfo2;

    private float _penaltyTime;

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
        listPlayers.Add(obj.GetComponent<MultiPlayerInput>());
        //player1.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.CurrentRoom.GetPlayer(1));
        //player2.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.CurrentRoom.GetPlayer(2));
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
        _penaltyTime = penaltyTime;
        player1Input = "";
        player2Input = "";
        MultiPlayerUI.Instance.photonView.RPC("Init", RpcTarget.All);


    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.START: photonView.RPC("StartGame", RpcTarget.All); break;
            case GameState.PAUSE: break;
            case GameState.RPS: photonView.RPC("TimeDeplete", RpcTarget.All); break;
            case GameState.END: photonView.RPC("EndGame", RpcTarget.All); break;
        }
    }

    #region temp
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

    #region PickChoise
    //[PunRPC]
    //public void PickChoise(string choise, int playerID)
    //{
    //    if (!isActive) return;

    //    if (gameState != GameState.RPS) return;

    //    if (player1.GetPlayerID == playerID)
    //    {
    //        if (player1Input != "") return;
    //        player1Input = choise;
    //        Debug.Log(string.Format("Player1Input : {0}", player1Input));
    //        player1HasChoise = true;
    //    }

    //    if (player2.GetPlayerID == playerID)
    //    {
    //        if (player2Input != "") return;
    //        player2Input = choise;
    //        Debug.Log(string.Format("Player2Input : {0}", player2Input));
    //        player2HasChoise = true;
    //    }

    //    if (player1Input != "" && player2Input != "")
    //    {
    //        Debug.Log(string.Format("Player1Input : {0}, Player2Input : {1}", player1Input, player2Input));
    //        //Play(playerID);
    //        photonView.RPC("Play", RpcTarget.All, playerID);
    //    }
    //}

    #region PickChoise 2
    [PunRPC]
    public void PickChoise(string choise, int playerId)
    {
        if (!isActive) return;

        if (gameState != GameState.RPS) return;

        if (playerId == 1)
        {
            if (player1Input != "") return;
            player1Input = choise;
            Debug.Log(string.Format("Player1Input : {0}", player1Input));
        }

        if (playerId == 2)
        {
            if (player2Input != "") return;
            player2Input = choise;
            Debug.Log(string.Format("Player2Input : {0}", player2Input));
        }

        if (player1Input != "" && player2Input != "")
        {
            Debug.Log(string.Format("Player1Input : {0}, Player2Input : {1}", player1Input, player2Input));
            //Play(playerID);
            photonView.RPC("Play", RpcTarget.AllBuffered, playerId);
        }
    }
    #endregion
    #endregion

    #region temp Play
    //[PunRPC]
    //public void Play(int playerID)
    //{
    //    if (!isActive) return;

    //    if (gameState != GameState.RPS) return;

    //    // P1 = Gunting
    //    // P2 = Kertas



    //    if (player1.GetPlayerID == playerID)
    //    {
    //        textInfo2.text = string.Format("Player1Input : {0}, Player2Input : {1}, Ini Player 1", player1Input, player2Input);
    //        Player1Condition(playerID);
    //        //photonView.RPC(nameof(Player1Condition), RpcTarget.All, playerID);
    //        //MultiPlayerUI.Instance.photonView.RPC("ShowResultImage", RpcTarget.AllBuffered, player2Input);
    //    }

    //    if (player2.GetPlayerID == playerID)
    //    {
    //        textInfo2.text = string.Format("Player1Input : {0}, Player2Input : {1}, Ini Player 2", player1Input, player2Input);
    //        Player2Condition(playerID);
    //        //photonView.RPC(nameof(Player2Condition), RpcTarget.All, playerID);
    //        //MultiPlayerUI.Instance.photonView.RPC("ShowResultImage", RpcTarget.AllBuffered, player1Input);
    //    }
    //}
    #endregion

    [PunRPC]
    public void Play(int playerID)
    {
        if (!isActive) return;

        if (gameState != GameState.RPS) return;

        // P1 = Gunting
        // P2 = Kertas

        if (playerID == 1)
        {
            textInfo2.text = string.Format("Player1Input : {0}, Player2Input : {1}, Ini Player 1", player1Input, player2Input);
            Player1Condition(playerID);
            MultiPlayerUI.Instance.ShowResultImage(player2Input);
            //photonView.RPC(nameof(Player1Condition), RpcTarget.AllBuffered, playerID);
            //MultiPlayerUI.Instance.photonView.RPC("ShowResultImage", RpcTarget.AllBuffered, player2Input);
        }

        if (playerID == 2)
        {
            textInfo2.text = string.Format("Player1Input : {0}, Player2Input : {1}, Ini Player 2", player1Input, player2Input);
            Player2Condition(playerID);
            MultiPlayerUI.Instance.ShowResultImage(player1Input);
            //photonView.RPC(nameof(Player2Condition), RpcTarget.AllBuffered, playerID);
            //MultiPlayerUI.Instance.photonView.RPC("ShowResultImage", RpcTarget.AllBuffered, player1Input);
        }
    }

    #region Player 1 dan 2 condition
    //private void Player1Condition(int playerID)
    //{
    //    switch (player1Input)
    //    {
    //        case "Rock":
    //            switch (player2Input)
    //            {
    //                case "Paper":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : KALAH", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.WRONG);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
    //                    break;
    //                case "Scissor":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : MENANG", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.CORRECT);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
    //                    break;
    //                case "Rock":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : DRAW", player1Input, player2Input);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
    //                    break;
    //            }
    //            break;
    //        case "Paper":
    //            switch (player2Input)
    //            {
    //                case "Rock":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : MENANG", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.CORRECT);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
    //                    break;
    //                case "Scissor":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : KALAH", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.WRONG);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
    //                    break;
    //                case "Paper":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : DRAW", player1Input, player2Input);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
    //                    break;
    //            }
    //            break;
    //        case "Scissor":
    //            switch (player2Input)
    //            {
    //                case "Rock":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : KALAH", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.WRONG);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
    //                    break;
    //                case "Paper":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : MENANG", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.CORRECT);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
    //                    break;
    //                case "Scissor":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : DRAW", player1Input, player2Input);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
    //                    break;
    //            }
    //            break;
    //    }
    //    player1Input = "";
    //}

    //private void Player2Condition(int playerID)
    //{
    //    switch (player2Input)
    //    {
    //        case "Rock":
    //            switch (player1Input)
    //            {
    //                case "Paper":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : KALAH", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.WRONG);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
    //                    break;
    //                case "Scissor":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : MENANG", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.CORRECT);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
    //                    break;
    //                case "Rock":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : DRAW", player1Input, player2Input);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
    //                    break;
    //            }
    //            break;
    //        case "Paper":
    //            switch (player1Input)
    //            {
    //                case "Rock":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : MENANG", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.CORRECT);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
    //                    break;
    //                case "Scissor":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : KALAH", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.WRONG);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
    //                    break;
    //                case "Paper":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : DRAW", player1Input, player2Input);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
    //                    break;
    //            }
    //            break;
    //        case "Scissor":
    //            switch (player1Input)
    //            {
    //                case "Rock":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : KALAH", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.WRONG);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
    //                    break;
    //                case "Paper":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : MENANG", player1Input, player2Input);
    //                    //CheckCondition(ConditionState.CORRECT);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
    //                    break;
    //                case "Scissor":
    //                    textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : DRAW", player1Input, player2Input);
    //                    //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
    //                    break;
    //            }
    //            break;
    //    }
    //    player2Input = "";
    //}
    #endregion

    private void Player1Condition(int playerID)
    {
        switch (player1Input)
        {
            case "Rock":
                switch (player2Input)
                {
                    case "Paper":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : KALAH", player1Input, player2Input);
                        CheckCondition(ConditionState.WRONG, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
                        break;
                    case "Scissor":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : MENANG", player1Input, player2Input);
                        CheckCondition(ConditionState.CORRECT, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
                        break;
                    case "Rock":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : DRAW", player1Input, player2Input);
                        CheckCondition(ConditionState.TIE, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
                        break;
                }
                break;
            case "Paper":
                switch (player2Input)
                {
                    case "Rock":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : MENANG", player1Input, player2Input);
                        CheckCondition(ConditionState.CORRECT, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
                        break;
                    case "Scissor":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : KALAH", player1Input, player2Input);
                        CheckCondition(ConditionState.WRONG, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
                        break;
                    case "Paper":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : DRAW", player1Input, player2Input);
                        CheckCondition(ConditionState.TIE, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
                        break;
                }
                break;
            case "Scissor":
                switch (player2Input)
                {
                    case "Rock":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : KALAH", player1Input, player2Input);
                        CheckCondition(ConditionState.WRONG, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
                        break;
                    case "Paper":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : MENANG", player1Input, player2Input);
                        CheckCondition(ConditionState.CORRECT, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
                        break;
                    case "Scissor":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player1 : DRAW", player1Input, player2Input);
                        CheckCondition(ConditionState.TIE, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
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
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : KALAH", player1Input, player2Input);
                        CheckCondition(ConditionState.WRONG, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
                        break;
                    case "Scissor":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : MENANG", player1Input, player2Input);
                        CheckCondition(ConditionState.CORRECT, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
                        break;
                    case "Rock":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : DRAW", player1Input, player2Input);
                        CheckCondition(ConditionState.TIE, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
                        break;
                }
                break;
            case "Paper":
                switch (player1Input)
                {
                    case "Rock":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : MENANG", player1Input, player2Input);
                        CheckCondition(ConditionState.CORRECT, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
                        break;
                    case "Scissor":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : KALAH", player1Input, player2Input);
                        CheckCondition(ConditionState.WRONG, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
                        break;
                    case "Paper":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : DRAW", player1Input, player2Input);
                        CheckCondition(ConditionState.TIE, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
                        break;
                }
                break;
            case "Scissor":
                switch (player1Input)
                {
                    case "Rock":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : KALAH", player1Input, player2Input);
                        CheckCondition(ConditionState.WRONG, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.WRONG, playerID);
                        break;
                    case "Paper":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : MENANG", player1Input, player2Input);
                        CheckCondition(ConditionState.CORRECT, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.CORRECT, playerID);
                        break;
                    case "Scissor":
                        textInfo.text = string.Format("Player1Input : {0}, Player2Input : {1}, Player2 : DRAW", player1Input, player2Input);
                        CheckCondition(ConditionState.TIE, playerID);
                        //photonView.RPC("CheckCondition", RpcTarget.AllBuffered, ConditionState.TIE, playerID);
                        break;
                }
                break;
        }
    }

    #region CheckCondition
    //private void CheckCondition(ConditionState state, int playerID)
    //{
    //    isActive = false;
    //    ConditionState _state = state;
    //    Debug.Log(_state);

    //    if (player1.GetPlayerID == playerID)
    //    {
    //        _state = state;
    //        //AddScore(_state, PlayerState.PLAYER1);
    //        photonView.RPC("AddScore", RpcTarget.AllBuffered, _state, playerID);
    //        MultiPlayerUI.Instance.photonView.RPC("ShowPopUpText", RpcTarget.AllBuffered, _state, PlayerState.PLAYER1);
    //    }

    //    if (player2.GetPlayerID == playerID)
    //    {
    //        _state = state;
    //        //AddScore(_state, PlayerState.PLAYER2);
    //        photonView.RPC("AddScore", RpcTarget.AllBuffered, _state, playerID);
    //        MultiPlayerUI.Instance.photonView.RPC("ShowPopUpText", RpcTarget.AllBuffered, _state, PlayerState.PLAYER2);
    //    }

    //    //MultiPlayerUI.Instance.ShowPopUpText(_state);
        

    //    LeanTween.delayedCall(.5f, () =>
    //    {
    //        player1Input = "";
    //        player2Input = "";
    //        isActive = true;
    //    });
    //}
    #endregion

    private void CheckCondition(ConditionState state, int playerID)
    {
        isActive = false;
        ConditionState _state = state;
        Debug.Log(_state);

        if (playerID == 1)
        {
            _state = state;
            AddScore(_state, playerID);
            //photonView.RPC("AddScore", RpcTarget.AllBuffered, _state, playerID);
            MultiPlayerUI.Instance.ShowPopUpText(_state, PlayerState.PLAYER1);
            //MultiPlayerUI.Instance.photonView.RPC("ShowPopUpText", RpcTarget.AllBuffered, _state, PlayerState.PLAYER1);
        }

        if (playerID == 2)
        {
            _state = state;
            AddScore(_state, playerID);
            MultiPlayerUI.Instance.ShowPopUpText(_state, PlayerState.PLAYER2);
            //photonView.RPC("AddScore", RpcTarget.AllBuffered, _state, playerID);
            //MultiPlayerUI.Instance.photonView.RPC("ShowPopUpText", RpcTarget.AllBuffered, _state, PlayerState.PLAYER2);
        }

        //MultiPlayerUI.Instance.ShowPopUpText(_state);
    }

    #region AddScore
    //private void AddScore(ConditionState state, int playerID)
    //{
    //    if (player1.GetPlayerID == playerID)
    //    {
    //        switch (state)
    //        {
    //            case ConditionState.CORRECT:
    //                player1Score += 2;
    //                break;
    //            case ConditionState.WRONG:
    //                player1Score -= 1;
    //                break;
    //            case ConditionState.TIE:
    //                break;
    //        }
    //    }

    //    if (player2.GetPlayerID == playerID)
    //    {
    //        switch (state)
    //        {
    //            case ConditionState.CORRECT:
    //                player2Score += 2;
    //                break;
    //            case ConditionState.WRONG:
    //                player2Score -= 1;
    //                break;
    //            case ConditionState.TIE:
    //                break;
    //        }
    //    }
    //    photonView.RPC("AddToData", RpcTarget.AllBuffered);

    //    MultiPlayerUI.Instance.photonView.RPC("ShowScore", RpcTarget.AllBuffered);
    //}
    #endregion

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
        photonView.RPC("AddToData", RpcTarget.AllBuffered, playerID);

        MultiPlayerUI.Instance.photonView.RPC("ShowScore", RpcTarget.AllBuffered, player1Score, player2Score);
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
    private void PenaltyTime(PlayerState state)
    {
        _penaltyTime -= Time.deltaTime;
        MultiPlayerUI.Instance.ShowPenaltyTimer(_penaltyTime, true);
        if (_penaltyTime <= 0 || (_penaltyTime > 0 && _penaltyTime < 1))
        {
            isActive = false;
            MultiPlayerUI.Instance.ShowPenaltyTimer(_penaltyTime, false);

            switch (state)
            {
                case PlayerState.PLAYER1:
                    MultiPlayerData.Player1Score -= 1;
                    MultiPlayerData.Player2Score += 2;
                    break;
                case PlayerState.PLAYER2:
                    MultiPlayerData.Player1Score += 2;
                    MultiPlayerData.Player2Score -= 1;
                    break;
            }
            LeanTween.delayedCall(.5f, () =>
            {
                _penaltyTime = penaltyTime;
                isActive = true;
            });
        }
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
        MultiPlayerUI.Instance.ShowResultPanel();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }

    [PunRPC]
    private void AddToData(int playerID)
    {
        if (playerID == 1)
        {
            if (player1Score <= 0)
            {
                player1Score = 0;
            }

            MultiPlayerData.Player1Score = player1Score;

        } 

        if (playerID == 2)
        {
            if (player2Score <= 0)
            {
                player2Score = 0;
            }

            MultiPlayerData.Player2Score = player2Score;

        }

        LeanTween.delayedCall(.5f, () =>
        {
            player1Input = "";
            player2Input = "";
            isActive = true;
        });

        Debug.Log(string.Format("Player1Score : {0}, Player2Score : {1}", MultiPlayerData.Player1Score, MultiPlayerData.Player2Score));
    }

    #endregion
}
