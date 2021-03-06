﻿public class DebugX
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object msg)
    {
        UnityEngine.Debug.Log(msg);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir, UnityEngine.Color color, float duration)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir, [UnityEngine.Internal.DefaultValue("Color.white")] UnityEngine.Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir)
    {
        UnityEngine.Debug.DrawRay(start, dir);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir, UnityEngine.Color color)
    {
        UnityEngine.Debug.DrawRay(start, dir, color);
    }
}     