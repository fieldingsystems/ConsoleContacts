using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContacts
{
    class Program
    {
        private const string trishasAPIKey = "6bf1e2564ff2c2588113c9e516cff154e88b1648";
        private const string davesAPIKey = "4248e3b08f313be6932d39775f2dc0f221d9d73a";

        private const string trishaOfficeLogin = "trisha.breckenridge@fieldingsystems.com";
		private const string trishaOfficePassword = "Tlbreck!89";

        static void Main(string[] args)
        {
            Task.WaitAll(DoWork());

            // pause so i can debug
            Console.WriteLine("\n\nContacts Imported.");
            Console.ReadKey();
        }

        private static async Task DoWork()
        {
            Task.WaitAll(await GetContacts(trishaOfficeLogin, trishaOfficePassword, trishasAPIKey));
        }

        private static async Task<Task> GetContacts(string officeLogin, string officePassword, string pipedriveAPIKey)
        {
            // get number of office contacts
            Console.WriteLine("\n\nNumber of office contacts: ");
            int numberOfOfficeContacts = await GetNumbumberOfOfficeContacts(officeLogin, officePassword);
            Console.WriteLine(numberOfOfficeContacts);

            // get all contacts from office
            Console.WriteLine("\n\nGetting Office Contacts...\n\n");
            List<OfficeContactCollection> tempOfficeContactsList = GetOfficeContacts(officeLogin, officePassword, numberOfOfficeContacts);

            // get all pipedrive contacts
            Console.WriteLine("\n\nGetting Trisha's Pipedrive Contacts...\n\n");
            List<PipedriveContactCollection> tempPipedriveContactList = GetPipedriveContacts(pipedriveAPIKey);

            // get daves contacts
            Console.WriteLine("\n\nGetting Daves's Pipedrive Contacts...\n\n");
            List<PipedriveContactCollection> tempPipedriveContactListDave = GetPipedriveContacts(davesAPIKey);
            foreach (PipedriveContactCollection davesCollection in tempPipedriveContactListDave)
                tempPipedriveContactList.Add(davesCollection);

            // merge into single list
            Console.WriteLine("\n\nCompare Contacts...\n\n");
            List<Contact> contactList = ContactCollection.CreateContactList(tempOfficeContactsList, tempPipedriveContactList);
            Console.WriteLine("\n\nCompare Complete...\n\n");

            // this is for debugging only
            Console.WriteLine("\n\nWriting to file...\n\n");
            await EmailLogs(contactList);
            Console.WriteLine("\n\nWrite to file Complete...\n\n");

            // add contacts
            Console.WriteLine("\n\nAdding Contacts...\n\n");
            await SyncContacts(contactList);

            return Task.Delay(0);
        }

        private static async Task SyncContacts(List<Contact> contactList)
        {
            // add contacts to office
            Console.WriteLine("\n\nAdding Contacts to Office..................\n\n");
            foreach (Contact contact in contactList.Where(x => !x.isOfficeContact))
            {
                if (!String.IsNullOrEmpty(contact.officeContact.GivenName))
                {
                    if(EmailChecked(contact.officeContact))
                    {
                        Console.WriteLine(contact.officeContact.DisplayName);
                        await OfficeApiWrite.CreateOfficeContact(contact, trishaOfficeLogin, trishaOfficePassword);
                    }   
                }
            }

            // add contacts to pipedrive
            Console.WriteLine("\n\nAdding Contacts to Pipedrive..................\n\n");
            foreach (Contact contact in contactList.Where(x => !x.isPipedriveContact))
            {
                if (!String.IsNullOrEmpty(contact.pipedriveContact.name))
                {
                    if(EmailChecked(contact.pipedriveContact))
                    {
                        Console.WriteLine(contact.pipedriveContact.name);
                        PipedriveAPIWrite.CreatePipedriveContact(contact, trishasAPIKey);
                    }
                }
            }
        }

        private static bool EmailChecked(PipedriveContact contact)
        {
            if (contact.email == null) return false;
            if (contact.email.Count == 0) return false;

            foreach(PipeEmail email in contact.email)
            {
                if(email != null)
                {
                    if (!String.IsNullOrEmpty(email.value))
                        return true;
                }
            }

            return false;
        }

        private static bool EmailChecked(OfficeContact contact)
        {
            if (contact.EmailAddresses == null) return false;
            if (contact.EmailAddresses.Count == 0) return false;

            foreach (OfficeEmail email in contact.EmailAddresses)
            {
                if(email != null)
                {
                    if (!String.IsNullOrEmpty(email.Address)) 
                        return true;
                }
            }

            return false;
        }

        private static async Task EmailLogs(List<Contact> contactList)
        {
            // lets see how fast this 
            StringBuilder text = new StringBuilder();

            // show all contacts
            text.AppendLine("\n\nALL CONTACTS to add to office..................\n\n");
            foreach (Contact contact in contactList.Where(x => !x.isOfficeContact && EmailChecked(x.officeContact)))
            {
                text.AppendLine(contact.pipedriveContact.name);
            }
            
            // show all contacts
            text.AppendLine("\n\nALL CONTACTS to add to pipedrive..................\n\n");
            foreach (Contact contact in contactList.Where(x => !x.isPipedriveContact && EmailChecked(x.pipedriveContact)))
            {
                text.AppendLine(contact.pipedriveContact.name);
            }

            await SendEmailLog.SendLog(text.ToString());
        }

        private static List<PipedriveContactCollection> GetPipedriveContacts(string pipedriveAPIKey)
        {
            // hold values returned
            List<PipedriveContactCollection> pipedriveBigList = new List<PipedriveContactCollection>();
            PipedriveContactCollection pipedriveContactList = new PipedriveContactCollection();

            // loop through pipedrive contacts
            int contactCount = 0;
            do
            {
                // get batch of contacts
                pipedriveContactList = PipedriveAPIRead.GetPipedrive(pipedriveAPIKey, contactCount);
                pipedriveBigList.Add(pipedriveContactList);

                // display output
                foreach (PipedriveContact person in pipedriveContactList.data)
                {
                    contactCount += 1;
                    Console.WriteLine("Pipe Contact: " + contactCount + "\t" + person.name);
                }

            } while (pipedriveContactList.additional_data.pagination.more_items_in_collection);

            return pipedriveBigList;
        }

        private static List<OfficeContactCollection> GetOfficeContacts(string userName, string password, int numberOfContacts)
        {
            // this is the list that will hold all the contacts that are returned
            List<OfficeContactCollection> tempOfficeContactsList = new List<OfficeContactCollection>();
            List<Task<OfficeContactCollection>> taskCollection = new List<Task<OfficeContactCollection>>();

            // keep going while there are more contacts to be retrieved
            int officeContactCount = 0;
            try
            {
                while (officeContactCount < numberOfContacts)
                {
                    taskCollection.Add(OfficeAPIRead.GetContactList(userName, password, officeContactCount));
                    officeContactCount += 50;
                }

                // wait for them to all show up
                Task.WaitAll(taskCollection.ToArray());
            }
            catch
            {
                Console.WriteLine("This Account Information Is Incorrect! for this user:\n" + userName);
            }

            taskCollection.ToString();
            // add to the contact list collection
            foreach (Task<OfficeContactCollection> task in taskCollection)
                tempOfficeContactsList.Add(task.Result);

            return tempOfficeContactsList;
        }

        private static async Task<int> GetNumbumberOfOfficeContacts(string userName, string password)
        {
            // hold the number of contacts in the list
            int numberOfOfficeContacts = new int();

            // find out how many contacts there are
            numberOfOfficeContacts = await OfficeAPIRead.GetNumberOfContacts(userName, password);

            return numberOfOfficeContacts;
        }

        private static List<Contact> MergeContacts(List<OfficeContactCollection> tempOfficeContactsList, List<PipedriveContactCollection> tempPipedriveContactList)
        {
            // this will hold all the contacts
            List<Contact> contactList = new List<Contact>();

            // make a list with all the office contacts
            List<Contact> officeContactList = ContactCollection.CreateContactList(tempOfficeContactsList, tempPipedriveContactList);

            return contactList;
        }
    }
}
