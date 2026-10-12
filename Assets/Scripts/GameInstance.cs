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

    [Header("Prefab Reference")]
    [SerializeField] private GameObject cardPrefab;

    public static GameInstance Instance {get; private set;}

    private void DrawCard()
    {
        if(hand >= maxHand) return;
        CardData cardData = ResourceManager.Instance.getCard("soldier");
        if(cardData != null)
        {
            GameObject newCard = Instantiate(cardPrefab, playerHand.transform);
            CardInstance cardInstance = newCard.GetComponent<CardInstance>();
            if(cardInstance != null) { cardInstance.Initialize(cardData); }
            hand++;
        }
    }

    private void UpdateUI()
    {
        PlayerNormalEnergy.text = playerNormalEnergy.ToString() + "/" + maxPlayerNormalEnergy.ToString();
        PlayerTacticalEnergy.text = playerTacticalEnergy.ToString() + "/" + maxPlayerTacticalEnergy.ToString();
        EnemyNormalEnergy.text = enemyNormalEnergy.ToString() + "/" + maxEnemyNormalEnergy.ToString();
        EnemyTacticalEnergy.text = enemyTacticalEnergy.ToString() + "/" + maxEnemyTacticalEnergy.ToString();
    }

    public bool DeployCard(CardInstance card, CardSlot slot, bool fromHand)
    {
        if(slot.transform.childCount != 0) return false;
        if(card.getCost > playerNormalEnergy) return false;
        playerNormalEnergy -= card.getCost;
        card.transform.SetParent(slot.transform);
        if(fromHand) hand--;
        UpdateUI();
        return true;
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
