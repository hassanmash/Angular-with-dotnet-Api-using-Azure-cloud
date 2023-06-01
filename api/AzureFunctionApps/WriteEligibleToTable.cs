using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Xml;
using Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TableAttribute = Microsoft.Azure.WebJobs.TableAttribute;

namespace AzureFunctionApps
{
    public class WriteEligibleToTable
    {
        [FunctionName("WriteEligibleToTable")]
        [return: Table("VoterTable")]
        public MyTableData Run(
            [BlobTrigger("eligiblecontainer/{name}")]Stream myBlob,
            [Blob("common/common.txt",FileAccess.Read)] Stream myFile,
            string name,
            ILogger log)
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
            return new MyTableData { PartitionKey = age, RowKey = voter, eligibilty = elibility, Timestamp = DateTime.Now};
        }
    }

    public class MyTableData : Azure.Data.Tables.ITableEntity
    {
        public string eligibilty { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
