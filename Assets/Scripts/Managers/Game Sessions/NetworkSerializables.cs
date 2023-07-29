using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
public interface INetworkList<T>
{
    public void Add(T item);

    public void Remove(int index);
}
public enum PlayerRole
{
    You,
    Opponent
}
public struct NetworkPlayerDatas : INetworkSerializable, INetworkList<NetworkPlayerData>
{
    private List<NetworkPlayerData> playerDatas;

    public List<NetworkPlayerData> PlayerDatas
    {
        get { return playerDatas; }
        set { playerDatas = value; }
    }

    public void Add(NetworkPlayerData item)
    {
        playerDatas.Add(item);
    }

    public void Remove(int index)
    {
        playerDatas.RemoveAt(index);
    }

    public NetworkPlayerData GetByGuid(FixedString32Bytes playerId)
    {
        return playerDatas.FirstOrDefault(playerData => playerData.PlayerSession.PlayerId == playerId);
    }
    public NetworkPlayerData GetOtherByGuid(FixedString32Bytes playerId)
    {
        return playerDatas.FirstOrDefault(playerData => playerData.PlayerSession.PlayerId != playerId);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        var playerDatasArray = new NetworkPlayerData[0];

        if (!serializer.IsReader)
        {
            playerDatasArray = playerDatas.ToArray();
        }

        var playerDatasLength = 0;

        if (!serializer.IsReader)
        {
            playerDatasLength = playerDatasArray.Length;
        }

        serializer.SerializeValue(ref playerDatasLength);

        if (serializer.IsReader)
        {
            playerDatasArray = new NetworkPlayerData[playerDatasLength];
        }

        for (int i = 0; i < playerDatasLength; i++)
        {
            serializer.SerializeValue(ref playerDatasArray[i]);
        }

        if (serializer.IsReader)
        {
            playerDatas = playerDatasArray.ToList();
        }
    }
}
public struct PlayerSession : INetworkSerializable
{
    private ulong clientId;
    public FixedString32Bytes playerId;

    public ulong ClientId { get { return clientId; } set { clientId = value; } }
    public FixedString32Bytes PlayerId { get { return playerId; } set { playerId = value; } }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);

        serializer.SerializeValue(ref playerId);
    }
}
public struct NetworkLobby : INetworkSerializable
{
    private FixedString32Bytes lobbyId;

    private FixedString32Bytes lobbyName;

    private FixedString32Bytes lobbyCode;

    private bool isPrivate;

    private FixedString128Bytes lobbyDescription;

    public FixedString32Bytes LobbyId
    {
        get { return lobbyId; }
        set { lobbyId = value; }
    }

    public FixedString32Bytes LobbyName
    {
        get { return lobbyName; }
        set { lobbyName = value; }
    }

    public FixedString32Bytes LobbyCode
    {
        get { return lobbyCode; }
        set { lobbyCode = value; }
    }

    public bool IsPrivate
    {
        get { return isPrivate; }
        set { isPrivate = value; }
    }

    public FixedString128Bytes LobbyDescription
    {
        get { return lobbyDescription; }
        set { lobbyDescription = value; }
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref lobbyId);

        serializer.SerializeValue(ref lobbyName);

        serializer.SerializeValue(ref lobbyCode);

        serializer.SerializeValue(ref isPrivate);

        serializer.SerializeValue(ref lobbyDescription);
    }
}
public struct NetworkPlayerData : INetworkSerializable
{
    private PlayerSession playerSession;

    private NetworkPlayer player;

    private NetworkComponentDeck playDeck;

    private NetworkComponentDeck characterDeck;

    private NetworkHand hand;

    public PlayerSession PlayerSession { get { return playerSession; } set { playerSession = value; } }
    public NetworkPlayer Player { get { return player; } set { player = value; } }
    public NetworkComponentDeck PlayDeck { get { return playDeck; } set { playDeck = value; } }
    public NetworkComponentDeck CharacterDeck { get { return characterDeck; } set { characterDeck = value; } }
    public NetworkHand Hand { get { return hand; } set { hand = value; } }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerSession);

        serializer.SerializeValue(ref player);

        serializer.SerializeValue(ref playDeck);

        serializer.SerializeValue(ref characterDeck);

        serializer.SerializeValue(ref  hand);
    }
}
public struct NetworkPlayer : INetworkSerializable
{
    private FixedString32Bytes username;

    private int picture;

    private int cardSleeveIndex;

    private bool isReady;

    public FixedString32Bytes Username
    {
        get { return username; }
        set { username = value; }
    }

    public int Picture
    {
        get { return picture; }
        set { picture = value; }
    }

    public int CardSleeveIndex
    {
        get { return cardSleeveIndex; }
        set { cardSleeveIndex = value; }
    }

    public bool IsReady
    {
        get { return isReady; }
        set { isReady = value; }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref username);

        serializer.SerializeValue(ref username);

        serializer.SerializeValue(ref cardSleeveIndex);

        serializer.SerializeValue(ref isReady);
    }
}
public struct NetworkComponentDeck : INetworkSerializable
{
    private List<FixedString32Bytes> cards;
    public List<FixedString32Bytes> Cards
    {
        get { return cards; }
        set { cards = value; }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        var cardsArray = new FixedString32Bytes[0];

        if (!serializer.IsReader)
        {
            cardsArray = cards.ToArray();
        }

        var cardsLength = 0;

        if (!serializer.IsReader)
        {
            cardsLength = cardsArray.Length;
        }

        serializer.SerializeValue(ref cardsLength);

        if (serializer.IsReader)
        {
            cardsArray = new FixedString32Bytes[cardsLength];
        }

        for (int i = 0; i < cardsLength; i++)
        {
            serializer.SerializeValue(ref cardsArray[i]);
        }

        for (int i = 0; i < cardsLength; i++)
        {
            serializer.SerializeValue(ref cardsArray[i]);
        }

        if (serializer.IsReader)
        {
            cards = cardsArray.ToList();
        }
    }
}
public struct NetworkHand : INetworkSerializable
{
    private List<FixedString32Bytes> cards;

    public List<FixedString32Bytes> Cards
    {
        get { return cards; }
        set { cards = value; }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        var cardsArray = new FixedString32Bytes[0];

        if (!serializer.IsReader)
        {
            cardsArray = cards.ToArray();
        }

        var cardsLength = 0;

        if (!serializer.IsReader)
        {
            cardsLength = cardsArray.Length;
        }

        serializer.SerializeValue(ref cardsLength);

        if (serializer.IsReader)
        {
            cardsArray = new FixedString32Bytes[cardsLength];
        }

        for (int i = 0; i < cardsLength; i++)
        {
            serializer.SerializeValue(ref cardsArray[i]);
        }

        if (serializer.IsReader)
        {
            cards = cardsArray.ToList();
        }
    }
}
