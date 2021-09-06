using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using electronicwatches.Common.Models;

namespace electronicwatches.Functions.Functions
{
    public static class ClockConsolidateApi
    {
        //[FunctionName(nameof(Consolidate))]
        //public static async Task<IActionResult> Consolidate(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "consolidate")] HttpRequest req,
        //    [Table("consolidate", Connection = "AzureWebJobsStorage")] CloudTable consolidateTable,
        //    ILogger log)
        //{
        //    log.LogInformation("Recieved a new consolidate.");

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    ClockConsolidate clockConsolidate = JsonConvert.DeserializeObject<ClockConsolidate>(requestBody);
        //    Clock clock = JsonConvert.DeserializeObject<Clock>(requestBody);

            
        //}
    }
}
