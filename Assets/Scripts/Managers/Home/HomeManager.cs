
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static LobbyUtility;
using static AnimatorUtility;
using static GameObjectUtility;
using static ImageUtility;
using static PathUtility;
using UnityEngine.SocialPlatforms.Impl;

public class HomeManager : Singleton<HomeManager>
{
    [SerializeField]
    private Transform ui;

    [SerializeField]
    private TMP_Text usernameText;

    [SerializeField]
    private Image picture;

    [SerializeField]
    private Button editPictureButton;

    [SerializeField]
    private Button goToCardWarehouseButton;

    [SerializeField]
    private Button goToLobbyRoomButton;

    [SerializeField]
    private Button quickPlayButton;

    [SerializeField]
    private Transform editPictureModal;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ui.gameObject.activeSelf);

        var user = LocalSessionManager.Instance.User;

        SetPicture(user.Picture);

        SetProfile(user.Username);

        goToCardWarehouseButton.onClick.AddListener(() => LoadingSceneManager.Instance.LoadScene(SceneName.CardWarehouse, false ));
        
        goToLobbyRoomButton.onClick.AddListener(() => LoadingSceneManager.Instance.LoadScene(SceneName.LobbyRoom, false ));
        
        quickPlayButton.onClick.AddListener(OnQuickPlayButtonClick);

        editPictureButton.onClick.AddListener(() => ModalManager.Instance.InstantiateModal(editPictureModal));
    }

    public void SetPicture(int _picture)
    {
        picture.sprite = CreateSpriteFromTexture(GetIndexedPicture(_picture));
    }

    public void SetProfile(string username)
    {
        usernameText.text = username;
    }

    public void SetActiveUI(bool value)
    {
        StartCoroutine(SetActiveUICoroutine(value));
    }

    private IEnumerator SetActiveUICoroutine(bool value)
    {   if (value)
        {
            SetInteractability(ui, false);
            ui.gameObject.SetActive(true);

            yield return WaitForAnimationCompletion(ui);

            SetInteractability(ui, true);
        } else
        {
            ui.gameObject.SetActive(false);
        }

    }

    private async void OnQuickPlayButtonClick()
    {
        var lobby = await QuickJoinLobby();

        LoadingSceneManager.Instance.JoinRelayAndStartClient(lobby);
    }

}