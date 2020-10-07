using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;


namespace SQLServerTierCalaculator
{

    public static class FuncWithReadAccessOnly
    {
        private static IConfiguration Configuration { set; get; }
        static FuncWithReadAccessOnly()
        {
            Configuration = new ConfigurationBuilder()
              .SetBasePath(Environment.CurrentDirectory)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();

        }

        [FunctionName("FuncWithReadAccessOnly")]
        public static void Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer, ILogger log)
        {
            HttpClient httpClient = new HttpClient();
            int i = 0;
            
            try
            {
                string myconfigurationUsage = Configuration["Usage"];
                string LogicAppPath = Configuration["LogicAppPathReadAccessOnly"];

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

    
}
