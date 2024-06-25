using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppHost : MonoSingleton<AppHost>
{
    public NetworkServer NetServer { get; private set; }

    public void MakeServer()
    {
        NetServer = new NetworkServer(NetworkManager.Singleton);
    }

    public void StartHost()
    {
        MakeServer();
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(
                   "GameScene", LoadSceneMode.Single);
    }

    public void ShutdownHost()
    {
        NetServer?.Dispose();
    }

    private void OnDestroy()
    {
        ShutdownHost();
    }
}
