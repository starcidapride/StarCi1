
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using System.Collections;

using static GameObjectUtility;
using static LobbyUtility;
using static EnumUtility;
using static AnimatorUtility;

public class LobbyRoomManager : Singleton<LobbyRoomManager>
{
    [SerializeField]
    private Transform ui;

    [SerializeField]
    private TMP_InputField searchTextInput;

    [SerializeField]
    private Button searchButton;

    [SerializeField]
    private Button refreshButton;

    [SerializeField]
    private Button joinLobbyByCodeButton;

    [SerializeField]
    private Button createLobbyButton;

    [SerializeField]
    private Button joinLobbyButton;

    [SerializeField]
    private Transform tableBody;

    [SerializeField]
    private Transform createLobbyModal;

    [SerializeField]
    private Transform joinLobbyByCodeModal;

    [SerializeField]
    private Transform tableRow;

    private string searchValue;

    private List<Lobby> lobbies;
    public static Lobby SelectedLobby { get; set; }

    private void Start()
    {
        searchValue = searchTextInput.text;

        searchTextInput.onEndEdit.AddListener(value => searchValue = value);

        searchButton.onClick.AddListener(OnSearchButtonClick);

        refreshButton.onClick.AddListener(OnRefreshButtonClick);

        createLobbyButton.onClick.AddListener(() => ModalManager.Instance.InstantiateModal(createLobbyModal));
        
        joinLobbyByCodeButton.onClick.AddListener(() => ModalManager.Instance.InstantiateModal(joinLobbyByCodeModal));

        joinLobbyButton.onClick.AddListener(() => LoadingSceneManager.Instance.JoinRelayAndStartClient(SelectedLobby));
    }
    public void SetActiveUI(bool value)
    {
        ui.gameObject.SetActive(value);
    }
    
    public void ShowLobbies()
    {
        DestroyAllChildGameObjects(tableBody);
        foreach (var lobby in lobbies)
        {
            var user = LocalSessionManager.Instance.User;

            var tableRowInstance = Instantiate(tableRow, tableBody);

            tableRowInstance.GetComponent<TableRowManager>().Setup(
                lobby.Name,
                user.Username,
                lobby.Data[GetDescription(LobbyKey.Description)].Value,
                lobby.Players.Count,
                bool.Parse(lobby.Data[GetDescription(LobbyKey.Status)].Value)
                );
        }
    }

    public async void OnRefreshButtonClick()
    {
        lobbies = await GetLobbies();

        ShowLobbies();
    }

    private IEnumerator SetActiveUICoroutine(bool value)
    {
        if (value)
        {
            SetInteractability(ui, false);
            ui.gameObject.SetActive(true);

            yield return WaitForAnimationCompletion(ui);

            SetInteractability(ui, true);
        }
        else
        {
            ui.gameObject.SetActive(false);
        }

    }

    public async void OnSearchButtonClick()
    {
        lobbies = await GetLobbies();

        lobbies = lobbies.Where(lobby => {
            var lobbyNameFilter = lobby.Name.ContainsInsensitive(searchValue);
            
            var lobbyHostFilter = lobby.Data[GetDescription(LobbyKey.Host)].Value.ContainsInsensitive(searchValue);

            var lobbyDescriptionFilter = lobby.Data[GetDescription(LobbyKey.Description)].Value.ContainsInsensitive(searchValue);

            return lobbyNameFilter || lobbyHostFilter || lobbyDescriptionFilter ;
            }).ToList();

        ShowLobbies();
    }
}