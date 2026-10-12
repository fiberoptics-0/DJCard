using UnityEngine;

public class CardProperties
{
    public enum Type
    {
        None,
        Minion,
        Spell
    }

    public enum State
    {
        None,
        Hand,
        Field,
        Battle
    }
}
