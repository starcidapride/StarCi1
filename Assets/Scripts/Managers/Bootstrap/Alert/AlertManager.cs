using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

using static GameObjectUtility;
public class AlertManager : SingletonPersistent<AlertManager>
{
    [SerializeField]
    [Range(0f, 1f)]
    private float a;

    [SerializeField]
    private Image backdrop;

    [SerializeField]
    private Transform messageBox;

    public void Hide()
    {
        DestroyAllChildGameObjects(backdrop.transform);

        backdrop.gameObject.SetActive(false);

        LoadingSceneManager.InputBlocked = false;
    }

    public void Show(AlertCaption caption, string message, List<AlertButton> buttons)
    {
        backdrop.gameObject.SetActive(true);

        var color = backdrop.color;
        color.a = a;

        backdrop.color = color;

        LoadingSceneManager.InputBlocked = true;

        DestroyAllChildGameObjects(backdrop.transform);

        AlertMessageBoxManager.Caption = caption;

        AlertMessageBoxManager.Message = message;

        AlertMessageBoxManager.Buttons = buttons;

        Instantiate(messageBox, backdrop.transform);
    }

}

public delegate void OnClickDelegate();
public class AlertButton
{
    public string ButtonText { get; set; }

    public OnClickDelegate HandleOnClick;

}

public enum AlertCaption
{
    [Description("Success")]
    Success,
    [Description("Failure")]
    Failure,
    [Description("Confirmation")]
    Confirmation
}

public enum ButtonName
{
    [Description("Cancel")]
    Cancel
}