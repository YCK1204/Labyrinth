using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static T FindChild<T>(this Transform transform, bool recursive = false, string name = null) where T : Component
    {
        if (recursive == false)
        {
            var childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                if (name == null || child.name == name)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
            return null;
        }

        var childs = transform.GetComponentsInChildren<T>();

        foreach (var child in childs)
        {
            if (name == null || child.name == name)
                return child;
        }
        return null;
    }
    public static T[] FindChilds<T>(this Transform transform, bool recursive = false) where T : Component
    {
        if (recursive == false)
        {
            List<T> results = new List<T>();
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                T component = child.GetComponent<T>();
                if (component != null)
                    results.Add(component);
            }
            return results.ToArray();
        }

        return transform.GetComponentsInChildren<T>();
    }
}