using DG.Tweening;
using UnityEngine;


public class LoadingFadeEffectManager : SingletonPersistent<LoadingFadeEffectManager>
{
    [SerializeField]
    private CanvasGroup transitionBackgroundCanvasGroup;

    [SerializeField]
    [Range(0f, 5f)]
    private float fadeDuration;

    public static bool EndFadeIn { get; set; } = false;
    public static bool EndFadeOut { get; set; } = false;

    public void FadeIn()
    {
        EndFadeOut = false;

        transitionBackgroundCanvasGroup.gameObject.SetActive(true);

        transitionBackgroundCanvasGroup.DOFade(1f, fadeDuration).OnComplete(
            () =>
            {
                EndFadeIn = true;
            }
        );
    }

    public void FadeOut()
    {
        EndFadeIn = false;

        transitionBackgroundCanvasGroup.DOFade(0f, fadeDuration).OnComplete(
            () =>
            {
                transitionBackgroundCanvasGroup.gameObject.SetActive(false);
                EndFadeOut = true;
            }
        );
    }

    public void FadeAll()
    {
        transitionBackgroundCanvasGroup.DOFade(1f, fadeDuration).OnComplete(
            () =>
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                transitionBackgroundCanvasGroup.DOFade(0f, fadeDuration);
            }
            );
        }
        );
    }
}