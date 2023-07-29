using static AuthenticationUtility;
public class BootstrapManager : Singleton<BootstrapManager>
{
    private async void Start()
    {
        await InitiateAnonymousSignIn();

        LoadingSceneManager.Instance.LoadScene(SceneName.Home, false);
    }
}