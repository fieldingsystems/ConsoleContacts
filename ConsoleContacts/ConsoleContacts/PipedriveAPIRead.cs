using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContacts
{
    class PipedriveAPIRead
    {
        private static string frontURL = "https://api.pipedrive.com/v1/persons?start=";
        private static string backURL = "&sort_mode=asc&api_token=";

        public static PipedriveContactCollection GetPipedrive(string token, int contactCount)
        {
            PipedriveContactCollection pipedriveContactList = new PipedriveContactCollection();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(frontURL + String.Format("{0}", contactCount) + backURL + token);

            // Set credentials to use for this request.
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Get the stream associated with the response.
            Stream recieveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(recieveStream, Encoding.UTF8);

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            string jsonOut = readStream.ReadToEnd();

            // replace hashlist for custom fields into human usable words for deserialization
            jsonOut = jsonOut.Replace("8f5b148f7f87b73615c1b0601ad807e0e5f7067f", "contact_type");
            jsonOut = jsonOut.Replace("258dd4a24e0e782635c95287fc136d3b27a61ee7", "marketing_permission");
            jsonOut = jsonOut.Replace("ca0f90a971f6b09fbc83aed6399c30db9ab147cc", "job_title");
            jsonOut = jsonOut.Replace("c0de27e8ca49d9179f2cc5dc6f140e9e8d2c7b09", "mobile"); // for trisha
            jsonOut = jsonOut.Replace("640c69d1147633b71c1d22d031f79a2ec095f4c9", "mobile"); // for daves
            jsonOut = jsonOut.Replace("d3b90173ad8a7f96188ddfbda75ee033a498a737", "decision_role");
            jsonOut = jsonOut.Replace("04ed97a8cf1db67f3c5c0f61adae077f9d9d3a34", "azure_user_id");
            jsonOut = jsonOut.Replace("75c05948e5b44900e2d94ad5f38aa95ba4fd6a6d", "contact_address");
            jsonOut = jsonOut.Replace("5c83bd66965eefafc13d630456b572a3815100c9", "contact_address_1");
            jsonOut = jsonOut.Replace("97b5993aa9f563d0887464cf8489c84b8107b2e0", "contact_address_2");
            jsonOut = jsonOut.Replace("c72c205f12f3164f20502829a9f062ca73e96171", "contact_city");
            jsonOut = jsonOut.Replace("21289d91acfd40fb7a8eea040a3d5c8ee4722dfe", "contact_state");
            jsonOut = jsonOut.Replace("f049560d41b9dbef4c7e2d611c21881d1b620204", "contact_zip");
            jsonOut = jsonOut.Replace("024a03449563ca94e52a2d8805f35c929c9564f6", "source");
            jsonOut = jsonOut.Replace("1f28414160f726dbe8b1b1a5e2f5d3587da474bd", "azure_username");
            jsonOut = jsonOut.Replace("04ed97a8cf1db67f3c5c0f61adae077f9d9d3a34", "azure_company_id");
            jsonOut = jsonOut.Replace("dc33d3b0ce9b9a8f7790de245d2949ecde06deaa", "azure_company_id#");
            jsonOut = jsonOut.Replace("7d8b40089c25498443ba7f7458162be884fa61f3", "azure_comp_3_letter_code");
            jsonOut = jsonOut.Replace("6b740582caa2acd02e6e9c63d0255a524c4f3525", "phone_ext");
            jsonOut = jsonOut.Replace("03264f17bc2011d5ad76c163d6d0a5b12a394dc7", "fax");
            jsonOut = jsonOut.Replace("745cdd3dc558d7f7619d94b0754fe080546933bb", "system_user");
            jsonOut = jsonOut.Replace("daa6f7b525203f3398dfd877b99642fb57b4fc9f", "address");
            jsonOut = jsonOut.Replace("address_subpremise", "subpremise");
            jsonOut = jsonOut.Replace("address_street_number", "street_number");
            jsonOut = jsonOut.Replace("address_route", "route");
            jsonOut = jsonOut.Replace("address_sublocality", "sublocality");
            jsonOut = jsonOut.Replace("address_locality", "locality");
            jsonOut = jsonOut.Replace("address_country", "country");
            jsonOut = jsonOut.Replace("address_postal_code", "postal-code");
            jsonOut = jsonOut.Replace("address_formatted_address", "formatted_address");

            // deserialize the contacts
            pipedriveContactList = JsonSerializer.DeserializeFromString<PipedriveContactCollection>(jsonOut);

            response.Close();
            readStream.Close();

            return pipedriveContactList;
        }
    }
}
