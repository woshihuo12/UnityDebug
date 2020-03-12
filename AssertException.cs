using System;

public class AssertException : Exception
{
	public AssertException()
	{
	}

	public AssertException(string inMsg)
		: base(inMsg)
	{
	}
}
