using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redis_using.Models;

namespace Redis_using.Services.Abstraction
{
    public interface IBankingOperationService
    {
        List<AccountItem> GetAccounts(string id);
    }
}
