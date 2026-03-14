namespace SuperMSConfig;

public class ServiceHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Services";

	public ServiceHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new ServiceHabit("diagnosticshub.standardcollector.service", "Service Standard Collector du hub de diagnostics Microsoft (R) : collecte des données de diagnostic pour la santé et les performances du système.", logger, 1));
		habits.Add(new ServiceHabit("AdobeARMservice", "Service de mise à jour Adobe Acrobat : maintient le logiciel Adobe Acrobat à jour.", logger, 1));
		habits.Add(new ServiceHabit("gupdate", "Service de mise à jour Google : maintient les applications Google comme Chrome à jour.", logger, 1));
		habits.Add(new ServiceHabit("gupdatem", "Service de mise à jour Google (système) : maintient les applications Google à jour pour tout le système.", logger, 1));
		habits.Add(new ServiceHabit("TeamViewer", "Service TeamViewer : permet l'accès à distance et les connexions de support.", logger, 1));
		habits.Add(new ServiceHabit("CWAUpdaterService", "Service de mise à jour de l'application Citrix Workspace : gère les mises à jour de Citrix Workspace.", logger, 1));
		habits.Add(new ServiceHabit("AppleMobileDeviceService", "Service Apple Mobile Device : permet à iTunes de communiquer avec les iPhones/iPads.", logger, 1));
		habits.Add(new ServiceHabit("Bonjour Service", "Service Apple Bonjour : permet la découverte automatique des appareils sur les réseaux locaux, souvent inutile.", logger, 1));
		habits.Add(new ServiceHabit("NvTelemetryContainer", "Service de télémétrie NVIDIA : collecte et envoie des données de télémétrie à NVIDIA.", logger, 1));
		habits.Add(new ServiceHabit("dbupdate", "Service de mise à jour Dropbox : maintient l'application Dropbox à jour.", logger, 1));
		habits.Add(new ServiceHabit("dbupdatem", "Service de mise à jour Dropbox (système) : maintient Dropbox à jour pour tous les utilisateurs du système.", logger, 1));
		habits.Add(new ServiceHabit("SkypeUpdate", "Service de mise à jour Skype : met à jour Skype automatiquement, souvent inutile.", logger, 1));
		habits.Add(new ServiceHabit("SpotifyWebHelper", "Spotify Web Helper : lance automatiquement Spotify depuis des liens web, souvent inutile.", logger, 1));
		habits.Add(new ServiceHabit("RtkAudioService", "Service Audio Realtek : gère les tâches audio pour le matériel Realtek, généralement inutile pour une utilisation de base.", logger, 1));
		habits.Add(new ServiceHabit("IntelRapidStorage", "Technologie de stockage Intel Rapid : souvent inutile sauf si vous utilisez des fonctionnalités RAID ou de gestion de disque avancées.", logger, 1));
		habits.Add(new ServiceHabit("Steam Client Service", "Service Client Steam : permet à Steam de mettre à jour et gérer les jeux, inutile si vous n'utilisez pas Steam.", logger, 1));
		habits.Add(new ServiceHabit("AdobeGenuineMonitorService", "Service d'intégrité du logiciel Adobe Genuine : vérifie l'authenticité des logiciels Adobe, pas essentiel pour une utilisation régulière.", logger, 1));
		habits.Add(new ServiceHabit("diagnosticshub.standardcollector.service", "Service Standard Collector du hub de diagnostics Microsoft (R) : collecte des données de diagnostic pour la santé et les performances du système.", logger, 1));
		habits.Add(new ServiceHabit("DiagTrack", "Service de suivi des diagnostics : surveille et suit les diagnostics et les mesures de performance.", logger, 1));
		habits.Add(new ServiceHabit("dmwappushservice", "Service de routage de messages WAP Push : gère les messages push WAP, peut être désactivé en toute sécurité s'il n'est pas utilisé.", logger, 1));
		habits.Add(new ServiceHabit("lfsvc", "Service de géolocalisation : gère les données de localisation pour diverses applications. Désactivez-le si vous n'utilisez pas de services basés sur la localisation.", logger, 1));
		habits.Add(new ServiceHabit("MapsBroker", "Gestionnaire de cartes téléchargées : gère les cartes hors ligne. Désactivez-le si vous n'utilisez pas de cartes hors ligne.", logger, 1));
		habits.Add(new ServiceHabit("NetTcpPortSharing", "Service de partage de ports Net.Tcp : permet la communication réseau via TCP. Désactivez-le si vous n'utilisez pas d'applications en réseau.", logger, 1));
		habits.Add(new ServiceHabit("RemoteRegistry", "Registre à distance : permet l'accès à distance au registre Windows. Désactivez-le si l'accès à distance au registre est inutile.", logger, 1));
		habits.Add(new ServiceHabit("TrkWks", "Client de suivi de liens distribués : maintient les liens vers les fichiers sur un réseau. Désactivez-le si vous n'utilisez pas de liens de fichiers en réseau.", logger, 1));
		habits.Add(new ServiceHabit("WbioSrvc", "Service biométrique Windows : prend en charge le matériel de reconnaissance d'empreintes digitales et faciale. Désactivez-le si vous n'utilisez pas d'appareils biométriques.", logger, 1));
		habits.Add(new ServiceHabit("WMPNetworkSvc", "Service de partage réseau du Lecteur Windows Media : partage des médias sur le réseau. Désactivez-le si vous ne partagez pas de médias.", logger, 1));
		habits.Add(new ServiceHabit("XblAuthManager", "Gestionnaire d'authentification Xbox Live : gère l'authentification Xbox Live. Désactivez-le si vous n'utilisez pas les services Xbox Live.", logger, 1));
		habits.Add(new ServiceHabit("XblGameSave", "Service de sauvegarde de jeux Xbox Live : gère les sauvegardes de jeux Xbox. Désactivez-le si vous n'utilisez pas les fonctionnalités de jeu Xbox Live.", logger, 1));
		habits.Add(new ServiceHabit("XboxNetApiSvc", "Service réseau Xbox Live : gère les connexions réseau Xbox Live. Désactivez-le si les services Xbox Live ne sont pas nécessaires.", logger, 1));
		habits.Add(new ServiceHabit("ndu", "Moniteur d'utilisation des données réseau Windows : suit l'utilisation des données réseau. Désactivez-le si vous n'avez pas besoin de surveiller l'utilisation des données.", logger, 1));
		habits.Add(new ServiceHabit("fxssvc", "Service de télécopie : gère les opérations de fax. Si vous n'utilisez pas de télécopieur, ce service peut être désactivé.", logger, 1));
		habits.Add(new ServiceHabit("WiaRpc", "Acquisition d'images Windows (WIA) : prend en charge les appareils d'acquisition d'images comme les scanners et les caméras. Désactivez-le si ces appareils ne sont pas utilisés.", logger, 1));
		habits.Add(new ServiceHabit("SmartCard", "Carte à puce : prend en charge les cartes à puce pour la sécurité. Désactivez-le si la fonctionnalité de carte à puce n'est pas nécessaire.", logger, 1));
		habits.Add(new ServiceHabit("Spooler", "Spouleur d'impression : gère les travaux d'impression. Désactivez-le si vous n'avez pas d'imprimante ou de travaux d'impression.", logger, 1));
		habits.Add(new ServiceHabit("SysMain", "Superfetch (SysMain) : améliore les temps de démarrage en préchargeant les applications fréquemment utilisées. Si vous avez un SSD, ce n'est généralement pas nécessaire.", logger, 1));
		habits.Add(new ServiceHabit("bthserv", "Service de prise en charge Bluetooth : gère les connexions Bluetooth. Désactivez-le si vous n'utilisez pas d'appareils Bluetooth.", logger, 1));
		habits.Add(new ServiceHabit("WerSvc", "Service de rapport d'erreurs Windows : envoie des rapports d'erreurs à Microsoft. Désactivez-le si vous ne souhaitez pas partager d'informations d'erreur.", logger, 1));
		habits.Add(new ServiceHabit("wisvc", "Service Windows Insider : gère les mises à jour Windows Insider. Désactivez-le si vous ne faites pas partie du programme Insider.", logger, 1));
		habits.Add(new ServiceHabit("seclogon", "Ouverture de session secondaire : permet d'exécuter des applications avec des informations d'identification différentes. Désactivez-le si vous n'utilisez pas cette fonctionnalité.", logger, 1));
	}
}
