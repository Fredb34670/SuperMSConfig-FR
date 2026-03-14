using Microsoft.Win32;
using SuperMSConfig;

public class UIHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Personnalisation";

	public UIHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Policies\\Windows\\Explorer", "DisableSearchBoxSuggestions", 1, 0, "Désactiver la recherche de contenu Cloud Bing", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "SOFTWARE\\Policies\\Microsoft\\Windows\\Explorer", "ShowOrHideMostUsedApps", 2, 1, "Masquer les applications les plus utilisées dans le menu Démarrer", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Search", "SearchboxTaskbarMode", 2, 0, "Masquer la zone de recherche sur la barre des tâches", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "TaskbarAl", 1, 0, "Aligner le bouton Démarrer à gauche", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "Start_Layout", 0, 1, "Épingler plus d'applications dans le menu Démarrer", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "TaskbarMn", 1, 0, "Masquer l'icône Chat (Teams) sur la barre des tâches", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ShowTaskViewButton", 1, 0, "Masquer le bouton Affichage des tâches sur la barre des tâches", "DWORD", logger));
	}
}
