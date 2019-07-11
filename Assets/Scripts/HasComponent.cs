using UnityEngine;

public static class HasComponent
{
    public static bool ObjectHasComponent<T>(this GameObject flag) where T : Component
    {
        return flag.GetComponent<T>() != null;
    }
}
