using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;

using static EnumUtility;
using static LobbyUtility;

public class NetworkSessionManager : SingletonNetworkPersistent<NetworkSessionManager>
{
    public static Lobby Lobby { get; set; }
    public static bool FinishLoad { get; set; } = false;

    public NetworkVariable<NetworkLobby> NetworkLobby = new();

    public NetworkVariable<NetworkPlayerDatas> NetworkPlayerDatas = new();
    public override void OnNetworkSpawn()
    {
        FinishLoad = true;

        if (IsHost)
        {
            NetworkLobby.Value = new ()
            {
                LobbyName = Lobby.Name,
                LobbyCode = Lobby.LobbyCode,
                LobbyDescription = Lobby.Data[GetDescription(LobbyKey.Description)].Value,
                IsPrivate = Lobby.IsPrivate
            };

            MaintainLobbyHeartbeat(Lobby.Id);

            NetworkPlayerDatas.Value = new()
            {
                PlayerDatas = new()
            };
        }

        var user = LocalSessionManager.Instance.User;

        var playerId = Guid.NewGuid().ToString();

        var playerDatas = NetworkPlayerDatas.Value;

        var playerData = new NetworkPlayerData()
        {
            PlayerSession = new()
            {
                ClientId = NetworkManager.LocalClientId,
                PlayerId = playerId
            },
            Player = new()
            {
                Username = user.Username,
                Picture = user.Picture,
                CardSleeveIndex = user.CardSleeveIndex,
                IsReady = false,

            },
            PlayDeck = new(),
            CharacterDeck = new(),
            Hand = new()
        };

        playerDatas.Add(playerData);

        UpdatePlayerDatasServerRpc(playerDatas);
        
        if (!IsHost)
        {
            JoinLobbyServerRpc(playerId);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerDatasServerRpc(NetworkPlayerDatas datas)
    {
        NetworkPlayerDatas.Value = datas;
    }

    [ServerRpc(RequireOwnership = false)]
    public void JoinLobbyServerRpc(FixedString32Bytes playerId)
    {
    }

    public override void OnNetworkDespawn()
    {
        FinishLoad = false;

        Lobby = null;
    }
}