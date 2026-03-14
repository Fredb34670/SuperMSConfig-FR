using Microsoft.Win32;
using SuperMSConfig;

public class PrivacyHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Confidentialité";

	public PrivacyHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Windows\\DataCollection", "AllowTelemetry", 2, 1, "Activer la collecte de données de télémétrie de base pour les éditions Famille/Pro", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Windows\\DataCollection", "AllowTelemetry", 1, 0, "Désactiver la collecte de données de télémétrie pour une confidentialité de niveau entreprise", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Privacy", "ActivityHistoryEnabled", 1, 0, "Désactiver l'historique d'activité (empêche Windows de suivre et stocker votre activité)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\LocationAndSensors", "LocationEnabled", 1, 0, "Désactiver le suivi de l'emplacement (empêche Windows d'accéder à votre position)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\BackgroundAccessApplications", "Disabled", 1, 0, "Désactiver les applications en arrière-plan (empêche les applications de s'exécuter en arrière-plan)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\WindowsStore\\WindowsUpdate", "AutoDownload", 1, 0, "Désactiver les mises à jour automatiques des applications (empêche les applications de se mettre à jour automatiquement)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Feedback", "EnableFeedback", 0, 1, "Désactiver l'envoi de commentaires et de rapports d'erreurs (empêche l'envoi de commentaires à Microsoft)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\camera", "Value", "Allow", "Deny", "Désactiver l'accès à la caméra (empêche les applications d'utiliser votre caméra)", "STRING", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\microphone", "Value", "Allow", "Deny", "Désactiver l'accès au microphone (empêche les applications d'utiliser votre microphone)", "STRING", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\location", "Value", "Allow", "Deny", "Désactiver l'accès à l'emplacement (empêche les applications d'utiliser votre position)", "STRING", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\contacts", "Value", "Allow", "Deny", "Désactiver l'accès aux contacts (empêche les applications d'accéder à vos contacts)", "STRING", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\calendar", "Value", "Allow", "Deny", "Désactiver l'accès au calendrier (empêche les applications d'accéder à votre calendrier)", "STRING", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\callHistory", "Value", "Allow", "Deny", "Désactiver l'accès à l'historique des appels (empêche les applications d'accéder à votre historique d'appels)", "STRING", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\email", "Value", "Allow", "Deny", "Désactiver l'accès aux e-mails (empêche les applications d'accéder à vos e-mails)", "STRING", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\messaging", "Value", "Allow", "Deny", "Désactiver l'accès à la messagerie (empêche les applications d'accéder à vos messages)", "STRING", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\radios", "Value", "Allow", "Deny", "Désactiver l'accès radio (empêche les applications d'accéder à vos radios)", "STRING", logger));
	}
}
