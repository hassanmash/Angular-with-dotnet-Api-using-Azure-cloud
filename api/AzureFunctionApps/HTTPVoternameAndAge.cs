using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionApps
{
    public static class HTTPVoternameAndAge
    {
        [FunctionName("HttpVoterNameAndAge")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("eligiblequeue")] IAsyncCollector<string> eligiblequeue,
            [Queue("noteligiblequeue")] IAsyncCollector<string> noteligiblequeue,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string votername = req.Query["votername"];
            string age = req.Query["age"];
            if (string.IsNullOrEmpty(votername) && string.IsNullOrEmpty(age))
                return new NotFoundObjectResult("Either name or age or both are not specified.");

            if(int.Parse(age) >= 18)
            {
                string message = votername + " aged " + age + " is ELIGIBLE to vote";
                await eligiblequeue.AddAsync(message);
            }
            else
            {
                string message = votername + " aged " + age + " is NOT ELIGIBLE to vote";
                await noteligiblequeue.AddAsync(message);
            }

            return new OkObjectResult("Message written to queue.");
        }
    }
}
