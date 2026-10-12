using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [NonSerialized] private Dictionary<string,CardData> cards = new Dictionary<string,CardData>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        CardData[] allCards = Resources.LoadAll<CardData>("Cards");
        foreach(var card in allCards)
        {
            cards.Add(card.name, card);
        }
    }

    public CardData getCard(string name)
    {
        if(cards.TryGetValue(name, out CardData card))
        {
            return card;
        }
        return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
