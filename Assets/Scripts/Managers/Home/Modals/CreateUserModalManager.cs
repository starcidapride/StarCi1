
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateUserModalManager : Singleton<CreateUserModalManager>
{
    [SerializeField]
    private TMP_InputField usernameTextInput;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button submitButton;

    private string username;

    private void Start()
    {
        username = usernameTextInput.text;
        
        usernameTextInput.onEndEdit.AddListener(value => username = value);

      
        cancelButton.onClick.AddListener(ModalManager.Instance.CloseNearestModal);

        submitButton.onClick.AddListener(OnSubmitButtonClick);
    }   

 
    private void OnSubmitButtonClick()
    {
        try
        {
            LocalSessionManager.Instance.Initialize(username);

            LocalSessionManager.Instance.SaveToPlayPrefs();

            HomeManager.Instance.SetActiveUI(true);
        }
        finally
        {
            ModalManager.Instance.CloseNearestModal();
        }
    }
}

