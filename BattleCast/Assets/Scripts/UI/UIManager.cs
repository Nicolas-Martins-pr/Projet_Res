using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;

public class UIManager : NetworkBehaviour
{

    [SerializeField]
    private GameObject gameDataController;

    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private Button startGameButton;

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

        //Setting all the buttons actions

        startHostButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host started...");
                startGameButton.gameObject.SetActive(true);
            } else
            {
                Debug.Log("Host could not be started...");
            }
        });

        startServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                Debug.Log("Server started...");
                startGameButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Server could not be started...");
            }
        });

        startClientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client started...");
            }
            else
            {
                Debug.Log("Client could not be started...");
            }
        });

        startGameButton.onClick.AddListener(() =>
        {
             gameDataController.GetComponent<GameDataController>().StartGame();
             startGameButton.gameObject.SetActive(false);

        });


        startGameButton.gameObject.SetActive(false);


    }
}
