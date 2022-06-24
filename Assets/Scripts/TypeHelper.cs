using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TypeHelper
{
    public static bool IsDefault<T>(this T val)
    {
        return EqualityComparer<T>.Default.Equals(val, default(T));
    }
}
