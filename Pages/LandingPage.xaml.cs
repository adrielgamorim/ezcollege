using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Media;
using EzCollege.Helpers;

namespace EzCollege.Pages
{
    public partial class LandingPage : Page
    {
        public LandingPage()
        {
            InitializeComponent();
            CheckServiceStatus();
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is NavButton clickedPageButton)
            {
                NavigationService.Navigate(clickedPageButton.NavUri);
            }
        }

        private void Github_Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/adrielgamorim",
                UseShellExecute = true
            });
        }

        private async void Install_Docker_Button(object sender, RoutedEventArgs e)
        {
            loadingImg.Visibility = Visibility.Visible;
            this.IsEnabled = false;
            string consoleResults = await DockerHelper.RunDockerInstallScript();
            dockerInstallationStatus.Text = consoleResults;
            this.IsEnabled = true;
            loadingImg.Visibility = Visibility.Hidden;

            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);
                dockerInstallationStatus.Dispatcher.Invoke(() =>
                {
                    dockerInstallationStatus.Text = "";
                });
            });
        }

        private async void CheckServiceStatus()
        {
            if (await DockerHelper.IsDockerContainerRunning())
                Helper.ChangeTextColor(dockerStatusIcon, Brushes.Green);
            else Helper.ChangeTextColor(dockerStatusIcon, Brushes.Red);
        }
        
        private async void Turn_On_Service_Button(object sender, RoutedEventArgs e)
        {
            loadingImg.Visibility = Visibility.Visible;
            this.IsEnabled = false;
            string consoleResults = await DockerHelper.StartDockerContainer(dockerStatusIcon);
            dockerStatus.Text = consoleResults;
            this.IsEnabled = true;
            loadingImg.Visibility = Visibility.Hidden;

            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);
                dockerStatus.Dispatcher.Invoke(() =>
                {
                    dockerStatus.Text = "";
                });
            });
        }
        
        private async void Turn_Off_Service_Button(object sender, RoutedEventArgs e)
        {
            loadingImg.Visibility = Visibility.Visible;
            this.IsEnabled = false;
            string consoleResults = await DockerHelper.StopDockerContainer(dockerStatusIcon);
            dockerStatus.Text = consoleResults;
            this.IsEnabled = true;
            loadingImg.Visibility = Visibility.Hidden;

            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);
                dockerStatus.Dispatcher.Invoke(() =>
                {
                    dockerStatus.Text = "";
                });
            });
        }
    }
}
