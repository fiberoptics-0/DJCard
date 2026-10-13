using Unity.VisualScripting;
using UnityEngine;

public static class Utils
{
    public static T GetComponentInChildrenByName<T>(this Transform parent, string name) where T : Component
    {
        foreach(T comp in parent.GetComponentsInChildren<T>())
        {
            if(comp.gameObject.name == name) { return comp; }
        }
        return null;
    }
    
    public enum Side
    {
        None,
        Player,
        Enemy
    }
    
}
