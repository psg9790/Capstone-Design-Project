// 배포시에 Player-> other settings-> Script Compilation-> DEBUG_MODE 삭제
// 모든 UnityEngine.Debug 관련 코드 컴파일 제외 처리 가능 (빠르다)

using System;
using UnityEngine;

public static class Debug
{
    public static bool isDebugBuild
    {
        get { return UnityEngine.Debug.isDebugBuild; }
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Log(object message)
    {
#if (DEBUG_MODE)
        UnityEngine.Debug.Log(message);
#endif
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Log(object message, UnityEngine.Object context)
    {
#if (DEBUG_MODE)
        UnityEngine.Debug.Log(message, context);
#endif
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void LogFormat(string format, params object[] args)
    {
#if (DEBUG_MODE)
        UnityEngine.Debug.LogFormat(format, args);
#endif
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void LogError(object message)
    {
#if (DEBUG_MODE)
        UnityEngine.Debug.LogError(message);
#endif
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void LogError(object message, UnityEngine.Object context)
    {
#if (DEBUG_MODE)
        UnityEngine.Debug.LogError(message, context);
#endif
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void LogWarning(object message)
    {
#if (DEBUG_MODE)
        UnityEngine.Debug.LogWarning(message.ToString());
#endif
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void LogWarning(object message, UnityEngine.Object context)
    {
#if (DEBUG_MODE)
        UnityEngine.Debug.LogWarning(message.ToString(), context);
#endif
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
#if (DEBUG_MODE)
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
#endif
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
#if (DEBUG_MODE)
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
#endif
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Assert(bool condition)
    {
#if (DEBUG_MODE)
        if (!condition) throw new Exception();
#endif
    }

    public static void Break()
    {
        throw new NotImplementedException();
    }
}