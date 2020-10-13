using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GuestGroupChat.Services
{
    public static class ChatService
    {
        private static readonly string _apiLink = "https://api.chat-api.com/instance182160/";
        private static readonly string _token = "9qjeeasoqn6xv7yl";

        public static async System.Threading.Tasks.Task<string> CreateNewChatAsync(DateTime reservationDate)
        {
             var phone = "4407378985181";
            var data = new Dictionary<string, string>()
            {
                { "groupName", "Restaurant Reservation" },
                { "phones", phone },
                { "messageText", "" }
            };
            return await SendRequest("group", JsonConvert.SerializeObject(data));
        }

        public static async Task<string> SendMessage(string chatId, string text)
        {
            var data = new Dictionary<string, string>()
            {
                {"chatId",chatId },
                { "body", text }
            };
            return await SendRequest("sendMessage", JsonConvert.SerializeObject(data));
        }
        private static async Task<string> SendRequest(string method, string data)
        {
            string url = $"{_apiLink}{method}?token={_token}";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("", content);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
