using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using static GameObjectUtility;
using static AnimatorUtility;

public class ModalManager : SingletonPersistent<ModalManager>
{
    [SerializeField]
    [Range(0f, 1f)]
    private float a;

    [SerializeField]
    private Image backdrop;

    public static bool modalActive = false;

    private void Hide()
    {
        modalActive = false;

        backdrop.gameObject.SetActive(false);

        LoadingSceneManager.InputBlocked = false;
    }
    private void Show()
    {
        LoadingSceneManager.InputBlocked = true;

        backdrop.gameObject.SetActive(true);

        modalActive = true;

        var color = backdrop.color;
        color.a = a;

        backdrop.color = color;
    }

    private bool HasActived()
    {
        return modalActive;
    }

    public void InstantiateModal(Transform modal)
    {
        StartCoroutine(InstantiateModalCoroutine(modal));
    }

    private IEnumerator InstantiateModalCoroutine(Transform modal)
    {

        if (!HasActived())
        {
            Show();
        }

        var modalInstance = Instantiate(modal, backdrop.transform);

        yield return WaitForAnimationCompletion(modalInstance);

        modalInstance.name = modal.name;

        modalInstance.localPosition = Vector2.zero;

        var closestYoungerSibling = GetClosestSiblingGameObject(modalInstance);
        if (closestYoungerSibling != null)
        {
            SetInteractability(closestYoungerSibling, false);
        }

    }

    public void CloseNearestModal()
    {
        var numSiblings = backdrop.transform.childCount;

        if (numSiblings == 0) return;

        var oldestSibling = backdrop.transform.GetChild(numSiblings - 1);

        var closestYoungerSibling = GetClosestSiblingGameObject(oldestSibling);

        Destroy(oldestSibling.gameObject);

        if (closestYoungerSibling != null)
        {
            SetInteractability(closestYoungerSibling, true);
            return;
        }

        Hide();
    }
}

