using Microsoft.Win32;
using SuperMSConfig;

public class SecurityHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Sécurité";

	public SecurityHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Windows Defender", "DisableAntiSpyware", 1, 0, "Activer Windows Defender (empêche les logiciels espions et malveillants)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Windows\\System", "EnableLUA", 0, 1, "Activer le Contrôle de compte d'utilisateur (UAC) (demande confirmation pour les actions administratives)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "System\\CurrentControlSet\\Services\\LanmanServer\\Parameters", "SMB1", 1, 0, "Désactiver SMBv1 (empêche l'exploitation d'anciennes vulnérabilités)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Windows Defender\\Real-Time Protection", "DisableRealtimeMonitoring", 1, 0, "Activer la protection en temps réel de Windows Defender (analyse les menaces en temps réel)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows Script Host\\Settings", "Enabled", 1, 0, "Désactiver Windows Script Host (empêche les scripts malveillants)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "System\\CurrentControlSet\\Control\\SecureBoot\\State", "UEFISecureBootEnabled", 0, 1, "Activer le démarrage sécurisé (empêche les OS et chargeurs de démarrage non autorisés)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\WindowsFirewall\\DomainProfile", "EnableFirewall", 0, 1, "Activer le pare-feu (protège toutes les connexions réseau)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", "NoDriveTypeAutoRun", 0, 255, "Désactiver l'exécution automatique (empêche le démarrage automatique de périphériques USB malveillants)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", "AutoAdminLogon", 1, 0, "Désactiver le compte Invité (empêche les accès non autorisés)", "DWORD", logger));
	}
}
