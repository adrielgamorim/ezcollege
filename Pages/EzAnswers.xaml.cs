using EzCollege.Models;
using EzCollege.Helpers;
using EzCollege.Requests;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace EzCollege.Pages
{
    public partial class EzAnswers : Page
    {
        private BitmapSource? _clipboardImage;

        public EzAnswers()
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

            GenChatResponseModel? genChatResponseModel = await ChatRequester.GetChatResponse(_clipboardImage);
            string content = genChatResponseModel!.choices!.First().message!.content!;
            if (genChatResponseModel!.provider! == null)
            {
                textBlockResponse.Text = content;
                loadingImg.Visibility = Visibility.Hidden;
                return;
            }

            content = Helper.RemoveWaterMark(genChatResponseModel);

            textBlockResponse.Text = content;
            loadingImg.Visibility = Visibility.Hidden;
        }
    }
}
