using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static EnumUtility;
public class AlertMessageBoxManager : Singleton<AlertMessageBoxManager>
{
    [SerializeField]
    private TMP_Text captionText;

    [SerializeField]
    private TMP_Text messageText;

    [SerializeField]
    private Button button1;

    [SerializeField]
    private Button button2;

    public static AlertCaption Caption { get; set; }

    public static string Message { get; set; }

    public static List<AlertButton> Buttons { get; set; }

    private void Start()
    {
        if (Buttons != null && (Buttons.Count < 0 || Buttons.Count > 2))
        {
            throw new ArgumentException("Invalid number of buttons. The allowed range is from 0 to 2.");
        }
        captionText.text = GetDescription(Caption);

        messageText.text = Message;

        if (Buttons != null)
        {
            if (Buttons.Count == 1)
            {
                button2.gameObject.SetActive(true);

                void OnButton1Click()
                {
                    Buttons[0].HandleOnClick?.Invoke();
                }

                button2.GetComponentInChildren<TMP_Text>().text = Buttons[0].ButtonText;
                button2.onClick.AddListener(OnButton1Click);

            }
            else
            {
                var buttonText1 = Buttons[0].ButtonText;
                var buttonText2 = Buttons[1].ButtonText;

                button1.gameObject.SetActive(true);
                button2.gameObject.SetActive(true);

                void OnButton1Click()
                {
                    Buttons[0].HandleOnClick?.Invoke();
                }

                void OnButton2Click()
                {
                    Buttons[1].HandleOnClick?.Invoke();
                }

                button1.onClick.AddListener(OnButton1Click);
                button2.onClick.AddListener(OnButton2Click);

                button1.GetComponentInChildren<TMP_Text>().text = buttonText1;
                button2.GetComponentInChildren<TMP_Text>().text = buttonText2;
            }
        }
    }
}
