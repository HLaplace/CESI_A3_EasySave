using classe;
using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using static Front.default_view;

namespace Front
{
    public delegate void BackupLogDelegate(string sourceFilePath, string backupDirPath, string backupName);

    public partial class task : System.Windows.Controls.UserControl
    {
        public BackupLogDelegate BackupLog { get; set; }

        public bool copyFile { get; set; }
        public bool isFullBackup;

        public bool backupType { get; set; }
        private bool isEncrypted { get; set; }

        Config config = Config.LoadConfig();

        private string backupName;
        private string sourceFilePath;
        private string backupDirPath;

        private System.Windows.Controls.TextBox sourceFileTextBox;
        private System.Windows.Controls.TextBox sourceFileTextBox_Copy;

        public task()
        {
            InitializeComponent();

            titre.Text = "Titre test de la fenetre"; // titre
            action.Text = JsonTemplate.GetJson(config.Langue, "choicecutcopy");
            source.Text = JsonTemplate.GetJson(config.Langue, "inputmessage");
            type.Text = JsonTemplate.GetJson(config.Langue, "savetype");

            Log logInstance = new Log();

            BackupLog = logInstance.SaveLog;

            sourceFileTextBox = FindName("sourceFileTextBox") as System.Windows.Controls.TextBox;
            sourceFileTextBox_Copy = FindName("sourceFileTextBox_Copy") as System.Windows.Controls.TextBox;

        }

        private void OnValidateButtonClick(object sender, RoutedEventArgs e)
        {
            backupName = nameFileTextBox_Copy.Text;

            // Créez une nouvelle tâche de sauvegarde
            BackupItem newBackupItem = new BackupItem
            {
                Name = backupName,
                Progress = 0,
                State = "En attente",
                Action = (isFullBackup ? "Backup complet" : "Backup Differentiel"),
                backupType = backupType, // Ajoutez ces lignes pour attribuer les valeurs appropriées
                copyFile = copyFile,
                sourceFilePath = sourceFilePath,
                backupDirPath = backupDirPath,
                IsEncrypted = isEncrypted
            };

            // Utilisez le délégué pour enregistrer les informations de sauvegarde
            BackupLog(sourceFilePath, backupDirPath, backupName);

            List<BackupItem> backupItemList = new List<BackupItem>();
            backupItemList.Add(newBackupItem);

            // Afficher les informations dans une MessageBox
            //string message = $"Nom de sauvegarde : {backupName}\nChemin source : {sourceFilePath}\nChemin de sauvegarde : {backupDirPath}";
            //System.Windows.MessageBox.Show(message, "Informations de sauvegarde", MessageBoxButton.OK, MessageBoxImage.Information);

            Dispatcher.Invoke(() =>
            {
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null)
                {
                    mainWindow.CurrentView = new default_view();
                    if (mainWindow.CurrentView is default_view defaultView)
                    {
                        default_view.BackupItems.Add(newBackupItem);
                    }
                }

                // Revenir à la page précédente après avoir ajouté la tâche
                RetourButton_Click(sender, e);
            });
        }



        private void SelectSourceFileButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath;

                // Afficher le chemin du dossier sélectionné dans une MessageBox
               // System.Windows.MessageBox.Show("Chemin du dossier sélectionné : " + selectedPath, "Chemin Sélectionné", MessageBoxButton.OK, MessageBoxImage.Information);

                // Vérifier si le chemin sélectionné est un fichier
                if (File.Exists(selectedPath))
                {
                    // Afficher une MessageBox pour informer l'utilisateur
                    System.Windows.MessageBox.Show("Veuillez choisir un dossier au lieu d'un fichier.", "Erreur de sélection", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Le chemin sélectionné est un dossier
                    sourceFilePath = selectedPath;
                    if (sourceFileTextBox != null)
                    {
                        sourceFileTextBox.Text = sourceFilePath;
                    }
                }
            }
        }

        private void SelectBackupDirButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath;

                // Afficher le chemin du dossier sélectionné dans une MessageBox
                //System.Windows.MessageBox.Show("Chemin du dossier sélectionné : " + selectedPath, "Chemin Sélectionné", MessageBoxButton.OK, MessageBoxImage.Information);

                // Vérifier si le chemin sélectionné est un fichier
                if (File.Exists(selectedPath))
                {
                    // Afficher une MessageBox pour informer l'utilisateur
                    System.Windows.MessageBox.Show("Veuillez choisir un dossier au lieu d'un fichier.", "Erreur de sélection", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Le chemin sélectionné est un dossier
                    backupDirPath = selectedPath;
                    if (sourceFileTextBox_Copy != null)
                    {
                        sourceFileTextBox_Copy.Text = backupDirPath;
                    }
                }
            }
        }

        private void ResetBackupWindow()
        {
            //sourceFileTextBox.Text = string.Empty;
            //sourceFileTextBox_Copy.Text = string.Empty;
        }

        private void OneCancelButtonClick(object sender, RoutedEventArgs e)
        {
            ResetBackupWindow();
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ChangeView(new default_view());
            }
        }






        private void nameTextBox_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;

            if (textBox != null)
            {
                string texteSaisi = textBox.Text;
                Console.WriteLine("Texte saisi : " + texteSaisi);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            isEncrypted  = true;
            UpdateBackupItemCrpyt();
        }

        private void completeBackupRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            backupType = false;

            // Si une tâche est déjà créée, mettre à jour le type de sauvegarde de l'objet BackupItem
            UpdateBackupItemBackupType();
        }

        private void incrementalBackupRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            backupType = true;

            // Si une tâche est déjà créée, mettre à jour le type de sauvegarde de l'objet BackupItem
            UpdateBackupItemBackupType();
        }

        private void copyRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            copyFile = false;

            // Si une tâche est déjà créée, mettre à jour l'action de l'objet BackupItem
            UpdateBackupItemAction();
        }

        private void pasteRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            copyFile = true;

            // Si une tâche est déjà créée, mettre à jour l'action de l'objet BackupItem
            UpdateBackupItemAction();
        }

        private void UpdateBackupItemBackupType()// Mettre à jour la propriété backupType pour toutes les tâches dans default_view.BackupItems
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null && mainWindow.CurrentView is default_view defaultView)
            {
                foreach (var backupItem in default_view.BackupItems)
                {
                    backupItem.backupType = backupType;
                }
            }
        }

        private void UpdateBackupItemAction()// Mettre à jour la propriété Action pour toutes les tâches dans default_view.BackupItems
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null && mainWindow.CurrentView is default_view defaultView)
            {
                foreach (var backupItem in default_view.BackupItems)
                {
                    backupItem.Action = (copyFile ? "Copier" : "Couper");
                }
            }
        }

        private void UpdateBackupItemCrpyt()// Mettre à jour la propriété Action pour toutes les tâches dans default_view.BackupItems
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null && mainWindow.CurrentView is default_view defaultView)
            {
                foreach (var backupItem in default_view.BackupItems)
                {
                    backupItem.IsEncrypted = isEncrypted;
                }
            }
        }



    }
}
