using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : IDisposable
{
    public Action<ulong> OnClientLeft;

    private NetworkManager _manager;

    public NetworkServer(NetworkManager manager)
    {
        _manager = manager;
        _manager.ConnectionApprovalCallback += HandleConnectionApproval;
    }

    private void HandleConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {

        UserData data = new UserData
        {
            clientID = request.ClientNetworkId,
            characterIndex = 0,
            playerName = $"Player_{request.ClientNetworkId}"
        };

        response.CreatePlayerObject = false;
        response.Approved = true;

    }
    public void Dispose()
    {
        if (_manager == null) return;

        _manager.ConnectionApprovalCallback -= HandleConnectionApproval;

        if (_manager.IsListening)
        {
            _manager.Shutdown();
        }
    }
}
