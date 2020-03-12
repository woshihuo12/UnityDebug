namespace LuaInterface
{
	public interface ICmd
	{
		void Log(string msg);

		void LogWarning(string msg);

		void LogError(string msg);

		void Show(bool flag);
	}
}
