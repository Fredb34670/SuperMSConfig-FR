using System;
using Microsoft.Win32;
using SuperMSConfig;

namespace Templates;

public class HabitFactory
{
	public BaseHabit CreateHabit(HabitTemplate template, Logger logger)
	{
		switch (template.Type)
		{
		case "RegistryHabit":
		{
			RegistryHive hive = (RegistryHive)Enum.Parse(typeof(RegistryHive), template.Hive);
			string valueType = template.ValueType ?? "DWORD";
			return new RegistryHabit(hive, template.Key, template.ValueName, template.BadValue, template.GoodValue, template.Description, valueType, logger);
		}
		case "StartupHabit":
			return new StartupHabit(template.AppName, template.Description, logger);
		case "ServiceHabit":
			return new ServiceHabit(template.ServiceName, template.Description, logger, Convert.ToInt32(template.BadValue));
		case "AppsHabit":
			return new AppsHabit(template.AppName, template.Description, logger);
		default:
			throw new ArgumentException("Unknown habit type: " + template.Type);
		}
	}
}
