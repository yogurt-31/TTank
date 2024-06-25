using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkClient : IDisposable
{
    private NetworkManager _manager;

    public NetworkClient(NetworkManager manager)
    {
        _manager = manager;
        _manager.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    private void OnClientDisconnectCallback(ulong clientID)
    {
        if (!_manager.IsServer && _manager.DisconnectReason != string.Empty)
        {
            Debug.Log($"Approval Declined Reason : {_manager.DisconnectReason}");
        }

        if (clientID != 0 && clientID != _manager.LocalClientId) return;
        Disconnect();
    }

    public void Disconnect()
    {
        if (_manager.IsConnectedClient)
        {
            _manager.Shutdown();
        }

        if (SceneManager.GetActiveScene().name != "MeneScene")
        {
            //이시점에서는 이미 셧다운 된거기 때문에 네트워크 씬과 상관없이 내가 씬을 옮겨도 된다.
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void Dispose()
    {
        if (_manager == null) return;
        _manager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }
}
