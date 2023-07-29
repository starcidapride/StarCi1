using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

using static ImageUtility;
public class CellPictureManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image image;

    private int index;

    public void SetImage(int _index, Texture2D _image)
    {
        index = _index;

        image.sprite = CreateSpriteFromTexture(_image);
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        try
        {
            HomeManager.Instance.SetPicture(index);

            LocalSessionManager.Instance.User.Picture = index;

            LocalSessionManager.Instance.SaveToPlayPrefs();
        } finally
        {
            ModalManager.Instance.CloseNearestModal();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            image.color = ConvertHexToColor(CustomColor.LightGray);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            image.color = Color.white;
    }
}