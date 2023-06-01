using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionApps
{
    public class WriteToNotEligibleBlob
    {
        [FunctionName("WriteToNotEligibleBlob")]
        public void Run(
            [QueueTrigger("noteligiblequeue")] string queueMessage,
            [Blob("common/common.txt",FileAccess.Read)] Stream common,
            [Blob("noteligiblecontainer/{DateTime}.txt",FileAccess.Write)] Stream noteligible,
            ILogger log
        )
        {
            log.LogInformation("C# Queue trigger function processed: " + queueMessage);

            StreamReader sr = new StreamReader(common);
            string data = sr.ReadToEnd();
            sr.Close();
            log.LogInformation("Common blob read: " + data);

            StreamWriter sw = new StreamWriter(noteligible);
            sw.WriteLine(data);
            sw.WriteLine(queueMessage);
            sw.Close();
            log.LogInformation("Not Eligible queue function wrote to blob");
        }
    }
}
