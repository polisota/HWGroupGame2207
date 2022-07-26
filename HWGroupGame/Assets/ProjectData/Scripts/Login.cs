using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviourPunCallbacks
{
    private const string PLAYFAB_TITLE = "2187E";
    private const string GAME_VERSION = "dev";
    private const string AUTHENTIFICATION_KEY = "AUTHENTIFICATION_KEY";

    [Header("UI")]
    [SerializeField] private GameObject _loadingWin;
    [SerializeField] private GameObject _createAcc;
    [SerializeField] private GameObject _signIn;


    //[Header("Photon UI")]
    //[SerializeField] private Button connectButton;    
    //[SerializeField] private Text connectText;
    //[SerializeField] private Image connectImage;
    private void Awake()
    {
        _loadingWin.SetActive(true);
    }

    private struct Data
    {
        public bool needCreation;
        public string id;
    }

    void Start()
    { 
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = PLAYFAB_TITLE;

        var needCreation = !PlayerPrefs.HasKey(AUTHENTIFICATION_KEY);
        var id = PlayerPrefs.GetString(AUTHENTIFICATION_KEY, Guid.NewGuid().ToString());
        var data = new Data { needCreation = needCreation, id = id };

        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = needCreation
        };
        PlayFabClientAPI.LoginWithCustomID(request, Success, Fail, data);

        _loadingWin.SetActive(false);
        _createAcc.SetActive(true);
    }

    public void SignInWin()
    {
        _createAcc.SetActive(false);
        _signIn.SetActive(true);
    }

    public void Success(LoginResult result)
    {
        PlayerPrefs.SetString(AUTHENTIFICATION_KEY, ((Data)result.CustomData).id);
        Debug.Log(result.PlayFabId);
        Debug.Log(((Data)result.CustomData).needCreation);
        Debug.Log(((Data)result.CustomData).id);
        Connect();

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), SuccessInfo, Error);

        //connectImage.color = Color.green;
        //connectText.text = "Connection succeeded";
        //connectButton.interactable = false;
    }

    private void Error(PlayFabError error)
    {
        Debug.LogError(error);
    }

    private void SuccessInfo(GetAccountInfoResult result)
    {
        Debug.Log(result.AccountInfo.PlayFabId);
    }

    private void Fail(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);

        //connectImage.color = Color.red;
        //connectText.text = "Connection failed";
        //connectButton.interactable = true;
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
        Debug.Log("PhotonConnect");        
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
