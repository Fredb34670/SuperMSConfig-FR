using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace SuperMSConfig;

public class HabitChecker
{
	private readonly List<BaseHabitCategory> habitCategories = new List<BaseHabitCategory>();

	private readonly Logger logger;

	public HabitChecker(Logger logger)
	{
		this.logger = logger;
		habitCategories.Add(new AdsHabits(logger));
		habitCategories.Add(new UIHabits(logger));
		habitCategories.Add(new AIHabits(logger));
		habitCategories.Add(new PrivacyHabits(logger));
		habitCategories.Add(new SecurityHabits(logger));
		habitCategories.Add(new ServiceHabits(logger));
		habitCategories.Add(new AppsHabits(logger));
		habitCategories.Add(new StartupHabits(logger));
		habitCategories.Add(new SystemHabits(logger));
		habitCategories.Add(new BrowserHabits(logger));
		habitCategories.Add(new GamingHabits(logger));
	}

	public async Task<List<BaseHabit>> GetAllHabits()
	{
		List<BaseHabit> habits = new List<BaseHabit>();
		foreach (BaseHabitCategory category in habitCategories)
		{
			List<BaseHabit> categoryHabits = category.GetHabits().ToList();
			if (categoryHabits?.Any() ?? false)
			{
				habits.AddRange(categoryHabits);
			}
		}
		Console.WriteLine($"Total des habitudes collectées : {habits.Count}");
		return habits;
	}

	public async Task CheckHabit(BaseHabit habit)
	{
		await habit.Check();
		logger.Log($"{habit.Name} - {habit.Description}: {habit.Status}", GetColorForStatus(habit.Status), habit.GetDetails());
	}

	public Color GetColorForStatus(BaseHabit.HabitStatus status)
	{
		return status switch
		{
			BaseHabit.HabitStatus.Good => Color.FromArgb(254, 252, 251), 
			BaseHabit.HabitStatus.Bad => Color.Pink, 
			BaseHabit.HabitStatus.NotConfigured => Color.FromArgb(253, 227, 207), 
			_ => Color.Black, 
		};
	}

	public List<string> GetCategoryNames()
	{
		return habitCategories.Select((BaseHabitCategory c) => c.CategoryName).Distinct().ToList();
	}

	public async Task<List<BaseHabit>> GetHabitsByCategory(string categoryName)
	{
		BaseHabitCategory category = habitCategories.FirstOrDefault((BaseHabitCategory c) => c.CategoryName == categoryName);
		if (category != null)
		{
			return category.GetHabits().ToList();
		}
		return new List<BaseHabit>();
	}
}
