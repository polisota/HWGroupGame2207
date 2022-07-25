using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviourPunCallbacks
{
    private const string PLAYFAB_ID = "2187E";
    private const string GAME_VERSION = "dev";

    [Header("Photon UI")]
    [SerializeField] private Button connectButton;    
    [SerializeField] private Text connectText;
    [SerializeField] private Image connectImage;


    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = PLAYFAB_ID;

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "ProgrammerLamer",
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, Success, Fail);
    }

    public void Success(LoginResult result)
    {
        Debug.Log(result.PlayFabId);
        Connect();

        connectImage.color = Color.green;
        connectText.text = "Connection succeeded";
        connectButton.interactable = false;
    }

    private void Fail(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);

        connectImage.color = Color.red;
        connectText.text = "Connection failed";
        connectButton.interactable = true;
    }

    private void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GAME_VERSION;
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        Debug.LogError("PhotonConnect");        
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
        PhotonNetwork.JoinRandomOrCreateRoom();
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
