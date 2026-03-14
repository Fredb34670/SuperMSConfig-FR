using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMSConfig;

namespace Updater;

internal class Updater
{
	private LinkLabel _linkUpdates;

	public Updater(LinkLabel linkUpdates)
	{
		_linkUpdates = linkUpdates;
	}

	public async Task CheckForAppUpdatesAsync()
	{
		_linkUpdates.Text = "Checking for updates...";
		_linkUpdates.LinkColor = Color.Black;
		_linkUpdates.Visible = true;
		await Task.Delay(4000);
		if (IsInet())
		{
			try
			{
				string[] readVersion = (await new WebClient().DownloadStringTaskAsync("https://raw.githubusercontent.com/builtbybel/SuperMSConfig/refs/heads/main/SuperMSConfig/Properties/AssemblyInfo.cs")).Split('\n');
				IEnumerable<string> infoVersion = readVersion.Where((string t) => t.Contains("[assembly: AssemblyFileVersion"));
				string latestVersion = "";
				foreach (string item in infoVersion)
				{
					latestVersion = item.Substring(item.IndexOf('(') + 2, item.LastIndexOf(')') - item.IndexOf('(') - 3);
				}
				if (latestVersion == Program.GetCurrentVersionTostring())
				{
					_linkUpdates.Text = "No new updates available.";
					_linkUpdates.LinkBehavior = LinkBehavior.NeverUnderline;
					await Task.Delay(3000);
					_linkUpdates.Visible = false;
				}
				else
				{
					_linkUpdates.Text = "New update (v" + latestVersion + ") available! Click to download.";
					_linkUpdates.LinkBehavior = LinkBehavior.AlwaysUnderline;
					_linkUpdates.LinkColor = Color.DarkOliveGreen;
					_linkUpdates.Click += delegate
					{
						Process.Start("https://github.com/builtbybel/SuperMSConfig/releases");
					};
					await Task.Delay(10000);
					_linkUpdates.Visible = false;
				}
				return;
			}
			catch (Exception)
			{
				_linkUpdates.Text = "⚠Error checking for updates.";
				_linkUpdates.LinkColor = Color.Red;
				_linkUpdates.LinkBehavior = LinkBehavior.NeverUnderline;
				return;
			}
		}
		_linkUpdates.Text = "No internet connection. Update check failed.";
		_linkUpdates.LinkColor = Color.Red;
		_linkUpdates.LinkBehavior = LinkBehavior.NeverUnderline;
		await Task.Delay(3000);
		_linkUpdates.Visible = false;
	}

	public static bool IsInet()
	{
		try
		{
			using WebClient webClient = new WebClient();
			using (webClient.OpenRead("http://clients3.google.com/generate_204"))
			{
				return true;
			}
		}
		catch
		{
			return false;
		}
	}
}
