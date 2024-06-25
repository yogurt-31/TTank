using Unity.Netcode;
using UnityEngine;

public class RespawnManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        PlayerController[] players =
            FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (PlayerController player in players)
        {
            HandlePlayerSpawn(player);
        }

        PlayerController.OnPlayerSpawn += HandlePlayerSpawn;
        PlayerController.OnPlayerDespawn += HandlePlayerDespawn;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) return;
        PlayerController.OnPlayerSpawn -= HandlePlayerSpawn;
        PlayerController.OnPlayerDespawn -= HandlePlayerDespawn;
    }

    private void HandlePlayerSpawn(PlayerController player)
    {
        player.HealthCompo.OnDieEvent += () =>
        {
            ulong clientID = player.OwnerClientId;


            Destroy(player.gameObject); // 이걸 서버만 할 수 있음.
            // 클라 아이디, 탱크 컬러, 딜레이
            GameManager.Instance.spawnTank(clientID, 1f);
        };
    }

    private void HandlePlayerDespawn(PlayerController player)
    {
        // 아직 할 건 없음.
    }
}
