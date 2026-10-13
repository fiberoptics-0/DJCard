using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInstance : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
{
    [SerializeField] private string title;
    [SerializeField] private int defense;
    [SerializeField] private int maxDefense;
    [SerializeField] private int attack;
    [SerializeField] private int cost;
    [SerializeField] private Sprite art;
    [SerializeField] private string description;

    [Header("Component Reference")]
    [SerializeField] private TextMeshProUGUI Attack;
    [SerializeField] private TextMeshProUGUI Defense;
    [SerializeField] private TextMeshProUGUI Cost;
    [SerializeField] private UnityEngine.UI.Image Art;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector3 startingPosition;
    private Transform startingParent;

    [Header("Misc")]
    [SerializeField] private CardProperties.State state;

    public CardProperties.State getState => state;
    public void SetState(CardProperties.State state) { this.state = state; }

    public int getAttack => attack;
    public int getDefense => defense;
    public int getCost => cost;
    public string getTitle => title;
    public int getMaxDefense => maxDefense;
    public Sprite getArt => art;
    public string getDescription => description;
    


    public void Initialize(CardData data)
    {
        this.defense = this.maxDefense = data.getDefense;
        this.attack = data.getAttack;
        this.art = data.getArt;
        this.cost = data.getCost;
        this.title = data.getTitle;
        this.description = data.getDescription;
    }

    private void UpdateUI()
    {
        Attack.text = attack.ToString();
        Defense.text = defense.ToString();
        Cost.text = cost.ToString();
    }

    private void UpdateArt()
    {
        Art.sprite = art;
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        UpdateUI();
        UpdateArt();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startingPosition = rectTransform.localPosition;
        startingParent = transform.parent;
        transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        GameObject raycast = eventData.pointerCurrentRaycast.gameObject;
        if (raycast == null)
        {
            transform.SetParent(startingParent, true);
            return;
        }
        CardSlot slot = raycast.GetComponent<CardSlot>();
        if (slot == null)
        {
            transform.SetParent(startingParent, true);
            return;
        }
        if(!GameInstance.Instance.DeployCard(this,slot,Utils.Side.Player)) transform.SetParent(startingParent, true);
        else GameInstance.Instance.EndTurn();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
