using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContacts
{
    class SendEmailLog
    {
        public const string userEmail = "alex.compton@fieldingsystems.com";
        public const string userPassword = "A1s2d3f4!";


        internal static async Task SendLog(string Content)
        {
            string json = "{\"Message\":{\"Subject\":\"Contact Sync Log\",\"Body\":{\"ContentType\":\"Text\",\"Content\":\"" +
                Content + "\"},\"ToRecipients\":[{\"EmailAddress\":{\"Address\":\"alexcompton11@gmail.com\"}}]},\"SaveToSentItems\":\"false\"}";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://outlook.office365.com/api/v1.0/me/sendmail"));

            // Add the Authorization header with the basic login credentials.
            var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(userEmail + ":" + userPassword));
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", auth);

            request.Content = new StringContent(json);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.SendAsync(request);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine("Logging Failed...");
                Console.WriteLine(e);
            }
        }
    }
}
