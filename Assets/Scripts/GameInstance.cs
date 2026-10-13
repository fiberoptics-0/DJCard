using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameInstance : MonoBehaviour
{
    [SerializeField] private int playerNormalEnergy;
    [SerializeField] private int playerTacticalEnergy;
    [SerializeField] private int enemyNormalEnergy;
    [SerializeField] private int enemyTacticalEnergy;
    [SerializeField] private int maxPlayerNormalEnergy;
    [SerializeField] private int maxPlayerTacticalEnergy;
    [SerializeField] private int maxEnemyNormalEnergy;
    [SerializeField] private int maxEnemyTacticalEnergy;

    [SerializeField] private int maxHand = 10;
    [SerializeField] private int hand = 0;

    [Header("Component Reference")]
    [SerializeField] private GameObject playerHand;
    [SerializeField] private TextMeshProUGUI PlayerNormalEnergy;
    [SerializeField] private TextMeshProUGUI PlayerTacticalEnergy;
    [SerializeField] private TextMeshProUGUI EnemyNormalEnergy;
    [SerializeField] private TextMeshProUGUI EnemyTacticalEnergy;

    [Header("Card Slots")]
    [SerializeField] private GameObject FieldSlots;
    [SerializeField] private GameObject BattleSlots;
    [SerializeField] private GameObject EnemyFieldSlots;
    [SerializeField] private GameObject EnemyBattleSlots;

    [Header("Prefab Reference")]
    [SerializeField] private GameObject cardPrefab;

    [Header("Misc")]
    [SerializeField] private TurnState turnState = TurnState.Player;

    public static GameInstance Instance {get; private set;}

    private void DrawCard()
    {
        if(hand >= maxHand) return;
        CardData cardData = ResourceManager.Instance.getCard("soldier");
        if(cardData != null)
        {
            GameObject newCard = Instantiate(cardPrefab, playerHand.transform);
            CardInstance cardInstance = newCard.GetComponent<CardInstance>();
            if(cardInstance != null)
            {
                cardInstance.Initialize(cardData);
                cardInstance.SetState(CardProperties.State.Hand);
            }
            hand++;
        }
    }

    public void EndTurn()
    {
        if(enemyNormalEnergy < 1) return;
        CardSlot[] enemyFieldSlots = EnemyFieldSlots.GetComponentsInChildren<CardSlot>();
        foreach(var slot in enemyFieldSlots)
        {
            if(slot.transform.childCount == 0)
            {
                GameObject newCard = Instantiate(cardPrefab, slot.transform);
                CardInstance cardInstance = newCard.GetComponent<CardInstance>();
                if(cardInstance != null)
                {
                    cardInstance.Initialize(ResourceManager.Instance.getCard("alligator"));
                    enemyNormalEnergy--;
                    UpdateUI();
                    return;
                }
            }
        }
    }

    private void UpdateUI()
    {
        PlayerNormalEnergy.text = playerNormalEnergy.ToString() + "/" + maxPlayerNormalEnergy.ToString();
        PlayerTacticalEnergy.text = playerTacticalEnergy.ToString() + "/" + maxPlayerTacticalEnergy.ToString();
        EnemyNormalEnergy.text = enemyNormalEnergy.ToString() + "/" + maxEnemyNormalEnergy.ToString();
        EnemyTacticalEnergy.text = enemyTacticalEnergy.ToString() + "/" + maxEnemyTacticalEnergy.ToString();
    }

    public bool DeployCard(CardInstance card, CardSlot slot, Utils.Side side)
    {
        if(slot.transform.childCount != 0) return false;
        if(slot.getSide != side) return false;
        if(slot.getSlotType == CardSlot.SlotType.Field)
        {
            if(card.getState == CardProperties.State.Created)
            {
                card.transform.SetParent(slot.transform);
                card.SetState(CardProperties.State.Field);
                UpdateUI();
                return true;
            }
            if(card.getState == CardProperties.State.Field && 
                (turnState == TurnState.Player || turnState == TurnState.PlayerAttack))
            {
                card.transform.SetParent(slot.transform);
                card.SetState(CardProperties.State.Field);
                UpdateUI();
                return true;
            }

            if(card.getState == CardProperties.State.Hand && turnState == TurnState.Player)
            {
                if(playerNormalEnergy < card.getCost) return false;
                playerNormalEnergy -= card.getCost;
                card.transform.SetParent(slot.transform);
                hand--;
                card.SetState(CardProperties.State.Field);
                UpdateUI();
                return true;
            }

            if(card.getState == CardProperties.State.Battle && turnState == TurnState.PlayerAttack)
            {
                card.transform.SetParent(slot.transform);
                card.SetState(CardProperties.State.Field);
                UpdateUI();
                return true;
            }

            return false;
        }
        if(slot.getSlotType == CardSlot.SlotType.Battle)
        {
            if(card.getState == CardProperties.State.Created)
            {
                card.transform.SetParent(slot.transform);
                card.SetState(CardProperties.State.Battle);
                UpdateUI();
                return true;
            }
            if((card.getState == CardProperties.State.Field || card.getState == CardProperties.State.Battle) 
                && turnState == TurnState.PlayerAttack)
            {
                card.transform.SetParent(slot.transform);
                card.SetState(CardProperties.State.Battle);
                UpdateUI();
                return true;
            }
        }
        return false;
    }

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        UpdateUI();
    }

    private enum TurnState
    {
        None,
        Player,
        Enemy,
        PlayerAttack,
        EnemyAttack,
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawCard();
    }
}
