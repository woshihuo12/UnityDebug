using UnityEngine;

namespace LuaInterface
{
	public interface ILogger
	{
		void Log(string msg, string stack, LogType type);
	}
}
