using Redis_using.Models;
using Redis_using.Services.Abstraction;
using System.Collections.Generic;

namespace Redis_using.Services.Implementation
{
    public class BankingOperationService : IBankingOperationService

    {
        public List<AccountItem> GetAccounts(string id)
        {
            List<AccountItem> accounts = new List<AccountItem>()
        {
            new AccountItem() { UserId = 1, AccountBalance = 1000 },
            new AccountItem() { UserId = 2, AccountBalance = 3000 }
        };
            return accounts;
        }
    }
}
