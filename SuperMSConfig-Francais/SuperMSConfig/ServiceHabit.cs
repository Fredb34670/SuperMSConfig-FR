using System;
using System.Drawing;
using System.ServiceProcess;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SuperMSConfig;

public class ServiceHabit : BaseHabit
{
	private readonly string serviceName;

	private readonly string description;

	private readonly Logger logger;

	private readonly int badValue;

	public override string Name => "Service : " + serviceName;

	public override string Description => description;

	public override HabitStatus Status { get; set; }

	public ServiceHabit(string serviceName, string description, Logger logger, int badValue)
	{
		this.serviceName = serviceName;
		this.description = description;
		this.logger = logger;
		this.badValue = badValue;
		Status = HabitStatus.NotConfigured;
	}

	public override async Task Check()
	{
		try
		{
			logger.Log("Vérification du service '" + serviceName + "'...", Color.Blue);
			ServiceControllerStatus status = CheckServiceStatus();
			if ((badValue == 1 && status == ServiceControllerStatus.Running) || (badValue == 0 && status != ServiceControllerStatus.Running))
			{
				Status = HabitStatus.Bad;
			}
			else
			{
				Status = HabitStatus.Good;
			}
			logger.Log(serviceName + " statut : " + ((Status == HabitStatus.Bad) ? "Mauvais" : "Bon") + ".", (Status == HabitStatus.Bad) ? Color.Red : Color.Green);
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			logger.Log("Erreur lors de la vérification : " + ex2.Message, Color.Red, ex2.StackTrace);
			Status = HabitStatus.NotConfigured;
		}
	}

	private ServiceControllerStatus CheckServiceStatus()
	{
		try
		{
			using ServiceController serviceController = new ServiceController(serviceName);
			serviceController.Refresh();
			return serviceController.Status;
		}
		catch (InvalidOperationException ex)
		{
			logger.Log("Service '" + serviceName + "' non trouvé : " + ex.Message, Color.Red);
			Status = HabitStatus.NotConfigured;
			return ServiceControllerStatus.Stopped;
		}
		catch (Exception ex2)
		{
			logger.Log("Erreur lors de la vérification du statut de '" + serviceName + "' : " + ex2.Message, Color.Red);
			Status = HabitStatus.NotConfigured;
			return ServiceControllerStatus.Stopped;
		}
	}

	public override async Task Fix()
	{
		try
		{
			if (Status != HabitStatus.Bad)
			{
				return;
			}
			logger.Log("Arrêt du service '" + serviceName + "' et configuration en manuel...", Color.Blue);
			await Task.Run(delegate
			{
				using ServiceController serviceController = new ServiceController(serviceName);
				if (serviceController.Status != ServiceControllerStatus.Stopped)
				{
					serviceController.Stop();
					serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
					logger.Log("Service '" + serviceName + "' arrêté avec succès.", Color.Green);
				}
				else
				{
					logger.Log("Le service '" + serviceName + "' est déjà arrêté.", Color.Orange);
				}
				SetServiceStartType(serviceName, 3);
			});
		}
		catch (InvalidOperationException ex)
		{
			InvalidOperationException ex2 = ex;
			logger.Log("Service '" + serviceName + "' non trouvé ou ne peut pas être arrêté : " + ex2.Message, Color.Red);
		}
		catch (Exception ex3)
		{
			Exception ex4 = ex3;
			logger.Log("Erreur lors de l'arrêt du service '" + serviceName + "' : " + ex4.Message, Color.Red);
		}
	}

	public override async Task Revert()
	{
		try
		{
			if (Status != HabitStatus.Good)
			{
				return;
			}
			logger.Log("Démarrage du service '" + serviceName + "' et configuration en automatique...", Color.Blue);
			await Task.Run(delegate
			{
				using ServiceController serviceController = new ServiceController(serviceName);
				if (serviceController.Status != ServiceControllerStatus.Running)
				{
					serviceController.Start();
					serviceController.WaitForStatus(ServiceControllerStatus.Running);
					logger.Log("Service '" + serviceName + "' démarré avec succès.", Color.Green);
				}
				else
				{
					logger.Log("Le service '" + serviceName + "' est déjà en cours d'exécution.", Color.Orange);
				}
				SetServiceStartType(serviceName, 2);
			});
		}
		catch (InvalidOperationException ex)
		{
			InvalidOperationException ex2 = ex;
			logger.Log("Service '" + serviceName + "' non trouvé ou ne peut pas être démarré : " + ex2.Message, Color.Red);
		}
		catch (Exception ex3)
		{
			Exception ex4 = ex3;
			logger.Log("Erreur lors du démarrage du service '" + serviceName + "' : " + ex4.Message, Color.Red);
		}
	}

	private void SetServiceStartType(string serviceName, int startType)
	{
		try
		{
			string name = "SYSTEM\\CurrentControlSet\\Services\\" + serviceName;
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name, writable: true);
			if (registryKey != null)
			{
				registryKey.SetValue("Start", startType, RegistryValueKind.DWord);
				logger.Log("Le type de démarrage du service '" + serviceName + "' est défini sur " + ((startType == 2) ? "Automatique" : "Manuel") + ".", Color.Green);
			}
			else
			{
				logger.Log("Clé de registre non trouvée pour le service '" + serviceName + "'.", Color.Red);
			}
		}
		catch (Exception ex)
		{
			logger.Log("Erreur lors de la configuration du type de démarrage du service : " + ex.Message, Color.Red);
		}
	}

	public override string GetDetails()
	{
		ServiceControllerStatus serviceControllerStatus = CheckServiceStatus();
		return $"Nom du service : {serviceName}, Description : {description}, Statut : {serviceControllerStatus}";
	}
}
