using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Media.Imaging;
using EzCollege.Models;
using EzCollege.Helpers;
using System.Text.Json;

namespace EzCollege.Requests
{
    class ChatRequester
    {
        private const string API_URL = "http://localhost:1337/v1/chat/completions";
        private static readonly string _model = "gpt-4";
        private static readonly string _systemPrompt =
            "A partir de agora você vai agir como um profissional em responder questões.\n" +
            "Você vai receber uma ou mais questões de múltipla escolha.\n" +
            "O seu trabalho é identificar se uma ou mais questões foram perguntadas, e responder uma após a outra.\n" +
            "Responda cada questão com todo o conteúdo da resposta, e nada mais.\n" +
            "Não responda apenas 'a', ou '2'.\n" +
            "Seja objetivo com a resposta.\n" +
            "Aqui vão as perguntas:\n";

        public ChatRequester()
        {
        }

        public static async Task<GenChatResponseModel?> GetChatResponse(BitmapSource image)
        {
            using HttpClient client = new();

            string textFromImage = await ImageToTextRequester.GetTextFromImage(image);
            string prompt = $"{_systemPrompt} {textFromImage}";
            GenChatModel payload = new(_model, prompt);

            try
            {
                var response = await client.PostAsync(API_URL, JsonContent.Create(payload));

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<GenChatResponseModel>();
                }
                else
                {
                    GenChatResponseModel errorResponse =
                        Helper.GenerateErrorGenChatResponseModel(content: "Erro ao obter resposta do servidor!");
                    return errorResponse;
                }
            }
            catch
            {
                GenChatResponseModel errorResponse =
                    Helper.GenerateErrorGenChatResponseModel(content: "Um erro ocorreu ao tentar se conectar ao servidor!");
                return errorResponse;
            }
        }

        public static async Task<GenChatResponseModel?> GetChatResponse(string prompt, bool isQuestionsExpert = true)
        {
            using HttpClient client = new();

            
            string finalPrompt = prompt;
            if (isQuestionsExpert) finalPrompt = $"{_systemPrompt} {prompt}";
            GenChatModel payload = new(_model, finalPrompt);

            try
            {
                var response = await client.PostAsync(API_URL, JsonContent.Create(payload));

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new()
                    {
                        WriteIndented = true
                    };

                    return await response.Content.ReadFromJsonAsync<GenChatResponseModel>(
                        new JsonSerializerOptions() { IncludeFields = true });
                }
                else
                {
                    GenChatResponseModel errorResponse =
                        Helper.GenerateErrorGenChatResponseModel(content: "Erro ao obter resposta do servidor!");
                    return errorResponse;
                }
            }
            catch
            {
                GenChatResponseModel errorResponse =
                    Helper.GenerateErrorGenChatResponseModel(content: "Um erro ocorreu ao tentar se conectar ao servidor!");
                return errorResponse;
            }
        }
    }
}
