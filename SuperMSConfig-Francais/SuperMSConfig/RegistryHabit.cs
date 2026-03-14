using System;
using System.Drawing;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SuperMSConfig;

public class RegistryHabit : BaseHabit
{
	private readonly RegistryHive hive;

	private readonly string keyPath;

	private readonly string valueName;

	private readonly object badValue;

	private readonly object goodValue;

	private readonly string description;

	private readonly Logger logger;

	private readonly RegistryValueKind valueKind;

	public override string Name => "Clé de registre : " + valueName;

	public override string Description => description;

	public override HabitStatus Status { get; set; } = HabitStatus.NotConfigured;

	public RegistryHabit(RegistryHive hive, string keyPath, string valueName, object badValue, object goodValue, string description, string valueType, Logger logger)
	{
		this.hive = hive;
		this.keyPath = keyPath;
		this.valueName = valueName;
		this.badValue = badValue;
		this.goodValue = goodValue;
		this.description = description;
		this.logger = logger;
		valueKind = GetRegistryValueKind(valueType);
	}

	public override string GetDetails()
	{
		return $"Chemin du registre : {hive}\\{keyPath}, Nom de la valeur : {valueName}, Valeur actuelle : {GetCurrentValue()}, Mauvaise valeur : {badValue}, Bonne valeur : {goodValue}";
	}

	public override async Task Check()
	{
		try
		{
			await Task.Run(delegate
			{
				using RegistryKey registryKey = OpenRegistryKey();
				if (registryKey == null)
				{
					Status = HabitStatus.NotConfigured;
					logger.Log("Clé non trouvée : " + keyPath + ". Statut : Non configurée", Color.Blue);
				}
				else
				{
					object value = registryKey.GetValue(valueName);
					if (value == null)
					{
						Status = HabitStatus.NotConfigured;
						logger.Log("La valeur de registre est manquante et non configurée.", Color.Blue);
					}
					else
					{
						if (valueKind == RegistryValueKind.DWord && int.TryParse(value.ToString(), out var result))
						{
							int num = Convert.ToInt32(badValue);
							int num2 = Convert.ToInt32(goodValue);
							if (result == num)
							{
								Status = HabitStatus.Bad;
							}
							else if (result == num2)
							{
								Status = HabitStatus.Good;
							}
						}
						else if (value.ToString() == badValue.ToString())
						{
							Status = HabitStatus.Bad;
						}
						else if (value.ToString() == goodValue.ToString())
						{
							Status = HabitStatus.Good;
						}
						logger.Log($"Vérifié {Name}. Statut : {Status}", (Status == HabitStatus.Bad) ? Color.Red : ((Status == HabitStatus.Good) ? Color.Green : Color.Blue), GetDetails());
					}
				}
			});
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			logger.Log("Erreur lors de Check : " + ex2.Message, Color.Red, ex2.StackTrace);
		}
	}

	public override async Task Fix()
	{
		try
		{
			await Task.Run(delegate
			{
				using (RegistryKey registryKey = OpenRegistryKey(writable: true))
				{
					if (registryKey != null)
					{
						object value = ConvertValueToType(goodValue);
						registryKey.SetValue(valueName, value, valueKind);
						Status = HabitStatus.Good;
					}
				}
				logger.Log("Corrigé " + Name, Color.Green, GetDetails());
			});
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			logger.Log("Erreur lors de Fix : " + ex2.Message, Color.Red, ex2.StackTrace);
		}
	}

	public override async Task Revert()
	{
		try
		{
			await Task.Run(delegate
			{
				RegistryKey rootKey = ((hive == RegistryHive.CurrentUser) ? Registry.CurrentUser : Registry.LocalMachine);
				using (RegistryKey registryKey = OpenRegistryKey(writable: true))
				{
					if (registryKey != null)
					{
						if (badValue != null && badValue.ToString().Equals("DELETE", StringComparison.OrdinalIgnoreCase))
						{
							registryKey.Close();
							try
							{
								DeleteKeyTree(rootKey, keyPath);
								logger.Log("Clé de registre supprimée avec succès : " + keyPath, Color.Green);
							}
							catch (Exception ex3)
							{
								logger.Log("Erreur lors de DeleteKeyTree : " + ex3.Message, Color.Red, ex3.StackTrace);
							}
						}
						else
						{
							object value = ConvertValueToType(badValue);
							registryKey.SetValue(valueName, value, valueKind);
						}
						Status = HabitStatus.Bad;
					}
					else
					{
						logger.Log("Échec de l'ouverture de la clé de registre : " + keyPath, Color.Red);
					}
				}
				logger.Log("Rétabli " + Name, Color.Orange, GetDetails());
			});
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			logger.Log("Erreur lors de Revert : " + ex2.Message, Color.Red, ex2.StackTrace);
		}
	}

	private void DeleteKeyTree(RegistryKey rootKey, string keyPath)
	{
		try
		{
			rootKey.DeleteSubKeyTree(keyPath, throwOnMissingSubKey: false);
		}
		catch (Exception ex)
		{
			logger.Log("Erreur lors de DeleteKeyTree : " + ex.Message, Color.Red, ex.StackTrace);
		}
	}

	private RegistryKey OpenRegistryKey(bool writable = false)
	{
		RegistryKey registryKey = ((hive == RegistryHive.CurrentUser) ? Registry.CurrentUser : Registry.LocalMachine);
		try
		{
			RegistryKey registryKey2 = registryKey.OpenSubKey(keyPath, writable);
			if (registryKey2 == null && writable)
			{
				registryKey2 = registryKey.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
			}
			return registryKey2;
		}
		catch (SecurityException)
		{
			return null;
		}
	}

	private object GetCurrentValue()
	{
		using RegistryKey registryKey = OpenRegistryKey();
		return registryKey?.GetValue(valueName);
	}

	private object ConvertValueToType(object value)
	{
		if (value == null)
		{
			return null;
		}
		if (valueKind == RegistryValueKind.DWord)
		{
			if (Convert.ToInt32(value) == -1)
			{
				return -1;
			}
			return Convert.ToInt32(value);
		}
		return value.ToString();
	}

	private RegistryValueKind GetRegistryValueKind(string valueType)
	{
		string text = valueType.ToUpper();
		string text2 = text;
		if (!(text2 == "DWORD"))
		{
			if (text2 == "STRING")
			{
				return RegistryValueKind.String;
			}
			return RegistryValueKind.String;
		}
		return RegistryValueKind.DWord;
	}
}
