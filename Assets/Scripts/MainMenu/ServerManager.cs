using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    GameObject serverState;

    GameObject saveButton;
    GameObject randomButton;
    GameObject createButton;

    void Start()
    {
        serverState = GameObject.FindGameObjectWithTag("ServerState");

        PhotonNetwork.ConnectUsingSettings();

        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        saveButton = GameObject.FindGameObjectWithTag("SaveButton");
        randomButton = GameObject.FindGameObjectWithTag("RandomButton");
        createButton = GameObject.FindGameObjectWithTag("CreateButton");

        serverState.GetComponent<TextMeshProUGUI>().text = "Connecting to Server";

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        serverState.GetComponent<TextMeshProUGUI>().text = "Connected!";

        if (!PlayerPrefs.HasKey("Username"))
        {
            saveButton.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            randomButton.gameObject.GetComponent<Button>().interactable = true;
            createButton.gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void EnterRandomRoom()
    {
        PhotonNetwork.LoadLevel(1);

        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        PhotonNetwork.LoadLevel(1);

        string roomKey = Random.Range(0, 999999).ToString();

        PhotonNetwork.JoinOrCreateRoom(roomKey, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        InvokeRepeating("CheckInfos", 0, 1f);

        GameObject myObject = PhotonNetwork.Instantiate("CannonPlayer", Vector3.zero, Quaternion.identity, 0, null);
        myObject.GetComponent<PhotonView>().Owner.NickName = PlayerPrefs.GetString("Username");

        if (PhotonNetwork.PlayerList.Length == 2)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            myObject.gameObject.tag = "Player2";
            Debug.Log("tag");
        }
    }

    public override void OnLeftRoom()
    {
        /*if(PhotonNetwork.PlayerList.Length == 2)
        {
            PlayerPrefs.SetInt("TotalMatches", PlayerPrefs.GetInt("TotalMatches") + 1);
            PlayerPrefs.SetInt("TotalLoses", PlayerPrefs.GetInt("TotalLoses") + 1);
        }*/
    }

    public override void OnLeftLobby()
    {
        //base.OnLeftLobby();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        /*PlayerPrefs.SetInt("TotalMatches", PlayerPrefs.GetInt("TotalMatches") + 1);
        PlayerPrefs.SetInt("TotalWins", PlayerPrefs.GetInt("TotalWins") + 1);
        PlayerPrefs.SetInt("TotalPoints", PlayerPrefs.GetInt("TotalPoints") + 5);*/

        InvokeRepeating("CheckInfos", 0, 0.1f);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        serverState.GetComponent<TextMeshProUGUI>().text = "Connecting Failed";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        serverState.GetComponent<TextMeshProUGUI>().text = "Connecting Failed";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        serverState.GetComponent<TextMeshProUGUI>().text = "Connecting Failed";
    }

    void CheckInfos()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            GameObject.FindGameObjectWithTag("WaitingPlayer").SetActive(false);
            GameObject.FindGameObjectWithTag("FirstPlayerName").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindGameObjectWithTag("SecondPlayerName").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName;

            CancelInvoke("CheckInfos");
        }
        else
        {
            GameObject.FindGameObjectWithTag("WaitingPlayer").SetActive(true);
            GameObject.FindGameObjectWithTag("FirstPlayerName").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindGameObjectWithTag("SecondPlayerName").GetComponent<TextMeshProUGUI>().text = "Player";
        }
    }
}
