using EzCollege.Helpers;
using EzCollege.Models;
using EzCollege.Requests;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace EzCollege.Pages
{
    public partial class EzChat : Page
    {
        private static readonly string _userName = "Você";

        public EzChat()
        {
            InitializeComponent();
        }

        private async void sendChatButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && !IsShiftDown())
            {
                await GetChatResponse();
            }
            else if (e.Key.Equals(Key.Enter) && IsShiftDown())
            {
                chatTextBox.AppendText(Environment.NewLine);
                chatTextBox.CaretIndex = chatTextBox.Text.Length;
            }
        }

        private async void sendChatButton_Click(object sender, RoutedEventArgs e)
        {
            await GetChatResponse();
        }

        private async Task GetChatResponse()
        {
            if (chatTextBox.Text.Equals(String.Empty)) return;
            loadingImg.Visibility = Visibility.Visible;
            chatResponseTextBox.Text += GenerateStructuredChatResponse(_userName, chatTextBox.Text);
            chatResponseTextBox.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1)));
            chatResponseTextBox.ScrollToEnd();

            string textBoxContent = chatTextBox.Text;
            bool isQuestionsExpert = (bool)isQuestionExpert.IsChecked!;

            chatTextBox.Clear();
            this.IsEnabled = false;

            GenChatResponseModel? chatResponse = await ChatRequester.GetChatResponse(textBoxContent, isQuestionsExpert);
            string? content = Helper.RemoveWaterMark(chatResponse!);
            chatResponseTextBox.Text += GenerateStructuredChatResponse(chatResponse!.provider!, content);
            chatResponseTextBox.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1)));
            chatResponseTextBox.ScrollToEnd();
            loadingImg.Visibility = Visibility.Hidden;

            this.IsEnabled = true;
            chatTextBox.Focus();
        }

        private string GenerateStructuredChatResponse(string entity, string content)
        {
            StringBuilder structuredResponse = new();
            structuredResponse.Append($"{ entity }:");
            structuredResponse.Append(Environment.NewLine);
            structuredResponse.Append(content);
            structuredResponse.Append(Environment.NewLine);
            structuredResponse.Append(Environment.NewLine);

            return structuredResponse.ToString();
        }

        private bool IsShiftDown()
        {
            return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        }
    }
}
