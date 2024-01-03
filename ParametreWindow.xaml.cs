using classe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Front
{
    /// <summary>
    /// Logique d'interaction pour Parameterwindow.xaml
    /// </summary>
    public partial class ParametreWindow : UserControl
    {
        Config config = Config.LoadConfig();


        public ParametreWindow()
        {
            InitializeComponent();

            configurationTextBlock.Text = JsonTemplate.GetJson(config.Langue, "config");
            langueTextBlock.Text = JsonTemplate.GetJson(config.Langue, "defaultlang");
            fichierLogTextBlock.Text = JsonTemplate.GetJson(config.Langue, "log_type");
            ValiderButton.Content = JsonTemplate.GetJson(config.Langue, "validate");
            AnnulerButton.Content = JsonTemplate.GetJson(config.Langue, "annul");
            RetourButton.Content = JsonTemplate.GetJson(config.Langue, "back");

            languageComboBox = this.FindName("languageComboBox") as ComboBox;
            jsonLogCheckBox = this.FindName("jsonLogCheckBox") as CheckBox;
            xmlLogCheckBox = this.FindName("xmlLogCheckBox") as CheckBox;

        }


        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            languageComboBox.SelectedIndex = 0;
            jsonLogCheckBox.IsChecked = false;
            xmlLogCheckBox.IsChecked = false;
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Récupérez l'élément sélectionné
            ComboBoxItem selectedItem = (ComboBoxItem)languageComboBox.SelectedItem;

            if (selectedItem != null)
            {
                string selectedLanguageCode = selectedItem.Tag.ToString();

                // Utilisez la valeur sélectionnée comme nécessaire
                langueTextBlock.Text = JsonTemplate.GetJson(selectedLanguageCode, "setlang");
                configurationTextBlock.Text = JsonTemplate.GetJson(selectedLanguageCode, "config");
                fichierLogTextBlock.Text = JsonTemplate.GetJson(selectedLanguageCode, "log_type");
                ValiderButton.Content = JsonTemplate.GetJson(selectedLanguageCode, "validate");
                AnnulerButton.Content = JsonTemplate.GetJson(selectedLanguageCode, "annul");
                RetourButton.Content = JsonTemplate.GetJson(selectedLanguageCode, "back");
                config.Langue = selectedLanguageCode;
                Config.SaveConfig(config);
            }
        }
        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtenez la fenêtre parente (MainWindow)
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                // Changez la vue vers la default_view
                mainWindow.ChangeView(new default_view());
            }
        }

        private void jsonLogRadioButton_Copy_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void OnValidateButtonClick(object sender, RoutedEventArgs e)
        {
            // Récupérer les choix de configuration
            bool logJson = jsonLogCheckBox.IsChecked ?? false;
            bool logXml = xmlLogCheckBox.IsChecked ?? false;
            bool logTxt = txtLogCheckBox.IsChecked ?? false;

            // Utiliser la classe Log pour configurer les logs
            ConfigurerLogs(logJson, logXml, logTxt);

            // Autres actions à effectuer lors de la validation
        }


        private void ConfigurerLogs(bool logJson, bool logXml, bool logTxt)
        {
            // Instancier la classe Log
            Log logger = new Log();

            // Mettre à jour les paramètres de la classe Log
            //logger.ConfigurerTypeLog(logJson, logXml, logTxt);
        }

        private void jsonLogCheckBox_Checked(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
