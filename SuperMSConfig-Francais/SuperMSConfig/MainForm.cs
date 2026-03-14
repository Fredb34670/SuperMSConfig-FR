using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMSConfig.Helpers;
using SuperMSConfig.Properties;
using Templates;
using Updater;

namespace SuperMSConfig;

public class MainForm : Form
{
	private Logger logger;

	private HabitChecker habitChecker;

	private PluginLoader pluginLoader;

	private bool isPanel1Visible = false;

	private List<BaseHabit> allItems = new List<BaseHabit>();

	private string templatesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "source");

	private int goodHabitsCount = 0;

	private int badHabitsCount = 0;

	private IContainer components = null;

	private Button assetMenuExtensions;

	private ContextMenuStrip extensionsMenuStrip;

	private Panel panelMain;

	private ToolStripMenuItem appInfoToolStripMenuItem;

	private Panel panelSettings;

	private CheckBox cbShowOnlyBadHabits;

	private TextBox searchTextBox;

	private Button assetMenuSettings;

	private Button btnCheck;

	private ContextMenuStrip contextMenuStrip;

	private ToolStripTextBox toolStripTextBox;

	private ToolStripComboBox toolStripComboTemplates;

	private Button assetMenu;

	private RichTextBox rtbLog;

	private ComboBox comboBoxMSConfig;

	private Label lblDescription;

	private Panel panelEngine;

	public FlowLayoutPanel panelPlugins;

	private Button assetNext;

	private FlowLayoutPanel panelTweaks;

	private Button assetBack;

	private Button assetDo;

	private Button assetUndo;

	private Label border;

	private LinkLabel linkUpdates;

	public MainForm()
	{
		InitializeComponent();
		InitializeLoggerAndHabitChecker();
		InitializeNativeCategories();
		InitializePluginLoader();
	}

	private async void MainForm_Shown(object sender, EventArgs e)
	{
		SetFormTitle();
		SetStyle();
		GetGreeting();
		await LoadTemplatesIntoContextMenu(contextMenuStrip, templatesFolderPath);
		global::Updater.Updater updater = new global::Updater.Updater(linkUpdates);
		await updater.CheckForAppUpdatesAsync();
	}

	private void InitializeLoggerAndHabitChecker()
	{
		logger = new Logger(this);
		habitChecker = new HabitChecker(logger);
	}

	private void InitializePluginLoader()
	{
		pluginLoader = new PluginLoader(this, extensionsMenuStrip);
	}

	private void GetGreeting()
	{
		int hour = DateTime.Now.Hour;
		if (hour >= 5 && hour < 12)
		{
			lblDescription.Text = "Bonjour !";
		}
		else if (hour >= 12 && hour < 22)
		{
			lblDescription.Text = "Bon après-midi !";
		}
		else
		{
			lblDescription.Text = "Bonsoir !";
		}
	}

	private void SetStyle()
	{
		base.Size = new Size(650, 700);
		Panel panel = panelMain;
		FlowLayoutPanel flowLayoutPanel = panelTweaks;
		RichTextBox richTextBox = rtbLog;
		Color color = (panelEngine.BackColor = Color.FromArgb(249, 243, 240));
		Color color3 = (richTextBox.BackColor = color);
		Color color5 = (flowLayoutPanel.BackColor = color3);
		Color backColor = (panel.BackColor = color5);
		BackColor = backColor;
		panelPlugins.BackColor = Color.FromArgb(244, 239, 237);
		panelEngine.BackColor = Color.FromArgb(252, 240, 231);
		assetMenu.Text = "\uede3";
		assetMenuSettings.Text = "\ue710";
		assetBack.Text = "\ue72b";
		assetNext.Text = "\ue72a";
	}

	private void SetFormTitle()
	{
		bool flag = Utils.IsRunningAsAdmin();
		Text = (flag ? "SuperMSConfig" : "SuperMSConfig: (No Admin)");
	}

	private void InitializeNativeCategories()
	{
		if (habitChecker == null)
		{
			Console.WriteLine("habitChecker is not initialized.");
			return;
		}
		comboBoxMSConfig.Items.Clear();
		foreach (string categoryName in habitChecker.GetCategoryNames())
		{
			comboBoxMSConfig.Items.Add(categoryName);
		}
		comboBoxMSConfig.SelectedIndex = 0;
	}

	private async void btnCheck_Click(object sender, EventArgs e)
	{
		await LoadHabits();
	}

	private void UpdateButtonStates()
	{
		assetNext.Enabled = comboBoxMSConfig.SelectedIndex < comboBoxMSConfig.Items.Count - 1;
		assetBack.Enabled = comboBoxMSConfig.SelectedIndex > 0;
	}

	private async void assetNext_Click(object sender, EventArgs e)
	{
		if (comboBoxMSConfig.SelectedIndex < comboBoxMSConfig.Items.Count - 1)
		{
			comboBoxMSConfig.SelectedIndex++;
			await LoadHabits();
		}
		UpdateButtonStates();
	}

	private async void assetBack_Click(object sender, EventArgs e)
	{
		if (comboBoxMSConfig.SelectedIndex > 0)
		{
			comboBoxMSConfig.SelectedIndex--;
			await LoadHabits();
		}
		UpdateButtonStates();
	}

	private async Task LoadHabits()
	{
		panelSettings.Visible = true;
		btnCheck.Enabled = false;
		panelTweaks.Visible = true;
		btnCheck.Text = "Vérification...";
		panelTweaks.Controls.Clear();
		allItems.Clear();
		goodHabitsCount = 0;
		badHabitsCount = 0;
		string selectedCategory = comboBoxMSConfig.SelectedItem.ToString();
		foreach (BaseHabit habit in await habitChecker.GetHabitsByCategory(selectedCategory))
		{
			allItems.Add(habit);
			await ProcessHabit(habit);
		}
		btnCheck.Enabled = true;
		btnCheck.Text = "Vérifier";
	}

	private async Task ProcessHabit(BaseHabit habit)
	{
		await habit.Check();
		UpdateHabitCounts(habit);
		Panel tweakPanel = CreateHabitPanel(habit);
		panelTweaks.Controls.Add(tweakPanel);
		tweakPanel.BringToFront();
	}

	private void UpdateHabitCounts(BaseHabit habit)
	{
		if (habit.Status == BaseHabit.HabitStatus.Good)
		{
			goodHabitsCount++;
		}
		else if (habit.Status == BaseHabit.HabitStatus.Bad)
		{
			badHabitsCount++;
		}
		lblDescription.Text = $"Bonne config : {goodHabitsCount} | Mauvaise config : {badHabitsCount}";
	}

	private Panel CreateHabitPanel(BaseHabit habit)
	{
		Panel tweakPanel = new Panel
		{
			Width = panelTweaks.Width - 25,
			Height = 120,
			Dock = DockStyle.Top,
			Padding = new Padding(10),
			Margin = new Padding(5),
			BorderStyle = BorderStyle.None,
			BackColor = habitChecker.GetColorForStatus(habit.Status),
			Tag = habit
		};
		TextBox descriptionTextBox = new TextBox
		{
			Text = habit.Description + " (" + habit.GetDetails() + ")",
			Height = 35,
			AutoSize = true,
			ReadOnly = true,
			BorderStyle = BorderStyle.None,
			Multiline = true,
			BackColor = tweakPanel.BackColor,
			Font = new Font("Segoe UI", 10f, FontStyle.Bold),
			ForeColor = Color.FromArgb(5, 60, 40),
			Dock = DockStyle.Top,
			Cursor = Cursors.IBeam
		};
		CheckBox checkBox = new CheckBox
		{
			Text = habit.Name,
			AutoSize = true,
			Checked = (habit.Status == BaseHabit.HabitStatus.Good),
			Dock = DockStyle.Top,
			Font = new Font("Segoe UI", 10f),
			ForeColor = Color.Black
		};
		LinkLabel linkLabel = new LinkLabel
		{
			Text = "Copilot Snapshot",
			AutoSize = true,
			LinkColor = Color.Blue,
			Font = new Font("Segoe UI", 10f, FontStyle.Italic),
			Dock = DockStyle.Left,
			TextAlign = ContentAlignment.MiddleLeft,
			Cursor = Cursors.Hand
		};
		Label statusLabel = new Label
		{
			AutoSize = true,
			Font = new Font("Segoe UI", 10f, FontStyle.Italic),
			ForeColor = Color.Gray,
			Dock = DockStyle.Right,
			TextAlign = ContentAlignment.MiddleRight
		};
		statusLabel.Text = ((habit.Status == BaseHabit.HabitStatus.Good) ? "Bonne (Active)" : ((habit.Status == BaseHabit.HabitStatus.Bad) ? "Mauvaise (Inactive)" : "Non configurée"));
		linkLabel.Click += delegate
		{
			Clipboard.SetText("Capture de '" + habit.Name + "' : " + descriptionTextBox.Text);
			MessageBox.Show("'" + habit.Name + "' préparé pour Copilot !", "Capture enregistrée", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		};
		checkBox.CheckedChanged += async delegate
		{
			await ToggleHabitStatus(habit, checkBox, tweakPanel, descriptionTextBox, statusLabel);
		};
		tweakPanel.Controls.Add(linkLabel);
		tweakPanel.Controls.Add(statusLabel);
		tweakPanel.Controls.Add(checkBox);
		tweakPanel.Controls.Add(descriptionTextBox);
		return tweakPanel;
	}

	private async Task ToggleHabitStatus(BaseHabit habit, CheckBox checkBox, Panel tweakPanel, TextBox descriptionTextBox, Label statusLabel)
	{
		if (checkBox.Checked)
		{
			await habit.Fix();
			habit.Status = BaseHabit.HabitStatus.Good;
			goodHabitsCount++;
			badHabitsCount--;
			statusLabel.Text = "Bonne (Active)";
		}
		else
		{
			await habit.Revert();
			habit.Status = BaseHabit.HabitStatus.Bad;
			goodHabitsCount--;
			badHabitsCount++;
			statusLabel.Text = "Mauvaise (Inactive)";
		}
		tweakPanel.BackColor = habitChecker.GetColorForStatus(habit.Status);
		descriptionTextBox.BackColor = tweakPanel.BackColor;
		lblDescription.Text = $"Bonne config : {goodHabitsCount} | Mauvaise config : {badHabitsCount}";
	}

	private async Task ProcessImportedFile(List<HabitTemplate> templates)
	{
		HabitFactory habitFactory = new HabitFactory();
		panelTweaks.Controls.Clear();
		panelSettings.Visible = true;
		goodHabitsCount = 0;
		badHabitsCount = 0;
		try
		{
			foreach (HabitTemplate template in templates)
			{
				BaseHabit habit = habitFactory.CreateHabit(template, logger);
				await ProcessHabit(habit);
			}
			lblDescription.Text = $"Bonne config : {goodHabitsCount} | Mauvaise config : {badHabitsCount}";
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			MessageBox.Show("Error loading templates: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private async Task LoadTemplatesIntoContextMenu(ContextMenuStrip contextMenuStrip, string folderPath)
	{
		contextMenuStrip.Items.Clear();
		try
		{
			if (!Directory.Exists(folderPath))
			{
				return;
			}
			string[] array = await Task.Run(() => Directory.GetFiles(folderPath, "*.json"));
			foreach (string file in array)
			{
				string templateName = Path.GetFileNameWithoutExtension(file);
				ToolStripMenuItem templateMenuItem = new ToolStripMenuItem
				{
					Text = templateName,
					Font = new Font("Segoe UI", 12f, FontStyle.Regular),
					BackColor = Color.White,
					ForeColor = Color.Black,
					AutoSize = true,
					Padding = new Padding(5)
				};
				templateMenuItem.MouseEnter += delegate
				{
					string filePath = Path.Combine(folderPath, templateName + ".json");
					TemplateLoader templateLoader = new TemplateLoader();
					TemplateFile templateFile = templateLoader.LoadTemplateFile(filePath);
					if (templateFile != null)
					{
						templateMenuItem.Text = templateFile.Header;
					}
				};
				templateMenuItem.MouseLeave += delegate
				{
					templateMenuItem.Text = templateName;
				};
				templateMenuItem.Click += async delegate
				{
					string selectedFilePath = Path.Combine(folderPath, templateName + ".json");
					if (File.Exists(selectedFilePath))
					{
						try
						{
							TemplateLoader templateLoader = new TemplateLoader();
							TemplateFile templateFile = templateLoader.LoadTemplateFile(selectedFilePath);
							if (templateFile != null)
							{
								Text = "SuperMSConfig - @" + templateName;
								await ProcessImportedFile(templateFile.Entries);
							}
						}
						catch (Exception ex3)
						{
							MessageBox.Show("Error processing template file (" + selectedFilePath + "): " + ex3.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
				};
				contextMenuStrip.Items.Add(templateMenuItem);
			}
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			MessageBox.Show("Error loading templates: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FilterBadTweaks()
	{
		bool flag = cbShowOnlyBadHabits.Checked;
		foreach (Panel item in panelTweaks.Controls.OfType<Panel>())
		{
			if (item.Tag is BaseHabit baseHabit)
			{
				item.Visible = !flag || baseHabit.Status == BaseHabit.HabitStatus.Bad;
			}
		}
	}

	private void cbShowOnlyBadHabits_CheckedChanged(object sender, EventArgs e)
	{
		FilterBadTweaks();
	}

	private async void searchTextBox_TextChanged(object sender, EventArgs e)
	{
		string searchText = searchTextBox.Text.ToLower();
		await Task.Run(delegate
		{
			foreach (Control control in panelTweaks.Controls)
			{
				Panel tweakPanel = control as Panel;
				if (tweakPanel != null && tweakPanel.Tag is BaseHabit baseHabit)
				{
					bool matchesSearch = baseHabit.Name.ToLower().Contains(searchText) || baseHabit.Description.ToLower().Contains(searchText);
					tweakPanel.Invoke((Action)delegate
					{
						tweakPanel.Visible = matchesSearch;
					});
				}
			}
		});
	}

	private async Task SetAllHabitsStatus(BaseHabit.HabitStatus newStatus)
	{
		string action = ((newStatus == BaseHabit.HabitStatus.Good) ? "Appliquer tous les paramètres" : "Rétablir tous les paramètres");
		string confirmationMessage = "Êtes-vous sûr de vouloir " + action + " ? Cela affectera tous les paramètres listés.";
		DialogResult result = MessageBox.Show(confirmationMessage, action, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
		if (result != DialogResult.Yes)
		{
			return;
		}
		foreach (BaseHabit habit in allItems)
		{
			if (habit.Status != newStatus)
			{
				switch (newStatus)
				{
				case BaseHabit.HabitStatus.Good:
					await habit.Fix();
					habit.Status = BaseHabit.HabitStatus.Good;
					goodHabitsCount++;
					badHabitsCount--;
					break;
				case BaseHabit.HabitStatus.Bad:
					await habit.Revert();
					habit.Status = BaseHabit.HabitStatus.Bad;
					goodHabitsCount--;
					badHabitsCount++;
					break;
				}
			}
		}
		lblDescription.Text = $"Bonne config : {goodHabitsCount} | Mauvaise config : {badHabitsCount}";
		panelTweaks.Controls.Clear();
		foreach (BaseHabit habit2 in allItems)
		{
			Panel tweakPanel = CreateHabitPanel(habit2);
			panelTweaks.Controls.Add(tweakPanel);
		}
	}

	private async void assetDo_Click(object sender, EventArgs e)
	{
		await SetAllHabitsStatus(BaseHabit.HabitStatus.Good);
	}

	private async void assetUndo_Click(object sender, EventArgs e)
	{
		await SetAllHabitsStatus(BaseHabit.HabitStatus.Bad);
	}

	private void searchTextBox_Click(object sender, EventArgs e)
	{
		searchTextBox.Clear();
	}

	private void assetMenuSettings_Click(object sender, EventArgs e)
	{
		contextMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y);
	}

	private void assetMenu_Click(object sender, EventArgs e)
	{
		isPanel1Visible = !isPanel1Visible;
		if (isPanel1Visible)
		{
			panelPlugins.Visible = true;
			pluginLoader.CreatePluginButtons();
		}
		else
		{
			panelPlugins.Visible = false;
		}
	}

	private void assetMenuExtensions_Click(object sender, EventArgs e)
	{
		extensionsMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuperMSConfig.MainForm));
		this.panelMain = new System.Windows.Forms.Panel();
		this.panelPlugins = new System.Windows.Forms.FlowLayoutPanel();
		this.panelTweaks = new System.Windows.Forms.FlowLayoutPanel();
		this.panelEngine = new System.Windows.Forms.Panel();
		this.comboBoxMSConfig = new System.Windows.Forms.ComboBox();
		this.assetMenuExtensions = new System.Windows.Forms.Button();
		this.assetMenuSettings = new System.Windows.Forms.Button();
		this.btnCheck = new System.Windows.Forms.Button();
		this.lblDescription = new System.Windows.Forms.Label();
		this.panelSettings = new System.Windows.Forms.Panel();
		this.cbShowOnlyBadHabits = new System.Windows.Forms.CheckBox();
		this.rtbLog = new System.Windows.Forms.RichTextBox();
		this.searchTextBox = new System.Windows.Forms.TextBox();
		this.assetUndo = new System.Windows.Forms.Button();
		this.assetDo = new System.Windows.Forms.Button();
		this.assetNext = new System.Windows.Forms.Button();
		this.assetBack = new System.Windows.Forms.Button();
		this.extensionsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.appInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
		this.toolStripComboTemplates = new System.Windows.Forms.ToolStripComboBox();
		this.assetMenu = new System.Windows.Forms.Button();
		this.border = new System.Windows.Forms.Label();
		this.linkUpdates = new System.Windows.Forms.LinkLabel();
		this.panelMain.SuspendLayout();
		this.panelEngine.SuspendLayout();
		this.panelSettings.SuspendLayout();
		this.extensionsMenuStrip.SuspendLayout();
		this.contextMenuStrip.SuspendLayout();
		base.SuspendLayout();
		this.panelMain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelMain.AutoScroll = true;
		this.panelMain.Controls.Add(this.panelPlugins);
		this.panelMain.Controls.Add(this.panelTweaks);
		this.panelMain.Controls.Add(this.panelEngine);
		this.panelMain.Controls.Add(this.lblDescription);
		this.panelMain.Controls.Add(this.panelSettings);
		this.panelMain.Controls.Add(this.assetNext);
		this.panelMain.Controls.Add(this.assetBack);
		this.panelMain.Location = new System.Drawing.Point(0, 69);
		this.panelMain.Name = "panelMain";
		this.panelMain.Size = new System.Drawing.Size(584, 491);
		this.panelMain.TabIndex = 1;
		this.panelPlugins.AutoScroll = true;
		this.panelPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelPlugins.Location = new System.Drawing.Point(0, 0);
		this.panelPlugins.Name = "panelPlugins";
		this.panelPlugins.Size = new System.Drawing.Size(584, 491);
		this.panelPlugins.TabIndex = 308;
		this.panelPlugins.Visible = false;
		this.panelTweaks.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelTweaks.AutoScroll = true;
		this.panelTweaks.Location = new System.Drawing.Point(25, 44);
		this.panelTweaks.Name = "panelTweaks";
		this.panelTweaks.Size = new System.Drawing.Size(527, 258);
		this.panelTweaks.TabIndex = 309;
		this.panelEngine.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.panelEngine.Controls.Add(this.comboBoxMSConfig);
		this.panelEngine.Controls.Add(this.assetMenuExtensions);
		this.panelEngine.Controls.Add(this.assetMenuSettings);
		this.panelEngine.Controls.Add(this.btnCheck);
		this.panelEngine.Location = new System.Drawing.Point(73, 422);
		this.panelEngine.Name = "panelEngine";
		this.panelEngine.Size = new System.Drawing.Size(419, 66);
		this.panelEngine.TabIndex = 307;
		this.comboBoxMSConfig.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.comboBoxMSConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBoxMSConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.comboBoxMSConfig.Font = new System.Drawing.Font("Segoe UI Variable Text Semiligh", 14.25f);
		this.comboBoxMSConfig.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.comboBoxMSConfig.FormattingEnabled = true;
		this.comboBoxMSConfig.Location = new System.Drawing.Point(92, 16);
		this.comboBoxMSConfig.Name = "comboBoxMSConfig";
		this.comboBoxMSConfig.Size = new System.Drawing.Size(212, 34);
		this.comboBoxMSConfig.TabIndex = 304;
		this.assetMenuExtensions.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.assetMenuExtensions.AutoSize = true;
		this.assetMenuExtensions.BackColor = System.Drawing.Color.Transparent;
		this.assetMenuExtensions.Cursor = System.Windows.Forms.Cursors.Hand;
		this.assetMenuExtensions.FlatAppearance.BorderColor = System.Drawing.Color.LavenderBlush;
		this.assetMenuExtensions.FlatAppearance.BorderSize = 0;
		this.assetMenuExtensions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LavenderBlush;
		this.assetMenuExtensions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.assetMenuExtensions.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12f, System.Drawing.FontStyle.Bold);
		this.assetMenuExtensions.Image = SuperMSConfig.Properties.Resources.assetCopilot;
		this.assetMenuExtensions.Location = new System.Drawing.Point(7, 13);
		this.assetMenuExtensions.Name = "assetMenuExtensions";
		this.assetMenuExtensions.Size = new System.Drawing.Size(38, 37);
		this.assetMenuExtensions.TabIndex = 283;
		this.assetMenuExtensions.UseVisualStyleBackColor = false;
		this.assetMenuExtensions.Click += new System.EventHandler(assetMenuExtensions_Click);
		this.assetMenuSettings.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.assetMenuSettings.AutoSize = true;
		this.assetMenuSettings.BackColor = System.Drawing.Color.Transparent;
		this.assetMenuSettings.Cursor = System.Windows.Forms.Cursors.Hand;
		this.assetMenuSettings.FlatAppearance.BorderColor = System.Drawing.Color.LavenderBlush;
		this.assetMenuSettings.FlatAppearance.BorderSize = 0;
		this.assetMenuSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
		this.assetMenuSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.assetMenuSettings.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12f, System.Drawing.FontStyle.Bold);
		this.assetMenuSettings.ForeColor = System.Drawing.Color.Black;
		this.assetMenuSettings.Location = new System.Drawing.Point(51, 16);
		this.assetMenuSettings.Name = "assetMenuSettings";
		this.assetMenuSettings.Size = new System.Drawing.Size(35, 30);
		this.assetMenuSettings.TabIndex = 278;
		this.assetMenuSettings.Text = "+";
		this.assetMenuSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.assetMenuSettings.UseVisualStyleBackColor = false;
		this.assetMenuSettings.Click += new System.EventHandler(assetMenuSettings_Click);
		this.btnCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.btnCheck.AutoEllipsis = true;
		this.btnCheck.BackColor = System.Drawing.Color.Linen;
		this.btnCheck.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnCheck.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
		this.btnCheck.FlatAppearance.BorderSize = 0;
		this.btnCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCheck.Font = new System.Drawing.Font("Segoe UI Variable Small", 11.5f);
		this.btnCheck.ForeColor = System.Drawing.Color.Black;
		this.btnCheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnCheck.Location = new System.Drawing.Point(310, 16);
		this.btnCheck.Name = "btnCheck";
		this.btnCheck.Size = new System.Drawing.Size(96, 34);
		this.btnCheck.TabIndex = 207;
		this.btnCheck.Text = "Vérifier";
		this.btnCheck.UseVisualStyleBackColor = false;
		this.btnCheck.Click += new System.EventHandler(btnCheck_Click);
		this.lblDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblDescription.AutoEllipsis = true;
		this.lblDescription.Font = new System.Drawing.Font("Segoe UI Variable Small Semibol", 22.25f, System.Drawing.FontStyle.Bold);
		this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(47, 37, 35);
		this.lblDescription.Location = new System.Drawing.Point(25, 1);
		this.lblDescription.Name = "lblDescription";
		this.lblDescription.Size = new System.Drawing.Size(527, 40);
		this.lblDescription.TabIndex = 302;
		this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panelSettings.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelSettings.BackColor = System.Drawing.Color.Transparent;
		this.panelSettings.Controls.Add(this.cbShowOnlyBadHabits);
		this.panelSettings.Controls.Add(this.rtbLog);
		this.panelSettings.Controls.Add(this.searchTextBox);
		this.panelSettings.Controls.Add(this.assetUndo);
		this.panelSettings.Controls.Add(this.assetDo);
		this.panelSettings.Location = new System.Drawing.Point(25, 308);
		this.panelSettings.Name = "panelSettings";
		this.panelSettings.Size = new System.Drawing.Size(527, 94);
		this.panelSettings.TabIndex = 297;
		this.panelSettings.Visible = false;
		this.cbShowOnlyBadHabits.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.cbShowOnlyBadHabits.AutoSize = true;
		this.cbShowOnlyBadHabits.Font = new System.Drawing.Font("Segoe UI Variable Small", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbShowOnlyBadHabits.ForeColor = System.Drawing.Color.Black;
		this.cbShowOnlyBadHabits.Location = new System.Drawing.Point(4, 68);
		this.cbShowOnlyBadHabits.Name = "cbShowOnlyBadHabits";
		this.cbShowOnlyBadHabits.Size = new System.Drawing.Size(136, 19);
		this.cbShowOnlyBadHabits.TabIndex = 8;
		this.cbShowOnlyBadHabits.TabStop = false;
		this.cbShowOnlyBadHabits.Text = "Afficher uniquement les mauvaises configs";
		this.cbShowOnlyBadHabits.UseVisualStyleBackColor = true;
		this.cbShowOnlyBadHabits.CheckedChanged += new System.EventHandler(cbShowOnlyBadHabits_CheckedChanged);
		this.rtbLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.rtbLog.BackColor = System.Drawing.SystemColors.Control;
		this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.rtbLog.Font = new System.Drawing.Font("Georgia", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.rtbLog.Location = new System.Drawing.Point(3, 7);
		this.rtbLog.Name = "rtbLog";
		this.rtbLog.ReadOnly = true;
		this.rtbLog.Size = new System.Drawing.Size(521, 53);
		this.rtbLog.TabIndex = 305;
		this.rtbLog.Text = "";
		this.searchTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.searchTextBox.BackColor = System.Drawing.Color.White;
		this.searchTextBox.Font = new System.Drawing.Font("Segoe UI Variable Small", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.searchTextBox.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.searchTextBox.Location = new System.Drawing.Point(401, 63);
		this.searchTextBox.Multiline = true;
		this.searchTextBox.Name = "searchTextBox";
		this.searchTextBox.Size = new System.Drawing.Size(116, 25);
		this.searchTextBox.TabIndex = 7;
		this.searchTextBox.TabStop = false;
		this.searchTextBox.Text = "Filtrer";
		this.searchTextBox.Click += new System.EventHandler(searchTextBox_Click);
		this.searchTextBox.TextChanged += new System.EventHandler(searchTextBox_TextChanged);
		this.assetUndo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.assetUndo.BackColor = System.Drawing.Color.FromArgb(254, 252, 250);
		this.assetUndo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.assetUndo.FlatAppearance.BorderColor = System.Drawing.Color.Tan;
		this.assetUndo.FlatAppearance.BorderSize = 0;
		this.assetUndo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SeaShell;
		this.assetUndo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.assetUndo.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.assetUndo.ForeColor = System.Drawing.Color.Tan;
		this.assetUndo.Location = new System.Drawing.Point(252, 62);
		this.assetUndo.Name = "assetUndo";
		this.assetUndo.Size = new System.Drawing.Size(89, 26);
		this.assetUndo.TabIndex = 297;
		this.assetUndo.Text = "Rétablir";
		this.assetUndo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.assetUndo.UseVisualStyleBackColor = false;
		this.assetUndo.Click += new System.EventHandler(assetUndo_Click);
		this.assetDo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.assetDo.BackColor = System.Drawing.Color.FromArgb(254, 252, 250);
		this.assetDo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.assetDo.FlatAppearance.BorderColor = System.Drawing.Color.Tan;
		this.assetDo.FlatAppearance.BorderSize = 0;
		this.assetDo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SeaShell;
		this.assetDo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.assetDo.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.assetDo.ForeColor = System.Drawing.Color.FromArgb(5, 60, 40);
		this.assetDo.Location = new System.Drawing.Point(157, 62);
		this.assetDo.Name = "assetDo";
		this.assetDo.Size = new System.Drawing.Size(89, 28);
		this.assetDo.TabIndex = 296;
		this.assetDo.Text = "Corriger";
		this.assetDo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.assetDo.UseVisualStyleBackColor = false;
		this.assetDo.Click += new System.EventHandler(assetDo_Click);
		this.assetNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.assetNext.AutoSize = true;
		this.assetNext.BackColor = System.Drawing.Color.FromArgb(248, 241, 234);
		this.assetNext.Cursor = System.Windows.Forms.Cursors.Hand;
		this.assetNext.FlatAppearance.BorderColor = System.Drawing.Color.Tan;
		this.assetNext.FlatAppearance.BorderSize = 0;
		this.assetNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SeaShell;
		this.assetNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.assetNext.Font = new System.Drawing.Font("Segoe MDL2 Assets", 15f, System.Drawing.FontStyle.Bold);
		this.assetNext.ForeColor = System.Drawing.Color.Tan;
		this.assetNext.Location = new System.Drawing.Point(541, 452);
		this.assetNext.Name = "assetNext";
		this.assetNext.Size = new System.Drawing.Size(40, 36);
		this.assetNext.TabIndex = 294;
		this.assetNext.Text = "...";
		this.assetNext.UseVisualStyleBackColor = false;
		this.assetNext.Click += new System.EventHandler(assetNext_Click);
		this.assetBack.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.assetBack.AutoSize = true;
		this.assetBack.BackColor = System.Drawing.Color.FromArgb(248, 241, 234);
		this.assetBack.Cursor = System.Windows.Forms.Cursors.Hand;
		this.assetBack.FlatAppearance.BorderColor = System.Drawing.Color.Tan;
		this.assetBack.FlatAppearance.BorderSize = 0;
		this.assetBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SeaShell;
		this.assetBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.assetBack.Font = new System.Drawing.Font("Segoe MDL2 Assets", 15f, System.Drawing.FontStyle.Bold);
		this.assetBack.ForeColor = System.Drawing.Color.Tan;
		this.assetBack.Location = new System.Drawing.Point(498, 452);
		this.assetBack.Name = "assetBack";
		this.assetBack.Size = new System.Drawing.Size(40, 36);
		this.assetBack.TabIndex = 295;
		this.assetBack.Text = "...";
		this.assetBack.UseVisualStyleBackColor = false;
		this.assetBack.Click += new System.EventHandler(assetBack_Click);
		this.extensionsMenuStrip.BackColor = System.Drawing.Color.WhiteSmoke;
		this.extensionsMenuStrip.Font = new System.Drawing.Font("Segoe UI Variable Small", 10.25f);
		this.extensionsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.appInfoToolStripMenuItem });
		this.extensionsMenuStrip.Name = "contextMenuStrip";
		this.extensionsMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.extensionsMenuStrip.Size = new System.Drawing.Size(122, 26);
		this.appInfoToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.appInfoToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
		this.appInfoToolStripMenuItem.Name = "appInfoToolStripMenuItem";
		this.appInfoToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
		this.appInfoToolStripMenuItem.Text = "Infos App";
		this.contextMenuStrip.BackColor = System.Drawing.Color.SeaShell;
		this.contextMenuStrip.Font = new System.Drawing.Font("Segoe UI Variable Small", 10.25f);
		this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripTextBox, this.toolStripComboTemplates });
		this.contextMenuStrip.Name = "contextMenuStrip";
		this.contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStrip.Size = new System.Drawing.Size(261, 75);
		this.toolStripTextBox.AutoSize = false;
		this.toolStripTextBox.AutoToolTip = true;
		this.toolStripTextBox.BackColor = System.Drawing.Color.SeaShell;
		this.toolStripTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.toolStripTextBox.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripTextBox.Margin = new System.Windows.Forms.Padding(1, 5, 1, 1);
		this.toolStripTextBox.Name = "toolStripTextBox";
		this.toolStripTextBox.Size = new System.Drawing.Size(200, 20);
		this.toolStripTextBox.Text = "Charger la config depuis la source :";
		this.toolStripComboTemplates.BackColor = System.Drawing.Color.White;
		this.toolStripComboTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.toolStripComboTemplates.Font = new System.Drawing.Font("Segoe UI Variable Text Semiligh", 11.25f);
		this.toolStripComboTemplates.Margin = new System.Windows.Forms.Padding(2, 2, 2, 15);
		this.toolStripComboTemplates.Name = "toolStripComboTemplates";
		this.toolStripComboTemplates.Size = new System.Drawing.Size(200, 28);
		this.assetMenu.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.assetMenu.AutoSize = true;
		this.assetMenu.BackColor = System.Drawing.Color.Transparent;
		this.assetMenu.Cursor = System.Windows.Forms.Cursors.Hand;
		this.assetMenu.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
		this.assetMenu.FlatAppearance.BorderSize = 0;
		this.assetMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SeaShell;
		this.assetMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.assetMenu.Font = new System.Drawing.Font("Segoe MDL2 Assets", 24.75f);
		this.assetMenu.ForeColor = System.Drawing.Color.FromArgb(40, 37, 35);
		this.assetMenu.Location = new System.Drawing.Point(529, 26);
		this.assetMenu.Name = "assetMenu";
		this.assetMenu.Size = new System.Drawing.Size(43, 43);
		this.assetMenu.TabIndex = 293;
		this.assetMenu.Text = "...";
		this.assetMenu.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.assetMenu.UseVisualStyleBackColor = false;
		this.assetMenu.Click += new System.EventHandler(assetMenu_Click);
		this.border.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.border.BackColor = System.Drawing.Color.Transparent;
		this.border.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.border.Location = new System.Drawing.Point(0, 1);
		this.border.Name = "border";
		this.border.Size = new System.Drawing.Size(583, 1);
		this.border.TabIndex = 296;
		this.linkUpdates.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.linkUpdates.AutoEllipsis = true;
		this.linkUpdates.Font = new System.Drawing.Font("Segoe UI Variable Small Semibol", 8.75f, System.Drawing.FontStyle.Bold);
		this.linkUpdates.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
		this.linkUpdates.LinkColor = System.Drawing.Color.Black;
		this.linkUpdates.Location = new System.Drawing.Point(277, 2);
		this.linkUpdates.Name = "linkUpdates";
		this.linkUpdates.Size = new System.Drawing.Size(304, 19);
		this.linkUpdates.TabIndex = 297;
		this.linkUpdates.TabStop = true;
		this.linkUpdates.Text = "...";
		this.linkUpdates.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.linkUpdates.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.ClientSize = new System.Drawing.Size(584, 572);
		base.Controls.Add(this.linkUpdates);
		base.Controls.Add(this.border);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.assetMenu);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		this.MinimumSize = new System.Drawing.Size(600, 500);
		base.Name = "MainForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		this.Text = "SuperMSConfig";
		base.Shown += new System.EventHandler(MainForm_Shown);
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.panelEngine.ResumeLayout(false);
		this.panelEngine.PerformLayout();
		this.panelSettings.ResumeLayout(false);
		this.panelSettings.PerformLayout();
		this.extensionsMenuStrip.ResumeLayout(false);
		this.contextMenuStrip.ResumeLayout(false);
		this.contextMenuStrip.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
