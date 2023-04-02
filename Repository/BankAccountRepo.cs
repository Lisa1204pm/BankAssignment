using BankAssignment.Data;
using BankAssignment.Models;
using BankAssignment.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace BankAssignment.Repository
{
    public class BankAccountRepo
    {

        ApplicationDbContext _db;

        public BankAccountRepo(ApplicationDbContext context)
        {
            _db = context;
        }
        /// <summary>
        /// 1. get the detail by email and id
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns>BankaccoutVM</returns>
        public BankAccountVM GetDetail(string email, int id)
        {
            ClientRepo cr = new ClientRepo(_db);
            Client client = cr.GetClient(email);
            BankAccount bankAccount = GetAccountNum(id);

            var info = new BankAccountVM
            {
                ClientID = client.ClientID,
                AccountNum = bankAccount.AccountNum,
                AccountType = bankAccount.AccountType,
                Balance = bankAccount.Balance
            };

            return info;
        }
        /// <summary>
        /// 1. get the account row by accountNum
        /// </summary>
        /// <param name="accountNums"></param>
        /// <returns>BankAccount</returns>
        public BankAccount GetAccountNum(int accountNums)
        {
            BankAccount accounts = _db.BankAccounts.Where(ba => accountNums == ba.AccountNum).FirstOrDefault();
            return accounts;
        }

        /// <summary>
        /// 1. get the bankaccount object
        /// 2. add into the database
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns>bankAccount</returns>
        public BankAccount CreateBankAccount(BankAccount bankAccount)
        {
            _db.BankAccounts.Add(bankAccount);
            _db.SaveChanges();

            return bankAccount;
        }
        /// <summary>
        /// 1. Delete Bankaccount by accountNums
        /// </summary>
        /// <param name="accountNums"></param>
        /// <returns>bankAccount</returns>
        public BankAccount DeleteBankAccount(int accountNums)
        {
            BankAccount accountNum = _db.BankAccounts.Where(ba => ba.AccountNum == accountNums).FirstOrDefault();
            _db.BankAccounts.Remove(accountNum);
            _db.SaveChanges();
            return accountNum;
        }
        /// <summary>
        /// 1.get the clientAccountvm object
        /// 2.update into the database
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns>bankaccount</returns>
        public BankAccount EditBankAccount(ClientAccountVM bankAccount)
        {
            BankAccount ba = new BankAccount
            {
                AccountNum = bankAccount.AccountNum,
                AccountType = bankAccount.AccountType,
                Balance = bankAccount.Balance
            };

            _db.BankAccounts.Update(ba);
            _db.SaveChanges();

            return ba;
        }

    }
}
