
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static LobbyUtility;

public class JoinLobbyByCodeModalManager : Singleton<JoinLobbyByCodeModalManager>
{
    [SerializeField]
    private TMP_InputField lobbyCodeInput;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button submitButton;

    private string lobbyCode;
    private void Start()
    {
        lobbyCode = lobbyCodeInput.text;

        lobbyCodeInput.onEndEdit.AddListener(value => lobbyCode = value) ;

        cancelButton.onClick.AddListener(ModalManager.Instance.CloseNearestModal);

        submitButton.onClick.AddListener(OnSubmitButtonClick);
    }

    private async void OnSubmitButtonClick()
    {
        try
        {
            var lobby = await JoinLobbyByCode(lobbyCode);

            LoadingSceneManager.Instance.JoinRelayAndStartClient(lobby);
        } finally
        {
            ModalManager.Instance.CloseNearestModal();
        }
    }
}