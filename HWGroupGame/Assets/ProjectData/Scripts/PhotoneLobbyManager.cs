using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotoneLobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _playerName;
    [SerializeField] private TMP_InputField _roomName;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private byte _maxPlayers = 4;


    private void Awake()
    {
        _createRoomButton.onClick.AddListener(OnCreatedRoom);
        _createRoomButton.onClick.AddListener(OnCreatedRoom);
    }

    private void OnCreateRoom()
    {
        //PhotonNetwork.CurrentRoom.IsOpen = false;
        //PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LocalPlayer.NickName = _playerName.text;

        string roomName = _roomName.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;       

        RoomOptions options = new RoomOptions { MaxPlayers = _maxPlayers, PlayerTtl = 10000 };
        //PhotonNetwork.CreateRoom(roomName, options, null);
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: roomName, roomOptions: options);
        SceneManager.LoadScene("RoomScene");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log($"You've joined room {PhotonNetwork.CurrentRoom.Name}");        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.LogError(message);
    }
}
