using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace ConsoleContacts
{
    class OfficeContact
    {
        public string Id { get; set; }
        public string Birthday { get; set; }
        public string FileAs { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Initials { get; set; }
        public string MiddleName { get; set; }
        public string NickName { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string Generation { get; set; }
        public List<OfficeEmail> EmailAddresses { get; set; }
        public List<String> ImAddresses { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Department { get; set; }
        public string OfficeLocation { get; set; }
        public string Profession { get; set; }
        public string BusinessHomePage { get; set; }
        public string AssistantName { get; set; }
        public string Manager { get; set; }
        public List<String> HomePhones { get; set; }
        public List<String> BusinessPhones { get; set; }
        public string MobilePhone1 { get; set; }
        public OfficeAddress HomeAddress { get; set; }
        public OfficeAddress BusinessAddress { get; set; }
        public OfficeAddress OtherAddress { get; set; }
        public string YomiSurname { get; set; }
        public string YomiGivenName { get; set; }
        public string YomiCompanyName { get; set; }
    }

    class OfficeAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryOrRegion { get; set; }
        public string PostalCode { get; set; }
    }

    class OfficeEmail
    {
        public string Address { get; set; }
        public string Name { get; set; }
    }
}
