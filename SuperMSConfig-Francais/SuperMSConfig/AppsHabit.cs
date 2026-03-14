using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMSConfig;

public class AppsHabit : BaseHabit
{
	private readonly Logger logger;

	public override string Name { get; }

	public override string Description { get; }

	public override HabitStatus Status { get; set; }

	public AppsHabit(string appName, string description, Logger logger)
	{
		Name = appName;
		Description = description;
		this.logger = logger;
		Status = HabitStatus.NotConfigured;
	}

	public override async Task Check()
	{
		try
		{
			logger.Log("Recherche de " + Name + "...", Color.Blue);
			Status = ((await IsAppInstalled(Name)) ? HabitStatus.Bad : HabitStatus.Good);
			logger.Log((Status == HabitStatus.Bad) ? (Name + " est installée.") : (Name + " n'est pas installée."), (Status == HabitStatus.Bad) ? Color.Red : Color.Green);
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			logger.Log("Erreur lors de la vérification : " + ex2.Message, Color.Red, ex2.StackTrace);
		}
	}

	public override async Task Fix()
	{
		try
		{
			if (Status == HabitStatus.Bad)
			{
				logger.Log("Désinstallation de " + Name + "...", Color.Blue);
				await UninstallApp(Name);
				Status = HabitStatus.Good;
				logger.Log(Name + " désinstallée avec succès.", Color.Green);
			}
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			logger.Log("Erreur lors de la correction : " + ex2.Message, Color.Red, ex2.StackTrace);
		}
	}

	public override async Task Revert()
	{
		try
		{
			string message = "La réinstallation de " + Name + " n'est pas possible. Veuillez utiliser le Microsoft Store";
			MessageBox.Show(message, "Rétablissement impossible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			logger.Log("Erreur lors du Revert : " + ex2.Message, Color.Red, ex2.StackTrace);
		}
	}

	public override string GetDetails()
	{
		return $"Nom de l'application : {Name}, Description : {Description}, Statut : {Status}";
	}

	private async Task<bool> IsAppInstalled(string appName)
	{
		PowerShell powerShell = PowerShell.Create();
		try
		{
			powerShell.AddScript("Get-AppxPackage -Name *" + appName + "*");
			Collection<PSObject> results = await Task.Run(() => powerShell.Invoke());
			if (powerShell.Streams.Error.Count > 0)
			{
				foreach (ErrorRecord error in powerShell.Streams.Error)
				{
					logger.Log($"Erreur PowerShell : {error}", Color.Red);
				}
				return false;
			}
			return results.Count > 0;
		}
		finally
		{
			if (powerShell != null)
			{
				((IDisposable)powerShell).Dispose();
			}
		}
	}

	private async Task UninstallApp(string appName)
	{
		try
		{
			string uninstallCommand = "Get-AppxPackage *" + appName + "* | Remove-AppxPackage";
			PowerShell powerShell = PowerShell.Create();
			try
			{
				powerShell.AddScript(uninstallCommand);
				await Task.Run(() => powerShell.Invoke());
			}
			finally
			{
				if (powerShell != null)
				{
					((IDisposable)powerShell).Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			logger.Log("Erreur lors de la désinstallation de l'application " + appName + " : " + ex2.Message, Color.Red, ex2.StackTrace);
		}
	}
}
