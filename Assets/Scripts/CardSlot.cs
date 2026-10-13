using Unity.VisualScripting;
using UnityEngine;

public class CardSlot : MonoBehaviour
{

    [SerializeField] private SlotType slotType;
    [SerializeField] private Utils.Side side;

    public SlotType getSlotType => slotType;
    public Utils.Side getSide => side;

    public enum SlotType
    {
        None,
        Field,
        Battle
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
