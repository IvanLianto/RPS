using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MultiPlayerInput : MonoBehaviourPunCallbacks
{
    public Player photonPlayer;

    public int playerId;

    public ChoiseButton rockButton;
    public ChoiseButton scissorButton;
    public ChoiseButton paperButton;

    public void ButtonClick(string choise)
    {
        //MultiPlayerManager.Instance.photonView.RPC("PickChoise", RpcTarget.AllBuffered, choise, playerId);
        //MultiPlayerManager.Instance.photonView.RPC("PickChoise", RpcTarget.All, choise, playerId);
        if (photonView.IsMine) MultiPlayerManager.Instance.photonView.RPC("PickChoise", RpcTarget.AllBuffered, choise, playerId);
    }

    public void Initialize(Player p)
    {
        photonPlayer = p;

        playerId = p.ActorNumber;

        if (p.IsLocal)
        {
            rockButton.SetButton(this);
            scissorButton.SetButton(this);
            paperButton.SetButton(this);
        }
    }

    public void Init(ChoiseButton rock, ChoiseButton paper, ChoiseButton scissor)
    {
        rockButton = rock;
        paperButton = paper;
        scissorButton = scissor;

        playerId = photonView.Owner.ActorNumber;

        if (photonView.IsMine)
        {
            rockButton.SetButton(this);
            scissorButton.SetButton(this);
            paperButton.SetButton(this);
        }
    }

    #region SETTER/GETTER
    public int GetPlayerID { get { return playerId; } }
    #endregion
}
