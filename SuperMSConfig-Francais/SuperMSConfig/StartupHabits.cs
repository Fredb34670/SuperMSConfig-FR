namespace SuperMSConfig;

public class StartupHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Démarrage";

	public StartupHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new StartupHabit("Discord", "Discord : Essentiel pour le jeu et la communication, mais peut être désactivé s'il impacte les performances au démarrage.", logger));
		habits.Add(new StartupHabit("UninstallT20", "Microsoft Teams Updater : Met à jour automatiquement Microsoft Teams ; désactivez-le s'il cause des retards au démarrage.", logger));
		habits.Add(new StartupHabit("Teams", "Microsoft Teams : Crucial pour la collaboration professionnelle ; désactivez-le s'il affecte le temps de démarrage.", logger));
		habits.Add(new StartupHabit("OneDrive", "OneDrive : Synchronise les fichiers vers le cloud ; peut être désactivé s'il n'est pas nécessaire ou s'il cause des problèmes.", logger));
		habits.Add(new StartupHabit("Spotify", "Spotify : Lance le streaming musical ; peut être désactivé s'il ralentit votre système au démarrage.", logger));
		habits.Add(new StartupHabit("Skype", "Skype : Utilisé pour les appels et la messagerie ; désactivez-le s'il n'est pas nécessaire pour accélérer votre démarrage.", logger));
		habits.Add(new StartupHabit("Acrobat", "Adobe Acrobat : Ouvre les PDF ; peut être désactivé si vous ne l'utilisez pas fréquemment ou s'il ralentit le démarrage.", logger));
		habits.Add(new StartupHabit("Java Update Scheduler", "Java Update Scheduler : Maintient Java à jour ; désactivez-le si vous préférez les mises à jour manuelles ou s'il cause des ralentissements.", logger));
		habits.Add(new StartupHabit("CCleaner", "CCleaner : Nettoie les fichiers inutiles du système ; peut être désactivé s'il cause des temps de démarrage lents.", logger));
		habits.Add(new StartupHabit("NVIDIA GeForce Experience", "NVIDIA GeForce Experience : Gère les pilotes et paramètres graphiques ; désactivez-le s'il impacte les performances au démarrage.", logger));
		habits.Add(new StartupHabit("QuickTime", "QuickTime : Lecteur multimédia ; peut être désactivé s'il n'est pas fréquemment utilisé ou s'il ralentit le démarrage.", logger));
		habits.Add(new StartupHabit("uTorrent", "uTorrent : Client BitTorrent ; peut être désactivé s'il cause des retards ou des problèmes au démarrage.", logger));
		habits.Add(new StartupHabit("RtHDVBg", "Realtek HD Audio Driver : Essentiel pour un son de haute qualité, mais peut être désactivé s'il cause des problèmes.", logger));
		habits.Add(new StartupHabit("RtHDVCpl", "Realtek HD Control Panel : Gère les paramètres audio ; peut être désactivé s'il perturbe les performances du système.", logger));
	}
}
