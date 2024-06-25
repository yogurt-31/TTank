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
            //�̽��������� �̹� �˴ٿ� �Ȱű� ������ ��Ʈ��ũ ���� ������� ���� ���� �Űܵ� �ȴ�.
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void Dispose()
    {
        if (_manager == null) return;
        _manager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }
}
