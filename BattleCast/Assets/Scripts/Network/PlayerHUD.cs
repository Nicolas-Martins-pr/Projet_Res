using Networking;
using System;
using System.Text;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerHUD : NetworkBehaviour
{
    private NetworkVariable<NetworkString> playersName = new NetworkVariable<NetworkString>();

    private bool overlaySet = false;
    private void Start()
    {
        if (IsClient && IsOwner)
        {

            string payload = Encoding.UTF8.GetString(NetworkManager.Singleton.NetworkConfig.ConnectionData);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);
            SetPlayerNameServerRPC(connectionPayload.playerName);
        }
    }

   [ServerRpc] 
    public void SetPlayerNameServerRPC(string newPlayerName)
    {
        playersName.Value = newPlayerName;
    }
    
    public void SetOverlay()
    {
        Debug.Log("Overlay");
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = playersName.Value;
    }

    private void Update()
    {

        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        if (!overlaySet && !string.IsNullOrEmpty(playersName.Value))
        {
            SetOverlay();
            overlaySet = true;
        }
    }
}
