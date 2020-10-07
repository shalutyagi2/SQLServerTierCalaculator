using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
namespace SQLServerTierCalaculator
{
    public static class FuncTriggerLogicApp
    {
        private static IConfiguration Configuration { set; get; }
        static FuncTriggerLogicApp()
        {
            Configuration = new ConfigurationBuilder()
              .SetBasePath(Environment.CurrentDirectory)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();

        }

        [FunctionName("FuncTriggerLogicApp")]
        public static void Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer, ILogger log)
        {
            HttpClient httpClient = new HttpClient();
            int i = 0;
            //log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            try
            {
                string myconfigurationUsage = Configuration["Usage"];
                string LogicAppPath = Configuration["LogicAppPath"];

                var req = new TierCalculatorRequest();
                req.Usage = myconfigurationUsage;
                var jsonval = JsonConvert.SerializeObject(req);
                Console.WriteLine(jsonval);
                var response = httpClient.PostAsync(LogicAppPath, new StringContent(jsonval, System.Text.Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                log.Log(LogLevel.Error, $"{e.Message} {e.StackTrace}");
                throw;
            }
        }

    
    }
   public class TierCalculatorRequest
    {
        public string Usage { get; set; }
    }
}
