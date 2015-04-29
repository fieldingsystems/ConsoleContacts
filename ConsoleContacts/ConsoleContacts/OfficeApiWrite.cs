using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleContacts
{
    class OfficeApiWrite
    {
        private const string odata = "@odata.type";
        private const string type = "#Microsoft.Exchange.Services.OData.Model.Contact";
        private const string URI = "https://outlook.office365.com/api/v1.0/me/contacts";

        internal static async Task CreateOfficeContact(Contact contact, string userEmail, string userPassword)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://outlook.office365.com/ews/odata/Me/Contacts"));

            // Add the Authorization header with the basic login credentials.
            var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(userEmail + ":" + userPassword));
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", auth);

            var createResponse = CreateResponse(contact.officeContact);
            string serialize = JsonConvert.SerializeObject(createResponse);

            request.Content = new StringContent(serialize);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.SendAsync(request);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static JObject CreateResponse(OfficeContact contact)
        {
            //brute force insert alot of contact fields
            var createResponse = new JObject();
            if (!String.IsNullOrEmpty(contact.Birthday)) createResponse["Birthday"] = contact.Birthday;
		    if (!String.IsNullOrEmpty(contact.FileAs)) createResponse["FileAs"] = contact.FileAs;
		    if (!String.IsNullOrEmpty(contact.DisplayName)) createResponse["DisplayName"] = contact.DisplayName;
		    if (!String.IsNullOrEmpty(contact.GivenName)) createResponse["GivenName"] = contact.GivenName;
		    if (!String.IsNullOrEmpty(contact.Initials)) createResponse["Initials"] = contact.Initials;
		    if (!String.IsNullOrEmpty(contact.MiddleName)) createResponse["MiddleName"] = contact.MiddleName;
		    if (!String.IsNullOrEmpty(contact.NickName)) createResponse["NickName"] = contact.NickName;
		    if (!String.IsNullOrEmpty(contact.Surname)) createResponse["Surname"] = contact.Surname;
		    if (!String.IsNullOrEmpty(contact.Title)) createResponse["Title"] = contact.Title;
		    if (!String.IsNullOrEmpty(contact.Generation)) createResponse["Generation"] = contact.Generation;
		    if (!String.IsNullOrEmpty(contact.JobTitle)) createResponse["JobTitle"] = contact.JobTitle;
		    if (!String.IsNullOrEmpty(contact.CompanyName)) createResponse["CompanyName"] = contact.CompanyName;
		    if (!String.IsNullOrEmpty(contact.Department)) createResponse["Department"] = contact.Department;
		    if (!String.IsNullOrEmpty(contact.OfficeLocation)) createResponse["OfficeLocation"] = contact.OfficeLocation;
		    if (!String.IsNullOrEmpty(contact.Profession)) createResponse["Profession"] = contact.Profession;
		    if (!String.IsNullOrEmpty(contact.BusinessHomePage)) createResponse["BusinessHomePage"] = contact.BusinessHomePage;
		    if (!String.IsNullOrEmpty(contact.AssistantName)) createResponse["AssistantName"] = contact.AssistantName;
		    if (!String.IsNullOrEmpty(contact.Manager)) createResponse["Manager"] = contact.Manager;
		    if (!String.IsNullOrEmpty(contact.MobilePhone1)) createResponse["MobilePhone1"] = contact.MobilePhone1;
		    if (!String.IsNullOrEmpty(contact.YomiSurname)) createResponse["YomiSurname"] = contact.YomiSurname;
		    if (!String.IsNullOrEmpty(contact.YomiGivenName)) createResponse["YomiGivenName"] = contact.YomiGivenName;
            if (!String.IsNullOrEmpty(contact.YomiCompanyName)) createResponse["YomiCompanyName"] = contact.YomiCompanyName;

            // insert the list of strings
            if (contact.HomePhones != null && contact.HomePhones.Count != 0) createResponse = CheckList(contact.HomePhones, createResponse, "HomePhones");
            if (contact.BusinessPhones != null && contact.BusinessPhones.Count != 0) createResponse = CheckList(contact.BusinessPhones, createResponse, "BusinessPhones");
            if (contact.ImAddresses != null && contact.ImAddresses.Count != 0) createResponse = CheckList(contact.ImAddresses, createResponse, "ImAddresses");

            // insert email addresses
            if (contact.EmailAddresses != null && contact.EmailAddresses.Count != 0) createResponse = CreateEmails(contact.EmailAddresses, createResponse);

            // insert address
            if (contact.HomeAddress != null) createResponse = CreateAddress(contact.HomeAddress, createResponse, "HomeAddress");
            if (contact.BusinessAddress != null) createResponse = CreateAddress(contact.BusinessAddress, createResponse, "BusinessAddress");
            if (contact.OtherAddress != null) createResponse = CreateAddress(contact.OtherAddress, createResponse, "OtherAddress");

            return createResponse;
        }

        private static JObject CreateAddress(OfficeAddress officeAddress, JObject createResponse, string field)
        {
            if (String.IsNullOrEmpty(officeAddress.City) && String.IsNullOrEmpty(officeAddress.CountryOrRegion) && String.IsNullOrEmpty(officeAddress.PostalCode) &&
                String.IsNullOrEmpty(officeAddress.State) && String.IsNullOrEmpty(officeAddress.Street)) return createResponse;

            JObject address = new JObject();

            if (!String.IsNullOrEmpty(officeAddress.Street)) address["Street"] = officeAddress.Street;
            if (!String.IsNullOrEmpty(officeAddress.State)) address["State"] = officeAddress.State;
            if (!String.IsNullOrEmpty(officeAddress.PostalCode)) address["PostalCode"] = officeAddress.PostalCode;
            if (!String.IsNullOrEmpty(officeAddress.CountryOrRegion)) address["CountryOrRegion"] = officeAddress.CountryOrRegion;
            if (!String.IsNullOrEmpty(officeAddress.City)) address["City"] = officeAddress.City;

            createResponse[field] = address;

            return createResponse;
        }

        private static JObject CreateEmails(List<OfficeEmail> list, JObject createResponse)
        {
            // avoid nulls
            List<JObject> jobjectList = new List<JObject>();

            foreach(OfficeEmail email in list)
            {
                if (email != null)
                {
                    if(!String.IsNullOrEmpty(email.Name) && !string.IsNullOrEmpty(email.Address))
                    {
                        JObject tempEmail = new JObject();

                        tempEmail["Address"] = email.Address;
                        tempEmail["Name"] = email.Name;

                        jobjectList.Add(tempEmail);
                    }
                }
            }

            if(jobjectList != null && jobjectList.Count != 0)
            {
                JArray array = new JArray();

                foreach (JObject email in jobjectList)
                    array.Add(email);

                createResponse["EmailAddresses"] = array;
            }

            return createResponse;
        }

        private static JObject CheckList(List<string> list, JObject createResponse, string field)
        {
            bool goodList = false;

            foreach (String str in list)
                if (!String.IsNullOrEmpty(str)) goodList = true;

            if(goodList)
            {
                JArray array = new JArray();

                foreach (String str in list)
                    if (!String.IsNullOrEmpty(str)) array.Add(str);

                createResponse[field] = array;
            }

            return createResponse;
        }
    }
}
