using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContacts
{
    class PipedriveContactCollection
    {
        public bool success { get; set; }
        public List<PipedriveContact> data { get; set; }
        public AdditionalData additional_data { get; set; }
    }

    public class AdditionalData
    {
        public Pagination pagination { get; set; }
    }

    public class Pagination
    {
        public int start { get; set; }
        public int limit { get; set; }
        public bool more_items_in_collection { get; set; }
    }

    public class PipedriveContact
    {
        public int id { get; set; }
        public int company_id { get; set; }
        public PipeOwner owner_id { get; set; }
        public PipeOrganization org_id { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public List<PipePhone> phone { get; set; }
        public List<PipeEmail> email { get; set; }
        public string contact_type { get; set; }
        public string marketing_permission { get; set; }
        public string job_title { get; set; }
        public string mobile { get; set; }
        public string decision_role { get; set; }
        public string azure_user_id { get; set; }
        public string contact_address { get; set; }
        public string contact_address_1 { get; set; }
        public string contact_address_2 { get; set; }
        public string contact_city { get; set; }
        public string contact_state { get; set; }
        public string contact_zip { get; set; }
        public string source { get; set; }
        public string azure_username { get; set; }
        public string azure_company_id { get; set; }
        public string azure_company_id_number { get; set; }
        public string azure_comp_3_letter_code { get; set; }
        public string phone_ext { get; set; }
        public string fax { get; set; }
        public string system_user { get; set; }
        public string org_name { get; set; }
    }

    public class PipeOrganization
    {
        public int value { get; set; }
        public string name { get; set; }
        public int people_count { get; set; }
    }

    public class PipeOwner
    {
        public int id { get; set; }
        public int value { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public bool has_pic { get; set; }
    }

    public class PipePhone
    {
        public string label { get; set; }
        public string value { get; set; }
        public bool primary { get; set; }
    }

    public class PipeEmail
    {
        public string label { get; set; }
        public string value { get; set; }
        public bool primary { get; set; }
    }
}
