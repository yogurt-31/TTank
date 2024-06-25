using Cinemachine.Utility;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Projectile _serverProjectilePrefab;
    [SerializeField] private Projectile _clientProjectilePrefab;

    [SerializeField] private Collider2D _playerCollider;

    [Header("Setting values")]
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _fireCooltime;

    private float _prevFireTime;
    public int damage = 10;

    public UnityEvent OnFireEvent;

    private PlayerMovement movement;
    private PlayerScore playerScore;
    private PlayerCannon playerCannon;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        playerScore = GetComponent<PlayerScore>();
        playerCannon = GetComponent<PlayerCannon>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        movement.OnFireEvent += HandleFireEvent;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        movement.OnFireEvent -= HandleFireEvent;
    }

    private void HandleFireEvent()
    {
        if (Time.time < _prevFireTime + _fireCooltime) return;

        for(int i = 0; i < playerCannon.cannonCount; ++i)
        {
            FireServerRpc(playerCannon.CannonPosList(i).position, playerCannon.CannonPosList(i).up);
            SpawnDummyProjectile(playerCannon.CannonPosList(i).position, playerCannon.CannonPosList(i).up);
        }

        _prevFireTime = Time.time;
    }

    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 dir)
    {
        Projectile projectile = Instantiate(_clientProjectilePrefab, spawnPos, Quaternion.identity);

        projectile.transform.localScale = playerScore.CalculateAtkSize();
        projectile.SetUpProjectile(_playerCollider, dir, _projectileSpeed, damage);

        OnFireEvent?.Invoke();
    }

    [ServerRpc]
    private void FireServerRpc(Vector3 spawnPos, Vector3 dir)
    {
        Projectile projectile = Instantiate(_serverProjectilePrefab, spawnPos, Quaternion.identity);

        projectile.SetUpProjectile(_playerCollider, dir, _projectileSpeed, damage);

        //¸¸µé¾î!
        SpawnDummyProjectileClientRpc(spawnPos, dir);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 dir)
    {
        if (IsOwner) return;

        SpawnDummyProjectile(spawnPos, dir);
    }
}
