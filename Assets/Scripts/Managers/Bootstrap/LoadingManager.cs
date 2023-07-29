using UnityEngine;

public class LoadingManager : SingletonPersistent<LoadingManager>
{
    [SerializeField]
    private Transform loadingSpinner;

    public void Hide()
    {
        loadingSpinner.gameObject.SetActive(false);

        LoadingSceneManager.InputBlocked = false;
    }
    public void Show()
    {
        LoadingSceneManager.InputBlocked = true;

        loadingSpinner.gameObject.SetActive(true);
    }
}