using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContacts
{
    class Contact
    {
        public bool isOfficeContact { get; set; }
        public bool isPipedriveContact { get; set; }
        public OfficeContact officeContact { get; set; }
        public PipedriveContact pipedriveContact { get; set; }

        public Contact(bool isOfficeContact, bool isPipedriveContact, OfficeContact officeContact, PipedriveContact pipedriveContact)
        {
            this.isOfficeContact = isOfficeContact;
            this.isPipedriveContact = isPipedriveContact;
            this.officeContact = officeContact;
            this.pipedriveContact = pipedriveContact;
        }

        public override string ToString()
        {
            // string builder
            StringBuilder text = new StringBuilder("\n");

            // pipedrive info
            text.AppendLine("\"isPipedriveContact\": " + isPipedriveContact);
            text.AppendLine("\"id\": " + pipedriveContact.id);
            text.AppendLine("\"company_id\": " + pipedriveContact.company_id);
            text.AppendLine("\"owner_id\": " + pipedriveContact.owner_id);
            text.AppendLine("\"org_id\": " + pipedriveContact.org_id);
            text.AppendLine("\"name\": " + pipedriveContact.name);
            text.AppendLine("\"first_name\": " + pipedriveContact.first_name);
            text.AppendLine("\"last_name\": " + pipedriveContact.last_name);
            text.Append("\"phone\": " + PrintPipePhone(pipedriveContact.phone));
            text.Append("\"email\": " + PrintPipeEmail(pipedriveContact.email));
            text.AppendLine("\"contact_type\": " + pipedriveContact.contact_type);
            text.AppendLine("\"marketing_permission\": " + pipedriveContact.marketing_permission);
            text.AppendLine("\"job_title\": " + pipedriveContact.job_title);
            text.AppendLine("\"mobile\": " + pipedriveContact.mobile);
            text.AppendLine("\"decision_role\": " + pipedriveContact.decision_role);
            text.AppendLine("\"azure_user_id\": " + pipedriveContact.azure_user_id);
            text.AppendLine("\"contact_address\": " + pipedriveContact.contact_address);
            text.AppendLine("\"contact_address_1\": " + pipedriveContact.contact_address_1);
            text.AppendLine("\"contact_address_2\": " + pipedriveContact.contact_address_2);
            text.AppendLine("\"contact_city\": " + pipedriveContact.contact_city);
            text.AppendLine("\"contact_state\": " + pipedriveContact.contact_state);
            text.AppendLine("\"contact_zip\": " + pipedriveContact.contact_zip);
            text.AppendLine("\"source\": " + pipedriveContact.source);
            text.AppendLine("\"azure_username\": " + pipedriveContact.azure_username);
            text.AppendLine("\"azure_company_id\": " + pipedriveContact.azure_company_id);
            text.AppendLine("\"azure_company_id_number\": " + pipedriveContact.azure_company_id_number);
            text.AppendLine("\"azure_comp_3_letter_code\": " + pipedriveContact.azure_comp_3_letter_code);
            text.AppendLine("\"phone_ext\": " + pipedriveContact.phone_ext);
            text.AppendLine("\"fax\": " + pipedriveContact.fax);
            text.AppendLine("\"system_user\": " + pipedriveContact.system_user);
            text.AppendLine("\"org_name\": " + pipedriveContact.org_name);

            // office info
            text.AppendLine("\"isOfficeContact\": " + isOfficeContact);
            text.AppendLine("\"Id\": " + officeContact.Id);
            text.AppendLine("\"Birthday\": " + officeContact.Birthday);
            text.AppendLine("\"FileAs\": " + officeContact.FileAs);
            text.AppendLine("\"DisplayName\": " + officeContact.DisplayName);
            text.AppendLine("\"GivenName\": " + officeContact.GivenName);
            text.AppendLine("\"Initials\": " + officeContact.Initials);
            text.AppendLine("\"MiddleName\": " + officeContact.MiddleName);
            text.AppendLine("\"NickName\": " + officeContact.NickName);
            text.AppendLine("\"Surname\": " + officeContact.Surname);
            text.AppendLine("\"Title\": " + officeContact.Title);
            text.AppendLine("\"Generation\": " + officeContact.Generation);
            text.Append("\"EmailAddresses\": " + GetOfficeEmail(officeContact.EmailAddresses));
            text.Append("\"ImAddresses\": " + PrintOfficeStrings(officeContact.ImAddresses));
            text.AppendLine("\"JobTitle\": " + officeContact.JobTitle);
            text.AppendLine("\"CompanyName\": " + officeContact.CompanyName);
            text.AppendLine("\"Department\": " + officeContact.Department);
            text.AppendLine("\"OfficeLocation\": " + officeContact.OfficeLocation);
            text.AppendLine("\"Profession\": " + officeContact.Profession);
            text.AppendLine("\"BusinessHomePage\": " + officeContact.BusinessHomePage);
            text.AppendLine("\"AssistantName\": " + officeContact.AssistantName);
            text.AppendLine("\"Manager\": " + officeContact.Manager);
            text.Append("\"HomePhones\": " + PrintOfficeStrings(officeContact.HomePhones));
            text.Append("\"BusinessPhones\": " + PrintOfficeStrings(officeContact.BusinessPhones));
            text.AppendLine("\"MobilePhone1\": " + officeContact.MobilePhone1);
            text.Append("\"HomeAddress\": " + PrintOfficeAddress(officeContact.HomeAddress));
            text.AppendLine("\"YomiSurname\": " + officeContact.YomiSurname);
            text.AppendLine("\"YomiGivenName\": " + officeContact.YomiGivenName);
            text.AppendLine("\"YomiCompanyName\": " + officeContact.YomiCompanyName);

            return text.ToString();
        }

        private string PrintOfficeAddress(OfficeAddress officeAddress)
        {
            if (officeAddress == null) return "\n";

            StringBuilder text = new StringBuilder("\n");
            text.AppendLine("\t\"street\": " + officeAddress.Street);
            text.AppendLine("\t\"City\": " + officeAddress.City);
            text.AppendLine("\t\"State\": " + officeAddress.State);
            text.AppendLine("\t\"PostalCode\": " + officeAddress.PostalCode);
            text.AppendLine("\t\"CountryOrRegion\": " + officeAddress.CountryOrRegion);

            return text.ToString();
        }

        private string PrintOfficeStrings(List<string> list)
        {
            if (list == null) return "\n";
            if (list.Count == 0) return "\n";

            StringBuilder text = new StringBuilder("\n");
            foreach (String str in list)
            {
                if(!String.IsNullOrEmpty(str))text.AppendLine("\t" + str);
            }

            return text.ToString();
        }

        private string PrintPipeEmail(List<PipeEmail> list)
        {
            if (list == null) return "\n";
            if (list.Count == 0) return "\n";

            StringBuilder text = new StringBuilder("\n");
            foreach (PipeEmail email in list)
            {
                text.AppendLine("\t" + email.label + "\t" + email.primary + "\t" + email.value);
            }

            return text.ToString();
        }

        private string PrintPipePhone(List<PipePhone> list)
        {
            if (list == null) return "\n";
            if (list.Count == 0) return "\n";

            StringBuilder text = new StringBuilder("\n");
            foreach (PipePhone phone in list)
            {
                text.AppendLine("\t" + phone.label + "\t" + phone.primary + "\t" + phone.value);
            }

            return text.ToString();
        }

        private string GetOfficeEmail(List<OfficeEmail> list)
        {
            if (list == null) return "\n";
            if (list.Count == 0) return "\n";

            StringBuilder text = new StringBuilder("\n");
            foreach (OfficeEmail email in list)
                if(email != null) text.AppendLine("\t" + email.Address + "\t" + email.Name);

            return text.ToString();
        }
    }
}
