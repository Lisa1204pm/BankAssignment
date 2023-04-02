using BankAssignment.Data;
using BankAssignment.Models;
using BankAssignment.ViewModels;
using System.Security.Principal;
/*using BankAssignment.ViewModels;*/

namespace BankAssignment.Repository
{
    public class ClientRepo
    {
        ApplicationDbContext _db;

        public ClientRepo(ApplicationDbContext context) {
            _db = context;
        }
        /// <summary>
        /// 1. Recive the Client object
        /// 2. add the object in the client table in database
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Client</returns>
        public Client CreateClient(Client client)
        {
            _db.Add(client);
            _db.SaveChanges();

            return client;
        }
        /// <summary>
        /// 1. find the client row by email
        /// 2. send the client object
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Client</returns>
        public Client GetClient(string email)
        {
            Client client = _db.Clients.Where(c => c.Email == email).FirstOrDefault();
            return client;
        }
        /// <summary>
        /// 1. Recive the clientaccountvm
        /// 2. update the changes
        /// 3. send the message
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string EditClient(ClientAccountVM client)
        {
            string message = "";
            try
            {
            Client c = new Client
            {
                ClientID = client.ClientID,
                LastName = client.LastName,
                FirstName = client.FirstName,
                Email = client.Email
            };

            _db.Clients.Update(c);
                _db.SaveChanges();
                message = $"Success Editing ";

            }
            catch (Exception e)
            {
                message = $"Error : {e.Message}";
            }

            return message;
        }
    }
}
