using BankAssignment.Data;
using BankAssignment.Models;
using BankAssignment.Repository;
using BankAssignment.Utilities;
using BankAssignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankAssignment.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext context)
        {
            _db = context;
        }
        
        /// <summary>
        /// 1.call the all the record from the clientaccountvm
        /// 2. if there is sortorder or searching, get the new list from database
        /// 3. pass the account list to the view
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <returns>IActionResult</returns>
        public IActionResult Index(string message, string sortOrder, string currentFilter, string searchString, int? page)

        {
            if (message == null){
                message = "";
            }
            if (searchString != null){
                page = 1;
            }else{
                searchString = currentFilter;
            }

            if (string.IsNullOrEmpty(sortOrder)){
                ViewData["accountSort"] = "accountNum_desc";
            }else{
                ViewData["accountSort"] = sortOrder == "AccountNum" ? "accountNum_desc" : "AccountNum";
            }
            ViewData["accountTypeSort"] = sortOrder == "AccountType" ? "accountType_desc" : "AccountType";

            ViewData["Message"] = message;
            var email = User.Identity.Name;
            ClientAccountRepo car = new ClientAccountRepo(_db);

            IQueryable<ClientAccountVM> accounts = car.GetAllRecords(email);
            if (!String.IsNullOrEmpty(searchString))
            {
                accounts = accounts.Where(ca => ca.AccountType.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "accountNum_desc":
                    accounts = accounts.OrderByDescending(ca => ca.AccountNum);
                    break;
                case "AccountType":
                    accounts = accounts.OrderBy(ca => ca.AccountType);
                    break;
                case "accountType_desc":
                    accounts = accounts.OrderByDescending(ca => ca.AccountType);
                    break;
                default:
                    accounts = accounts.OrderBy(ca => ca.AccountNum);
                    break;
            }

            int pageSize = 4;

            return View(PaginatedList<ClientAccountVM>.Create(accounts.AsNoTracking()
                                                     , page ?? 1, pageSize));


        }

        /// <summary>
        /// 1. send the accountType viewdata and sent it to the view
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult Create()
        {
            ViewData["accountType"] =new SelectList(_db.BankAccountType, "AccountType", "AccountType");

            return View();
        }

        /// <summary>
        /// 1. get email and clientId 
        /// 2. get the data from the bind and send it to the each of database
        /// 3.post it to the view with a message
        /// </summary>
        /// <param name="bankAccountVM"></param>
        /// <returns>IActionResult</returns>
        [HttpPost] // POST: Home/Create
        public IActionResult Create([Bind("AccountNum, AccountType, Balance")] BankAccountVM bankAccountVM)
        {
            var email = User.Identity.Name;
            var clientId = _db.Clients.Where(c => c.Email == email).FirstOrDefault();
            var user = clientId.ClientID;
            string message = "";
            BankAccountRepo bar = new BankAccountRepo(_db);
            ClientAccountRepo car = new ClientAccountRepo(_db);
            
            if (ModelState.IsValid)
            {
                try
                {
                    BankAccount ba = new BankAccount
                    {
                        //AccountNum = bankAccountVM.AccountNum,
                        AccountType = bankAccountVM.AccountType,
                        Balance = bankAccountVM.Balance
                    };
                    bar.CreateBankAccount(ba);
                    
                    ClientAccount ca = new ClientAccount
                    {
                        ClientID = user,
                        AccountNum = ba.AccountNum
                    };
                    car.CreateClientAccount(ca);

                    message = "Creates ClientAccounts Successfully";
                    return RedirectToAction("Details", new { id = ba.AccountNum, message = message });
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }
            ViewData["accountType"] = new SelectList(_db.BankAccountType, "AccountType", "AccountType");
            return View(bankAccountVM);
        }

        /// <summary>
        /// 1. get the details by id
        /// 2. send it to the view
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult Edit(int id)
        {
            var email = User.Identity.Name;
            ClientAccountRepo car = new ClientAccountRepo(_db);
            var accounts = car.GetDetails(email, id);
            string massage = "";
            ViewData["accountType"] = new SelectList(_db.BankAccountType, "AccountType", "AccountType");

            return View(accounts);
        }


        /// <summary>
        /// 1.get the edited data from the bind and send it to the each of database
        /// 2.save the changes and post it to the view with a message
        /// </summary>
        /// <param name="bankAccountVM"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult Edit([Bind("AccountNum, FirstName, LastName, AccountType, Balance, ClientID, Email")] ClientAccountVM clientAccountVM)
        {
            BankAccountRepo bar = new BankAccountRepo(_db);
            ClientRepo ca = new ClientRepo(_db);
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {   
                    bar.EditBankAccount(clientAccountVM);
                    clientAccountVM.Email = User.Identity.Name;
                    message = ca.EditClient(clientAccountVM);

                    
                    return RedirectToAction("Details", new { id = clientAccountVM.AccountNum, message = message });
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }
            ViewData["accountType"] = new SelectList(_db.BankAccountType, "AccountType", "AccountType");
            return View(clientAccountVM);
        }
        /// <summary>
        /// 1. get the details by id
        /// 2. send it to the view
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns>IActionResult</returns>
        public IActionResult Details(int id, string message)
        {
            
            var email = User.Identity.Name;
            ClientAccountRepo car = new ClientAccountRepo(_db);
            var accounts = car.GetDetails(email, id);
            accounts.Message = message;
            return View(accounts);
        }

        
        /// <summary>
        /// 1. get the details by id
        /// 2. send it to the view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        public IActionResult Delete(int id)
        {
            var email = User.Identity.Name;
            BankAccountRepo bar = new BankAccountRepo(_db);
            var accounts = bar.GetDetail(email, id);
            return View(accounts);
        }
        /// <summary>
        /// 1.get the edited data from the bind and send it to the database
        /// 2.delete the data and post it to the view with a message
        /// </summary>
        /// <param name="bankAccountVM"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult Delete([Bind("AccountNum")] BankAccountVM bankAccountVM)
        {
            ViewData["Message"] = "";

            try
            {
                ClientAccountRepo car = new ClientAccountRepo(_db);

                var clientAccount = car.DeleteClientAccount(bankAccountVM.AccountNum.Value);

                BankAccountRepo bar = new BankAccountRepo(_db);
                var accounts = bar.DeleteBankAccount(bankAccountVM.AccountNum.Value);

                ViewData["Message"] = "Deletes BankAccount Successfully ";

            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
            }
            return RedirectToAction("Index", ViewData);
        }

    }
}
