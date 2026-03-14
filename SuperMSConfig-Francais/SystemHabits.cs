using Microsoft.Win32;
using SuperMSConfig;

public class SystemHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Système";

	public SystemHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "SYSTEM\\CurrentControlSet\\Control", "WaitToKillServiceTimeout", "5000", "2000", "Accélérer le processus d'arrêt", "STRING", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Microsoft\\Windows\\CurrentVersion\\PerformanceMonitoring", "EnablePerformanceMonitoring", 0, 1, "Activer la surveillance des performances", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "LaunchFolderWindowsInNewProcess", 0, 1, "Lancer les dossiers dans un nouveau processus", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Windows\\System", "EnableSuperFetch", 0, 1, "Activer SuperFetch (améliore les performances en préchargeant les applications fréquemment utilisées)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options", "DisableTaskMgr", 1, 0, "Autoriser l'accès au Gestionnaire des tâches", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\BackgroundAccessApplications", "GlobalUserDisabled", 0, 1, "Désactiver les applications en arrière-plan (améliore les performances du système)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ShowSuperHidden", 1, 0, "Masquer les fichiers et dossiers super-cachés (améliore les performances de l'Explorateur)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Windows\\WindowsUpdate", "DisableWindowsUpdateAccess", 1, 0, "Activer les mises à jour Windows", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Power", "HiberbootEnabled", 0, 1, "Activer le démarrage rapide pour accélérer le démarrage", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "SOFTWARE\\Policies\\Microsoft\\Windows NT\\CurrentVersion\\SystemRestore", "DisableSR", 1, 0, "Activer la restauration du système pour de meilleures options de récupération", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "SYSTEM\\CurrentControlSet\\Control\\Terminal Server", "fDenyTSConnections", 1, 0, "Activer le Bureau à distance pour l'accès distant à votre système", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "HideFileExt", 1, 0, "Afficher les extensions de fichiers pour une meilleure gestion des fichiers", "DWORD", logger));
	}
}
