using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PluginInterface;
using SuperMSConfig;

public class PluginLoader
{
	private ContextMenuStrip extensionsMenuStrip;

	private Dictionary<string, string> pluginPaths;

	private Form pluginsForm;

	private Panel pluginPanel;

	private Form mainForm;

	private string unblockFileIndicator;

	private List<Button> allPluginButtons = new List<Button>();

	private List<Button> allToolButtons = new List<Button>();

	// Dictionnaire de traduction des descriptions de plugins (PluginInfo)
	private static readonly Dictionary<string, string> _traductionsInfo = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{
			"Let Microsoft AI assist you, if you're unsure about the settings. The app integrates seamlessly with Microsoft Copilot, allowing you to ask for the optimal configuration directly within a WebView2 control. With real-time guidance from Copilot, you can easily adjust settings to fit your needs.",
			"Laissez Microsoft AI vous guider si vous avez des doutes sur les paramètres. L'application s'intègre parfaitement avec Microsoft Copilot, vous permettant de demander la configuration optimale directement dans un contrôle WebView2. Grâce aux conseils en temps réel de Copilot, ajustez vos paramètres facilement."
		},
		{
			"This Plugin is a powerful tool designed to streamline and optimize your Windows 10/11 experience. With advanced community-driven detection capabilities, this plugin allows you to easily identify and remove unnecessary bloatware, enhancing system performance and freeing up valuable resources.",
			"Ce plugin est un outil puissant conçu pour optimiser votre expérience Windows 10/11. Grâce à ses capacités de détection avancées alimentées par la communauté, il vous permet d'identifier et de supprimer facilement les logiciels superflus (bloatware), améliorant ainsi les performances système et libérant des ressources précieuses."
		},
		{
			"Microsoft Navigator is your modern, assistant for Windows. Like Clippy, Butler handles simple requests to manage your system-install apps, tweak settings, and more.",
			"Microsoft Navigator est votre assistant moderne pour Windows. Comme Clippy, cet outil traite des requêtes simples pour gérer votre système : installation d'applications, ajustement des paramètres, et plus encore."
		}
	};

	// Retourne la traduction française de la description d'un plugin, ou le texte original si non trouvé
	private static string TraduireInfoPlugin(string infoOriginal)
	{
		if (string.IsNullOrEmpty(infoOriginal))
			return infoOriginal;

		if (_traductionsInfo.TryGetValue(infoOriginal.Trim(), out string traduction))
			return traduction;

		// Recherche partielle : si le texte commence par une clé connue
		foreach (var kvp in _traductionsInfo)
		{
			if (infoOriginal.Trim().StartsWith(kvp.Key.Substring(0, Math.Min(40, kvp.Key.Length)), StringComparison.OrdinalIgnoreCase))
				return kvp.Value;
		}

		return infoOriginal; // Aucune traduction connue : texte original conservé
	}

	public PluginLoader(Form mainForm, ContextMenuStrip menuStrip)
	{
		this.mainForm = mainForm;
		extensionsMenuStrip = menuStrip;
		pluginPaths = new Dictionary<string, string>();
		unblockFileIndicator = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", "unblockIndicator.txt");
		InitializePluginsForm();
		mainForm.LocationChanged += delegate
		{
			PositionPluginsForm();
		};
		LoadAllPlugins();
	}

	private void InitializePluginsForm()
	{
		pluginsForm = new Form
		{
			Size = new Size(400, mainForm.Height),
			StartPosition = FormStartPosition.Manual,
			FormBorderStyle = FormBorderStyle.Sizable,
			ShowInTaskbar = false,
			ShowIcon = false,
			TopMost = true
		};
		pluginPanel = new Panel
		{
			Dock = DockStyle.Fill,
			BackColor = Color.White
		};
		pluginsForm.Controls.Add(pluginPanel);
		pluginsForm.FormClosing += delegate(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			pluginsForm.Hide();
		};
		PositionPluginsForm();
	}

	private void PositionPluginsForm()
	{
		if (pluginsForm != null && !pluginsForm.IsDisposed)
		{
			pluginsForm.Location = new Point(mainForm.Location.X + mainForm.Width - 10, mainForm.Location.Y);
		}
	}

	private bool IsFileUnblocked(string filePath)
	{
		return File.Exists(unblockFileIndicator) && File.ReadAllLines(unblockFileIndicator).Contains(filePath);
	}

	private void UnblockFile(string filePath)
	{
		if (!IsFileUnblocked(filePath))
		{
			string arguments = "Unblock-File -Path '" + filePath + "'";
			Process.Start(new ProcessStartInfo
			{
				FileName = "powershell",
				Arguments = arguments,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			}).WaitForExit();
			File.AppendAllText(unblockFileIndicator, filePath + Environment.NewLine);
		}
	}

	public bool LoadPlugin(string pluginPath)
	{
		if (!File.Exists(pluginPath))
		{
			MessageBox.Show(pluginPath + " DLL non trouvée !");
			return false;
		}
		try
		{
			Assembly assembly = Assembly.LoadFrom(pluginPath);
			bool flag = false;
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
				{
					IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
					UserControl control = plugin.GetControl();
					control.Dock = DockStyle.Fill;
					pluginPanel.Controls.Clear();
					pluginPanel.Controls.Add(control);
					pluginsForm.Text = plugin.PluginName ?? "";
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				MessageBox.Show("Aucun plugin valide trouvé dans " + pluginPath + " DLL.");
				return false;
			}
			if (!pluginsForm.Visible)
			{
				pluginsForm.Show();
				PositionPluginsForm();
			}
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show("Erreur lors du chargement du plugin depuis " + pluginPath + ": " + ex.Message);
			return false;
		}
	}

	public void LoadAllPlugins()
	{
		extensionsMenuStrip.Items.Clear();
		ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem
		{
			Text = "Infos App",
			Font = new Font("Segoe UI Variable Small", 9f, FontStyle.Bold),
			ForeColor = Color.DimGray,
			AutoSize = true
		};
		toolStripMenuItem.Click += delegate
		{
			new AboutForm().ShowDialog();
		};
		extensionsMenuStrip.Items.Add(toolStripMenuItem);
		string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
		if (!Directory.Exists(path))
		{
			return;
		}
		string[] directories = Directory.GetDirectories(path);
		foreach (string path2 in directories)
		{
			string[] files = Directory.GetFiles(path2, "*.dll");
			foreach (string pluginPath in files)
			{
				UnblockFile(pluginPath);
				try
				{
					Assembly assembly = Assembly.LoadFrom(pluginPath);
					IPlugin pluginInstance = null;
					Type[] types = assembly.GetTypes();
					foreach (Type type in types)
					{
						if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
						{
							pluginInstance = (IPlugin)Activator.CreateInstance(type);
							break;
						}
					}
					if (pluginInstance == null)
					{
						continue;
					}
					string pluginName = pluginInstance.PluginName;
					pluginPaths[pluginName] = pluginPath;
					ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem
					{
						Text = pluginName,
						Font = new Font("Segoe UI Variable Small", 12f, FontStyle.Regular),
						Padding = new Padding(1)
					};
					toolStripMenuItem2.Click += delegate
					{
						pluginPanel.Controls.Clear();
						if (LoadPlugin(pluginPath))
						{
							pluginPanel.Visible = true;
						}
					};
					extensionsMenuStrip.Items.Add(toolStripMenuItem2);
					ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem
					{
						Text = "Voir les infos",
						Font = new Font("Segoe UI Variable Small", 9f, FontStyle.Underline),
						ForeColor = Color.DeepPink,
						Padding = new Padding(1)
					};
					toolStripMenuItem3.Click += delegate
					{
						MessageBox.Show(TraduireInfoPlugin(pluginInstance.PluginInfo), "Infos du plugin" /* description traduite en français */, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					};
					extensionsMenuStrip.Items.Add(toolStripMenuItem3);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Erreur lors du chargement du plugin depuis \"" + pluginPath + "\": " + ex.Message, "Erreur de chargement du plugin", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
	}

	public void CreatePluginButtons()
	{
		FlowLayoutPanel panelPlugins = ((MainForm)mainForm).panelPlugins;
		panelPlugins.SuspendLayout();
		panelPlugins.Controls.Clear();
		TextBox searchBox = new TextBox
		{
			Text = "Rechercher...",
			BackColor = Color.FromArgb(250, 243, 235),
			Font = new Font("Georgia", 10f),
			Dock = DockStyle.Top,
			Margin = new Padding(10)
		};
		searchBox.Enter += delegate
		{
			if (searchBox.Text == "Rechercher...")
			{
				searchBox.Text = string.Empty;
			}
		};
		searchBox.TextChanged += delegate
		{
			FilterPlugins(searchBox.Text);
		};
		panelPlugins.Controls.Add(searchBox);
		panelPlugins.Controls.Add(CreateCloseButton(panelPlugins));
		panelPlugins.Controls.Add(CreateAboutButton());
		allPluginButtons.Clear();
		foreach (KeyValuePair<string, string> pluginPath in pluginPaths)
		{
			Button button = CreatePluginButton(pluginPath.Key, pluginPath.Value);
			allPluginButtons.Add(button);
			panelPlugins.Controls.Add(button);
		}
		AddMsConfigToolButtons(panelPlugins);
		AddWin11TasksButtons(panelPlugins);
		panelPlugins.ResumeLayout();
		panelPlugins.Visible = true;
	}

	private Button CreateCloseButton(FlowLayoutPanel pluginButtonPanel)
	{
		Button button = new Button();
		button.Text = "\ue711";
		button.Font = new Font("Segoe MDL2 Assets", 14f, FontStyle.Bold);
		button.BackColor = Color.SeaShell;
		button.ForeColor = Color.SeaGreen;
		button.AutoSize = true;
		button.FlatStyle = FlatStyle.Flat;
		button.Height = 20;
		button.Width = 20;
		button.TextAlign = ContentAlignment.MiddleCenter;
		button.FlatAppearance.BorderSize = 0;
		button.Margin = new Padding(10);
		return button.WithClick(delegate
		{
			pluginButtonPanel.Visible = false;
		});
	}

	private Panel CreateAboutButton()
	{
		Button button = new Button();
		button.Text = "À propos";
		button.Font = new Font("Georgia", 9f, FontStyle.Regular);
		button.BackColor = Color.LightGray;
		button.ForeColor = Color.Black;
		button.AutoSize = true;
		button.FlatStyle = FlatStyle.Flat;
		button.Padding = new Padding(8);
		button.Margin = new Padding(10);
		button.TextAlign = ContentAlignment.MiddleCenter;
		button.FlatAppearance.BorderSize = 0;
		Button value = button.WithClick(delegate
		{
			new AboutForm().ShowDialog();
		});
		LinkLabel linkLabel = new LinkLabel
		{
			Text = "\ud83d\udd17 Suivre sur GitHub",
			Font = new Font("Georgia", 9f, FontStyle.Regular),
			AutoSize = true,
			LinkColor = Color.Blue,
			ActiveLinkColor = Color.DarkBlue,
			LinkBehavior = LinkBehavior.HoverUnderline,
			Margin = new Padding(10, 0, 0, 0)
		};
		linkLabel.Click += delegate
		{
			string fileName = "https://github.com/builtbybel/SuperMSConfig";
			Process.Start(new ProcessStartInfo
			{
				FileName = fileName,
				UseShellExecute = true
			});
		};
		return new FlowLayoutPanel
		{
			AutoSize = true,
			FlowDirection = FlowDirection.TopDown,
			Controls = 
			{
				(Control)value,
				(Control)linkLabel
			}
		};
	}

	private Button CreatePluginButton(string pluginName, string pluginPath)
	{
		string shortPluginDescription = GetShortPluginDescription(pluginPath);
		Button button = new Button();
		button.Text = pluginName + "\n" + shortPluginDescription;
		button.Font = new Font("Georgia", 11f, FontStyle.Bold);
		button.BackColor = Color.Pink;
		button.ForeColor = Color.FromArgb(5, 60, 40);
		button.AutoSize = true;
		button.FlatStyle = FlatStyle.Flat;
		button.Padding = new Padding(10);
		button.Margin = new Padding(10);
		button.TextAlign = ContentAlignment.TopLeft;
		button.FlatAppearance.BorderSize = 0;
		button.Tag = pluginName.ToLower();
		return button.WithClick(delegate
		{
			pluginPanel.Controls.Clear();
			if (LoadPlugin(pluginPath))
			{
				pluginPanel.Visible = true;
			}
		});
	}

	private void FilterPlugins(string searchText)
	{
		searchText = searchText.ToLower();
		foreach (Button allPluginButton in allPluginButtons)
		{
			allPluginButton.Visible = allPluginButton.Text.ToLower().Contains(searchText);
		}
		foreach (Button allToolButton in allToolButtons)
		{
			allToolButton.Visible = allToolButton.Text.ToLower().Contains(searchText);
		}
	}

	private string GetShortPluginDescription(string pluginPath)
	{
		try
		{
			Assembly assembly = Assembly.LoadFrom(pluginPath);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
				{
					IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
					// Utiliser la description traduite en français pour l'aperçu court
					string pluginInfo = TraduireInfoPlugin(plugin.PluginInfo);
					return (pluginInfo.Length > 70) ? (pluginInfo.Substring(0, 47) + "...") : pluginInfo;
				}
			}
		}
		catch (Exception)
		{
			return "Description non disponible.";
		}
		return "Description non disponible.";
	}

	private void AddMsConfigToolButtons(FlowLayoutPanel panel)
	{
		(string, string)[] array = new(string, string)[16]
		{
			("À propos de Windows", "winver"),
			("Modifier les paramètres UAC", "UserAccountControlSettings"),
			("Centre de sécurité et de maintenance", "wscui.cpl"),
			("Dépannage Windows", "msdt.exe"),
			("Gestion de l'ordinateur", "compmgmt.msc"),
			("Informations système", "msinfo32"),
			("Observateur d'événements", "eventvwr.msc"),
			("Programmes et fonctionnalités", "appwiz.cpl"),
			("Propriétés système", "sysdm.cpl"),
			("Analyseur de performances", "perfmon"),
			("Moniteur de ressources", "resmon"),
			("Gestionnaire des tâches", "taskmgr"),
			("Invite de commandes", "cmd"),
			("Éditeur du Registre", "regedit"),
			("Assistance à distance", "msra"),
			("Restauration du système", "rstrui.exe")
		};
		(string, string)[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			(string, string) tuple = array2[i];
			Button button = CreateMsConfigToolButton(tuple.Item1, tuple.Item2);
			allToolButtons.Add(button);
			panel.Controls.Add(button);
		}
	}

	private Button CreateMsConfigToolButton(string toolName, string command)
	{
		Button button = new Button();
		button.Text = toolName;
		button.Font = new Font("Georgia", 11f, FontStyle.Regular);
		button.BackColor = Color.MistyRose;
		button.ForeColor = Color.FromArgb(5, 60, 40);
		button.AutoSize = true;
		button.FlatStyle = FlatStyle.Flat;
		button.Padding = new Padding(5);
		button.Margin = new Padding(5);
		button.FlatAppearance.BorderSize = 0;
		return button.WithClick(delegate
		{
			try
			{
				Process.Start(command);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Could not open " + toolName + ": " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		});
	}

	private void AddWin11TasksButtons(FlowLayoutPanel panel)
	{
		(string, string, string, Color)[] array = new(string, string, string, Color)[7]
		{
			("\ud83d\udee0\ufe0f Rechercher des mises à jour", "Assurez-vous que votre système est à jour avec les derniers correctifs.", "ms-settings:windowsupdate", Color.Peru),
			("\ud83d\udd0d Paramètres de confidentialité", "Gérez vos options de confidentialité pour garder le contrôle sur vos données.", "ms-settings:privacy", Color.SeaGreen),
			("\ud83c\udfa8 Personnaliser l'apparence", "Changez les thèmes, fonds d'écran et couleurs selon votre style.", "ms-settings:personalization", Color.Coral),
			("\ud83d\udd10 Configurer Windows Hello", "Configurez la reconnaissance faciale, l'empreinte ou le code PIN.", "ms-settings:signinoptions", Color.MediumPurple),
			("\ud83c\udf10 Configurer le réseau", "Gérez le Wi-Fi, le VPN et les autres préférences réseau.", "ms-settings:network-status", Color.Goldenrod),
			("\ud83d\uddd1\ufe0f Libérer de l'espace disque", "Supprimez les fichiers temporaires pour optimiser les performances.", "cleanmgr.exe", Color.Pink),
			("\ud83d\udcc2 Sauvegarde OneDrive", "Synchronisez vos fichiers et activez la sauvegarde automatique.", "ms-settings:backup", Color.CadetBlue)
		};
		(string, string, string, Color)[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			(string, string, string, Color) tuple = array2[i];
			Button button = CreateTaskButton(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
			allToolButtons.Add(button);
			panel.Controls.Add(button);
		}
	}

	private Button CreateTaskButton(string taskName, string description, string command, Color buttonColor)
	{
		Button button = new Button();
		button.Text = taskName + "\n" + description;
		button.Font = new Font("Georgia", 12f, FontStyle.Bold);
		button.ForeColor = Color.White;
		button.BackColor = buttonColor;
		button.AutoSize = true;
		button.FlatStyle = FlatStyle.Flat;
		button.Padding = new Padding(10);
		button.Margin = new Padding(5);
		button.TextAlign = ContentAlignment.MiddleLeft;
		button.FlatAppearance.BorderSize = 0;
		return button.WithClick(delegate
		{
			// Utiliser UseShellExecute = true pour supporter les URI ms-settings:* et autres commandes système
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = command,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show("Impossible d'ouvrir : " + taskName + "\n" + ex.Message,
					"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		});
	}
}
