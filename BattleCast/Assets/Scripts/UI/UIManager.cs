using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;

public class UIManager : NetworkBehaviour
{

    [SerializeField]
    private GameObject gameDataController;

    [SerializeField]
    private GameObject startMenu;

    [SerializeField]
    private GameObject lobbyMenu;

    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    private Button exitGameButton;

    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    private void Awake()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame} ";
    }
    private void Start()
    {

        
    }
}
