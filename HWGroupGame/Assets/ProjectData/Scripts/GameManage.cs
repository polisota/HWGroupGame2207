using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        Vector3 pos = new Vector3(Random.Range(-2f, 2f), -0.5f, Random.Range(-8f, -6f));
        PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
    }


    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void Lock()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"Player {newPlayer} entered room");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"Player {otherPlayer} left room");
    }
}
