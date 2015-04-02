using System;

namespace monarquia
{
	public class MainClass
	{
		public static int Main (string[] args)
		{
			var commands = ManyConsole.ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs (typeof(MainClass));

			return ManyConsole.ConsoleCommandDispatcher.DispatchCommand (commands, args, Console.Out);
		}
	}
}
