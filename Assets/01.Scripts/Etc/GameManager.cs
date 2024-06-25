using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetMonoSingleton<GameManager>
{
    [SerializeField] private PlayerController _playerPrefab;
    private void Awake()
    {
        if (NetworkManager.Singleton == null) return;
    }

    public override void OnNetworkSpawn()
    {
        DontDestroyOnLoad(gameObject); //다음씬까지 유지되게.
    }

    public void spawnTank(ulong clientID, float delay = 0)
    {
        StartCoroutine(DelayedSpawn(clientID, delay));
    }

    private IEnumerator DelayedSpawn(ulong clientID, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        Vector3 position = new Vector3(0, 0, 0);

        PlayerController tank = Instantiate(_playerPrefab, position, Quaternion.identity);
        tank.NetworkObject.SpawnAsPlayerObject(clientID); // 이 클라이언트 아이디가 주인이 됨
    }

    #region Only Server 

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene",
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    #endregion
}
