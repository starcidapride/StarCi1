using TMPro;
using UnityEngine;

public class TableRowManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text lobbyNameText;

    [SerializeField] 
    private TMP_Text lobbyHostText;

    [SerializeField]
    private TMP_Text lobbyDescriptionText;

    [SerializeField]
    private TMP_Text lobbyPlayersText;

    [SerializeField]
    private TMP_Text lobbyStatusText;

    public void Setup(string lobbyName, string lobbyHost, string lobbyDescription, int lobbyPlayers, bool lobbyStatus)
    {
        lobbyNameText.text = lobbyName;
        lobbyHostText.text = lobbyHost;
        lobbyDescriptionText.text = lobbyDescription;
        lobbyPlayersText.text = $"{lobbyPlayers} / 2";
        lobbyStatusText.text = lobbyStatus ? "Available" : "Not Available";
    }
}