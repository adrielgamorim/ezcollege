using System.Net.Http;
using System.Windows.Media.Imaging;

namespace EzCollege.Models
{
    public class GenChatModel
    {
        public string model { get; set; }
        public object[] messages { get; set; }

        public GenChatModel(string model, string message)
        {
            this.model = model;
            this.messages = [
                new {
                    role = "user",
                    content = message
                }
            ];
        }
    }
}
