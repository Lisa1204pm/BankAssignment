using BankAssignment.Data;
using BankAssignment.Models;
using BankAssignment.ViewModels;

namespace BankAssignment.Repository
{
    public class ClientAccountRepo
    {
        ApplicationDbContext _db;

        public ClientAccountRepo(ApplicationDbContext context)
        {
            _db = context;
        }
        /// <summary>
        /// 1. get the recored by email
        /// 2. send the clientAccountVM list
        /// </summary>
        /// <param name="email"></param>
        /// <returns>IQueryable</returns>
        public IQueryable<ClientAccountVM> GetAllRecords(string email)
        {
            ClientRepo cr = new ClientRepo(_db);
            Client client = cr.GetClient(email);

            var vm = from c in _db.Clients
                     join ca in _db.ClientAccounts on c.ClientID equals ca.ClientID
                     join ba in _db.BankAccounts on ca.AccountNum equals ba.AccountNum
                     where c.Email == client.Email
                     orderby ca.AccountNum descending
                     select new ClientAccountVM
                     {
                         ClientID = c.ClientID,
                         AccountNum = ca.AccountNum,
                         LastName = c.LastName,
                         FirstName = c.FirstName,
                         Email = c.Email,
                         AccountType = ba.AccountType,
                         Balance = ba.Balance
                     };
            return vm;
        }
        /// <summary>
        /// 1. get the details by email and id
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
            public ClientAccountVM GetDetails(string email, int id)
        {
            ClientRepo cr = new ClientRepo(_db);
            Client client = cr.GetClient(email);
            BankAccountRepo bar = new BankAccountRepo(_db);
            BankAccount bankAccount = bar.GetAccountNum(id);

            
            var info = new ClientAccountVM
            {
                ClientID = client.ClientID,
                AccountNum = bankAccount.AccountNum,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                AccountType = bankAccount.AccountType,
                Balance = bankAccount.Balance
            };
            
            return info;
        }
        /// <summary>
        /// 1. delete clientaccount by account num
        /// </summary>
        /// <param name="num"></param>
        /// <returns>clientAccount</returns>
        public ClientAccount DeleteClientAccount(int num)
        {
            ClientAccount clientAccount = _db.ClientAccounts.Where(c => c.AccountNum == num).FirstOrDefault();
            _db.ClientAccounts.Remove(clientAccount);
            _db.SaveChanges();
            return clientAccount;

        }
        /// <summary>
        /// 1. get the clientAccount object
        /// 2. update the databace
        /// </summary>
        /// <param name="clientAccount"></param>
        /// <returns>ClientAccount</returns>
        public ClientAccount CreateClientAccount(ClientAccount clientAccount)
        {
            _db.ClientAccounts.Add(clientAccount);
            _db.SaveChanges();
            return clientAccount;
        }
    }
}
