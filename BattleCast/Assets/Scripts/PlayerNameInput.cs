using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Class needed to manage the player name input fields and their save in the Player Prefs
public class PlayerNameInput : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField nameInputField = null;


    [SerializeField]
    private Button continueButton = null;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    // Start is called before the first frame update
    void Start()
    {
        SetUpInputField(); 
    }

    private void SetUpInputField()
    {
        //We check if the player already chose a name
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) return;

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    //we activate/deactivate the continue button if the player entered a name
    private void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    //We save the player name in the playerprefs
    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }

}
