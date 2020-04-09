using System;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public static class Debug
{
    public enum LogLevel
    {
        Log,
        Warning,
        Error,
        Exception,
        Max
    }

    public static bool EnableDebugMode = true;

    public static bool isDebugBuild
    {
        get
        {
            return UnityEngine.Debug.isDebugBuild;
        }
    }

    public static bool EnableLogFile = false;

    private static FileStream sm_logFileStream = null;

    private static StreamWriter sm_logFileWriteStream = null;

    private static StringBuilder bd = new StringBuilder();

    private static bool _initialized = false;

    private static bool[] _logLevelEnabled = new bool[4];

    private static string[] _logPre = new string[4]
    {
        "[Log]",
        "[Warning]",
        "[Error]",
        "[Exception]"
    };

    public static int MainThreadID
    {
        get;
        private set;
    }

    public static void Initialize()
    {
        if (!_initialized)
        {
            MainThreadID = Thread.CurrentThread.ManagedThreadId;
            _logLevelEnabled[0] = EnableDebugMode;
            _logLevelEnabled[1] = EnableDebugMode;
            _logLevelEnabled[2] = EnableDebugMode;
            _logLevelEnabled[3] = EnableDebugMode;
            _initialized = true;
            if (EnableLogFile)
            {
                try
                {
                    sm_logFileStream = new FileStream(GetWriteLogFilename(), FileMode.Create, FileAccess.Write, FileShare.Read);
                    sm_logFileWriteStream = new StreamWriter(sm_logFileStream);
                }
                catch (Exception ex)
                {
                    LogError("Debug.sm_logFileStream Initialize Failed Exception:" + ex.Message);
                }
            }
        }
    }

    public static string GetWriteLogFilename()
    {
        return Application.persistentDataPath + "/" + SystemInfo.deviceUniqueIdentifier + "_log.txt";
    }

    public static void Release()
    {
        bd.Remove(0, bd.Length);
        if (sm_logFileWriteStream != null)
        {
            sm_logFileWriteStream.Close();
            sm_logFileWriteStream = null;
        }
        if (sm_logFileStream != null)
        {
            sm_logFileStream.Close();
            sm_logFileStream = null;
        }
    }

    public static void EnableLogLevel(LogLevel logLevel, bool enable)
    {
        Initialize();
        _logLevelEnabled[(int)logLevel] = enable;
    }

    public static bool IsLogLevelEnabled(LogLevel logLevel)
    {
        Initialize();
        return _logLevelEnabled[(int)logLevel];
    }

    public static void Log(params object[] msg)
    {
        bd.Length = 0;
        foreach(var tmp in msg)
        {
            bd.Append(tmp.ToString());
        }
        LogObject(LogLevel.Log, bd.ToString());
    }

    public static void LogFormat(string format, object arg0)
    {
        if (IsLogLevelEnabled(LogLevel.Log))
        {
            LogInternalFormat(LogLevel.Log, format, arg0);
        }
    }

    public static void LogFormat(string format, object arg0, object arg1)
    {
        if (IsLogLevelEnabled(LogLevel.Log))
        {
            LogInternalFormat(LogLevel.Log, format, arg0, arg1);
        }
    }

    public static void LogFormat(string format, object arg0, object arg1, object arg2)
    {
        if (IsLogLevelEnabled(LogLevel.Log))
        {
            LogInternalFormat(LogLevel.Log, format, arg0, arg1, arg2);
        }
    }

    public static void LogFormat(string format, params object[] msg)
    {
        if (IsLogLevelEnabled(LogLevel.Log))
        {
            LogInternalFormat(LogLevel.Log, format, msg);
        }
    }

    public static void LogWarning(object msg)
    {
        LogObject(LogLevel.Warning, msg);
    }

    public static void LogWarning(string format, object arg0)
    {
        if (IsLogLevelEnabled(LogLevel.Warning))
        {
            LogInternalFormat(LogLevel.Warning, format, arg0);
        }
    }

    public static void LogWarning(string format, object arg0, object arg1)
    {
        if (IsLogLevelEnabled(LogLevel.Warning))
        {
            LogInternalFormat(LogLevel.Warning, format, arg0, arg1);
        }
    }

    public static void LogWarning(string format, object arg0, object arg1, object arg2)
    {
        if (IsLogLevelEnabled(LogLevel.Warning))
        {
            LogInternalFormat(LogLevel.Warning, format, arg0, arg1, arg2);
        }
    }

    public static void LogWarningFormat(string format, params object[] msg)
    {
        if (IsLogLevelEnabled(LogLevel.Warning))
        {
            LogInternalFormat(LogLevel.Warning, format, msg);
        }
    }

    public static void LogError(object msg)
    {
        LogObject(LogLevel.Error, msg);
    }

    public static void LogError(string msg1, object msg2)
    {
        LogObject(LogLevel.Error, msg1 + msg2);
    }

    public static void LogErrorFormat(string format, object arg0)
    {
        if (IsLogLevelEnabled(LogLevel.Error))
        {
            LogInternalFormat(LogLevel.Error, format, arg0);
        }
    }

    public static void LogErrorFormat(string format, object arg0, object arg1)
    {
        if (IsLogLevelEnabled(LogLevel.Error))
        {
            LogInternalFormat(LogLevel.Error, format, arg0, arg1);
        }
    }

    public static void LogErrorFormat(string format, object arg0, object arg1, object arg2)
    {
        if (IsLogLevelEnabled(LogLevel.Error))
        {
            LogInternalFormat(LogLevel.Error, format, arg0, arg1, arg2);
        }
    }

    public static void LogErrorFormat(string format, params object[] msg)
    {
        if (IsLogLevelEnabled(LogLevel.Error))
        {
            LogInternalFormat(LogLevel.Error, format, msg);
        }
    }

    public static void LogException(Exception ex)
    {
        LogException(string.Empty, ex);
    }

    public static void LogException(string message, Exception ex)
    {
        if (IsLogLevelEnabled(LogLevel.Exception))
        {
            LogInternalFormat(LogLevel.Exception, $"{message} : {ex.Message} \n {ex.StackTrace}");
        }
    }

    public static void Assert(bool condition, string msg)
    {
        if (!condition)
        {
            throw new AssertException(msg);
        }
    }

    public static void DrawLine(Vector3 pos1, Vector3 pos2, Color color, float duration, bool depthTest)
    {
        UnityEngine.Debug.DrawLine(pos1, pos2, color, duration, depthTest);
    }

    private static void LogInternalFormat(LogLevel logLevel, string format, params object[] msg)
    {
        bd.Remove(0, bd.Length);
        bd.AppendFormat(format, msg);
        DoLog(logLevel, bd.ToString());
    }

    private static void LogObject(LogLevel logLevel, object msg)
    {
        if (IsLogLevelEnabled(logLevel))
        {
            DoLog(logLevel, msg.ToString());
        }
    }

    private static void DoLog(LogLevel logLevel, string msg)
    {
        if (IsLogLevelEnabled(logLevel))
        {
            bd.Remove(0, bd.Length);
            DateTime now = DateTime.Now;
            bd.Append("[");
            bd.Append(ConstStringTable.GetTimeIntern(now.Hour)).Append(":");
            bd.Append(ConstStringTable.GetTimeIntern(now.Minute)).Append(":");
            bd.Append(ConstStringTable.GetTimeIntern(now.Second)).Append("-");
            bd.Append(now.Millisecond).Append("][");
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                bd.Append(Time.frameCount);
            }
            else
            {
                bd.Append("0");
            }
            bd.Append("]").Append(_logPre[(int)logLevel]).Append(msg);
            string text = bd.ToString();
            switch (logLevel)
            {
                case LogLevel.Log:
                    UnityEngine.Debug.Log((object)text);
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning((object)text);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError((object)text);
                    break;
                case LogLevel.Exception:
                    UnityEngine.Debug.LogError((object)text);
                    break;
            }
            if (sm_logFileWriteStream != null)
            {
                sm_logFileWriteStream.WriteLine(text);
                sm_logFileWriteStream.Flush();
            }
        }
    }
}
