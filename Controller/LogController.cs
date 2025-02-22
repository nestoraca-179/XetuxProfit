using System;
using System.IO;

namespace XetuxProfit.Controller
{
	public class LogController
	{
		public static void WriteLog(string type, string msg)
		{
			string file = $"logs\\logs_{DateTime.Now:ddMMyyyy}.txt";

			if (!Directory.Exists("logs"))
				Directory.CreateDirectory("logs");

			using (StreamWriter writer = new StreamWriter(file, true)) // true para append
			{
				if (type != "LINE")
					writer.WriteLine($"{type} [{DateTime.Now:dd/MM/yyyy HH:mm:ss.fff}]: {msg}");
				else
					writer.WriteLine("------------------------------------------------------------------------------------------------------------");
			}
		}
	}
}