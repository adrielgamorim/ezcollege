using System.Net.Http;
using System.Windows.Media.Imaging;

namespace EzCollege.Models
{
    public class GenChatResponseModel
    {
        public string? id { get; set; }
        public string? model { get; set; }
        public string? provider { get; set; }
        public Choices[]? choices { get; set; }
    }

    public class Choices
    {
        public int? index { get; set; }
        public Message? message { get; set; }
    }

    public class Message
    {
        public string? role { get; set; }
        public string? content { get; set; }
    }
}
