using classe;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Front.default_view;


namespace Front
{
    public partial class task : UserControl
    {
        //private RadioButton copyRadioButton;
        //    private RadioButton pasteRadioButton;
        //   private RadioButton completeBackupRadioButton;
        //private RadioButton incrementalBackupRadioButton;

        public bool copyFile { get; set; }
        public bool isFullBackup;

        public bool backupType { get; set; }

        public TextBox backupNameTextBox;
        public TextBox sourceFileTextBox_x;
        public TextBox sourceFileTextBox_Copy_xx;

        public string backupName;
        public string sourceFilePath;
        public string backupDirPath;

        Config config = Config.LoadConfig();

        public task()
        {
            InitializeComponent();

            // Initialisez les références aux contrôles XAML
            copyRadioButton = FindName("copyRadioButton") as RadioButton;
            pasteRadioButton = FindName("pasteRadioButton") as RadioButton;
            completeBackupRadioButton = FindName("completeBackupRadioButton") as RadioButton;
            incrementalBackupRadioButton = FindName("incrementalBackupRadioButton") as RadioButton;


            titre.Text = ""; // titre
            action.Text = JsonTemplate.GetJson(config.Langue, "choicecutcopy");
            source.Text = JsonTemplate.GetJson(config.Langue, "inputmessages");
            target.Text = JsonTemplate.GetJson(config.Langue, "inputmessage");
            type.Text = JsonTemplate.GetJson(config.Langue, "savetype");

            backupNameTextBox = FindName("nameTextBox_Copy") as TextBox; // Ajout de la référence à la TextBox
            sourceFileTextBox_x = FindName("sourceFileTextBox") as TextBox;
            sourceFileTextBox_Copy_xx = FindName("sourceFileTextBox_Copy") as TextBox;

            backupName = string.Empty;
            sourceFilePath = string.Empty;
            backupDirPath = string.Empty;
        }

        private void OnValidateButtonClick(object sender, RoutedEventArgs e)
        {
            backupName = nameFileTextBox_Copy.Text;
            sourceFilePath = sourceFileTextBox.Text;
            backupDirPath = sourceFileTextBox_Copy.Text;

            // Créez une nouvelle tâche de sauvegarde
            BackupItem newBackupItem = new BackupItem
            {
                Name = backupName,
                Progress = 0,
                State = "En attente",
                Action = (isFullBackup ? "Backup complet" : "Backup Differentiel")
            };
            List<BackupItem> backupItemList = new List<BackupItem>();

            Log log = new Log();

            log.SaveLog(sourceFilePath, backupDirPath, backupName);


            // Créer une nouvelle instance de BackupItem

            // Ajouter l'élément à la liste
            backupItemList.Add(newBackupItem);
            // Utilisez le Dispatcher pour mettre à jour l'interface utilisateur
            Dispatcher.Invoke(() =>
            {
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null)
                {
                    mainWindow.CurrentView = new default_view();
                    if (mainWindow.CurrentView is default_view defaultView)
                    {
                        defaultView.AddBackupItem(newBackupItem);
                    }
                }
            });
        }

        private bool GetRadioButtonCheckedState(StackPanel group)
        {
            // Vérifiez si un RadioButton du groupe est sélectionné
            foreach (RadioButton radioButton in group.Children)
            {
                if (radioButton.IsChecked == true)
                {
                    return true;
                }
            }
            return false;
        }

        // Fonction pour réinitialiser la page BackupWindow
        private void ResetBackupWindow()
        {
            //copyRadioButton.IsChecked = true;
            sourceFileTextBox.Text = string.Empty;
            sourceFileTextBox_Copy.Text = string.Empty;
            //completeBackupRadioButton.IsChecked = true;
        }

        private void OneCancelButtonClick(object sender, RoutedEventArgs e)
        {
            ResetBackupWindow();
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtenez la fenêtre parente (MainWindow)
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            // Vérifiez si la fenêtre parente est non nulle
            if (mainWindow != null)
            {
                // Changez la vue vers la default_view
                mainWindow.ChangeView(new default_view());
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Inverser l'état de cryptage à chaque clic
            isEncrypted = !isEncrypted;

            // Mettre à jour le texte du bouton
            UpdateSaveButtonContent();

            // Logique pour effectuer la sauvegarde en fonction de l'état de cryptage
            if (isEncrypted)
            {
                // Sauvegarde cryptée
            }
            else
            {
                // Sauvegarde en clair
            }
        }

        private bool isEncrypted = false;

        private void UpdateSaveButtonContent()
        {
            //saveButton.Content = isEncrypted ? "Sauvegarder crypté" : "Sauvegarder en clair";
        }

        private void sourceFileTextBox_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void nameTextBox_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Convertir le sender en TextBox
            TextBox textBox = sender as TextBox;

            // Vérifier si la conversion a réussi
            if (textBox != null)
            {
                // Récupérer le texte du TextBox
                string texteSaisi = textBox.Text;

                // Faire quelque chose avec le texte récupéré
                Console.WriteLine("Texte saisi : " + texteSaisi);
            }

        }



        private void sourceFileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void completeBackupRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            backupType = false;
        }

        private void incrementalBackupRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            backupType = true;
        }

        private void copyRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            copyFile = false;
        }

        private void pasteRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            copyFile = true;
        }
    }
}
