using Cinemachine;
using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public static event Action<PlayerController> OnPlayerSpawn;
    public static event Action<PlayerController> OnPlayerDespawn;

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera _followCam;

    [Header("Setting values")]
    [SerializeField] private int _ownerCamPriority;

    public Health HealthCompo { get; private set; }

    private void Awake()
    {
        HealthCompo = GetComponent<Health>();
    }

    public override void OnNetworkSpawn()
    {
        // ���� �̸��� ����Ǿ��� �� Text�� �Բ� ����ǵ��� �����ؾ߰���?

        if (IsOwner)
        {
            _followCam.Priority = _ownerCamPriority;
        }
        OnPlayerSpawn?.Invoke(this); // ������ �̺�Ʈ ����
    }

    public override void OnNetworkDespawn()
    {
        OnPlayerDespawn?.Invoke(this);
    }

}
