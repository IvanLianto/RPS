using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField nameInput;
    [SerializeField] private InputField createInput;
    [SerializeField] private InputField joinInput;

    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject matchPanel;

    [SerializeField] private Button leaveButton;

    [SerializeField] private Text playerName;
    [SerializeField] private Text gameStartText;
    [SerializeField] private Text roomNotFound;

    private void Start()
    {
        SetActivePanel(loginPanel);
    }

    public void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.PublishUserId = true;
        PhotonNetwork.CreateRoom(createInput.text, options);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        roomNotFound.gameObject.SetActive(true);
        roomNotFound.text = "Room not Found!";
    }

    public override void OnJoinedRoom()
    {
        roomNotFound.gameObject.SetActive(false);
        if (nameInput.text == "")
        {
            nameInput.text = "No Name";
        }

        PhotonNetwork.NickName = nameInput.text;

        leaveButton.interactable = true;

        SetActivePanel(matchPanel);

        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    [PunRPC]
    private void UpdateLobbyUI()
    {
        playerName.text = PhotonNetwork.NickName;

        if (PhotonNetwork.PlayerList.Length < 2)
        {
            gameStartText.text = "Please wait for another player to join";
        }
        

        if (PhotonNetwork.PlayerList.Length == 2)
        {
            gameStartText.text = "The game will start";
            leaveButton.interactable = false;

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(LoadLevel), RpcTarget.All, "MultiPlayer");
            }
        }
    }

    [PunRPC]
    private IEnumerator LoadLevel(string sceneName)
    {
        yield return new WaitForSeconds(4f);
        if (sceneName != null)
            PhotonNetwork.LoadLevel(sceneName);
        else
            yield return null;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    [PunRPC]
    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
        SetActivePanel(loginPanel);
    }

    private void SetActivePanel(GameObject panel)
    {
        loginPanel.SetActive(false);
        matchPanel.SetActive(false);

        panel.SetActive(true);
    }
}
