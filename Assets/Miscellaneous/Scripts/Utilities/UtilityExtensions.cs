using UnityEngine;
using System.Collections.Generic;

public static class UtilityExtensions
{
    public static Color PingPong(Color startColor, Color endColor, float duration)
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;

        var color = Color.Lerp(startColor, endColor, lerp);
        return color;
    }

    public static T[] GetComponentsOnlyInChildren<T>(this Transform transform) where T : class
    {
        List<T> group = new List<T>();

        //collect only if its an interface or a Component
        if (typeof(T).IsInterface
         || typeof(T).IsSubclassOf(typeof(Component))
         || typeof(T) == typeof(Component))
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<T>() != null)
                {
                    group.Add(child.GetComponent<T>());
                }
            }
        }

        return group.ToArray();
    }


    public static T[] GetComponentsOnlyInChildrenAndDescendants<T>(this Transform transform) where T : class
    {
        List<T> group = new List<T>();

        //collect only if its an interface or a Component
        if (typeof(T).IsInterface
         || typeof(T).IsSubclassOf(typeof(Component))
         || typeof(T) == typeof(Component))
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponentInChildren<T>() != null)
                {
                    group.AddRange(child.GetComponentsInChildren<T>());
                }
            }
        }

        return group.ToArray();
    }

    public static void ThrowErrorIfNull(this Object obj)
    {
        if (obj == null)
        {
            Debug.LogError("No " + obj.GetType() + " assigned in inspector/initialized.");
        }
    }
}
