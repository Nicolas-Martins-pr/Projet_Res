using Networking;


using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject startMenu;

    [SerializeField]
    private GameObject nameInputMenu;

    [SerializeField]
    private TMP_InputField nameInputField = null;

    [SerializeField]
    private Button continueButton = null;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    // Start is called before the first frame update
    void Start()
    {

        continueButton.interactable = false;
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        //We check if the player already chose a name
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) return;

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        CheckPlayerName();
    }

    //we activate/deactivate the continue button if the player entered a name
    public void CheckPlayerName()
    {
        continueButton.interactable = !string.IsNullOrEmpty(nameInputField.text);
    }

    //We save the player name in the playerprefs
    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }

    public  void onContinueButtonClicked()
    {
        SavePlayerName();
        ClientGameNetPortal.Instance.StartClient();
        
    }

    public void onLaunchServerButtonClicked()
    {
        GameNetPortal.Instance.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void onJoinLobbyButtonClicked()
    {
        startMenu.SetActive(false);
        nameInputMenu.SetActive(true);
    }
    public void onExitButtonClicked()
    {
        Application.Quit();
    }

}
