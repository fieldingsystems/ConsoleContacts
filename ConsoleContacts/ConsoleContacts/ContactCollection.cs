using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContacts
{
    class ContactCollection
    {
        private const string WORK = "work";
        private const string HOME = "home";
        private const string MOBILE = "mobile";

        // create a list of all the contacts in office
        private static List<Contact> GetOfficeContacts(List<OfficeContactCollection> officeContactCollection)
        {
            // temp list to be returned
            List<Contact> tempContactList = new List<Contact>();

            // get every contact in the collection and add it to the list
            foreach (OfficeContactCollection collection in officeContactCollection)
            {
                // each individual office contact turn it into a contact object
                foreach (OfficeContact officeContact in collection.value)
                {
                    // new temp contact of the office contact and add it to the list
                    Console.WriteLine("Compare Office Contact:\t" + officeContact.DisplayName);
                    tempContactList.Add(new Contact(true, false, officeContact, CreatePipedriveContact(officeContact)));                  
                }
            }               

            return tempContactList;
        }

        // creates a list of all the contacts in pipedrive
        private static List<Contact> GetPipedriveContacts(List<PipedriveContactCollection> pipedriveContactCollection, List<Contact> contactList)
        {
            // iterate through each collection of pipedrive contacts
            foreach(PipedriveContactCollection collection in pipedriveContactCollection)
            {
                // iterate through each 
                foreach(PipedriveContact pipedriveContact in collection.data)
                {
                    Console.WriteLine("Compare Pipedrive Contact: " + pipedriveContact.name);
                    contactList = CheckPipedriveContact(pipedriveContact, contactList);
                }
            }

            // return the appended list
            return contactList;
        }

        // this checks if the contact already exist, if it does well skip it
        private static List<Contact> CheckPipedriveContact(PipedriveContact pipedriveContact, List<Contact> contactList)
        {
            // new contact
            bool isNew = true;

            foreach(Contact contact in contactList)
            {
                // compare the email addresses 
                if (contact.pipedriveContact.email.Count != 0 && pipedriveContact.email.Count != 0)
                    foreach (PipeEmail email in pipedriveContact.email)
                        foreach (PipeEmail email2 in contact.pipedriveContact.email)
                            if(!String.IsNullOrEmpty(email.value) && !String.IsNullOrEmpty(email2.value) 
                                && String.Equals(email.value,email2.value,StringComparison.OrdinalIgnoreCase))
                            {
                                isNew = false;
                                contact.pipedriveContact = pipedriveContact;
                                contact.isPipedriveContact = true;
                            }
            }

            if (isNew)
            {
                contactList.Add(new Contact(false, true, CreateOfficeContact(pipedriveContact), pipedriveContact));
            }   

            return contactList;
        }

        // combines pipedrive and office list into a single master list
        public static List<Contact> CreateContactList(List<OfficeContactCollection> officeContactCollection, List<PipedriveContactCollection> pipedriveContactCollection)
        {
            // first add all the office contacts
            List<Contact> contactList = GetOfficeContacts(officeContactCollection);

            // make pipedrive list
            contactList = GetPipedriveContacts(pipedriveContactCollection, contactList);

            return contactList;
        }


        // this method does all the work that creates the pipedrive part of an office contact
        private static PipedriveContact CreatePipedriveContact(OfficeContact officeContact)
        {
            // create this new pipedrive contact from this office contact
            PipedriveContact tempPipedriveContact = new PipedriveContact();

            // first make all the name stuff worked out
            tempPipedriveContact = GetPipeName(tempPipedriveContact, officeContact);

            // copy over company info
            tempPipedriveContact = GetPipeCompanyInfo(tempPipedriveContact, officeContact);

            // copy over the phone information
            tempPipedriveContact = GetPipePhoneInfo(tempPipedriveContact, officeContact);

            // copy over email information
            tempPipedriveContact = GetPipeEmailInfo(tempPipedriveContact, officeContact);

            // copy over address info
            tempPipedriveContact = GetPipeAddressInfo(tempPipedriveContact, officeContact);

            return tempPipedriveContact;
        }

        private static PipedriveContact GetPipeAddressInfo(PipedriveContact tempPipedriveContact, OfficeContact officeContact)
        {
            // copy over all the info
            tempPipedriveContact.contact_address_1 = officeContact.HomeAddress.Street;
            tempPipedriveContact.contact_city = officeContact.HomeAddress.City;
            tempPipedriveContact.contact_state = officeContact.HomeAddress.State;
            tempPipedriveContact.contact_zip = officeContact.HomeAddress.PostalCode;

            return tempPipedriveContact;
        }

        private static PipedriveContact GetPipeEmailInfo(PipedriveContact tempPipedriveContact, OfficeContact officeContact)
        {
            // check if these emails are null
            if (officeContact.EmailAddresses == null || officeContact.EmailAddresses.Count == 0) return tempPipedriveContact;

            tempPipedriveContact.email = new List<PipeEmail>();

            // add all emails to the pipedrive contact
            foreach (OfficeEmail email in officeContact.EmailAddresses)
                if(email != null)
                    if (!String.IsNullOrEmpty(email.Address))
                        tempPipedriveContact.email.Add(new PipeEmail() { label = "work", primary = false, value = email.Address });  
                             

            return tempPipedriveContact;
        }

        // translate all the phone stuff
        private static PipedriveContact GetPipePhoneInfo(PipedriveContact tempPipedriveContact, OfficeContact officeContact)
        {
            tempPipedriveContact.phone = new List<PipePhone>();

            // first copy over mobile phones
            if (!String.IsNullOrEmpty(officeContact.MobilePhone1))
            {
                // custom mobile field
                tempPipedriveContact.mobile = officeContact.MobilePhone1;

                // create new instance of phone object
                tempPipedriveContact.phone.Add(new PipePhone() { label = "mobile", primary = false, value = officeContact.MobilePhone1 });
            }

            // Copy over home phone info
            foreach (String phoneNumber in officeContact.HomePhones)
            {
                if(!String.IsNullOrEmpty(phoneNumber)) tempPipedriveContact.phone.Add(new PipePhone() { label = "home", primary = false, value = phoneNumber });
            }

            // copy over buisiness phone info
            foreach (String phoneNumber in officeContact.BusinessPhones)
            {
                if (!String.IsNullOrEmpty(phoneNumber)) tempPipedriveContact.phone.Add(new PipePhone() { label = "work", primary = false, value = phoneNumber });
            }

            return tempPipedriveContact;
        }

        private static PipedriveContact GetPipeCompanyInfo(PipedriveContact tempPipedriveContact, OfficeContact officeContact)
        {
            // get all the info from the company stuff copied over
            if (!String.IsNullOrEmpty(officeContact.JobTitle)) tempPipedriveContact.job_title = officeContact.JobTitle;
            if (!String.IsNullOrEmpty(officeContact.Profession)) tempPipedriveContact.job_title = officeContact.Profession;
            if (!String.IsNullOrEmpty(officeContact.CompanyName)) tempPipedriveContact.org_name = officeContact.CompanyName;

            return tempPipedriveContact;
        }

        // this adds all the name functions of the pipedrive contact
        private static PipedriveContact GetPipeName(PipedriveContact tempPipedriveContact, OfficeContact officeContact)
        {
            // get the name stuff copied over
            if (!String.IsNullOrEmpty(officeContact.DisplayName)) tempPipedriveContact.first_name = officeContact.GivenName;
            if (!String.IsNullOrEmpty(officeContact.GivenName)) tempPipedriveContact.name = officeContact.DisplayName;
            if (!String.IsNullOrEmpty(officeContact.Surname)) tempPipedriveContact.last_name = officeContact.Surname;

            return tempPipedriveContact;
        }

        // this method does all the work that creates the office part of a pipedrive contact
        private static OfficeContact CreateOfficeContact(PipedriveContact pipedriveContact)
        {
            // create this new pipedrive contact from this office contact
            OfficeContact tempOfficeContact = new OfficeContact();

            // first make all the name stuff worked out
            tempOfficeContact = GetOfficeName(tempOfficeContact, pipedriveContact);

            // copy over company info
            tempOfficeContact = GetOfficeCompanyInfo(tempOfficeContact, pipedriveContact);

            // copy over the phone information
            tempOfficeContact = GetOfficePhoneInfo(tempOfficeContact, pipedriveContact);

            // copy over email information
            tempOfficeContact = GetOfficeEmailInfo(tempOfficeContact, pipedriveContact);

            // copy over mail address
            tempOfficeContact = GetOfficeAddressInfo(tempOfficeContact, pipedriveContact);

            return tempOfficeContact;
        }

        private static OfficeContact GetOfficeAddressInfo(OfficeContact tempOfficeContact, PipedriveContact pipedriveContact)
        {
            // copy over all the pipedrive info intoa new office address location
            tempOfficeContact.HomeAddress = new OfficeAddress();

            // copy over all the office address information
            tempOfficeContact.HomeAddress.Street = String.Concat(pipedriveContact.contact_address_1, pipedriveContact.contact_address_2);
            tempOfficeContact.HomeAddress.City = pipedriveContact.contact_city;
            tempOfficeContact.HomeAddress.State = pipedriveContact.contact_state;
            tempOfficeContact.HomeAddress.PostalCode = pipedriveContact.contact_zip;

            return tempOfficeContact;
        }

        private static OfficeContact GetOfficeEmailInfo(OfficeContact tempOfficeContact, PipedriveContact pipedriveContact)
        {
            // copy all emails over
            if(pipedriveContact.email != null)
            {
                tempOfficeContact.EmailAddresses = new List<OfficeEmail>();

                foreach(PipeEmail email in pipedriveContact.email)
                {
                    tempOfficeContact.EmailAddresses.Add(new OfficeEmail() { Address = email.value, Name = email.label});
                }
            }


            return tempOfficeContact;
        }

        private static OfficeContact GetOfficePhoneInfo(OfficeContact tempOfficeContact, PipedriveContact pipedriveContact)
        {
            // handle the mobile exception first
            if (!String.IsNullOrEmpty(pipedriveContact.mobile)) tempOfficeContact.MobilePhone1 = pipedriveContact.mobile;

            // tranfer phone objects to office phone strings
            if (pipedriveContact.phone != null)
                if (pipedriveContact.phone.Count != 0)
                    tempOfficeContact = AddPipePhones(tempOfficeContact, pipedriveContact);

            return tempOfficeContact;
        }

        private static OfficeContact AddPipePhones(OfficeContact tempOfficeContact, PipedriveContact pipedriveContact)
        {
            // phone logic
            foreach(PipePhone phone in pipedriveContact.phone)
            {
                // add a work phone
                if (WORK.Equals(phone.label, StringComparison.OrdinalIgnoreCase)) 
                {
                    // handel null cases
                    if (tempOfficeContact.BusinessPhones == null) tempOfficeContact.BusinessPhones = new List<String>();
                    
                    // add to list
                    if(tempOfficeContact.BusinessPhones.Count < 2) tempOfficeContact.BusinessPhones.Add(phone.value);
                }

                // add the home number
                else if (HOME.Equals(phone.label, StringComparison.OrdinalIgnoreCase))
                {
                    // handel null cases
                    if (tempOfficeContact.HomePhones == null) tempOfficeContact.HomePhones = new List<String>();

                    // add to list
                    if (tempOfficeContact.HomePhones.Count < 2) tempOfficeContact.HomePhones.Add(phone.value);
                }

                // add mobile phone numbers
                else if (MOBILE.Equals(phone.label, StringComparison.OrdinalIgnoreCase)
                    && String.IsNullOrEmpty(tempOfficeContact.MobilePhone1))
                {
                    tempOfficeContact.MobilePhone1 = phone.value;
                }
            }

            return tempOfficeContact;
        }

        private static OfficeContact GetOfficeCompanyInfo(OfficeContact tempOfficeContact, PipedriveContact pipedriveContact)
        {
            // get all the info from the company stuff copied over
            if (!String.IsNullOrEmpty(pipedriveContact.job_title)) tempOfficeContact.JobTitle = pipedriveContact.job_title;
            if (!String.IsNullOrEmpty(pipedriveContact.job_title)) tempOfficeContact.Profession = pipedriveContact.job_title;
            if (!String.IsNullOrEmpty(pipedriveContact.org_name)) tempOfficeContact.CompanyName = pipedriveContact.org_name;

            return tempOfficeContact;
        }

        private static OfficeContact GetOfficeName(OfficeContact tempOfficeContact, PipedriveContact pipedriveContact)
        {
            // transfer the name over to the new office contact
            if (!String.IsNullOrEmpty(pipedriveContact.name)) tempOfficeContact.DisplayName = pipedriveContact.name;
            if (!String.IsNullOrEmpty(pipedriveContact.first_name)) tempOfficeContact.GivenName = pipedriveContact.first_name;
            if (!String.IsNullOrEmpty(pipedriveContact.last_name)) tempOfficeContact.Surname = pipedriveContact.last_name;

            return tempOfficeContact;
        }
    }
}
