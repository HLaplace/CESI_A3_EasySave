using classe;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace Front
{
    public partial class default_view : System.Windows.Controls.UserControl
    {
        public static ObservableCollection<BackupItem> BackupItems { get; } = new ObservableCollection<BackupItem>();

        public default_view()
        {
            InitializeComponent();
            backupListView.ItemsSource = BackupItems;
        }

        private void LancerToutButton_Click(object sender, RoutedEventArgs e)
        {
            // Parcourir toutes les tâches affichées dans backupListView
            foreach (var backupItem in BackupItems)
            {

                // Construire le message à afficher, incluant les valeurs des booléens
                string message = $"Nom : {backupItem.Name}\n" +
                                 $"Chemin source : {backupItem.sourceFilePath}\n" +
                                 $"Chemin de sauvegarde : {backupItem.backupDirPath}\n" +
                                 $"Type de sauvegarde : {(backupItem.backupType)}\n" +
                                 $"Action : {(backupItem.copyFile)}\n" +
                                 $"Cryptage : {(backupItem.IsEncrypted)}"; // Ajout de l'état de cryptage;
             //   $"Cryptage : {(backupItem.IsEncrypted ? "Activé" : "Désactivé")}"; // Ajout de l'état de cryptage;

                System.Windows.MessageBox.Show(message, "Détails de la sauvegarde", MessageBoxButton.OK, MessageBoxImage.Information);
                

                Task task = Task.Run(() =>
                {
                    SaveTools.SauvegarderDossier(backupItem.sourceFilePath, backupItem.backupDirPath, backupItem.copyFile, backupItem.backupType, backupItem.IsEncrypted);
                });
            }
        }
            
        private void ParametreButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtenir la fenêtre parente (MainWindow)
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            // Vérifier si la fenêtre parente est non nulle
            if (mainWindow != null)
            {
                // Changer la vue vers ParameterView
                mainWindow.ChangeView(new ParametreWindow());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            // Vérifier si la fenêtre parente est non nulle
            if (mainWindow != null)
            {
                // Créer une nouvelle instance de task
                task newTask = new task();
                mainWindow.ChangeView(newTask);
            }
        }

        private  void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            // Mettre la sauvegarde en pause
           SaveTools.Pause();

            // Afficher un message
            System.Windows.MessageBox.Show("Sauvegarde en pause.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private  void StopButton_Click(object sender, RoutedEventArgs e)
        {
            // Arrêter la sauvegarde
            SaveTools.Stop();

            // Afficher un message
            System.Windows.MessageBox.Show("Sauvegarde arrêtée.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private  void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            // Reprise de la sauvegarde
            SaveTools.Restart();

            // Afficher un message
            System.Windows.MessageBox.Show("Reprise de la sauvegarde.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NotepadButton_Click(object sender, RoutedEventArgs e)
        {
            SaveTools.Stop();
            Process.Start("calc.exe");
        }

        public class BackupItem
        {
            public string Name { get; set; }
            public int Progress { get; set; }
            public string State { get; set; }
            public string Action { get; set; }
            public bool backupType { get; set; }
            public bool copyFile { get; set; }
            public string sourceFilePath { get; set; }
            public string backupDirPath { get; set; }
            public bool viderDossierSource { get; set; }
            public bool IsEncrypted { get; set; }
        }
    }
}
