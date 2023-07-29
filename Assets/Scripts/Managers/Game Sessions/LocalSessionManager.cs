using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class LocalSessionManager : SingletonPersistent<LocalSessionManager>
{
    public static readonly string USER = "User";

    public User User { get; set; }

    private void Start()
    {
        LoadFromPlayPrefs();
    }

    public void Initialize(string username)
    {
        User = new User()
        {
            Username = username,
            Picture = 0,
            CardSleeveIndex = 0,
            DeckCollection = new DeckCollection()
        };
    }

    public void SaveToPlayPrefs()
    {
        PlayerPrefs.SetString(USER, JsonConvert.SerializeObject(User));
    }

    public void LoadFromPlayPrefs()
    {
        User = JsonConvert.DeserializeObject<User>(PlayerPrefs.GetString(USER));
    }

    public Deck GetSelectedDeck()
    {
        return User.DeckCollection.Decks[User.DeckCollection.SelectedDeckIndex];
    }
}


public class User
{
    public string Username { get; set; }
    public int Picture { get; set; }
    public int CardSleeveIndex { get; set; }
    public DeckCollection DeckCollection { get; set; }
}

public class DeckCollection
{
    public int SelectedDeckIndex { get; set; }
    public List<Deck> Decks { get; set; }
}
public class Deck
{   
    public string DeckName { get; set; }
    public List<string> PlayDeck { get; set; }
    public List<string> CharacterDeck { get; set; } 
}