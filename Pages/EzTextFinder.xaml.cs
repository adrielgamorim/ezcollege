using EzCollege.Requests;
using EzCollege.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace EzCollege.Pages
{
    public partial class EzTextFinder : Page
    {
        private BitmapSource? _clipboardImage;

        public EzTextFinder()
        {
            InitializeComponent();
        }

        private void Select_Image_Click(object sender, RoutedEventArgs e)
        {
            _clipboardImage = Helper.GetBitmapImage();
            imageBox.Source = _clipboardImage;
        }

        private async void Send_Request_Click(object sender, RoutedEventArgs e)
        {
            loadingImg.Visibility = Visibility.Visible;
            if (_clipboardImage == null)
            {
                textBlockResponse.Text = "Selecione uma imagem!";
                loadingImg.Visibility = Visibility.Hidden;
                return;
            }
            textBlockResponse.Text = await ImageToTextRequester.GetTextFromImage(_clipboardImage);
            loadingImg.Visibility = Visibility.Hidden;
        }
    }
}
