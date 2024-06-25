using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class PlayerCannon : NetworkBehaviour
{
    [SerializeField] private GameObject cannonPrefab;
    [SerializeField] private Transform _turretTrm;

    [HideInInspector] public int cannonCount = 0;
    private List<GameObject> cannonTrms = new List<GameObject>();

    private void Awake()
    {
        CreateCannon();
    }
    private void Update()
    {
        //if (!IsOwner) return;

        Vector2 mousePos = Input.mousePosition;

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 dir = (worldMousePos - transform.position).normalized;

        _turretTrm.right = new Vector3(dir.x, dir.y);
    }

    public Transform CannonPosList(int i)
    {
        return cannonTrms[i].transform.GetChild(0).transform;
    }

    public void CreateCannon()
    {
        GameObject currentCannon = Instantiate(cannonPrefab, _turretTrm);
        cannonTrms.Add(currentCannon);
        cannonCount++;
        for (int i = 0; i < cannonCount; ++i)
        {
            int angle = Mathf.RoundToInt((360f / cannonCount) * i);
            cannonTrms[i].transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
}
