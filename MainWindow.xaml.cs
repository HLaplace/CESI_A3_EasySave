using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Front
{
    public partial class MainWindow : Window
    {
        private Mutex mutex;

        public MainWindow()
        {
            InitializeComponent();

            mutex = new Mutex(true, "{1A4DB5B6-49B1-4B22-A0FB-C3A39E309684}");

            // Si un autre processus de l'application est en cours d'exécution, fermez cette instance
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                MessageBox.Show("L'application est déjà en cours d'exécution.", "Easysave", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }

            // Initialiser la vue par défaut
            CurrentView = new default_view();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private UserControl currentView;

        public UserControl CurrentView
        {
            get { return currentView; }
            set
            {
                mainContent.Content = value;
                currentView = value;
            }
        }

        public void ChangeView(UserControl newView)
        {
            mainContent.Content = newView;
        }
    }
}
