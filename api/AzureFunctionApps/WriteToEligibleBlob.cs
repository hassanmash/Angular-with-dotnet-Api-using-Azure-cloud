using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionApps
{
    public class WriteToEligibleBlob
    {
        [FunctionName("WriteToEligibleBlob")]
        public void Run(
            [QueueTrigger("eligiblequeue")] string queueMessage,
            [Blob("common/common.txt",FileAccess.Read)] Stream common,
            [Blob("eligiblecontainer/{DateTime}.txt",FileAccess.Write)] Stream eligible,
            ILogger log
        )
        {
            log.LogInformation("C# Queue trigger function processed: " + queueMessage);
            
            StreamReader sr = new StreamReader(common);
            string data = sr.ReadToEnd();
            sr.Close();

            log.LogInformation("Common Blob read: " + data);

            StreamWriter sw = new StreamWriter(eligible);
            sw.WriteLine(data);
            sw.WriteLine(queueMessage);
            sw.Close();
            log.LogInformation("Eligible queue function wrote to blob");
        }
    }
}
