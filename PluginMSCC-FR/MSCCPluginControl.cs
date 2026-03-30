using Newtonsoft.Json;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSCleanupCompanion
{
    public partial class MSCCPluginControl : UserControl, IPlugin
    {
        private List<AppInfo> originalAppsInfo = new List<AppInfo>();
        private bool selectAll = true;

        public UserControl GetControl()
        {
            return this;
        }

        public string PluginName => "Microsoft Cleanup Companion";
        public string PluginVersion => "1.0";
        public string PluginInfo => "Ce plugin est un outil puissant conçu pour simplifier et optimiser votre expérience sur Windows 10/11. Grâce à des capacités de détection avancées basées sur la communauté, ce plugin vous permet d'identifier et de supprimer facilement les logiciels préinstallés inutiles (bloatware), améliorant ainsi les performances du système et libérant des ressources précieuses.";

        private void ShowPluginInfo()
        {
            string pluginDetails = $"{PluginName}\n" +
                                   $"Version : {PluginVersion}\n\n" +
                                   $"{PluginInfo}";

            MessageBox.Show(pluginDetails, "Informations sur le plugin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public class AppInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string RemoveCommand { get; set; }

            public override string ToString()
            {
                return $"{Name} - {Description}";
            }
        }

        public MSCCPluginControl()
        {
            InitializeComponent();
            SetStyle();
        }

        private void SetStyle()
        {
            // Segoe MDL2 Assets font
            mdlMenu.Text = "\uE712";

            // BackColor
            BackColor =
            checkedListBoxApps.BackColor =
               Color.FromArgb(250, 251, 247);
        }

        private async void toolDebloaterPageView_Load(object sender, EventArgs e)
        {
            await ScanApps();
        }

        private async Task ScanApps()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins\\PluginMSCleanupCompanion", "MSCleanupCompanion.json");
            await LoadAppsFromJson(jsonFilePath);
        }

        private async Task LoadAppsFromJson(string jsonFilePath)
        {
            installedToolStripMenuItem.Enabled = false; // Disable showing all apps checkbox during loading

            try
            {
                string jsonString = File.ReadAllText(jsonFilePath);
                originalAppsInfo = JsonConvert.DeserializeObject<List<AppInfo>>(jsonString);

                checkedListBoxApps.Items.Clear();
                bool bloatwareFound = false;

                foreach (var appInfo in originalAppsInfo)
                {
                    bool isInstalled = await IsAppInstalled(appInfo.Name);
                    if (isInstalled)
                    {
                        checkedListBoxApps.Items.Add(appInfo, false);
                        bloatwareFound = true;
                    }
                }

                // Afficher un message si aucun bloatware n'est trouvé
                if (!bloatwareFound)
                {
                    lblStatus.Text = "Aucune application inutile trouvée.";
                    lblStatus.BackColor = Color.PaleGreen;
                    lblStatus.TextAlign = ContentAlignment.MiddleCenter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du fichier JSON : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            installedToolStripMenuItem.Enabled = true;
        }

        // Chargement manuel de toutes les applications installées pour suppression
        private async Task LoadAllInstalledApps()
        {
            try
            {
                checkedListBoxApps.Items.Clear();
                originalAppsInfo.Clear();

                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.AddScript("Get-AppxPackage | Select-Object Name, PackageFullName");
                    var results = await Task.Run(() => powerShell.Invoke());

                    foreach (var result in results)
                    {
                        var appName = result.Members["Name"].Value.ToString();
                        var appPackageFullName = result.Members["PackageFullName"].Value.ToString();
                        var appInfo = new AppInfo { Name = appName, Description = appPackageFullName, RemoveCommand = $"Get-AppxPackage -Name {appName} | Remove-AppxPackage" };
                        originalAppsInfo.Add(appInfo);
                        checkedListBoxApps.Items.Add(appInfo, false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des applications installées : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> IsAppInstalled(string appName)
        {
            lblStatus.BackColor = Color.FromArgb(234,240,227);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            bool isInstalled = false;
            try
            {
                lblStatus.Text = $"Vérification de {appName}...";
                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.AddScript($"Get-AppxPackage -Name *{appName}*");
                    var results = await Task.Run(() => powerShell.Invoke());

                    isInstalled = results.Count > 0;
                }
                lblStatus.Text = $"Vérification de {appName} terminée.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la vérification de l'application {appName} : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblStatus.Text = "Vérification terminée.";
            return isInstalled;
        }

        private async Task ExecutePowerShellCommand(string command)
        {
            try
            {
                using (PowerShell ps = PowerShell.Create())
                {
                    ps.AddScript(command);
                    await Task.Run(() => ps.Invoke());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exécution de la commande PowerShell : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            // S'assurer qu'au moins une application est cochée
            if (checkedListBoxApps.CheckedItems.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner au moins une application à supprimer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Désactiver le bouton de suppression pendant le processus
            btnRemoveSelected.Enabled = false;

            // Créer une copie des éléments cochés pour itérer
            var checkedItems = checkedListBoxApps.CheckedItems.Cast<AppInfo>().ToList();

            // Itérer à travers les éléments cochés et exécuter la commande de suppression de façon asynchrone
            foreach (var checkedItem in checkedItems)
            {
                AppInfo appInfo = checkedItem as AppInfo;
                if (appInfo != null)
                {
                    // Suppression en cours
                    lblStatus.Text = $"Suppression de {appInfo.Name}...";
                    await ExecutePowerShellCommand(appInfo.RemoveCommand);
                    lblStatus.Text = $"{appInfo.Name} supprimé.";

                    // Supprimer l'application de la CheckedListBox après suppression
                    checkedListBoxApps.Items.Remove(appInfo);
                }
            }

            // Réactiver le bouton de suppression après la fin du processus
            btnRemoveSelected.Enabled = true;

            lblStatus.Text = "Applications supprimées avec succès.";
            MessageBox.Show("Applications supprimées avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Tout sélectionner ou tout désélectionner
        private void selectAllStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxApps.Items.Count; i++)
            {
                checkedListBoxApps.SetItemChecked(i, selectAll);
            }
            selectAll = !selectAll;
            selectAllToolStripMenuItem.Text = selectAll ? "Tout sélectionner" : "Tout désélectionner";
        }

        // Afficher toutes les applications installées
        private async void installedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Basculer l'état de la coche
            installedToolStripMenuItem.Checked = !installedToolStripMenuItem.Checked;

            if (installedToolStripMenuItem.Checked)
            {
                // Charger toutes les applications installées
                await LoadAllInstalledApps();
            }
            else
            {
                // Charger les bloatwares depuis la base JSON
                string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins\\PluginMSCleanupCompanion", "MSCleanupCompanion.json");
                await LoadAppsFromJson(jsonFilePath);
            }
        }

        // Charger une base de données JSON personnalisée
        private async void customDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers JSON|*.json";
            openFileDialog.Title = "Sélectionner un fichier JSON";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string jsonFilePath = openFileDialog.FileName;
                await LoadAppsFromJson(jsonFilePath);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBoxSearch.Text.ToLower();
            checkedListBoxApps.Items.Clear();

            foreach (var appInfo in originalAppsInfo)
            {
                if (appInfo.Name.ToLower().Contains(searchText) || appInfo.Description.ToLower().Contains(searchText))
                {
                    checkedListBoxApps.Items.Add(appInfo, false);
                }
            }
        }

        private void textBoxSearch_Click(object sender, EventArgs e)
        {
            textBoxSearch.Clear();
        }

        private void mdlMenu_Click(object sender, EventArgs e)
        {
            this.contextMenu.Show(Cursor.Position.X, Cursor.Position.Y);
        }

        private void pluginInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPluginInfo();
        }

        private async void scanAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ScanApps();
        }
    }
}