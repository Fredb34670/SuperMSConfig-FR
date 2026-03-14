using System;
using Microsoft.Win32;
using SuperMSConfig;

public class AdsHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Publicité et annonces";

	public AdsHabits(Logger logger)
	{
		this.logger = logger;
		if (logger == null)
		{
			Console.WriteLine("Warning: Logger is null. Logging will be disabled.");
		}
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ShowSyncProviderNotifications", 1, 0, "Désactiver les publicités dans l'Explorateur de fichiers", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\UserProfileEngagement", "ScoobeSystemSettingEnabled", 1, 0, "Désactiver les publicités de fin de configuration", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "RotatingLockScreenOverlayEnabled", 1, 0, "Désactiver les publicités sur l'écran de verrouillage", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-338387Enabled", 1, 0, "Désactiver les publicités sur l'écran de verrouillage", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\AdvertisingInfo", "Enabled", 1, 0, "Désactiver les publicités personnalisées", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-338393Enabled", 1, 0, "Désactiver les publicités dans les Paramètres", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-353694Enabled", 1, 0, "Désactiver les publicités dans les Paramètres", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-353696Enabled", 1, 0, "Désactiver les publicités dans les Paramètres", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "Start_IrisRecommendations", 1, 0, "Désactiver les publicités dans le menu Démarrer", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-338389Enabled", 1, 0, "Désactiver les conseils et suggestions de Windows", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 1, 0, "Désactiver les expériences personnalisées", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-310093Enabled", 1, 0, "Désactiver les publicités de l'expérience de bienvenue", "DWORD", logger));
	}
}
