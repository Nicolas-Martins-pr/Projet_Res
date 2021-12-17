using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    //Our player state in the lobby
    public struct LobbyPlayerState : INetworkSerializable, IEquatable<LobbyPlayerState>
    {
        public ulong ClientId;
        public FixedString32Bytes PlayerName;
        public bool IsReady;

        public LobbyPlayerState(ulong clientId, FixedString32Bytes playerName, bool isReady)
        {
            ClientId = clientId;
            PlayerName = playerName;
            IsReady = isReady;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref PlayerName);
            serializer.SerializeValue(ref IsReady);
        }

        public bool Equals(LobbyPlayerState other)
        {
            return ClientId == other.ClientId &&
                PlayerName.Equals(other.PlayerName) &&
                IsReady == other.IsReady;
        }
    }

    //Card corresponding for each player in the lobby
    public class LobbyPlayerCard : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject waitingForPlayerPanel;
        [SerializeField] private GameObject playerDataPanel;

        [Header("Data Display")]
        [SerializeField] private TMP_Text playerDisplayNameText;
        [SerializeField] private Image selectedCharacterImage;
        [SerializeField] private Toggle isReadyToggle;

        public void UpdateDisplay(LobbyPlayerState lobbyPlayerState)
        {
            playerDisplayNameText.text = lobbyPlayerState.PlayerName.ToString();
            isReadyToggle.isOn = lobbyPlayerState.IsReady;

            waitingForPlayerPanel.SetActive(false);
            playerDataPanel.SetActive(true);
        }

        public void DisableDisplay()
        {
            waitingForPlayerPanel.SetActive(true);
            playerDataPanel.SetActive(false);
        }
    }
}
