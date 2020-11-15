using System;
using UnityEngine;
using WhiteBird;

namespace LuaInterface
{
	public static class Debugger
	{
		public static bool useLog;

		public static string threadStack;

		public static ILogger logger;

		private static CString sb;

		static Debugger()
		{
			useLog = true;
			threadStack = string.Empty;
			logger = null;
			sb = new CString(256);
			for (int i = 24; i < 70; i++)
			{
				StringPool.PreAlloc(i, 2);
			}
		}

		private static string GetLogFormat(string str)
		{
			DateTime now = DateTime.Now;
			sb.Clear();
			sb.Append(ConstStringTable.GetTimeIntern(now.Hour)).Append(":").Append(ConstStringTable.GetTimeIntern(now.Minute))
				.Append(":")
				.Append(ConstStringTable.GetTimeIntern(now.Second))
				.Append(".")
				.Append(now.Millisecond)
				.Append("-")
				.Append(Time.frameCount % 999)
				.Append(": ")
				.Append(str);
			string text = StringPool.Alloc(sb.Length);
			sb.CopyToString(text);
			return text;
		}

		public static void Log(string str)
		{
			str = GetLogFormat(str);
			if (useLog)
			{
				Debug.Log(str);
			}
			else if (logger != null)
			{
				logger.Log(str, string.Empty, (LogType)3);
			}
			StringPool.Collect(str);
		}

		public static void Log(object message)
		{
			Log(message.ToString());
		}

		public static void Log(string str, object arg0)
		{
			Log(string.Format(str, arg0));
		}

		public static void Log(string str, object arg0, object arg1)
		{
			Log(string.Format(str, arg0, arg1));
		}

		public static void Log(string str, object arg0, object arg1, object arg2)
		{
			Log(string.Format(str, arg0, arg1, arg2));
		}

		public static void Log(string str, params object[] param)
		{
			Log(string.Format(str, param));
		}

		public static void LogWarning(string str)
		{
			str = GetLogFormat(str);
			if (useLog)
			{
				Debug.LogWarning(str);
			}
			else if (logger != null)
			{
				string stack = StackTraceUtility.ExtractStackTrace();
				logger.Log(str, stack, (LogType)2);
			}
			StringPool.Collect(str);
		}

		public static void LogWarning(object message)
		{
			LogWarning(message.ToString());
		}

		public static void LogWarning(string str, object arg0)
		{
			LogWarning(string.Format(str, arg0));
		}

		public static void LogWarning(string str, object arg0, object arg1)
		{
			LogWarning(string.Format(str, arg0, arg1));
		}

		public static void LogWarning(string str, object arg0, object arg1, object arg2)
		{
			LogWarning(string.Format(str, arg0, arg1, arg2));
		}

		public static void LogWarning(string str, params object[] param)
		{
			LogWarning(string.Format(str, param));
		}

		public static void LogError(string str)
		{
			str = GetLogFormat(str);
			if (useLog)
			{
				Debug.LogError(str);
			}
			else if (logger != null)
			{
				string stack = StackTraceUtility.ExtractStackTrace();
				logger.Log(str, stack, (LogType)0);
			}
			StringPool.Collect(str);
		}

		public static void LogError(object message)
		{
			LogError(message.ToString());
		}

		public static void LogError(string str, object arg0)
		{
			LogError(string.Format(str, arg0));
		}

		public static void LogError(string str, object arg0, object arg1)
		{
			LogError(string.Format(str, arg0, arg1));
		}

		public static void LogError(string str, object arg0, object arg1, object arg2)
		{
			LogError(string.Format(str, arg0, arg1, arg2));
		}

		public static void LogError(string str, params object[] param)
		{
			LogError(string.Format(str, param));
		}

		public static void LogException(Exception e)
		{
			threadStack = e.StackTrace;
			string logFormat = GetLogFormat(e.Message);
			if (useLog)
			{
				Debug.LogError(logFormat);
			}
			else if (logger != null)
			{
				logger.Log(logFormat, threadStack, (LogType)4);
			}
			StringPool.Collect(logFormat);
		}

		public static void LogException(string str, Exception e)
		{
			threadStack = e.StackTrace;
			str = GetLogFormat(str + e.Message);
			if (useLog)
			{
				Debug.LogError(str);
			}
			else if (logger != null)
			{
				logger.Log(str, threadStack, (LogType)4);
			}
			StringPool.Collect(str);
		}
	}
}
