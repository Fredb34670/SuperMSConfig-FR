using System;
using System.Security.Principal;

namespace SuperMSConfig.Helpers;

internal static class Utils
{
	public static bool IsRunningAsAdmin()
	{
		try
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
			bool flag = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
			if (!flag)
			{
			}
			return flag;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
