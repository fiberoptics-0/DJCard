using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{

    [SerializeField] private string title;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int cost;
    [SerializeField] private Sprite art;
    [TextArea, SerializeField] private string description;

    public string getTitle => title;
    public int getAttack => attack;
    public int getDefense => defense;
    public int getCost => cost;
    public Sprite getArt => art;
    public string getDescription => description;
}