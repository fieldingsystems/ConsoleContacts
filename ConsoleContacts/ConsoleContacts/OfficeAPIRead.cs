using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContacts
{
    class OfficeAPIRead
    {
        private const string URL = "https://outlook.office365.com/api/v1.0/me/contacts/?$top=50&$skip=";

        public static async Task<OfficeContactCollection> GetContactList(string userEmail, string userPassword, int skip)
        {
            // hold the current collection returned by the api
            OfficeContactCollection tempOfficeContacts = new OfficeContactCollection();

            // format the request
            var client = new HttpClient();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("{0}{1}", URL, skip));
            var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(userEmail + ":" + userPassword));
            request.Headers.Add("Authorization", auth);

            // try and read it, if it comes back as a success then we use it its contents otherwise
            // the null string gets serialzed. to an empty collection
            string result = null;
            try
            {
                using (HttpWebResponse resp = await request.GetResponseAsync() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(resp.GetResponseStream());
                    result = reader.ReadToEnd();
                }
            }
            catch (System.Net.WebException web)
            {
                Console.WriteLine(web.ToString());
            }

            // serialize and return the contacts
            tempOfficeContacts = JsonSerializer.DeserializeFromString<OfficeContactCollection>(result);
            
            // print results to console
            int count = 0;
            foreach (OfficeContact contact in tempOfficeContacts.value)
            {
                count++;
                Console.WriteLine("Office Contact: " + (count + skip) + "\t" + contact.DisplayName);
            }

            return tempOfficeContacts;
        }

        public static async Task<int> GetNumberOfContacts(string userName, string password)
        {
            // hold the current collection returned by the api
            int numberOfContacts = new int();

            // format the request
            var client = new HttpClient();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://outlook.office365.com/api/v1.0/me/contacts/$count"));
            var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(userName + ":" + password));
            request.Headers.Add("Authorization", auth);

            // try and read it, if it comes back as a success then we use it its contents otherwise
            // the null string gets serialzed. to an empty collection
            string result = null;
            try
            {
                using (HttpWebResponse resp = await request.GetResponseAsync() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(resp.GetResponseStream());
                    result = reader.ReadToEnd();
                }
            }
            catch (System.Net.WebException web)
            {
                Console.WriteLine(web.ToString());
            }

            // serialize and return the contacts
            numberOfContacts = JsonSerializer.DeserializeFromString<int>(result);
            return numberOfContacts;
        }
    }
}
