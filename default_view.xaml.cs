using classe;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Front
{
    public partial class default_view : UserControl
    {
        public ObservableCollection<BackupItem> BackupItems { get; set; }
        public default_view()
        {

            InitializeComponent();
            BackupItems = new ObservableCollection<BackupItem>();
            backupListView.ItemsSource = BackupItems;

            // Ajoutez un exemple d'élément pour la démonstration

            BackupItems.Add(new BackupItem
            {
                Name = "Exemple de sauvegarde",
                Progress = 50,  // Valeur de progression entre 0 et 100
                State = "En cours",  // État de la sauvegarde
                Action = ""  // Action associée
            });

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
                // Changer la vue vers task
                mainWindow.ChangeView(new task());
            }
        }

        public void AddBackupItem(BackupItem backupItem)
        {
            // Utilisez le Dispatcher pour mettre à jour l'interface utilisateur
            Dispatcher.Invoke(() =>
            {
                BackupItems.Add(backupItem);
            });
        }



        private void LancerToutButton_Click(object sender, RoutedEventArgs e)
        {
            // Créez une instance de la classe Log

            Log log = new Log();
            string[] logValues = log.ReadLogFile();
            task task = new task();
            if (logValues != null)
            {
                string T_0 = logValues[1];
                string T_1 = logValues[2];
                SaveTools.saveIHM(task.backupType, task.copyFile, T_1, T_0);
            }
            else
            {
                MessageBox.Show("Une erreur s'est produite lors de la lecture du fichier de log.");
            }
            
        }

        public class BackupItem
        {
            public string Name { get; set; }
            public int Progress { get; set; }
            public string State { get; set; }
            public string Action { get; set; }
        }
    }
}
