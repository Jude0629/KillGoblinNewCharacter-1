using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LoadingHandler : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI statusTxt;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        statusTxt.text = "server connected";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        statusTxt.text = "lobby joined";
        string roomName = PhotonNetwork.CountOfRooms.ToString() + "Room" + (PhotonNetwork.CountOfRooms).ToString();
/*
        RoomOptions roomOps = new RoomOptions();
        roomOps.MaxPlayers = (byte)(8);
        roomOps.IsOpen = true;
        roomOps.IsVisible = true;
        PhotonNetwork.JoinRandomOrCreateRoom(null, (byte)(8), MatchmakingMode.FillRoom, TypedLobby.Default, null, roomName, roomOps);*/
    }
    public void CreateRoom()
    {
        statusTxt.text = "lobby joined";
        string roomName = PhotonNetwork.CountOfRooms.ToString() + "Room" + (PhotonNetwork.CountOfRooms).ToString();

        RoomOptions roomOps = new RoomOptions();
        roomOps.MaxPlayers = (byte)(8);
        roomOps.IsOpen = true;
        roomOps.IsVisible = true;
        PhotonNetwork.CreateRoom("MyRoom",roomOps,TypedLobby.Default);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("MyRoom");
    }
    public override void OnJoinedRoom()
    {
        statusTxt.text = "room joined";
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
