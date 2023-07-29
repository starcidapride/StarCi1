using System.Collections;
using System.ComponentModel;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

using static EnumUtility;
using static RelayUtility;
using static LobbyUtility;


public class LoadingSceneManager : SingletonPersistent<LoadingSceneManager>
{
    [SerializeField]
    private Transform createUserModal;

    
    public static bool InputBlocked { get; set; } = false;
    private void Start()
    {   
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(OnSceneLoadedCoroutine(scene));
    }

    private IEnumerator OnSceneLoadedCoroutine(Scene scene)
    {   
        yield return new WaitUntil(() => LoadingFadeEffectManager.EndFadeOut);
        
        var user = LocalSessionManager.Instance.User;

        var loadedScene = GetEnumValueByDescription<SceneName>(scene.name);

        switch (loadedScene)
        {
            case SceneName.Home:
                if (user == null)
                {
                    ModalManager.Instance.InstantiateModal(createUserModal);
                } else
                {
                    HomeManager.Instance.SetActiveUI(true);
                }

                break;

            case SceneName.CardWarehouse: 

                break;

            case SceneName.LobbyRoom:
                LobbyRoomManager.Instance.SetActiveUI(true);
                break;
        }
    }

    public void LoadScene(SceneName sceneToLoad, bool isNetworkSessionAction = true)
    {
        StartCoroutine(LoadSceneCoroutine(sceneToLoad, isNetworkSessionAction));
    }

    private IEnumerator LoadSceneCoroutine(SceneName sceneToLoad, bool isNetworkSessionActive = true)
    {
        InputBlocked = true;

        LoadingFadeEffectManager.Instance.FadeIn();

        yield return new WaitUntil(() => LoadingFadeEffectManager.EndFadeIn);

        if (isNetworkSessionActive)
        {
            if (NetworkManager.Singleton.IsServer)
                LoadSceneNetwork(sceneToLoad);
        }
        else
        {
            LoadSceneLocal(sceneToLoad);
        }

        yield return new WaitForSeconds(1f);

        LoadingFadeEffectManager.Instance.FadeOut();

        yield return new WaitUntil(() => LoadingFadeEffectManager.EndFadeOut);

        InputBlocked = false;
    }

    private void LoadSceneLocal(SceneName sceneToLoad)
    {
        SceneManager.LoadScene(GetDescription(sceneToLoad));
    }

    public void LoadSceneNetwork(SceneName sceneToLoad)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(
            GetDescription(sceneToLoad),
            LoadSceneMode.Single);
    }

    public void CreateRelayAndStartHost(string lobbyName, string lobbyDescription, bool lobbyPrivate = false)
    {
        StartCoroutine(CreateRelayAndStartHostCoroutine(lobbyName, lobbyDescription, lobbyPrivate));
    }

    public IEnumerator CreateRelayAndStartHostCoroutine(string lobbyName, string lobbyDescription, bool lobbyPrivate = false)
    {
        LoadingManager.Instance.Show();
        try
        {
            var createRelayTask = CreateRelay();

            yield return new WaitUntil(() => createRelayTask.IsCompleted);

            var relayCode = createRelayTask.Result;

            if (relayCode == null) yield break;

            var createLobbyTask = CreateLobby(lobbyName, 
                LocalSessionManager.Instance.User.Username, 
                lobbyDescription, 
                relayCode,
                lobbyPrivate);

            yield return new WaitUntil(() => createLobbyTask.IsCompleted);

            var lobby = createLobbyTask.Result;

            if (lobby == null) yield break;

            SaveToPlayPrefs(lobby);

            LoadScene(SceneName.WaitingRoom);

            NetworkManager.Singleton.StartHost();

        }
        finally
        {
            LoadingManager.Instance.Hide();
        }
    }

    public void JoinRelayAndStartClient(Lobby lobby)
    {
        StartCoroutine(JoinRelayAndStartClientCoroutine(lobby));
    }

    private IEnumerator JoinRelayAndStartClientCoroutine(Lobby lobby)
    {
        LoadingManager.Instance.Show();
        try
        {
            var joinCode = lobby.Data[GetDescription(LobbyKey.RelayCode)].Value;

            var resultTask = JoinRelay(joinCode);

            yield return new WaitUntil(() => resultTask.IsCompleted);

            if (!resultTask.Result) yield break;

            LoadingFadeEffectManager.Instance.FadeIn();

            yield return new WaitUntil(() => LoadingFadeEffectManager.EndFadeIn);

            NetworkManager.Singleton.StartClient();

            yield return new WaitForSeconds(1f);

            LoadingFadeEffectManager.Instance.FadeOut();
        }
        finally
        {
            LoadingManager.Instance.Hide();
        }
    }
}

public enum SceneName
{
    [Description("Bootstrap")]
    Bootstrap,
    [Description("Home")]
    Home,
    [Description("Card Warehouse")]
    CardWarehouse,
    [Description("Lobby Room")]
    LobbyRoom,
    [Description("Waiting Room")]
    WaitingRoom
}