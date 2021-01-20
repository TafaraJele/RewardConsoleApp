using Microsoft.Extensions.Configuration;
using RewardConsoleApp.Models;
using RewardConsoleApp.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RewardConsoleApp
{
    class Program
    {
        private static IConfiguration _iconfiguration;
        static void Main(string[] args)
        {
            GetAppSettingsFile();            
            var reward = new CreateRewardFile(_iconfiguration);
            var customers = new RewardFileDbContext(_iconfiguration);
            var customerList = customers.GetList();
            var cards = reward.GetRewardCards(customerList);

          //  var customerparameters = new List<RewardCustomerDetails>();

          //foreach(var customer in customerList)
          //  {

              
          //  }
            var parameters = customerList.Where(p => p.IssuerId == "1").Select(p => p).FirstOrDefault();
            string[] fileNameParameters = new string[] { parameters.IssueBankShortName, $"{parameters.ProductBinCode}{parameters.ProductCode}" };
            if (cards.rewardCardList.Count > 0)
            {
               // var fileNameParameters = reward.GetFileName();
                reward.GenerateRewardFile(cards, fileNameParameters);
            }
            else
            {
                Console.WriteLine("No RewardFiles Found");
            }
        }
        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }
        
    }
}
