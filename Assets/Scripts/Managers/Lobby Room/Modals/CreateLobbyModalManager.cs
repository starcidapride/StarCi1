using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyModalManager : Singleton<CreateLobbyModalManager>
{
    [SerializeField]
    private TMP_InputField lobbyNameTextInput;

    [SerializeField]
    private Toggle lobbyPrivateToggleInput;

    [SerializeField] 
    private TMP_InputField lobbyDescriptionTextInput;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button submitButton;

    private string lobbyName, lobbyDescription;
    private bool lobbyPrivate;

    private void Start()
    {
        lobbyName = lobbyNameTextInput.text;
        lobbyDescription = lobbyDescriptionTextInput.text;
        lobbyPrivate = lobbyPrivateToggleInput.isOn;

        lobbyNameTextInput.onEndEdit.AddListener(value => lobbyName = value);
        lobbyDescriptionTextInput.onEndEdit.AddListener(value => lobbyDescription = value);
        lobbyPrivateToggleInput.onValueChanged.AddListener(value => lobbyPrivate = value);

        cancelButton.onClick.AddListener(() => ModalManager.Instance.CloseNearestModal());

        submitButton.onClick.AddListener(OnSubmitButtonClick);
    }

    private void OnSubmitButtonClick()
    {
        try 
        {
            LoadingSceneManager.Instance.CreateRelayAndStartHost(lobbyName, lobbyDescription, lobbyPrivate);
        }
        finally
        {
            ModalManager.Instance.CloseNearestModal();
        }
    }
}