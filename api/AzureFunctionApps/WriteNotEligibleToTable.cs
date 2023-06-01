using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionApps
{
    public class WriteNotEligibleToTable
    {
        [FunctionName("WriteNotEligibleToTable")]
        [return: Table("VoterTable")]
        public MyTableData Run(
            [BlobTrigger("noteligiblecontainer/{name}")]Stream myBlob,
            [Blob("common/common.txt",FileAccess.Read)] Stream myFile,
            string name,
            ILogger log
        )
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            StreamReader srcommon = new StreamReader(myFile);
            string datacommon = srcommon.ReadToEnd();
            srcommon.Close();

            StreamReader sr = new StreamReader(myBlob);
            string data = sr.ReadToEnd();
            sr.Close();

            data = data.Trim(datacommon.ToCharArray()).Trim();
            log.LogInformation("Blob contents: " + data);

            string voter = data.Split(" aged ")[0];
            string age = data.Split("aged ")[1].Split(" is ")[0];
            string elibility = data.Split("aged ")[1].Split(" is ")[1].Split(" to ")[0];

            log.LogInformation("name: " + voter);
            log.LogInformation("age: " + age);
            log.LogInformation("elibility: " + elibility);

            return new MyTableData { PartitionKey = age, RowKey = voter, eligibilty = elibility, Timestamp = DateTime.Now };
        }
    }

}
