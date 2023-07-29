using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviour
{
    [SerializeField]
    private Transform ui;

    [SerializeField]
    private TMP_Text lobbyNameText;

    [SerializeField]
    private Button leaveButton;

    [SerializeField]
    private TMP_Text lobbyCodeText;

    [SerializeField]
    private Image yourPicture;

    [SerializeField]
    private Transform yourHost;

    [SerializeField]
    private TMP_Text yourUsername;

    [SerializeField]
    private Transform yourReadyStatus;

    [SerializeField]
    private Transform yourIdleStatus;

    [SerializeField]
    private Image opponentsCard;

    [SerializeField]
    private Transform opponentsContainer;

    [SerializeField]
    private TMP_Text opponentsUsername;

    [SerializeField]
    private Transform opponentsPicture;

    [SerializeField]
    private Transform opponentsHost;

    [SerializeField]
    private Transform opponentsReadyStatus;

    [SerializeField]
    private Transform opponentsIdleStatus;

    [SerializeField]
    private Button kickButton;

    [SerializeField]
    private Button readyButton;

    [SerializeField]
    private Button startButton;

    private IEnumerator Start()
    {
        yield return null;
    }
}
