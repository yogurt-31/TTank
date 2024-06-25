using DG.Tweening;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class ConnectPanel : MonoBehaviour
{
    public enum PanelRole
    {
        Host, Client
    }

    [SerializeField] private TMP_InputField _ipInput, _portInput;
    [SerializeField] private Button _connectBtn, _closeBtn;

    private CanvasGroup _canvasGroup;
    private PanelRole _role;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _closeBtn.onClick.AddListener(() => SetActive(false));
        _connectBtn.onClick.AddListener(HandleConnectClick);
    }

    public void OpenPanel(PanelRole role)
    {
        _role = role;
        string ip = FindIPAddress();
        _ipInput.text = string.IsNullOrEmpty(ip) ? "127.0.0.1" : ip;
        _portInput.text = "9999";
        SetActive(true);
    }

    private string FindIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        try
        {
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
        return null;
    }

    private void HandleConnectClick()
    {
        if (_role == PanelRole.Host)
        {
            AppHost.Instance.StartHost(); //네트워크 서버랑 호스트가 같이 시작되는거
        }
        else if (_role == PanelRole.Client)
        {
            AppClient.Instance.StartClient(); //네트워크 클라이언트랑 같이 시작되는거.
        }
    }

    private void SetActive(bool value)
    {
        _canvasGroup.interactable = value;
        _canvasGroup.blocksRaycasts = value;
        float alpha = value ? 1f : 0;
        _canvasGroup.DOFade(alpha, 0.4f);
    }
}
