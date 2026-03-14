using Microsoft.Win32;
using SuperMSConfig;

public class GamingHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Expérience de jeu";

	public GamingHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "SYSTEM\\CurrentControlSet\\Control\\DeviceGuard", "EnableVirtualizationBasedSecurity", 1, 0, "Désactiver la sécurité basée sur la virtualisation (VBS) (Cette fonctionnalité est connue pour consommer beaucoup de ressources système. Sa désactivation libère des cycles CPU et garantit que vos jeux fonctionnent de manière plus fluide, avec moins de surcharge ralentissant les performances et rendant votre expérience de jeu moins réactive)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "SYSTEM\\CurrentControlSet\\Control\\DeviceGuard\\Scenarios\\HypervisorEnforcedCodeIntegrity", "Enabled", 1, 0, "Désactiver l'intégrité de la mémoire pour de meilleurs FPS en jeu (Isolation du noyau)", "DWORD", logger));
	}
}
