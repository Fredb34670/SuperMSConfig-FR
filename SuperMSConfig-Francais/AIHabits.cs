using Microsoft.Win32;
using SuperMSConfig;

public class AIHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Fonctionnalités intelligentes";

	public AIHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Policies\\Microsoft\\Windows\\WindowsCopilot", "TurnOffWindowsCopilot", 0, 1, "IA Windows (Copilot)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Policies\\Microsoft\\Windows\\WindowsAI", "DisableAIDataAnalysis", 0, 1, "IA Windows (Recall > Utilisateur actuel)", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Windows\\WindowsAI", "DisableAIDataAnalysis", 0, 1, "IA Windows (Recall > Tous les utilisateurs)", "DWORD", logger));
	}
}
