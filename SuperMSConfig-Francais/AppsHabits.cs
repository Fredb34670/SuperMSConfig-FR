using SuperMSConfig;

public class AppsHabits : BaseHabitCategory
{
	private readonly Logger logger;

	public override string CategoryName => "Gestion des applications";

	public AppsHabits(Logger logger)
	{
		this.logger = logger;
		habits.Add(new AppsHabit("Microsoft.BingWeather", "Supprimer l'application Météo", logger));
		habits.Add(new AppsHabit("Microsoft.XboxApp", "Supprimer l'application Xbox", logger));
		habits.Add(new AppsHabit("Microsoft.OfficeHub", "Supprimer Office Hub", logger));
		habits.Add(new AppsHabit("Microsoft.SkypeApp", "Supprimer Skype", logger));
		habits.Add(new AppsHabit("Microsoft.MixedReality.Portal", "Supprimer Portail de réalité mixte", logger));
		habits.Add(new AppsHabit("Microsoft.WindowsPhone", "Supprimer l'application Windows Phone", logger));
		habits.Add(new AppsHabit("Microsoft.3DBuilder", "Supprimer 3D Builder", logger));
		habits.Add(new AppsHabit("Microsoft.BingFinance", "Supprimer MSN Finance", logger));
		habits.Add(new AppsHabit("Microsoft.BingNews", "Supprimer MSN Actualités", logger));
		habits.Add(new AppsHabit("Microsoft.BingSports", "Supprimer MSN Sports", logger));
		habits.Add(new AppsHabit("Microsoft.BingTravel", "Supprimer MSN Voyages", logger));
		habits.Add(new AppsHabit("Microsoft.GetHelp", "Supprimer l'application Obtenir de l'aide", logger));
		habits.Add(new AppsHabit("Microsoft.GetStarted", "Supprimer l'application Astuces / Prise en main", logger));
		habits.Add(new AppsHabit("Microsoft.MicrosoftSolitaireCollection", "Supprimer Microsoft Solitaire Collection", logger));
		habits.Add(new AppsHabit("Microsoft.MicrosoftStickyNotes", "Supprimer Microsoft Sticky Notes (Pense-bête)", logger));
		habits.Add(new AppsHabit("Microsoft.MicrosoftToDo", "Supprimer Microsoft To Do", logger));
		habits.Add(new AppsHabit("Microsoft.MicrosoftPhotos", "Supprimer Microsoft Photos", logger));
		habits.Add(new AppsHabit("Microsoft.MicrosoftNews", "Supprimer Microsoft Actualités", logger));
		habits.Add(new AppsHabit("Facebook", "Supprimer Facebook", logger));
		habits.Add(new AppsHabit("Instagram", "Supprimer Instagram", logger));
		habits.Add(new AppsHabit("Twitter.Twitter", "Supprimer Twitter", logger));
		habits.Add(new AppsHabit("King.CandyCrushSaga", "Supprimer Candy Crush Saga", logger));
		habits.Add(new AppsHabit("King.CandyCrushSodaSaga", "Supprimer Candy Crush Soda Saga", logger));
		habits.Add(new AppsHabit("King.CandyCrushFriendsSaga", "Supprimer Candy Crush Friends Saga", logger));
	}
}
