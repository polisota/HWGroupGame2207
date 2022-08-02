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
    //[SerializeField] private TMP_InputField _playerName;
    [SerializeField] private TMP_InputField _roomName;
    [SerializeField] private TMP_InputField _roomPass;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _joinRoomButton;
    [SerializeField] private byte _maxPlayers = 4;
    [SerializeField] private Text _roomList;
    [SerializeField] private Text _lobbyUIText;

    [Header("FriendsRoom")]
    [SerializeField] private string friendRoom = "Friends";
    [SerializeField] private string friendPass = "friends";

    private const string GAME_VERSION = "dev";

    private void Start()
    {
        //Authorization target = (Authorization)FindObjectOfType(typeof(Authorization));
        //var userName = target._userName;

        PhotonNetwork.GameVersion = GAME_VERSION;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        //_lobbyUIText.text = $"Hello, {userName}!";
    }

    private void Awake()
    {
        _createRoomButton.onClick.AddListener(OnCreatedRoom);
        //_createRoomButton.onClick.AddListener(OnCreatedRoom);
    }

    public void CreateNewBtnClick()
    {
        if (PhotonNetwork.IsConnected)
        {
            CreateRoom();
        }
        
        //Debug.Log("PhotonConnect");
    }


    private void CreateRoom()
    {
        //PhotonNetwork.CurrentRoom.IsOpen = false;
        //PhotonNetwork.CurrentRoom.IsVisible = false;

        //PhotonNetwork.LocalPlayer.NickName = _playerName.text;

        string roomName = _roomName.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;       

        RoomOptions options = new RoomOptions { MaxPlayers = _maxPlayers, PlayerTtl = 10000, IsOpen = true };
        //PhotonNetwork.CreateRoom(roomName, options, null);
        PhotonNetwork.CreateRoom(roomName: roomName, roomOptions: options);
        PhotonNetwork.LoadLevel("RoomScene");
        Debug.Log($"Player join room named {roomName}");
        
    }

    public void JoinRoomBtnClick()
    {        
        JoinRoom();
    }

    private void JoinRoom()
    {
        string roomForJoin = _roomName.text;
        string passForJoin = _roomPass.text;

        if (roomForJoin == friendRoom && passForJoin == friendPass)
        {            
            RoomOptions options = new RoomOptions { MaxPlayers = _maxPlayers, PlayerTtl = 10000, IsOpen = true };
            PhotonNetwork.JoinOrCreateRoom("Friends", options, TypedLobby.Default);
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.IsOpen == true)
            {
                PhotonNetwork.JoinRoom(roomForJoin);
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //_roomList.text = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in roomList)
        {
            _roomList.text = room.Name;
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
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

    public void PhotonDisconnect()
    {
        PhotonNetwork.Disconnect();
        Debug.LogError("PhotonDisconnect");
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        //PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        foreach (KeyValuePair<string, object> kvp in data)
            Debug.Log($"key: {kvp.Key}/value: {kvp.Value}");
    }

    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log(debugMessage);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName);
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
    }

}
