using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis_using.Models;
using Redis_using.Services.Abstraction;
using Redis_using.Services.Implementation;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace Redis_using.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingOperationController : ControllerBase
    {
        private IBankingOperationService _service;
        private IDistributedCache _cache;
        public BankingOperationController(IBankingOperationService service, IDistributedCache cache)
        {
            _service = service;
            _cache = cache;
        }

        [HttpGet]
        public List<AccountItem> GetUsers(string id)
        {
            List<AccountItem> accounts;
            string cacheJsonItem;
            //***
            //redis uzerine yazilib yoxsa yox
            var accountsFromCache = _cache.Get(id);

            //***
            //id redisde varsa oxuyub string kimi aliram(json kimi)
            if (accountsFromCache!=null)
            {
                cacheJsonItem = Encoding.UTF8.GetString(accountsFromCache);
                accounts = JsonConvert.DeserializeObject<List<AccountItem>>(cacheJsonItem);
            }
            //id redisde yoxdursa yazdigim service'dan oxuyuram
            //absoluteexpiration- datanin cache'da tutulma muddeti
            //slidingexpiration-datanin saxlanma muddetini artitirir
            else
            {
                accounts = _service.GetAccounts(id);
                cacheJsonItem = JsonConvert.SerializeObject(accounts);
                accountsFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1))
                    .SetAbsoluteExpiration(DateTime.Now.AddMonths(1));
                _cache.Set(id,accountsFromCache,options);
            }

            return accounts;
        }
    }
}
