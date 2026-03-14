using System;
using System.Windows.Forms;

namespace SuperMSConfig;

internal static class Program
{
	internal static string GetCurrentVersionTostring()
	{
		return new Version(Application.ProductVersion).ToString(3);
	}

	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new MainForm());
	}
}
