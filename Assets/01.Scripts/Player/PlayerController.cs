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
        // 유저 이름이 변경되었을 때 Text도 함께 변경되도록 구독해야겠지?

        if (IsOwner)
        {
            _followCam.Priority = _ownerCamPriority;
        }
        OnPlayerSpawn?.Invoke(this); // 서버만 이벤트 실행
    }

    public override void OnNetworkDespawn()
    {
        OnPlayerDespawn?.Invoke(this);
    }

}
