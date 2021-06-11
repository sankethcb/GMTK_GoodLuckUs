using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SimpleExtensions
{
    public static void Log(this string obj) => Debug.Log(obj);

    public static void Log(this int obj) => Debug.Log(obj);

    public static void Log(this float obj) => Debug.Log(obj);

    public static void Log(this bool obj) => Debug.Log(obj);
 
}
