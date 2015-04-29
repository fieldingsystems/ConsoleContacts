using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleContacts
{
    class PipedriveAPIWrite
    {
        public static string sURL = "https://api.pipedrive.com/v1/persons?api_token=";

        internal static void CreatePipedriveContact(Contact contact, string token)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sURL + token);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var createResponse = CreateResponse(contact.pipedriveContact);
                    string json = JsonConvert.SerializeObject(createResponse);
                    json = FormatCustomFields(json);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static string FormatCustomFields(string json)
        {
            json = json.Replace("contact_type", "8f5b148f7f87b73615c1b0601ad807e0e5f7067f");
            json = json.Replace("marketing_permission", "258dd4a24e0e782635c95287fc136d3b27a61ee7");
            json = json.Replace("job_title", "ca0f90a971f6b09fbc83aed6399c30db9ab147cc");
            json = json.Replace("decision_role", "d3b90173ad8a7f96188ddfbda75ee033a498a737");
            json = json.Replace("azure_user_id", "04ed97a8cf1db67f3c5c0f61adae077f9d9d3a34");
            json = json.Replace("contact_address", "75c05948e5b44900e2d94ad5f38aa95ba4fd6a6d");
            json = json.Replace("contact_address_1", "5c83bd66965eefafc13d630456b572a3815100c9");
            json = json.Replace("contact_address_2", "97b5993aa9f563d0887464cf8489c84b8107b2e0");
            json = json.Replace("contact_city", "c72c205f12f3164f20502829a9f062ca73e96171");
            json = json.Replace("contact_state", "21289d91acfd40fb7a8eea040a3d5c8ee4722dfe");
            json = json.Replace("contact_zip", "f049560d41b9dbef4c7e2d611c21881d1b620204");
            json = json.Replace("azure_username", "1f28414160f726dbe8b1b1a5e2f5d3587da474bd");
            json = json.Replace("azure_company_id", "04ed97a8cf1db67f3c5c0f61adae077f9d9d3a34");
            json = json.Replace("azure_company_id#", "dc33d3b0ce9b9a8f7790de245d2949ecde06deaa");
            json = json.Replace("azure_comp_3_letter_code", "7d8b40089c25498443ba7f7458162be884fa61f3");
            json = json.Replace("phone_ext", "6b740582caa2acd02e6e9c63d0255a524c4f3525");
            json = json.Replace("system_user", "745cdd3dc558d7f7619d94b0754fe080546933bb");

            return json;
        }

        private static object CreateResponse(PipedriveContact contact)
        {
            // first brute force the strings
            var createResponse = new JObject();
            if (!String.IsNullOrEmpty(contact.contact_type)) createResponse["contact_type"] = contact.contact_type;
            if (!String.IsNullOrEmpty(contact.marketing_permission)) createResponse["marketing_permission"] = contact.marketing_permission;
            if (!String.IsNullOrEmpty(contact.job_title)) createResponse["job_title"] = contact.job_title;
            if (!String.IsNullOrEmpty(contact.decision_role)) createResponse["decision_role"] = contact.decision_role;
            if (!String.IsNullOrEmpty(contact.azure_user_id)) createResponse["azure_user_id"] = contact.azure_user_id;
            if (!String.IsNullOrEmpty(contact.contact_address)) createResponse["contact_address"] = contact.contact_address;
            if (!String.IsNullOrEmpty(contact.contact_address_1)) createResponse["contact_address_1"] = contact.contact_address_1;
            if (!String.IsNullOrEmpty(contact.contact_address_2)) createResponse["contact_address_2"] = contact.contact_address_2;
            if (!String.IsNullOrEmpty(contact.contact_city)) createResponse["contact_city"] = contact.contact_city;
            if (!String.IsNullOrEmpty(contact.contact_state)) createResponse["contact_state"] = contact.contact_state;
            if (!String.IsNullOrEmpty(contact.contact_zip)) createResponse["contact_zip"] = contact.contact_zip;
            if (!String.IsNullOrEmpty(contact.azure_username)) createResponse["azure_username"] = contact.azure_username;
            if (!String.IsNullOrEmpty(contact.azure_company_id)) createResponse["azure_company_id"] = contact.azure_company_id;
            if (!String.IsNullOrEmpty(contact.azure_company_id_number)) createResponse["azure_company_id_number"] = contact.azure_company_id_number;
            if (!String.IsNullOrEmpty(contact.azure_comp_3_letter_code)) createResponse["azure_comp_3_letter_code"] = contact.azure_comp_3_letter_code;
            if (!String.IsNullOrEmpty(contact.name)) createResponse["name"] = contact.name;

            // get the emails
            if (contact.email != null && contact.email.Count != 0) createResponse = CreateEmail(contact.email, createResponse);

            // get the phones
            if (contact.phone != null && contact.phone.Count != 0) createResponse = CreatePhone(contact.phone, createResponse);

            return createResponse;
        }

        private static JObject CreatePhone(List<PipePhone> list, JObject createResponse)
        {
            JArray array = new JArray();

            foreach (PipePhone phone in list)
            {
                if (phone != null)
                {
                    if (!String.IsNullOrEmpty(phone.value))
                    {
                        JObject tempPhone = new JObject();

                        tempPhone["label"] = phone.label;
                        tempPhone["value"] = phone.value;
                        tempPhone["primary"] = phone.primary;

                        array.Add(tempPhone);
                    }
                }
            }

            if (array != null && array.Count != 0) createResponse["phone"] = array;

            return createResponse;
        }

        private static JObject CreateEmail(List<PipeEmail> list, JObject createResponse)
        {
            JArray array = new JArray();

            foreach(PipeEmail email in list)
            {
                if(email != null)
                {
                    if(!String.IsNullOrEmpty(email.value))
                    {
                        JObject tempEmail = new JObject();

                        tempEmail["label"] = email.label;
                        tempEmail["value"] = email.value;
                        tempEmail["primary"] = email.primary;

                        array.Add(tempEmail);
                    }
                }
            }

            if (array != null && array.Count != 0) createResponse["email"] = array;

            return createResponse;
        }
    }
}
