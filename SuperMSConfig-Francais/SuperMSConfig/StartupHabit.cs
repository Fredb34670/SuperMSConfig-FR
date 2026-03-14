using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SuperMSConfig;

public class StartupHabit : BaseHabit
{
	private readonly string appName;

	private readonly string description;

	private readonly Logger logger;

	public override string Name => appName;

	public override string Description => description;

	public override HabitStatus Status { get; set; } = HabitStatus.NotConfigured;

	public StartupHabit(string appName, string description, Logger logger)
	{
		this.appName = appName;
		this.description = description ?? "Aucune description fournie";
		this.logger = logger;
	}

	public override async Task Check()
	{
		await Task.Run(delegate
		{
			try
			{
				bool flag = CheckRegistry("Software\\Microsoft\\Windows\\CurrentVersion\\Run", Registry.CurrentUser);
				bool flag2 = CheckRegistry("Software\\Microsoft\\Windows\\CurrentVersion\\Run", Registry.LocalMachine);
				if (flag || flag2)
				{
					Status = HabitStatus.Bad;
				}
				else
				{
					Status = HabitStatus.Good;
				}
				logger.Log("Vérifié le démarrage pour '" + appName + "' : " + ((Status == HabitStatus.Bad) ? "Trouvé" : "Non trouvé"), (Status == HabitStatus.Bad) ? Color.Red : Color.Green);
			}
			catch (Exception ex)
			{
				logger.Log("Erreur lors de la vérification de '" + appName + "' : " + ex.Message, Color.Red);
			}
		});
	}

	private bool CheckRegistry(string subKey, RegistryKey baseKey)
	{
		try
		{
			using RegistryKey registryKey = baseKey.OpenSubKey(subKey);
			if (registryKey != null)
			{
				string[] valueNames = registryKey.GetValueNames();
				return valueNames.Contains(appName);
			}
		}
		catch (Exception ex)
		{
			logger.Log("Erreur lors de la vérification de '" + appName + "' : " + ex.Message, Color.Red);
		}
		return false;
	}

	public override async Task Fix()
	{
		await Task.Run(delegate
		{
			try
			{
				bool flag = RemoveFromRegistry("Software\\Microsoft\\Windows\\CurrentVersion\\Run", Registry.CurrentUser);
				bool flag2 = RemoveFromRegistry("Software\\Microsoft\\Windows\\CurrentVersion\\Run", Registry.LocalMachine);
				if (flag || flag2)
				{
					Status = HabitStatus.Good;
					logger.Log("Démarrage corrigé pour '" + appName + "' : Supprimé du démarrage", Color.Green);
				}
				else
				{
					logger.Log("Échec de la suppression de l'entrée de démarrage pour '" + appName + "'", Color.Orange);
				}
			}
			catch (Exception ex)
			{
				logger.Log("Erreur lors de Fix pour '" + appName + "' : " + ex.Message, Color.Red);
			}
		});
	}

	private bool RemoveFromRegistry(string subKey, RegistryKey baseKey)
	{
		try
		{
			using RegistryKey registryKey = baseKey.OpenSubKey(subKey, writable: true);
			if (registryKey != null && Status == HabitStatus.Bad)
			{
				string[] valueNames = registryKey.GetValueNames();
				if (valueNames.Contains(appName))
				{
					registryKey.DeleteValue(appName, throwOnMissingValue: false);
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			logger.Log("Erreur lors de la suppression du registre '" + appName + "' : " + ex.Message, Color.Red);
		}
		return false;
	}

	public override Task Revert()
	{
		return Task.Run(delegate
		{
			try
			{
				string text = "Le rétablissement de cette action n'est pas possible. Veuillez activer le démarrage automatique dans les paramètres de l'application.";
				logger.Log(text, Color.Orange);
				MessageBox.Show(text, "Rétablissement impossible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			catch (Exception ex)
			{
				logger.Log("Erreur lors du Revert pour '" + appName + "' : " + ex.Message, Color.Red);
			}
		});
	}

	public override string GetDetails()
	{
		return "Nom de l'app : " + appName + ", Description : " + description;
	}
}
