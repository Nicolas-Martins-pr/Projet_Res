using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;

public class UIManager : NetworkBehaviour
{



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
