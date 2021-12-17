using Networking;

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LobbyUIManager : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private LobbyPlayerCard[] lobbyPlayerCards;
        [SerializeField] private Button startGameButton;

        private NetworkList<LobbyPlayerState> lobbyPlayers;

        private void Awake()
        {
            lobbyPlayers = new NetworkList<LobbyPlayerState>();
        }

        private void Update()
        {
            if(IsEveryoneReady())
                startGameButton.gameObject.SetActive(true);
            else
                startGameButton.gameObject.SetActive(false);
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
            }

            if (IsServer)
            {
                startGameButton.gameObject.SetActive(true);

                NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

                foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandleClientConnected(client.ClientId);
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            lobbyPlayers.OnListChanged -= HandleLobbyPlayersStateChanged;

            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        //Check if every player has checked the ready toggle
        private bool IsEveryoneReady()
        {
            if (lobbyPlayers.Count < 2)
            {
                return false;
            }

            foreach (var player in lobbyPlayers)
            {
                if (!player.IsReady)
                {
                    return false;
                }
            }

            return true;
        }

        private void HandleClientConnected(ulong clientId)
        {
            var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);

            if (!playerData.HasValue) { return; }

            lobbyPlayers.Add(new LobbyPlayerState(
                clientId,
                playerData.Value.PlayerName,
                false
            ));
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            for (int i = 0; i < lobbyPlayers.Count; i++)
            {
                if (lobbyPlayers[i].ClientId == clientId)
                {
                    lobbyPlayers.RemoveAt(i);
                    break;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            for (int i = 0; i < lobbyPlayers.Count; i++)
            {
                Debug.Log(lobbyPlayers[i]);
                if (lobbyPlayers[i].ClientId == serverRpcParams.Receive.SenderClientId)
                {
                    lobbyPlayers[i] = new LobbyPlayerState(
                        lobbyPlayers[i].ClientId,
                        lobbyPlayers[i].PlayerName,
                        !lobbyPlayers[i].IsReady
                    );
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void StartGameServerRpc()
        {
            ServerGameNetPortal.Instance.StartGame();
        }

        public void OnLeaveClicked()
        {
            GameNetPortal.Instance.RequestDisconnect();

            foreach(LobbyPlayerCard lobbyPlayerCard in lobbyPlayerCards)
            {
                lobbyPlayerCard.DisableDisplay();
            }
        }

        public void OnReadyClicked()
        {
            ToggleReadyServerRpc();
        }

        public void OnStartGameClicked()
        {
            StartGameServerRpc();
        }

        private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
        {
            for (int i = 0; i < lobbyPlayerCards.Length; i++)
            {
                if (lobbyPlayers.Count > i)
                {
                    lobbyPlayerCards[i].UpdateDisplay(lobbyPlayers[i]);
                }
                else
                {
                    lobbyPlayerCards[i].DisableDisplay();
                }
            }

            if (IsHost)
            {
                startGameButton.interactable = IsEveryoneReady();
            }
        }
    }
}
