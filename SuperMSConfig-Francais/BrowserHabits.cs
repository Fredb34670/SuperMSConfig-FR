using Microsoft.Win32;
using SuperMSConfig;

public class BrowserHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Paramètres Edge";

	public BrowserHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "PasswordManagerEnabled", 1, 0, CategoryName + " : Désactiver l'enregistrement des mots de passe pour une meilleure sécurité", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "AutofillAddressEnabled", 1, 0, CategoryName + " : Désactiver la saisie automatique pour une meilleure confidentialité", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "SafeBrowsingEnabled", 0, 1, CategoryName + " : Activer la navigation sécurisée pour se protéger contre les sites malveillants", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "PopupsAllowed", 1, 0, CategoryName + " : Bloquer les fenêtres contextuelles pour éviter les interruptions indésirables", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "DoNotTrackEnabled", 0, 1, CategoryName + " : Activer 'Do Not Track' pour améliorer la confidentialité", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "AutofillCreditCardEnabled", 1, 0, CategoryName + " : Désactiver la saisie automatique des cartes de crédit pour une meilleure sécurité", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "BrowserSignin", 1, 0, CategoryName + " : Désactiver la connexion au navigateur pour empêcher la synchronisation des comptes", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "DefaultBrowserSettingEnabled", 1, 0, CategoryName + " : Désactiver le paramètre de navigateur par défaut", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "EdgeCollectionsEnabled", 1, 0, CategoryName + " : Désactiver les Collections Edge", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "EdgeShoppingAssistantEnabled", 1, 0, CategoryName + " : Désactiver l'assistant d'achat Edge", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "GamerModeEnabled", 1, 0, CategoryName + " : Désactiver le mode Jeu", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "HideFirstRunExperience", 0, 1, CategoryName + " : Masquer l'expérience de première exécution", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "ImportOnEachLaunch", 1, 0, CategoryName + " : Désactiver l'importation à chaque lancement", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "NewTabPageHideDefaultTopSites", 0, 1, CategoryName + " : Ne pas afficher les liens sponsorisés sur la page Nouvel onglet", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "NewTabPageQuickLinksEnabled", 1, 0, CategoryName + " : Désactiver les liens rapides sur la page Nouvel onglet", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "StartupBoostEnabled", 1, 0, CategoryName + " : Désactiver le boost au démarrage", "DWORD", logger));
		habits.Add(new RegistryHabit(RegistryHive.LocalMachine, "Software\\Policies\\Microsoft\\Edge", "UserFeedbackAllowed", 1, 0, CategoryName + " : Désactiver les commentaires utilisateur", "DWORD", logger));
	}
}
